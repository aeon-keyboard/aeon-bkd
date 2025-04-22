using Aeon.Domain.Entities;

namespace Aeon.Tests.Entities
{
    public class LayerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeDefaultProperties()
        {
            // Arrange & Act
            var layer = new Layer();

            // Assert
            Assert.NotEqual(Guid.Empty, layer.Id);
            Assert.Equal("Base", layer.Name);
            Assert.Equal(0, layer.Index);
            Assert.Empty(layer.Bindings);
        }

        [Fact]
        public void AddBinding_ShouldAddBindingToCollection()
        {
            // Arrange
            var layer = new Layer();
            var binding = new SimpleKeyBinding
            {
                Position = new Position(0, 0),
                KeyCode = "A"
            };

            // Act
            layer.AddBinding(binding);

            // Assert
            Assert.Single(layer.Bindings);
            Assert.Contains(binding, layer.Bindings);
        }

        [Fact]
        public void AddBinding_WhenPositionConflicts_ShouldReplaceOldBinding()
        {
            // Arrange
            var layer = new Layer();
            var binding1 = new SimpleKeyBinding
            {
                Position = new Position(0, 0),
                KeyCode = "A"
            };
            var binding2 = new SimpleKeyBinding
            {
                Position = new Position(0, 0),
                KeyCode = "B"
            };

            // Act
            layer.AddBinding(binding1);
            layer.AddBinding(binding2);

            // Assert
            Assert.Single(layer.Bindings);
            Assert.Contains(binding2, layer.Bindings);
            Assert.DoesNotContain(binding1, layer.Bindings);
        }

        [Fact]
        public void RemoveBinding_WhenBindingExists_ShouldRemoveBinding()
        {
            // Arrange
            var layer = new Layer();
            var binding = new SimpleKeyBinding
            {
                Position = new Position(1, 2),
                KeyCode = "A"
            };
            layer.AddBinding(binding);

            // Act
            layer.RemoveBinding(1, 2);

            // Assert
            Assert.Empty(layer.Bindings);
        }

        [Fact]
        public void RemoveBinding_WhenBindingDoesNotExist_ShouldNotChangeCollection()
        {
            // Arrange
            var layer = new Layer();
            var binding = new SimpleKeyBinding
            {
                Position = new Position(1, 2),
                KeyCode = "A"
            };
            layer.AddBinding(binding);

            // Act
            layer.RemoveBinding(0, 0); // Posição diferente

            // Assert
            Assert.Single(layer.Bindings);
            Assert.Contains(binding, layer.Bindings);
        }
    }
}