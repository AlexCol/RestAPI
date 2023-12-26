using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Flunt.Notifications;
using RestAPI.Hypermedia;
using RestAPI.Hypermedia.Abstract;


public class PersonVO : ISupporstHypermedia
{
    [JsonPropertyName("codigo")]
    public long Id { get; set; }

    [JsonPropertyName("nome")]
    public string FirstName { get; set; }

    [JsonPropertyName("sobrenome")]
    public string LastName { get; set; }

    [JsonPropertyName("endereco")]
    public string Address { get; set; }

    public string Gender { get; set; }
    public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
}


