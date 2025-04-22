using Aeon.Domain.Entities;

namespace Aeon.Domain.Interfaces;

public interface IKeymapRepository
{
    Task<Keyboard> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Keyboard>> GetAllAsync(CancellationToken ct = default);
    Task<Guid> SaveAsync(Keyboard keyboard, CancellationToken ct = default);
    Task UpdateAsync(Keyboard keyboard, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<Keyboard>> FindByNameAsync(string name, CancellationToken ct = default);
    Task<bool> KeyboardExistsAsync(string name, CancellationToken ct = default);
}