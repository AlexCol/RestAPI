using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

public class MySqlContext : DbContext //IdentityDbContext<IdentityUser> =>esse para o caso de desejar que o aspnet crie as tabelas de identificações
{
    public MySqlContext() { }

    public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Claims> Claims { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Notification>(); //!para ifgnorar os erros se usar a função de notificação
        modelBuilder.Entity<Person>()
            .Property(p => p.FirstName).IsRequired();

        modelBuilder.Entity<Claims>()
            .Property("UserId")
            .IsRequired();

        modelBuilder.Entity<Claims>()
            .HasKey("UserId", "Key", "Value");
    }
}