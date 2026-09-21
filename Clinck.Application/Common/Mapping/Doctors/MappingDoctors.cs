using AutoMapper;
using Clinck.Application.ViewModels.Doctors;
using Clinck.Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.Common.Mapping.Doctors
{
    public class MappingDoctors:Profile
    {
        public MappingDoctors()
        {
            CreateMap<Doctor, GetDoctorsVm>()
                .ForMember(dest=>dest.DepartmentName,opt=>opt.MapFrom(src=>src.Department.Name));

            CreateMap<FormVm, Doctor>().ReverseMap();
        }
    }
}
