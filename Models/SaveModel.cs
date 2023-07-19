using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SubmitCheckBoxListDemo.Models
{
    public class SaveModel : BaseModel
    {
        [Required]
        public string Username { get; set; }
        [ImageModelRequired(ErrorMessage = "Image is required")]
        [ImageModelFileExtentions(new string[] { "jpg", "jpeg", "png" })]
        [ImageModelSizeRange(100, 10 * 1024)]
        public ImageModel Image { get; set; }
        [ImageModelRequired(ErrorMessage = "Image is required")]
        [ImageModelFileExtentions(new string[] { "jpg", "jpeg", "png" })]
        [ImageModelSizeRange(100, 10 * 1024)]
        public ImageModel Image2 { get; set; }
    }
    public class ImageModel
    {
        public const string DefaultSrc = "img/default.jpeg";
        public IFormFile? FormFile { get; set; }
        public string? TempImageName { get; set; }
        public string? Src { get; set; }
        public string? Alt { get; set; }
    }
    public class ImageModelFileExtentions : ValidationAttribute
    {
        private readonly string[] _extentions;
        public string GetErrorMessage() => ErrorMessage;
        public ImageModelFileExtentions(string[] extentions)
        {
            _extentions = extentions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is ImageModel model && model.FormFile is IFormFile image)
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
    public class ImageModelRequired : ValidationAttribute
    {
        public string GetErrorMessage() => ErrorMessage;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not ImageModel image)
            {
                return new ValidationResult("Invalid value for ImageModel.");
            }
            if (image.FormFile == null && string.IsNullOrEmpty(image.TempImageName))
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
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
            if (value is ImageModel model && model.FormFile is IFormFile image)
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
}