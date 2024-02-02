using Microsoft.EntityFrameworkCore;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
    : base(dbContextOptions) { }

    public DbSet<Stock> Stocks { get; set; }

    public DbSet<Comment> Comments { get; set; }
}