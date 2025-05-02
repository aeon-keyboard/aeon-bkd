using Aeon.Application.DTOs;
using Aeon.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace aeon_bkd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeymapController : ControllerBase
{
    private readonly IKeymapService _keymapService;

    public KeymapController(IKeymapService keymapService)
    {
        _keymapService = keymapService;
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateKeymap([FromBody] KeyboardDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _keymapService.GenerateKeymapAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("store")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StoreKeymap([FromBody] KeyboardDto request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _keymapService.StoreKeymapAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetKeymap), new { id }, id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(KeyboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetKeymap(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var keymap = await _keymapService.GetKeymapAsync(id, cancellationToken);

            return Ok(keymap);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}