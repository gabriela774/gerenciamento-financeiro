using System.ComponentModel.DataAnnotations;

namespace GerenciamentoFinanceiro.Api.Models;

public class Categoria
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
}