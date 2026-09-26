using AutoMapper;
using Clinck.Application.ViewModels.PatientVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.Common.Mapping.Patients
{
    public class MappingPatients : Profile
    {
        public MappingPatients()
        {
            CreateMap<Clinck.Domain.Enities.Patient, GetPatients>();

            CreateMap<FormVm, Clinck.Domain.Enities.Patient>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Clinck.Domain.Enities.Patient, DrawPatientVm>();

            CreateMap<Clinck.Domain.Enities.Patient, FormVm>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}