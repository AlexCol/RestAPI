
using Microsoft.Extensions.Configuration;

namespace ExemploDeConsumoDaRestAPIviaSSharp.Model.Extension;

public static class DadosDeAcessoExtension
{
    public static void DadosDeJson(this DadosDeAcesso acesso)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        acesso.BaseUrl = configuration["DadosConexao:BaseUrl"];
        acesso.UserName = configuration["DadosConexao:User"];
        acesso.Password = configuration["DadosConexao:Password"];
    }

    public static void DadosDeVariavelDeAmbiente(this DadosDeAcesso acesso, string varName)
    {
        var dadosAcesso = Environment.GetEnvironmentVariable(varName);

        var acessoSeparado = dadosAcesso.Split(";");
        acesso.BaseUrl = acessoSeparado[0].Trim();
        acesso.UserName = acessoSeparado[1].Trim();
        acesso.Password = acessoSeparado[2].Trim();
    }
}
