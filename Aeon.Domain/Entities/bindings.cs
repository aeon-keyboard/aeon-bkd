namespace Aeon.Domain.Entities;

public abstract class Binding
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Position Position { get; set; } = null!;

    public abstract string GenerateZmkCode();
}

public record Position(int Row, int Col);

public class SimpleKeyBinding : Binding
{
    public string KeyCode { get; set; } = string.Empty;

    public override string GenerateZmkCode()
    {
        return $"&kp {KeyCode}";
    }
}

public class MacroBinding : Binding
{
    public string MacroName { get; set; } = string.Empty;
    public List<string> KeySequence { get; set; } = new();

    public override string GenerateZmkCode()
    {
        return $"&macro_{MacroName}";
    }
}

public class ModifierKeyBinding : Binding
{
    public string Modifier { get; set; } = string.Empty;
    public string KeyCode { get; set; } = string.Empty;

    public override string GenerateZmkCode()
    {
        return $"&mt {Modifier} {KeyCode}";
    }
}

public class LayerToggleBinding : Binding
{
    public int TargetLayerIndex { get; set; }

    public override string GenerateZmkCode()
    {
        return $"&to {TargetLayerIndex}";
    }
}

public class MomentaryLayerBinding : Binding
{
    public int TargetLayerIndex { get; set; }

    public override string GenerateZmkCode()
    {
        return $"&mo {TargetLayerIndex}";
    }
}