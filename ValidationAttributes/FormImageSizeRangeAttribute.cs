using System.ComponentModel.DataAnnotations;
using FormSubmissionDemo.Models.Shared;

namespace FormSubmissionDemo.ValidationAttributes;

public class ImageModelSizeRange : ValidationAttribute
{
    private readonly int _minKb;
    private readonly int _maxKb;

    public ImageModelSizeRange(int minKB = -1, int maxKB = -1)
    {
        _minKb = minKB;
        _maxKb = maxKB;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is FormImage model && model.FormFile is IFormFile image)
        {
            var fileSizeKb = image.Length / 1024;

            if (_minKb > 0 && fileSizeKb < _minKb)
            {
                return new ValidationResult($"The image size must be greater than or equal to {_minKb}KB.");
            }

            if (_maxKb > 0 && fileSizeKb > _maxKb)
            {
                return new ValidationResult($"The image size must be less than or equal to {_maxKb}KB.");
            }
        }

        return ValidationResult.Success;
    }
}
