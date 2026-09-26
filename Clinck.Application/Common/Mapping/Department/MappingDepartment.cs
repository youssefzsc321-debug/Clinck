using AutoMapper;
using Clinck.Application.ViewModels.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.Common.Mapping.Department
{
    public class MappingDepartment:Profile
    {
        public MappingDepartment()
        {
            CreateMap<Clinck.Domain.Enities.Department, GetDepts>()
                .ForMember(dest=>dest.NumberOfDoctors,opt=>opt.MapFrom(src=>src.Doctors.Count()));

            CreateMap<Clinck.Domain.Enities.Department, FormVm>().ReverseMap();
        }
    }
}
