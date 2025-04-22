namespace Aeon.Domain.Entities;

public class Keyboard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RowCount { get; set; }
    public int ColCount { get; set; }
    public List<Layer> Layers { get; private set; } = new();

    public void AddLayer(Layer layer) => Layers.Add(layer);

    public void RemoveLayer(Guid layerId)
    {
        var layer = Layers.FirstOrDefault(lyr => lyr.Id == layerId);
        if (layer != null)
        {
            Layers.Remove(layer);
        }
    }
}