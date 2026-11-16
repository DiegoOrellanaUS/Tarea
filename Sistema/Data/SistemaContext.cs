using Microsoft.EntityFrameworkCore;

public class SistemaContext(DbContextOptions<SistemaContext> options) : DbContext(options)
{
    public DbSet<Sistema.Dominio.Persona> Persona { get; set; } = default!;
}
