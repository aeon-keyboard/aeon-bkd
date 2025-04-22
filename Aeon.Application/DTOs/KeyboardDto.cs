namespace Aeon.Application.DTOs;

public class KeyboardDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public List<LayerDto> Layers { get; set; } = new();
}

public class LayerDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Index { get; set; }
    public List<BindingDto> Bindings { get; set; } = new();
}

public abstract class BindingDto
{
    public string BindingType { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Col { get; set; }
}

public class SimpleKeyBindingDto : BindingDto
{
    public string KeyCode { get; set; } = string.Empty;
}

public class MacroBindingDto : BindingDto
{
    public string MacroName { get; set; } = string.Empty;
    public List<string> KeySequence { get; set; } = new();
}

public class ModifierKeyBindingDto : BindingDto
{
    public string Modifier { get; set; } = string.Empty;
    public string KeyCode { get; set; } = string.Empty;
}

public class LayerToggleBindingDto : BindingDto
{
    public int TargetLayerIndex { get; set; }
}

public class MomentaryLayerBindingDto : BindingDto
{
    public int TargetLayerIndex { get; set; }
}