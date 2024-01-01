using System.ComponentModel.DataAnnotations.Schema;
using Flunt.Validations;

[Table("person")]
public class Person : BaseEntity
{
    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("gender")]
    public string Gender { get; set; }

    [Column("enabled")]
    public bool Enabled { get; set; }

    public Person() { }

    public Person(long id, string firstName, string lastName, string address, string gender, bool enabled) : this(firstName, lastName, address, gender, enabled)
    {
        Id = id;
    }

    public Person(string firstName, string lastName, string address, string gender, bool enabled)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Gender = gender;
        Enabled = enabled;
        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Person>()
            .IsNotNullOrEmpty(FirstName, "Name", "Nome tem que estar preenchido.")
            .IsGreaterOrEqualsThan(FirstName, 3, "Name", "Nome deve ter pelo menos tres caracteres.");
        AddNotifications(contract);
    }
}


