using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Flunt.Notifications;


public class PersonVO
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
}


