using System;
using System.Collections.Generic;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
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

        [HttpGet(Name = "GetAuthors")]
        //[HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]AuthorFilters authorFilters)
        {
            try
            {
                var authorFromRepo = _courseLibraryRepository.GetAuthors(authorFilters);
                //Url.Link()
                return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorFromRepo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
                return StatusCode(500, "Internal server error, try again later.");
            }
            
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
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

        [HttpOptions]
        public IActionResult GetAuthorOptions()
        {
            Response.Headers.Add("Allow","GET,OPTIONS,POST");
            return Ok();
        }

        public string CreateAuthorResouceUri(AuthorFilters authorResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage: return Url.Link("GetAuthors", new
                    {
                        pageNumber = authorResourceParameters.PageNumber - 1,
                        pageSize = authorResourceParameters.PageSize,
                        mainCategory = authorResourceParameters.MainCategory,
                        searchQuery = authorResourceParameters.SearchQuery
                    });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors", new
                    {
                        pageNumber = authorResourceParameters.PageNumber + 1,
                        pageSize = authorResourceParameters.PageSize,
                        mainCategory = authorResourceParameters.MainCategory,
                        searchQuery = authorResourceParameters.SearchQuery
                    });
            }

        }
    }
}
