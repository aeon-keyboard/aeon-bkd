using Aeon.Application.DTOs;
using FluentValidation;

namespace Aeon.API.Validators;

public class KeyboardDtoValidator : AbstractValidator<KeyboardDto>
{
    public KeyboardDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do teclado é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.RowCount)
            .GreaterThan(0).WithMessage("O número de linhas deve ser maior que zero");

        RuleFor(x => x.ColumnCount)
            .GreaterThan(0).WithMessage("O número de colunas deve ser maior que zero");

        RuleFor(x => x.Layers)
            .NotEmpty().WithMessage("O teclado deve ter pelo menos uma camada")
            .ForEach(layer => layer.SetValidator(new LayerDtoValidator()));
    }
}

public class LayerDtoValidator : AbstractValidator<LayerDto>
{
    public LayerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da camada é obrigatório");

        RuleFor(x => x.Index)
            .GreaterThanOrEqualTo(0).WithMessage("O índice da camada deve ser maior ou igual a zero");

        RuleForEach(x => x.Bindings)
            .SetInheritanceValidator(v =>
            {
                v.Add(new SimpleKeyBindingDtoValidator());
                v.Add(new MacroBindingDtoValidator());
                v.Add(new ModifierKeyBindingDtoValidator());
                v.Add(new LayerToggleBindingDtoValidator());
                v.Add(new MomentaryLayerBindingDtoValidator());
            });
    }
}

public class BindingDtoValidator<T> : AbstractValidator<T> where T : BindingDto
{
    public BindingDtoValidator()
    {
        RuleFor(x => x.Row)
            .GreaterThanOrEqualTo(0).WithMessage("A linha deve ser maior ou igual a zero");

        RuleFor(x => x.Col)
            .GreaterThanOrEqualTo(0).WithMessage("A coluna deve ser maior ou igual a zero");

        RuleFor(x => x.BindingType)
            .NotEmpty().WithMessage("O tipo de binding é obrigatório");
    }
}

public class SimpleKeyBindingDtoValidator : BindingDtoValidator<SimpleKeyBindingDto>
{
    public SimpleKeyBindingDtoValidator()
    {
        RuleFor(x => x.KeyCode)
            .NotEmpty().WithMessage("O código da tecla é obrigatório");
    }
}

public class MacroBindingDtoValidator : BindingDtoValidator<MacroBindingDto>
{
    public MacroBindingDtoValidator()
    {
        RuleFor(x => x.MacroName)
            .NotEmpty().WithMessage("O nome da macro é obrigatório");

        RuleFor(x => x.KeySequence)
            .NotEmpty().WithMessage("A sequência de teclas não pode estar vazia");
    }
}

public class ModifierKeyBindingDtoValidator : BindingDtoValidator<ModifierKeyBindingDto>
{
    public ModifierKeyBindingDtoValidator()
    {
        RuleFor(x => x.Modifier)
            .NotEmpty().WithMessage("O modificador é obrigatório");

        RuleFor(x => x.KeyCode)
            .NotEmpty().WithMessage("O código da tecla é obrigatório");
    }
}

public class LayerToggleBindingDtoValidator : BindingDtoValidator<LayerToggleBindingDto>
{
    public LayerToggleBindingDtoValidator()
    {
        RuleFor(x => x.TargetLayerIndex)
            .GreaterThanOrEqualTo(0).WithMessage("O índice da camada alvo deve ser maior ou igual a zero");
    }
}

public class MomentaryLayerBindingDtoValidator : BindingDtoValidator<MomentaryLayerBindingDto>
{
    public MomentaryLayerBindingDtoValidator()
    {
        RuleFor(x => x.TargetLayerIndex)
            .GreaterThanOrEqualTo(0).WithMessage("O índice da camada alvo deve ser maior ou igual a zero");
    }
}