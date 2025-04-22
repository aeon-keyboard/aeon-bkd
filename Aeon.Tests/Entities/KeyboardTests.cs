using Aeon.Domain.Entities;

namespace Aeon.Tests.Entities
{
    public class KeyboardTests
    {
        [Fact]
        public void Constructor_ShouldInitializeDefaultProperties()
        {
            // Arrange & Act
            var keyboard = new Keyboard();

            // Assert
            Assert.NotEqual(Guid.Empty, keyboard.Id);
            Assert.Equal(string.Empty, keyboard.Name);
            Assert.Equal(string.Empty, keyboard.Description);
            Assert.Equal(0, keyboard.RowCount);
            Assert.Equal(0, keyboard.ColCount);
            Assert.Empty(keyboard.Layers);
        }

        [Fact]
        public void AddLayer_ShouldAddLayerToCollection()
        {
            // Arrange
            var keyboard = new Keyboard();
            var layer = new Layer { Name = "Test Layer" };

            // Act
            keyboard.AddLayer(layer);

            // Assert
            Assert.Single(keyboard.Layers);
            Assert.Contains(layer, keyboard.Layers);
        }

        [Fact]
        public void RemoveLayer_WhenLayerExists_ShouldRemoveLayer()
        {
            // Arrange
            var keyboard = new Keyboard();
            var layer = new Layer { Name = "Test Layer" };
            keyboard.AddLayer(layer);

            // Act
            keyboard.RemoveLayer(layer.Id);

            // Assert
            Assert.Empty(keyboard.Layers);
        }

        [Fact]
        public void RemoveLayer_WhenLayerDoesNotExist_ShouldNotChangeCollection()
        {
            // Arrange
            var keyboard = new Keyboard();
            var layer = new Layer { Name = "Test Layer" };
            keyboard.AddLayer(layer);

            // Act
            keyboard.RemoveLayer(Guid.NewGuid());

            // Assert
            Assert.Single(keyboard.Layers);
            Assert.Contains(layer, keyboard.Layers);
        }
    }
}