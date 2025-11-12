using Microsoft.EntityFrameworkCore;
using iTaskAPI.Models;

namespace iTaskAPI.Connection
{
    public class ConnectionDB : DbContext
    {
        public ConnectionDB(DbContextOptions<ConnectionDB> options) : base(options) { }
        
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Gestor> Gestores { get; set; }
        public DbSet<Programador> Programadores { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<TipoTarefa> TiposTarefa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índices únicos para Email e Username
            modelBuilder.Entity<Utilizador>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Utilizador>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Índice único para nome do TipoTarefa
            modelBuilder.Entity<TipoTarefa>()
                .HasIndex(t => t.Nome)
                .IsUnique();
            
            // Relação 1:1 entre Utilizador e Gestor
            modelBuilder.Entity<Gestor>()
                .HasOne(g => g.Utilizador)
                .WithOne() //Sem referência em Utilizador
                .HasForeignKey<Gestor>(g => g.IdUtilizador)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação 1:1 entre Utilizador e Programador
            modelBuilder.Entity<Programador>()
                .HasOne(p => p.Utilizador)
                .WithOne() // Sem referência em Utilizador
                .HasForeignKey<Programador>(p => p.IdUtilizador)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação 1 Gestor → N Programadores
            modelBuilder.Entity<Programador>()
                .HasOne(p => p.Gestor)
                .WithMany(g => g.Programadores)
                .HasForeignKey(p => p.IdGestor)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação 1 Gestor → N Tarefas
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Gestor)
                .WithMany(g => g.Tarefas)
                .HasForeignKey(t => t.IdGestor)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação 1 Programador → N Tarefas
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Programador)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(t => t.IdProgramador)
                .OnDelete(DeleteBehavior.Restrict);

            // Relação 1 TipoTarefa → N Tarefas
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.TipoTarefa)
                .WithMany(tt => tt.Tarefas)
                .HasForeignKey(t => t.IdTipoTarefa)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}