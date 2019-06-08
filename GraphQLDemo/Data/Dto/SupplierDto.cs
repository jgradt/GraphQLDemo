using AutoMapper;
using FluentValidation;
using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.Dto
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

    }

    // Fluent Validation
    public class SupplierDtoValidator : AbstractValidator<SupplierDto>
    {
        public SupplierDtoValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.CompanyName).NotNull().Length(1, 50);
            RuleFor(x => x.ContactName).NotNull().Length(1, 50);
            RuleFor(x => x.ContactTitle).NotNull().Length(1, 50);
            RuleFor(x => x.StreetAddress).NotNull().Length(1, 50);
            RuleFor(x => x.City).NotNull().Length(1, 50);
            RuleFor(x => x.State).NotNull().Length(2);
            RuleFor(x => x.PostalCode).NotNull().Length(5, 10);
            RuleFor(x => x.Phone).NotNull().Length(7, 20);
        }
    }

    // AutoMapper
    public class SupplierAutoMapProfile : Profile
    {
        public SupplierAutoMapProfile()
        {
            CreateMap<Supplier, SupplierDto>()
                .ForSourceMember(m => m.CreatedDate, opt => opt.Ignore())
                .ForSourceMember(m => m.LastUpdatedDate, opt => opt.Ignore())
                .ForSourceMember(m => m.Products, opt => opt.Ignore())
                .ReverseMap();
        }

    }
}
