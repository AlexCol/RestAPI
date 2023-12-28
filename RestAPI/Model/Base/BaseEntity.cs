using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flunt.Notifications;

public class BaseEntity : Notifiable<Notification>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
}