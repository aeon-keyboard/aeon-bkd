using Aeon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aeon.Infra.Data;

public class KeymapDbContext : DbContext
{
    public KeymapDbContext(DbContextOptions<KeymapDbContext> opts) : base(opts) { }

    public DbSet<Keyboard> Keyboards => Set<Keyboard>();
}