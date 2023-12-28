using System.Security.Policy;
using EvolveDb;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using RestAPI.Hypermedia.Enricher;
using RestAPI.Hypermedia.Filters;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//! Add services to the container.
builder.Services.AddControllers();

//? inicio custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//!ativando serilog
//+bem no topo, pois qualquer solicitação acima dele que use o log, não vai pegar o log
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.MySQL(
            context.Configuration["ConnectionStrings:MySql"], //pode-se usar tbm conectionString ajustado acima
            "LogsAplication",
            LogEventLevel.Information,
            false,
            1
        );
});

//! colocando a conexão ao banco como dependencia
var conectionString = builder.Configuration["ConnectionStrings:MySql"];
builder.Services.AddMySql<MySqlContext>(conectionString, ServerVersion.AutoDetect(conectionString));

if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(conectionString);
}

//! adicionando servicos personalizados
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();

//! versionamento de API
builder.Services.AddApiVersioning();

//! Processo para que as requisições aceitem XML.
//! é preciso adicionar o header 'Accept' com o nome application/xml
builder.Services.AddMvc(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
    //options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
    //options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"))        
})
.AddXmlSerializerFormatters();

//! injetando dependencias para uso de HATEOAS
var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentRsponseEnticherList.Add(new PersonEnricher());
builder.Services.AddSingleton(filterOptions);

string appName = "Minha API Rest";
string appVersion = "v1";
string appDescription = $"{appName} criada em curso.";
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(appVersion,
    new OpenApiInfo
    {
        Title = appName, //titulo no swagger
        Version = appVersion,
        Description = appDescription,
        Contact = new OpenApiContact
        {
            Name = "Alexandre",
            Url = new Uri("https://alexcol.github.io/")
        }
    });
});
builder.Services.AddRouting(opt => opt.LowercaseUrls = true); //!para que fique tudo em minusculo os links no swagger

//! adicionando liberação para que se permita o consumo da API por outra origem que não C# e fora do dominio
builder.Services.AddCors(opt => opt.AddDefaultPolicy(build =>
{
    build
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));
//? fim custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//! Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//! (padrão gerado, ajustado foi criado dentro de custom injections) builder.Services.AddSwaggerGen();

var app = builder.Build();

//! Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //! (padrão gerado, ajustado foi criado dentro de custom injections)app.UseSwagger();
    //! (padrão gerado, ajustado foi criado dentro de custom injections)app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//? inicio custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//! para uso do cors, precisa ser declarado após UseHttpsRedirection()
app.UseCors();
//? fim custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

app.UseAuthorization();

app.MapControllers();

//? inicio custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//!mapear a versão do endpoint
app.MapControllerRoute("DefaultApi", "api/v{version=apiVersion}/{controller=values}/{id?}");
//!mapear o uso do swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appDescription} - {appVersion}");
});
var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

//? fim custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

app.Run();


void MigrateDatabase(string connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, Log.Information)
        {
            Locations = new List<string> { "db/migrations", "db/dataset" },
            IsEraseDisabled = true
        };
        evolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed", ex);
        throw;
    }
}