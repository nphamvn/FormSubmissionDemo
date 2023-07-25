using System.ComponentModel.DataAnnotations;
using FormSubmissionDemo.Models.Shared;
using FormSubmissionDemo.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormSubmissionDemo.Models.Users;
public class UserSaveViewModel : BasePostModel
{
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Please enter your name.")]
    public string Username { get; set; }

    [Display(Name = "Profile Picture")]
    [FormImageRequired(ErrorMessage = "Image is required")]
    [ImageModelFileExtentions(new string[] { "jpg", "jpeg", "png" })]
    [ImageModelSizeRange(100, 10 * 1024)]
    public FormImage ProfilePicture { get; set; }

    [Display(Name = "Favorite Colors")]
    [CheckBoxListRequired(ErrorMessage = "At least one color must be selected")]
    public List<FavoriteColorItem> FavoriteColorItems { get; set; } = new();
    public List<string>? FavoriteColors { get; set; }

    [Display(Name = "Address")]
    public UserAddress Address { get; set; }

    [Display(Name = "Skills")]
    [Required(ErrorMessage = "Please write something about you.")]
    public string Skills { get; set; }
    public List<int> ProfileIds { get; set; } = new();
    public List<TagfifyTag> Tags { get; set; } = new();


    public List<ProfileItem>? ProfileItems { get; set; }
    public List<SelectListItem>? FavoriteColorSelectListItems { get; set; }
    public List<SelectListItem>? AddressCountrySelectListItems { get; set; }
    public List<SelectListItem>? AddressStateProvinceSelectListItems { get; set; }
    public List<TagfifyTag>? TagfifyTagWhiteList { get; set; }
}

public class UserAddress
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

public class FavoriteColorItem : CheckBoxItem
{
    public string Color { get; set; }
}
public class ProfileItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}