using System;
using System.Collections.Generic;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Model;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly ILogger<CoursesController> _logger;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper, ILogger<CoursesController> logger)
        {
            _courseLibraryRepository = courseLibraryRepository ?? 
                                       throw  new ArgumentNullException(nameof(_courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            try
            {
                if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
                return Ok(_courseLibraryRepository.GetCourses(authorId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
        }

        [HttpGet("{courseId}", Name= "GetCourseForAuthor")]
        public ActionResult<IEnumerable<CourseDto>> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            try
            {
                if (!_courseLibraryRepository.AuthorExists(authorId)) return NotFound();
                var courseByAuthor = _courseLibraryRepository.GetCourse(authorId, courseId);
                return Ok(_mapper.Map<CourseDto>(courseByAuthor));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
        }


        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreationDto course)
        {
            try
            {
                if (!_courseLibraryRepository.AuthorExists(authorId))
                {
                    return NotFound();
                }

                var courseEntity = _mapper.Map<Course>(course);
                _courseLibraryRepository.AddCourse(authorId, courseEntity);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
                return CreatedAtRoute("GetCourseForAuthor",
                    new
                    {
                        authorId = authorId,
                        courseId = courseToReturn.Id
                    }, courseToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
        }

        [HttpPut("{courseId}")]
        public ActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo is null)
            {  
                return NotFound();
            }
                                                          
            //map the entity to a CourseForUpdateDto      
            //apply the updated field values to that dto  
            //map the CourseFrUpdateDto back to an entity 
            _mapper.Map(course, courseForAuthorFromRepo); 
            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);
            _courseLibraryRepository.Save();
            return NoContent();
        }

    }
}