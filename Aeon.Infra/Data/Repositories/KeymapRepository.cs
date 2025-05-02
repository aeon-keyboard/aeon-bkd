using Aeon.Domain.Entities;
using Aeon.Domain.Interfaces;
using Aeon.Infra.Data;
using Microsoft.EntityFrameworkCore;


namespace Aeon.Infra.Data.Repositories;

public class KeymapRepository : IKeymapRepository
{
    private readonly KeymapDbContext _ctx;
    public KeymapRepository(KeymapDbContext ctx) => _ctx = ctx;

    public async Task SaveAsync(Keyboard keyboard, CancellationToken ct = default)
    {
        _ctx.Keyboards.Add(keyboard);
        await _ctx.SaveChangesAsync(ct);
    }

    public Task<Keyboard?> GetAsync(Guid id, CancellationToken ct = default)
        => _ctx.Keyboards.Include(k => k.Layers).FirstOrDefaultAsync(k => k.Id == id, ct);
}
