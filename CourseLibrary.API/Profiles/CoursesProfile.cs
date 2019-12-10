using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Model;

namespace CourseLibrary.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseDto>();
        }
    }
}
