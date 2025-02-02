﻿using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Entities; 
using System;
using System.Collections.Generic;
using System.Linq;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Model;
using CourseLibrary.API.Profiles.Filters;

namespace CourseLibrary.API.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository, IDisposable
    {
        private readonly CourseLibraryContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public CourseLibraryRepository(CourseLibraryContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService =
                propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            // always set the AuthorId to the passed-in authorId
            course.AuthorId = authorId;
            _context.Courses.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }

        public Course GetCourse(Guid authorId, Guid courseId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses.FirstOrDefault(c => c.AuthorId == authorId && c.Id == courseId);
        }

        public IEnumerable<Course> GetCourses(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Courses
                .Where(c => c.AuthorId == authorId)
                .OrderBy(c => c.Title).ToList();
        }

        public void UpdateCourse(Course course)
        {
            // no code in this implementation
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Authors.Add(author);
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(a => a.Id == authorId);
        }

        public PagedList<Author> GetAuthors(AuthorFilters authorFilters)
        {
            var collection = _context.Authors as IQueryable<Author>;
            if (authorFilters == null)
            {
                throw new ArgumentNullException(nameof(authorFilters));
            }

            if (!string.IsNullOrWhiteSpace(authorFilters.MainCategory))
            {
                authorFilters.MainCategory = authorFilters.MainCategory.Trim();
                collection = collection.Where(m => m.MainCategory.Contains(authorFilters.MainCategory));
            }

            if (!string.IsNullOrWhiteSpace(authorFilters.SearchQuery))
            {
                authorFilters.SearchQuery = authorFilters.SearchQuery.Trim();
                collection = collection.Where(a => a.MainCategory.Contains(authorFilters.SearchQuery)
                                                   || a.FirstName.Contains(authorFilters.SearchQuery)
                                                   || a.LastName.Contains(authorFilters.SearchQuery));
            }

            if (!string.IsNullOrEmpty(authorFilters.OrderBy))
            {
               var authorPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<AuthorDto, Author>();
               collection = collection.ApplySort(authorFilters.OrderBy, authorPropertyMappingDictionary);
            }
            return PagedList<Author>.Create(collection, authorFilters.PageNumber, authorFilters.PageSize);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
        }
        
        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList<Author>();
        }
         
        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            return _context.Authors
                                .Where(a => authorIds.Contains(a.Id))
                                    .OrderBy(a => a.FirstName)
                                        .OrderBy(a => a.LastName)
                                            .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               // dispose resources when needed
            }
        }
    }
}
