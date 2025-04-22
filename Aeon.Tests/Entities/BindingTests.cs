using Aeon.Domain.Entities;

namespace Aeon.Tests.Entities
{
    public class BindingTests
    {
        [Fact]
        public void SimpleKeyBinding_GenerateZmkCode_ShouldGenerateCorrectFormat()
        {
            // Arrange
            var binding = new SimpleKeyBinding
            {
                Position = new Position(0, 0),
                KeyCode = "A"
            };

            // Act
            var result = binding.GenerateZmkCode();

            // Assert
            Assert.Equal("&kp A", result);
        }

        [Fact]
        public void MacroBinding_GenerateZmkCode_ShouldGenerateCorrectFormat()
        {
            // Arrange
            var binding = new MacroBinding
            {
                Position = new Position(0, 0),
                MacroName = "my_macro",
                KeySequence = new List<string> { "A", "B", "C" }
            };

            // Act
            var result = binding.GenerateZmkCode();

            // Assert
            Assert.Equal("&macro_my_macro", result);
        }

        [Fact]
        public void ModifierKeyBinding_GenerateZmkCode_ShouldGenerateCorrectFormat()
        {
            // Arrange
            var binding = new ModifierKeyBinding
            {
                Position = new Position(0, 0),
                Modifier = "LSHIFT",
                KeyCode = "A"
            };

            // Act
            var result = binding.GenerateZmkCode();

            // Assert
            Assert.Equal("&mt LSHIFT A", result);
        }

        [Fact]
        public void LayerToggleBinding_GenerateZmkCode_ShouldGenerateCorrectFormat()
        {
            // Arrange
            var binding = new LayerToggleBinding
            {
                Position = new Position(0, 0),
                TargetLayerIndex = 2
            };

            // Act
            var result = binding.GenerateZmkCode();

            // Assert
            Assert.Equal("&to 2", result);
        }

        [Fact]
        public void MomentaryLayerBinding_GenerateZmkCode_ShouldGenerateCorrectFormat()
        {
            // Arrange
            var binding = new MomentaryLayerBinding
            {
                Position = new Position(0, 0),
                TargetLayerIndex = 3
            };

            // Act
            var result = binding.GenerateZmkCode();

            // Assert
            Assert.Equal("&mo 3", result);
        }

        [Fact]
        public void Position_EqualityCheck_ShouldWorkAsExpected()
        {
            // Arrange
            var pos1 = new Position(1, 2);
            var pos2 = new Position(1, 2);
            var pos3 = new Position(2, 1);

            // Act & Assert
            Assert.Equal(pos1, pos2);
            Assert.NotEqual(pos1, pos3);
        }
    }
}