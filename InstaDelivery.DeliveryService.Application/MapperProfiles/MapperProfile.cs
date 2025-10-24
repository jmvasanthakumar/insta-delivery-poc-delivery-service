using AutoMapper;
using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Proxy.Response;

namespace InstaDelivery.DeliveryService.Application.MapperProfiles;

internal class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<DeliveryAgent, DeliveryAgentDto>().ReverseMap();
        CreateMap<CreateDeliveryAgentDto, DeliveryAgent>().ReverseMap();

        CreateMap<AvailableOrder, AvailableOrderDto>().ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
