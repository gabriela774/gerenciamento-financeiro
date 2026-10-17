using Microsoft.EntityFrameworkCore;
using GerenciamentoFinanceiro.Api.Models;

namespace GerenciamentoFinanceiro.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Lancamento> Lancamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nome)
            .IsUnique();

        modelBuilder.Entity<Lancamento>()
            .HasOne(l => l.Categoria)
            .WithMany()
            .HasForeignKey(l => l.CategoriaId)
            .IsRequired();

        modelBuilder.Entity<Lancamento>()
            .ToTable(t => t.HasCheckConstraint("CK_Lancamento_Tipo", "\"Tipo\" IN ('entrada', 'saida')"));

        modelBuilder.Entity<Lancamento>()
            .ToTable(t => t.HasCheckConstraint("CK_Lancamento_Valor_Positivo", "\"Valor\" > 0"));
    }
}