using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CourseLibrary.API.Model
{
    public class AuthorDto
    {
        [Key()]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
       
        [Required]
        public int Age { get; set; }

        [Required]
        [MaxLength(100)]
        public string MainCategory { get; set; }
    }
}