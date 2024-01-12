using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExemploDeConsumoDaRestAPIviaSSharp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ExemploDeConsumoDaRestAPIviaSSharp.Requests;

public class Token
{
    private readonly RestClientOptions options;
    private readonly RestClient client;
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }

    public Token(DadosDeAcesso acesso)
    {
        options = new RestClientOptions(acesso.BaseUrl);
        client = new RestClient(options);

        RestRequest requestToken = new RestRequest("Auth/signin", Method.Post);

        requestToken.AddBody(new { userName = acesso.UserName, password = acesso.Password }, ContentType.Json);
        RestResponse responseToken = getClient().ExecutePost(requestToken);

        if (responseToken.StatusCode == HttpStatusCode.Unauthorized)
            throw new Exception("Não autorizado");

        var content = (JObject)JsonConvert.DeserializeObject(responseToken.Content);
        AccessToken = content.SelectToken("accesToken").Value<string>();
        RefreshToken = content.SelectToken("refreshToken").Value<string>();

        System.Console.WriteLine("Token requisitado com sucesso.");
    }

    public RestClient getClient()
    {
        return client;
    }
}
