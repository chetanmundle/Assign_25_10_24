using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models.Patient
{
    internal class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty().LessThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.ContactNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.StateId).NotEmpty();
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.BloodGroup).NotEmpty();
            RuleFor(x => x.IsChecked).NotEmpty();
            RuleFor(x => x.CreatedBy).NotEmpty();
        }
    }
}
