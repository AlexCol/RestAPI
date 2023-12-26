using System.ComponentModel.DataAnnotations.Schema;
using Flunt.Notifications;

public class BaseEntity : Notifiable<Notification>
{
    [Column("id")]
    public long Id { get; set; }
}