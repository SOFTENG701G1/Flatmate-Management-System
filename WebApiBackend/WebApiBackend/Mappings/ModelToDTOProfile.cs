using AutoMapper;
using WebApiBackend.Dto;
using WebApiBackend.Model;

namespace WebApiBackend.Mappings
{
    public class ModelToDTOProfile: Profile
    {
        // Mapper class to user AutoMapper
        public ModelToDTOProfile()
        {
            CreateMap<Flat, FlatDTO>();
            CreateMap<FlatDTO, Flat>();
            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentDTO, Payment>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Schedule, ScheduleDTO>();
            CreateMap<ScheduleDTO, Schedule>();
        }
    }
}
