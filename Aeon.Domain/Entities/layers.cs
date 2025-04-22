namespace Aeon.Domain.Entities;

public class Layer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "Base";
    public int Index { get; set; }
    public List<Binding> Bindings { get; private set; } = new();

    public void AddBinding(Binding binding)
    {
        var existingBinding = Bindings.FirstOrDefault(b =>
            b.Position.Row == binding.Position.Row &&
            b.Position.Col == binding.Position.Col);

        if (existingBinding != null)
        {
            Bindings.Remove(existingBinding);
        }
        Bindings.Add(binding);
    }

    public void RemoveBinding(int row, int col)
    {
        var binding = Bindings.FirstOrDefault(bdg =>
            bdg.Position.Row == row &&
            bdg.Position.Col == col
        );

        if (binding != null)
        {
            Bindings.Remove(binding);
        }
    }

}