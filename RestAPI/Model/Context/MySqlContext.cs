using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;

public class MySqlContext : DbContext
{
    public MySqlContext() { }

    public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Notification>(); //!para ifgnorar os erros se usar a função de notificação
        modelBuilder.Entity<Person>()
        .Property(p => p.FirstName).IsRequired();
    }
}