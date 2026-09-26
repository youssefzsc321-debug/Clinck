using AutoMapper;
using Clinck.Application.ViewModels.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.Common.Mapping.Appointment
{
    public class MappingAppointment:Profile
    {
        public MappingAppointment()
        {
            CreateMap<Clinck.Domain.Enities.Appointment ,GetApointments>()
                .ForMember(dest=>dest.Doctor,opt=>opt.MapFrom(src=>$"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ForMember(dest=>dest.Department,opt=>opt.MapFrom(src=>src.Department.Name))
                .ForMember(dest => dest.AppointmentState, opt => opt.MapFrom(src => src.AppointmentState));

        }
    }
}
