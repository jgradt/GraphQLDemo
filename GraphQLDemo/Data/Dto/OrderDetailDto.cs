using AutoMapper;
using FluentValidation;
using GraphQLDemo.Data.Entities;

namespace GraphQLDemo.Data.Dto
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // Fluent Validation
    public class OrderDetailDtoValidator : AbstractValidator<OrderDetailDto>
    {
        public OrderDetailDtoValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }

    // AutoMapper
    public class OrderDetailAutoMapProfile : Profile
    {
        public OrderDetailAutoMapProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForSourceMember(m => m.CreatedDate, opt => opt.Ignore())
                .ForSourceMember(m => m.LastUpdatedDate, opt => opt.Ignore())
                .ForSourceMember(m => m.Order, opt => opt.Ignore())
                .ForSourceMember(m => m.Product, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
