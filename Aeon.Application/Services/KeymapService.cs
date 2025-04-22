using Aeon.Application.DTOs;
using Aeon.Application.Interfaces;
using Aeon.Domain.Entities;
using Aeon.Domain.Interfaces;
using System.Text;

namespace Aeon.Application.Services;

public class KeymapService(IKeymapRepository repository) : IKeymapService
{
    public async Task<string> GenerateKeymapAsync(KeyboardDto dto, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            var sb = new StringBuilder();
            sb.AppendLine("/ {");
            sb.AppendLine("    keymap {");
            sb.AppendLine("        compatible = \"zmk,keymap\";");

            foreach (var layer in dto.Layers.OrderBy(l => l.Index))
            {
                sb.AppendLine($"        {layer.Name}_layer {layer.Index} {{");
                sb.AppendLine("            bindings = <");

                var rowGroups = layer.Bindings
                    .GroupBy(b => b.Row)
                    .OrderBy(g => g.Key);

                foreach (var rowGroup in rowGroups)
                {
                    sb.Append("                ");

                    var rowBindings = rowGroup
                        .OrderBy(b => b.Col)
                        .ToList();

                    for (int i = 0; i < rowBindings.Count; i++)
                    {
                        var bindingCode = GetBindingZmkCode(rowBindings[i]);
                        sb.Append(bindingCode);

                        if (i < rowBindings.Count - 1)
                        {
                            sb.Append(" ");
                        }
                    }

                    sb.AppendLine();
                }

                sb.AppendLine("            >;");
                sb.AppendLine("        };");
            }

            sb.AppendLine("    };");
            sb.AppendLine("};");

            return sb.ToString();
        }, ct);
    }


    private static string GetBindingZmkCode(BindingDto binding)
    {
        return binding switch
        {
            SimpleKeyBindingDto simple => $"&kp {simple.KeyCode}",
            MacroBindingDto macro => $"&macro_{macro.MacroName}",
            ModifierKeyBindingDto modifier => $"&mt {modifier.Modifier} {modifier.KeyCode}",
            LayerToggleBindingDto toggle => $"&to {toggle.TargetLayerIndex}",
            MomentaryLayerBindingDto momentary => $"&mo {momentary.TargetLayerIndex}",
            _ => "&none"
        };
    }

    public async Task<Guid> StoreKeymapAsync(KeyboardDto dto, CancellationToken ct = default)
    {
        var keyboard = new Keyboard
        {
            Name = dto.Name,
            Description = dto.Description,
            RowCount = dto.RowCount,
            ColCount = dto.ColumnCount
        };

        if (dto.Id.HasValue)
        {
            keyboard.Id = dto.Id.Value;
        }

        foreach (var layerDto in dto.Layers.OrderBy(l => l.Index))
        {
            var layer = new Layer
            {
                Name = layerDto.Name,
                Index = layerDto.Index
            };

            if (layerDto.Id.HasValue)
            {
                layer.Id = layerDto.Id.Value;
            }

            foreach (var bindingDto in layerDto.Bindings)
            {
                Binding binding = CreateBindingFromDto(bindingDto);
                layer.AddBinding(binding);
            }

            keyboard.AddLayer(layer);
        }

        if (dto.Id.HasValue)
        {
            await repository.UpdateAsync(keyboard, ct);
            return keyboard.Id;
        }
        else
        {
            return await repository.SaveAsync(keyboard, ct);
        }
    }

    private Binding CreateBindingFromDto(BindingDto dto)
    {
        var position = new Position(dto.Row, dto.Col);

        return dto switch
        {
            SimpleKeyBindingDto simple => new SimpleKeyBinding
            {
                Position = position,
                KeyCode = simple.KeyCode
            },
            MacroBindingDto macro => new MacroBinding
            {
                Position = position,
                MacroName = macro.MacroName,
                KeySequence = macro.KeySequence
            },
            ModifierKeyBindingDto modifier => new ModifierKeyBinding
            {
                Position = position,
                Modifier = modifier.Modifier,
                KeyCode = modifier.KeyCode
            },
            LayerToggleBindingDto toggle => new LayerToggleBinding
            {
                Position = position,
                TargetLayerIndex = toggle.TargetLayerIndex
            },
            MomentaryLayerBindingDto momentary => new MomentaryLayerBinding
            {
                Position = position,
                TargetLayerIndex = momentary.TargetLayerIndex
            },
            _ => throw new ArgumentException($"Unsupported binding type: {dto.GetType().Name}")
        };
    }

    public async Task<KeyboardDto> GetKeymapAsync(Guid id, CancellationToken ct = default)
    {
        var keyboard = await repository.GetByIdAsync(id, ct);
        if (keyboard == null)
        {
            throw new KeyNotFoundException($"Keyboard with ID {id} not found");
        }

        return MapToDto(keyboard);
    }

    private KeyboardDto MapToDto(Keyboard keyboard)
    {
        var dto = new KeyboardDto
        {
            Id = keyboard.Id,
            Name = keyboard.Name,
            Description = keyboard.Description,
            RowCount = keyboard.RowCount,
            ColumnCount = keyboard.ColCount,
            Layers = new List<LayerDto>()
        };

        foreach (var layer in keyboard.Layers.OrderBy(l => l.Index))
        {
            var layerDto = new LayerDto
            {
                Id = layer.Id,
                Name = layer.Name,
                Index = layer.Index,
                Bindings = new List<BindingDto>()
            };

            foreach (var binding in layer.Bindings)
            {
                var bindingDto = MapBindingToDto(binding);
                if (bindingDto != null)
                {
                    layerDto.Bindings.Add(bindingDto);
                }
            }

            dto.Layers.Add(layerDto);
        }

        return dto;
    }

    private BindingDto? MapBindingToDto(Binding binding)
    {
        BindingDto? dto = binding switch
        {
            SimpleKeyBinding simple => new SimpleKeyBindingDto
            {
                Row = simple.Position.Row,
                Col = simple.Position.Col,
                KeyCode = simple.KeyCode,
                BindingType = "SimpleKey"
            },
            MacroBinding macro => new MacroBindingDto
            {
                Row = macro.Position.Row,
                Col = macro.Position.Col,
                MacroName = macro.MacroName,
                KeySequence = macro.KeySequence,
                BindingType = "Macro"
            },
            ModifierKeyBinding modifier => new ModifierKeyBindingDto
            {
                Row = modifier.Position.Row,
                Col = modifier.Position.Col,
                Modifier = modifier.Modifier,
                KeyCode = modifier.KeyCode,
                BindingType = "Modifier"
            },
            LayerToggleBinding toggle => new LayerToggleBindingDto
            {
                Row = toggle.Position.Row,
                Col = toggle.Position.Col,
                TargetLayerIndex = toggle.TargetLayerIndex,
                BindingType = "LayerToggle"
            },
            MomentaryLayerBinding momentary => new MomentaryLayerBindingDto
            {
                Row = momentary.Position.Row,
                Col = momentary.Position.Col,
                TargetLayerIndex = momentary.TargetLayerIndex,
                BindingType = "MomentaryLayer"
            },
            _ => null
        };

        return dto;
    }
}