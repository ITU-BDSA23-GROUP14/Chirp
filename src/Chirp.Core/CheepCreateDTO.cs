using FluentValidation;

namespace Chirp.Core;

public class CheepCreateDTO
{
    public required string Text { get; set; }
    public required string Author { get; set; }
    public required string Email { get; set; }
}

public class CheepCreateValidator : AbstractValidator<CheepCreateDTO>
{
    public CheepCreateValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(50);
    }
}