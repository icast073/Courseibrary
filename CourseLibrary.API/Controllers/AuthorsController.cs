using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Model;
using CourseLibrary.API.Profiles.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly ILogger<AuthorsController> _logger;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, ILogger<AuthorsController> logger, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        //[HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]AuthorFilters authorFilters)
        {
            try
            {
                var authorFromRepo = _courseLibraryRepository.GetAuthors(authorFilters);
                return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorFromRepo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
            
        }

        [HttpGet("{authorId}")]
        public IActionResult GetAuthor(Guid authorId)
        {
            try
            {
                var authorRepo = _courseLibraryRepository.GetAuthor(authorId);
                if(authorRepo is null) return NotFound();
                return Ok(_mapper.Map<AuthorDto>(authorRepo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
            
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            try
            {
                var authorEntity = _mapper.Map<Author>(author);
                _courseLibraryRepository.AddAuthor(authorEntity);
                _courseLibraryRepository.Save();

                var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
                return CreatedAtRoute("GetAuthor",
                    new { authorId = authorToReturn.Id },
                    authorToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
        }




    }
}
