using Microsoft.EntityFrameworkCore;

namespace ContactUsApi.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<ContactForm> ContactForms { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}