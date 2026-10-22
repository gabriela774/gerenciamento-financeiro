namespace GerenciamentoFinanceiro.Api.Dtos;

public record SaldoDto(decimal TotalEntradas, decimal TotalSaidas, decimal Saldo);

public record ResumoMensalDto(
    int Ano,
    int Mes,
    decimal TotalEntradas,
    decimal TotalSaidas,
    decimal Resultado,
    string Situacao);

public record MesDisponivelDto(int Ano, int Mes);