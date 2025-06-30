using BlazorSchulung2025.Models;
using FluentValidation;

namespace BlazorSchulung2025.Validators;

public class UserInfoValidator : AbstractValidator<UserInfo>
{
    public UserInfoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Benutzername ist erforderlich")
            .MaximumLength(4).WithMessage("Username zu lang (max. 4 Zeichen)");

        RuleFor(x => x.Email)
            .MaximumLength(40).WithMessage("E-Mailadresse zu lang (max. 40 Zeichen)")
            .EmailAddress().WithMessage("Das ist keine E-Mailadresse");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Vorname ist erforderlich");
        
        RuleFor(x => x.Birthday)
            .LessThan(DateTime.Now).WithMessage("Geburtstag muss in der Vergangenheit liegen");
    }
    
}