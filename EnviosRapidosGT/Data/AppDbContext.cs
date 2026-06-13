using EnviosRapidosGT.Models;
using Microsoft.EntityFrameworkCore;

namespace EnviosRapidosGT.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Envio> Envios => Set<Envio>();

    public DbSet<Cliente> Clientes => Set<Cliente>();

    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();
}