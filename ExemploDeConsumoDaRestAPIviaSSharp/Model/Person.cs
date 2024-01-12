using ExemploDeConsumoDaRestAPIviaSSharp.Model;

public class Person
{
    public long Codigo { get; set; }

    public string Nome { get; set; }

    public string Sobrenome { get; set; }

    public string Endereco { get; set; }

    public string Gender { get; set; }

    public bool Enabled { get; set; }

    public List<Link> Links { get; set; }


}


