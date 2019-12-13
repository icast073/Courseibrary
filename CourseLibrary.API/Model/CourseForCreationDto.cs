using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Model
{
    public class CourseForCreationDto : IValidatableObject
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
            {
                yield return new ValidationResult("", new []{"CourseForCreationDto"});
            }
        }
    }
}
