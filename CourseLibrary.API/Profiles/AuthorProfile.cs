using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Model;
using Microsoft.AspNetCore.Routing.Constraints;

namespace CourseLibrary.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(des => des.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));
            CreateMap<AuthorForCreationDto, Author>();
        }
    }
}
