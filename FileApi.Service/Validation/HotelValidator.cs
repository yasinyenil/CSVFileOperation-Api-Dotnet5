using FileApi.Models;
using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileApi.Service.Validation
{
    public class HotelValidator : AbstractValidator<Hotel>
    {
        private const string PatternForPhone = @"(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2‌​-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9]‌​[02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})";
        private const string PatternForPhoneTR = @"/^[+]*[0-9]*[ ]{0,1}[(]{0,1}[ ]{0,1}[0-9]{1,3}[ ]{0,1}[)]{0,1}[ ]{0,1}[0-9]{1,3}[ ]{0,1}[0-9]{2}[ ]{0,1}[0-9]{2}[ ]{0,1}[-\.\/]{0,1}[ ]{0,1}[0-9]{1,5}$/g";
        public HotelValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage("{PropertyName} should not be empty")
                .Length(2, 150).WithMessage("{PropertyName} should not be less than 2 character and more than 100")
                .Must(BeValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage("{PropertyName} should not be empty")
                .Length(2, 80).WithMessage("{PropertyName} should not be less than 2 character and more than 60")
                .Must(BeValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(x => x.Address)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} should not be empty")
                .MaximumLength(300).WithMessage("{PropertyName} line should be maximum 300 letter");

            RuleFor(x => x.Phone)
               .Cascade(CascadeMode.Stop)
               .NotNull().WithMessage("{PropertyName} should not be empty")
               .MaximumLength(22).WithMessage("{PropertyName} line should be maximum 15 number")
               .Must(BeValidPhone).WithMessage("Invalid Phone number");

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
            name = name.Replace(" ", "");
            bool result = name.All(Char.IsLetter);
            return result;
        }

        protected bool BeValidPhone(string phone)
        {
            phone = phone.TrimStart('+');
            phone = phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ","");

            //Regex regex = new(pattern: PatternForPhoneTR);
            //regex.Matches(phone).Any();
            return phone.All(Char.IsNumber);
        }

    }
}
