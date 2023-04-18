using AutoMapper;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using MentalHealthApp.Common.DTOs;

namespace MentalHealthApp.WebApp.Code.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<EditUserDto, UserVM>();
            CreateMap<UserVM, EditUserDto>();
            CreateMap<DoctorVM, EditDoctorDto>();
            CreateMap<EditDoctorDto, DoctorVM>();
            CreateMap<ProgramareProfilMedicDto, AppointmentsDoctorProfileVM>();
            CreateMap<AppointmentsDoctorProfileVM, ProgramareProfilMedicDto>();
            CreateMap<AcceptedAppointmentsDto, AcceptedAppointmentsVM>();
            CreateMap<AcceptedAppointmentsVM, AcceptedAppointmentsDto>();

        }
    }
}
