﻿using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.Model;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var course = (CourseForCreationDto) validationContext.ObjectInstance;

            if (course.Title == course.Description)
            {
                return new ValidationResult(
                    ErrorMessage,
                    new[] { nameof(CourseForCreationDto) }
                );
            }
            return ValidationResult.Success;
        }
    }
}
