using System;
using System.Collections.Generic;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Model;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        [HttpGet("{courseId}")]
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


    }
}