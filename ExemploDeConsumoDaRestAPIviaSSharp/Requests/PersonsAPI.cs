using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace ExemploDeConsumoDaRestAPIviaSSharp.Requests;

public class PersonsAPI
{
    Token _token;
    public PersonsAPI(Token token)
    {
        _token = token;
    }

    public Person buscaPorId(long id)
    {
        RestRequest request = new RestRequest($"Person/{id}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_token.AccessToken}");
        RestResponse<Person> response = _token.getClient().ExecuteGet<Person>(request);
        return response.Data;
    }

    public List<Person> buscaTodos()
    {
        RestRequest request = new RestRequest($"Person", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_token.AccessToken}");
        RestResponse<List<Person>> response = _token.getClient().ExecuteGet<List<Person>>(request);
        return response.Data;
    }

    public Person insereNovo(Person novaPerson)
    {
        RestRequest request = new RestRequest($"Person", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_token.AccessToken}");
        request.AddBody(novaPerson, ContentType.Json);
        RestResponse<Person> response = _token.getClient().ExecutePost<Person>(request);
        return response.Data;
    }

    public Person atualizaPerson(Person person)
    {
        RestRequest request = new RestRequest($"Person", Method.Put);
        request.AddHeader("Authorization", $"Bearer {_token.AccessToken}");
        request.AddBody(person, ContentType.Json);
        RestResponse<Person> response = _token.getClient().ExecutePut<Person>(request);
        return response.Data;
    }

    public bool deletaPerson(long id)
    {
        RestRequest request = new RestRequest($"Person/{id}", Method.Delete);
        request.AddHeader("Authorization", $"Bearer {_token.AccessToken}");
        var response = _token.getClient().Execute(request);
        return response.StatusCode == HttpStatusCode.NoContent ? true : false;
    }
}
