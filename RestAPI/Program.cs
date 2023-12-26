using EvolveDb;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using RestAPI.Hypermedia.Enricher;
using RestAPI.Hypermedia.Filters;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//! Add services to the container.
builder.Services.AddControllers();

//? custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

//? custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//! Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//! Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//? custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.MapControllerRoute("DefaultApi", "api/v{version=apiVersion}/{controller=values}/{id?}");
//? custom injections //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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