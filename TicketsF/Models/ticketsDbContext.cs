using Microsoft.EntityFrameworkCore;

namespace TicketsF.Models
{
    public class ticketsDbContext : DbContext
    {
        public ticketsDbContext(DbContextOptions<ticketsDbContext> options) : base(options)
        {
        }

        public DbSet<roles_t> roles { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<categoria> categorias { get; set; }
        public DbSet<prioridad> prioridades { get; set; }
        public DbSet<estado> estados { get; set; }
        public DbSet<tickets> tickets { get; set; }
        public DbSet<archivo_t> archivos { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
        public DbSet<historial> historial { get; set; }
        public DbSet<notificaciones> notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<usuarios>()
                .HasOne(u => u.roles)
                .WithMany()
                .HasForeignKey(u => u.id_roles)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<roles_t>()
                .ToTable("roles_t")
                .HasKey(r => r.id_roles);

            
            modelBuilder.Entity<usuarios>()
                .ToTable("usuarios")
                .HasKey(u => u.id_usuario);
        }
    }
}
