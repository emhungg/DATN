using AutoMapper;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Infrastructure.Extensions
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<UserDetail, UserRegisterRequestModel>();
            CreateMap<AddressUser, AddressUserDataModel>();
        }

    }
}
