using System.ComponentModel.DataAnnotations;
using FormSubmissionDemo.Models.Shared;

namespace FormSubmissionDemo.ValidationAttributes;
public class FormImageRequiredAttribute : ValidationAttribute
{
    public string GetErrorMessage() => ErrorMessage;
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not FormImage image)
        {
            return new ValidationResult("Invalid value for ImageModel.");
        }
        if (image.FormFile == null && image.AppFileId == null && image.TempFileId == null)
        {
            return new ValidationResult(GetErrorMessage());
        }
        return ValidationResult.Success;
    }
}