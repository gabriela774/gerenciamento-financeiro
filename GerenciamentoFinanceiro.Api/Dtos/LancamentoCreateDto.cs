using System.ComponentModel.DataAnnotations;

namespace GerenciamentoFinanceiro.Api.Dtos;

public class LancamentoCreateDto
{
    public DateOnly Data { get; set; }

    [Required]
    [RegularExpression("^(entrada|saida)$", ErrorMessage = "Tipo deve ser 'entrada' ou 'saida'.")]
    public string Tipo { get; set; } = string.Empty;

    public Guid CategoriaId { get; set; }

    [MaxLength(50)]
    public string? Origem { get; set; }

    [Range(0.01, 99999999.99, ErrorMessage = "Valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [MaxLength(500)]
    public string? Observacao { get; set; }
}