using Aeon.Application.DTOs;

namespace Aeon.Application.Interfaces;

public interface IKeymapService
{
    Task<string> GenerateKeymapAsync(KeyboardDto dto, CancellationToken ct = default);
    Task<Guid> StoreKeymapAsync(KeyboardDto dto, CancellationToken ct = default);
    Task<KeyboardDto> GetKeymapAsync(Guid id, CancellationToken ct = default);
}