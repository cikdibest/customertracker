using AutoMapper;
using CustomerTracker.Web.Controllers;
using CustomerTracker.Web.Models;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;

namespace CustomerTracker.Web.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Map()
        {
            Mapper.CreateMap<Customer, CustomerDetailModel>()
                .ForMember(dest => dest.Id, src => src.MapFrom(q => q.Id))
                .ForMember(dest => dest.CustomerTitle, src => src.MapFrom(q => q.Title))
                .ForMember(dest => dest.CityName, src => src.MapFrom(q => q.City.Name))
                .ForMember(dest => dest.Explanation, src => src.MapFrom(q => q.Explanation))  ;

            Mapper.CreateMap<Communication, CommunicationModel>()
            .ForMember(dest => dest.FullName, src => src.MapFrom(q => q.FullName))
            .ForMember(dest => dest.Email, src => src.MapFrom(q => q.Email))
            .ForMember(dest => dest.GenderName, src => src.MapFrom(q => q.Gender.GetDescription()))
            .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(q => q.PhoneNumber))
            .ForMember(dest => dest.DepartmentName, src => src.MapFrom(q => q.Department.Name));

            Mapper.CreateMap<RemoteMachine, RemoteMachineModel>()
                .ForMember(dest => dest.Name, src => src.MapFrom(q => q.Name))
                .ForMember(dest => dest.Username, src => src.MapFrom(q => q.Username))
                .ForMember(dest => dest.RemoteConnectionType, src => src.MapFrom(q => q.RemoteConnectionType.GetDescription()))
                .ForMember(dest => dest.Password, src => src.MapFrom(q => q.Password))
                .ForMember(dest => dest.CustomerTitle, src => src.MapFrom(q => q.Customer.Title))
                .ForMember(dest => dest.Explanation, src => src.MapFrom(q => q.Explanation))
                .ForMember(dest => dest.RemoteAddress, src => src.MapFrom(q => q.RemoteAddress))
                .ForMember(dest => dest.LogoName, src => src.MapFrom(q => string.Format("{0}/remoteconnectiontype/{1}.jpg", "content/images", q.RemoteConnectionType.ToString())));

            Mapper.CreateMap<Product, ProductModel>()
           .ForMember(dest => dest.Id, src => src.MapFrom(q => q.Id))
           .ForMember(dest => dest.Name, src => src.MapFrom(q => q.Name));
        }
    }
}