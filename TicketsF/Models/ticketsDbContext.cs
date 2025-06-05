using Microsoft.EntityFrameworkCore;
namespace TicketsF.Models
{
    public class ticketsDbContext : DbContext
    {
        public ticketsDbContext(DbContextOptions<ticketsDbContext> options) : base(options)
        {
        }
        public DbSet<categoria> categorias { get; set; }
        public DbSet<estado> estados { get; set; }
        public DbSet<prioridad> prioridades { get; set; }
        public DbSet<roles_t> roles { get; set; }
        public DbSet<tickets> tickets { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<archivo_t> archivos { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
        public DbSet<historial> historial { get; set; }

    }
    

}

