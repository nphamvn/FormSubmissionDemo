using System.ComponentModel.DataAnnotations;
using FormSubmissionDemo.Models.Shared;

namespace FormSubmissionDemo.ValidationAttributes;

public class ImageModelFileExtentionsAttribute : ValidationAttribute
    {
        private readonly string[] _extentions;
        public string GetErrorMessage() => ErrorMessage;
        public ImageModelFileExtentionsAttribute(string[] extentions)
        {
            _extentions = extentions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is FormImage model && model.FormFile is IFormFile image)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!_extentions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
    }
