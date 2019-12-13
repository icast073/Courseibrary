using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseLibrary.API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CourseLibrary.API.Model
{
    public class CourseDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [ForeignKey("AuthorId")] 
        public Author Author { get; set; }

        public Guid AuthorId { get; set; }
    }

}