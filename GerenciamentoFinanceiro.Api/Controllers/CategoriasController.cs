[HttpPost]
public async Task<ActionResult<Categoria>> Criar(Categoria categoria)
{
    var existe = await _context.Categorias
        .AnyAsync(c => c.Nome.ToLower() == categoria.Nome.ToLower());
    if (existe)
        return Conflict("Já existe uma categoria com esse nome.");

    _context.Categorias.Add(categoria);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(Listar), new { id = categoria.Id }, categoria);
}