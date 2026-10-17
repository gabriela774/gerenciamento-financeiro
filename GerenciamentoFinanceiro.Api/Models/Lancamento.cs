namespace GerenciamentoFinanceiro.Api.Models;

public class Lancamento
{
    public Guid Id {get;set;}
    public DateOnly Data {get;set;}
    public string Tipo {get;set;} = string.Empty;
    public Guid CategoriaId {get;set;}
    public Categoria? Categoria {get;set;}
    public string? Origem {get;set;}
    public Decimal Valor {get;set;}
    public string? Observacao {get;set;}
    public DateTimeOffset CriadoEm {get;set;}
}