using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciamentoFinanceiro.Api.Data;
using GerenciamentoFinanceiro.Api.Dtos;
using GerenciamentoFinanceiro.Api.Models;

namespace GerenciamentoFinanceiro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LancamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public LancamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Lancamento>>> Listar()
    {
        var lancamentos = await _context.Lancamentos
            .Include(l => l.Categoria)
            .OrderByDescending(l => l.Data)
            .ToListAsync();
        return Ok(lancamentos);
    }

    [HttpPost]
    public async Task<ActionResult<Lancamento>> Criar(LancamentoCreateDto dto)
    {
        var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
        if (!categoriaExiste)
            return BadRequest("Categoria não encontrada.");

        var lancamento = new Lancamento
        {
            Data = dto.Data,
            Tipo = dto.Tipo,
            CategoriaId = dto.CategoriaId,
            Origem = dto.Origem,
            Valor = dto.Valor,
            Observacao = dto.Observacao,
            CriadoEm = DateTimeOffset.UtcNow
        };

        _context.Lancamentos.Add(lancamento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Listar), new { id = lancamento.Id }, lancamento);
    }
}