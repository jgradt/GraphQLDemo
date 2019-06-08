using AutoMapper;
using FluentValidation;
using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public int UnitPrice { get; set; }
        public bool Discontinued { get; set; }

    }

    // Fluent Validation
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.ProductName).NotNull().Length(1, 50);
            RuleFor(x => x.SupplierId).NotNull().GreaterThan(0);
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        }
    }

    // AutoMapper
    public class ProductAutoMapProfile : Profile
    {
        public ProductAutoMapProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForSourceMember(m => m.CreatedDate, opt => opt.Ignore())
                .ForSourceMember(m => m.LastUpdatedDate, opt => opt.Ignore())
                .ForSourceMember(m => m.Supplier, opt => opt.Ignore())
                .ReverseMap();
        }

    }
}
