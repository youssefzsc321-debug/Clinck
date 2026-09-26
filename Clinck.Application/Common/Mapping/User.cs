using AutoMapper;
using Clinck.Application.ViewModels.UserVms;
using Clinck.Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.Common.Mapping
{
    public class User:Profile
    {
        public User()
        {
            CreateMap<AppUser, UserVm>();

            CreateMap<UserForm, AppUser>()
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                .ReverseMap();
        }
    }
}
