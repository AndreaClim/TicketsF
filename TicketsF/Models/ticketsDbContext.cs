using Microsoft.EntityFrameworkCore;

namespace TicketsF.Models
{
    public class ticketsDbContext : DbContext
    {
        public ticketsDbContext(DbContextOptions<ticketsDbContext> options) : base(options)
        {
        }

        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<categoria> categoria { get; set; }
        public DbSet<prioridad> prioridad { get; set; }
        public DbSet<estado> estado { get; set; }
        public DbSet<tickets> tickets { get; set; }
        public DbSet<archivo_t> archivos_t { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
        public DbSet<historial> historial { get; set; }
        public DbSet<notificaciones> notificaciones { get; set; }
    }
}
