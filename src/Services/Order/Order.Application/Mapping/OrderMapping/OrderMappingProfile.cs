using System.Linq;
using AutoMapper;
using Order.Application.Dtos;
using Order.Application.Features.Commands.Orders.CreateOrderCommand;
using Order.Application.Features.Queries.ViewModels;
using Order.Domain.AggregateModels.OrderAggregate;

namespace Order.Application.Mapping.OrderMapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Domain.AggregateModels.OrderAggregate.Order, CreateOrderCommand>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<Domain.AggregateModels.OrderAggregate.Order, OrderDetailViewModel>()
                .ForMember(x => x.City, opt => opt.MapFrom(x => x.Address.City))
                .ForMember(x => x.AddressLine, opt => opt.MapFrom(x => x.Address.AddressLine))
                .ForMember(x => x.Zip, opt => opt.MapFrom(x => x.Address.Zip))
                .ForMember(x => x.District, opt => opt.MapFrom(x => x.Address.District))
                .ForMember(x => x.Date, opt => opt.MapFrom(x => x.OrderDate))
                .ForMember(x => x.OrderNumber, opt => opt.MapFrom(x => x.Id.ToString()))
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.OrderStatus.Name))
                .ForMember(x => x.Total, opt => opt.MapFrom(x => x.OrderItems.Sum(x => x.Units * x.UnitPrice)));
        }
    }
}