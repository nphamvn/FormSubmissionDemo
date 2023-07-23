using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FormSubmissionDemo.Models
{
    public class SaveModel : BaseModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name.")]
        public string Username { get; set; }

        [Display(Name = "Profile Picture")]
        [ImageModelRequired(ErrorMessage = "Image is required")]
        [ImageModelFileExtentions(new string[] { "jpg", "jpeg", "png" })]
        [ImageModelSizeRange(100, 10 * 1024)]
        public ImageModel ProfilePicture { get; set; }

        [Display(Name = "Favorite Colors")]
        [CheckBoxListRequired(ErrorMessage = "At least one color must be selected")]
        public List<FavoriteColorItem> FavoriteColorItems { get; set; } = new();
        public List<string>? FavoriteColors { get; set; }

        [Display(Name = "Address")]
        public Address Address { get; set; }

        [Display(Name = "Skills")]
        [Required(ErrorMessage = "Please write something about you.")]
        public string Skills { get; set; }
        public List<int> ProfileIds { get; set; } = new();
        public string? TagsJson { get; set; }
        public List<Tag>? Tags { get; set; }
    }
    public class Tag {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
    public class ProfileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Address
    {
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select your country.")]
        public int CountryId { get; set; }
        [Display(Name = "State/Province")]
        [Required(ErrorMessage = "Please select your state/procine.")]
        public int StateProvinceId { get; set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "Please enter your city.")]
        public string City { get; set; }
    }
    public class ImageModel
    {
        public const string DefaultSrc = "img/default.jpeg";
        public const int DefaultImageFileId = 1;
        [JsonIgnore]
        public IFormFile? FormFile { get; set; }
        public int? AppFileId { get; set; }
        public int? TempFileId { get; set; }
        public string? Src { get; set; }
        public string? Alt { get; set; }
    }
    public class FavoriteColorItem : CheckBoxItem
    {
        public string Color { get; set; }
    }
    public class CheckBoxItem
    {
        public bool Checked { get; set; }
    }
    public class CheckBoxListRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is IEnumerable<CheckBoxItem> list && list.GetEnumerator().MoveNext())
            {
                return list.Any(i => i.Checked);
            }
            return false;
        }
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
            if (image.FormFile == null && image.AppFileId == null && image.TempFileId == null)
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