

using Microsoft.EntityFrameworkCore;
using puc.Models;

namespace puc.Context
{
#pragma warning disable CS1591
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Consumo> Consumos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
         public DbSet<VeiculoUsuarios> VeiculosUsuarios { get; set; }

        //fluent api
        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Consumo>()
            .Property(p => p.Valor)
            .HasColumnType("decimal(18,2)") // Define o tipo decimal com precisão e escala
            .IsRequired();


            //VeiculoUsuarios tem chave composta de 2
            mb.Entity<VeiculoUsuarios>()
            .HasKey(c => new { c.VeiculoId, c.UsuarioId });

            mb.Entity<VeiculoUsuarios>()
            .HasOne(c => c.Veiculo).WithMany(c => c.Usuarios)
            .HasForeignKey(c => c.VeiculoId);


            mb.Entity<VeiculoUsuarios>()
           .HasOne(c => c.Usuario).WithMany(c => c.Veiculos)
           .HasForeignKey(c => c.UsuarioId);


        }

    }
}