using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciamentoFinanceiro.Api.Data;
using GerenciamentoFinanceiro.Api.Dtos;

namespace GerenciamentoFinanceiro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumoController : ControllerBase
{
    private readonly AppDbContext _context;

    public ResumoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("saldo")]
    public async Task<ActionResult<SaldoDto>> Saldo()
    {
        var entradas = await _context.Lancamentos
            .Where(l => l.Tipo == "entrada")
            .SumAsync(l => l.Valor);

        var saidas = await _context.Lancamentos
            .Where(l => l.Tipo == "saida")
            .SumAsync(l => l.Valor);

        return Ok(new SaldoDto(entradas, saidas, entradas - saidas));
    }

    [HttpGet("mensal")]
    public async Task<ActionResult<ResumoMensalDto>> Mensal(int? ano, int? mes)
    {
        var fuso = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        var hoje = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fuso);

        var anoRef = ano ?? hoje.Year;
        var mesRef = mes ?? hoje.Month;

        if (mesRef < 1 || mesRef > 12)
            return BadRequest("Mês deve estar entre 1 e 12.");
        if (anoRef < 2000 || anoRef > 2100)
            return BadRequest("Ano fora do intervalo válido.");

        var inicio = new DateOnly(anoRef, mesRef, 1);
        var fim = inicio.AddMonths(1);

        var doMes = _context.Lancamentos.Where(l => l.Data >= inicio && l.Data < fim);

        var entradas = await doMes.Where(l => l.Tipo == "entrada").SumAsync(l => l.Valor);
        var saidas = await doMes.Where(l => l.Tipo == "saida").SumAsync(l => l.Valor);

        var resultado = entradas - saidas;
        var situacao = resultado > 0 ? "positivo" : resultado < 0 ? "negativo" : "zerado";

        return Ok(new ResumoMensalDto(anoRef, mesRef, entradas, saidas, resultado, situacao));
    }

    [HttpGet("meses")]
    public async Task<ActionResult<List<MesDisponivelDto>>> Meses()
    {
        var meses = await _context.Lancamentos
            .GroupBy(l => new { l.Data.Year, l.Data.Month })
            .OrderByDescending(g => g.Key.Year)
            .ThenByDescending(g => g.Key.Month)
            .Select(g => new MesDisponivelDto(g.Key.Year, g.Key.Month))
            .ToListAsync();

        return Ok(meses);
    }
}