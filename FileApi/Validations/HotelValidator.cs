using FileApi.Models;
using FluentValidation;
using System;
using System.Linq;

namespace FileApi.Validations
{
    public class HotelValidator : AbstractValidator<HotelDTO>
    {
        public HotelValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage("{PropertyName} should not be empty")
                .Length(2, 100).WithMessage("{PropertyName} should not be less than 2 character and more than 100")
                .Must(BeValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage("{PropertyName} should not be empty")
                .Length(2, 60).WithMessage("{PropertyName} should not be less than 2 character and more than 60")
                .Must(BeValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(x => x.Address)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} should not be empty")
                .MaximumLength(300).WithMessage("{PropertyName} line should be maximum 300 letter");

            RuleFor(x => x.Phone)
               .Cascade(CascadeMode.Stop)
               .NotNull().WithMessage("{PropertyName} should not be empty")
               .MaximumLength(15).WithMessage("{PropertyName} line should be maximum 15 number")
               .Matches(@"/^[+]*[0-9]*[ ]{0,1}[(]{0,1}[ ]{0,1}[0-9]{1,3}[ ]{0,1}[)]{0,1}[ ]{0,1}[0-9]{1,3}[ ]{0,1}[0-9]{2}[ ]{0,1}[0-9]{2}[ ]{0,1}[-\.\/]{0,1}[ ]{0,1}[0-9]{1,5}$/g").WithMessage("Invalid Phone number");

            RuleFor(x => x.Stars)
               .Cascade(CascadeMode.Stop)
               .NotNull().WithMessage("{PropertyName} should not be empty")
               .InclusiveBetween(0, 5).WithMessage("{PropertyName} should be between from 0 to 5");

            RuleFor(x => x.URI)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} should not be empty")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .MaximumLength(300).WithMessage("{PropertyName} line should be maximum 300 letter");
        }


        protected bool BeValidName(string name)
        {
            //name = name.Trim();
            //name = name.Replace("-", "");

            return name.All(Char.IsLetter);
        }

        protected bool BeValidPhone(string phone)
        {
            phone = phone.TrimStart('+');
            phone = phone.Replace("(", "").Replace(")","").Replace("-","");


            return phone.All(Char.IsNumber);
        }
    }
}
