using System.Diagnostics;
using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormSubmissionDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHostEnvironment _environment;

    public HomeController(ILogger<HomeController> logger
                        , IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var model = new SaveModel
        {
        };
        return await ViewResult(model);
    }

    [HttpGet("Edit/{Id:int}")]
    public async Task<IActionResult> Edit(int Id)
    {
        var model = await GetSaveModel(Id);
        return await ViewResult(model, Id);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(SaveModel model)
    {
        await PreProcess(model);
        await Validate(model, ModelState);
        if (model.IsConfirmBack || !ModelState.IsValid)
        {
            model.FormMode = FormMode.Edit;
            return await ViewResult(model);
        }
        if (model.FormMode == FormMode.Edit)
        {
            model.FormMode = FormMode.Confirm;
            return await ViewResult(model);
        }
        await Save(model);
        model.FormMode = FormMode.Finish;
        return await ViewResult(model);
    }

    [HttpPost("Edit/{Id:int}")]
    public async Task<IActionResult> Edit(int Id, SaveModel model)
    {
        await PreProcess(model, Id);
        await Validate(model, ModelState, Id);
        if (model.IsConfirmBack || !ModelState.IsValid)
        {
            model.FormMode = FormMode.Edit;
            return await ViewResult(model, Id);
        }
        if (model.FormMode == FormMode.Edit)
        {
            model.FormMode = FormMode.Confirm;
            return await ViewResult(model, Id);
        }
        await Save(model);
        model.FormMode = FormMode.Finish;
        return await ViewResult(model, Id);
    }

    [HttpGet("image")]
    public async Task<IActionResult> Image(int? id = null, string? tempImageName = null)
    {
        if (id != null)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Images", "3q4sinxj.pvy");
            return new FileStreamResult(new FileStream(filePath, FileMode.Open), "image/*");
        }
        if (!string.IsNullOrEmpty(tempImageName))
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Images/temp", tempImageName);
            return new FileStreamResult(new FileStream(filePath, FileMode.Open), "image/*");
        }
        return null;
    }

    private async Task PreProcess(SaveModel model, int? id = null)
    {
        if (model.ProfilePicture is ImageModel imageModel && imageModel.FormFile is IFormFile image && image.Length > 0)
        {
            var filePath = await SaveTempImage(image);
            model.ProfilePicture.TempImageName = Path.GetFileName(filePath);
        }
        model.FavoriteColors = model.FavoriteColorItems?.Where(x => x.Checked).Select(x => x.Color).ToList() ?? new List<string>();
    }

    private async Task<string> SaveTempImage(IFormFile file)
    {
        var filePath = Path.Combine(_environment.ContentRootPath, "Images/temp", Path.GetRandomFileName());
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filePath;
    }
    private void BeforeRender(SaveModel model, int? id = null)
    {
        model.ProfilePicture ??= new();
        if (!string.IsNullOrEmpty(model.ProfilePicture.TempImageName))
        {
            model.ProfilePicture.Src = Url.Action(nameof(Image), new { tempImageName = model.ProfilePicture.TempImageName });
        }
        else if (id != null)
        {
            model.ProfilePicture.Src = Url.Action(nameof(Image), new { id = id.Value });
        }
        model.ProfilePicture.Src ??= ImageModel.DefaultSrc;

        //
        var favoriteColorSelectListItems = ViewBag._FavoriteColorSelectListItems as List<SelectListItem>;
        model.FavoriteColorItems = favoriteColorSelectListItems.Select(i => new FavoriteColorItem()
        {
            Color = i.Value,
            Checked = model.FavoriteColors?.Contains(i.Value) ?? false
        }).ToList();

    }
    private async Task Validate(SaveModel model, ModelStateDictionary modelState, int? id = null)
    {

    }
    private async Task Save(SaveModel model)
    {
        if (!string.IsNullOrEmpty(model?.ProfilePicture?.TempImageName))
        {
            var sourceFile = Path.Combine(_environment.ContentRootPath, "Images/temp", model.ProfilePicture.TempImageName);
            var destinationFile = Path.Combine(_environment.ContentRootPath, "Images", model.ProfilePicture.TempImageName);
            await CopyFileAsync(sourceFile, destinationFile);
        }
    }
    public static async Task CopyFileAsync(string sourceFile, string destinationFile)
    {
        using var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var destinationStream = new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan);
        await sourceStream.CopyToAsync(destinationStream);
    }
    private async Task<SaveModel> GetSaveModel(int id)
    {
        return new SaveModel
        {
            Username = "Nam Pham",
            ProfilePicture = new ImageModel()
            {
                ImageName = "3q4sinxj.pvy"
            },
            Address = new Address()
            {
                CountryId = 1,
                StateProvinceId = 1,
                City = "Can Loc"
            },
            Skills = "I AM GIFTED",
            ProfileIds = Enumerable.Range(0, 10).Select(x => x).ToList(),
            FavoriteColors = new() { "red", "orange" }
        };
    }
    private async Task<IActionResult> ViewResult(SaveModel model, int? id = null)
    {
        ViewBag._FavoriteColorSelectListItems = GetFavoriteColorSelectListItems();
        ViewBag._AddressCountrySelectListItems = GetAddressCountrySelectListItems();
        ViewBag._AddressStateProvinceSelectListItems = GetCountryStateProvinceSelectListItems(model.Address?.CountryId ?? 0);
        ViewBag._ProfileItems = GetProfileItems(model.ProfileIds);
        ViewData["Action"] = id == null ? "Create" : "Edit";
        BeforeRender(model);
        return View("/Views/Home/Save.cshtml", model);
    }

    private List<ProfileItem> GetProfileItems(List<int> profileIds)
    {
        return profileIds.Select(id => new ProfileItem()
        {
            Id = id,
            Name = $"Profile {id}"
        }).ToList();
    }

    private List<SelectListItem> GetFavoriteColorSelectListItems()
    {
        return new()
        {
            new("Red", "red"),
            new("Indigo", "indigo"),
            new("Orange", "orange")
        };
    }

    [HttpGet("AjaxCountryStateProvinceOptionsPartial")]
    public async Task<IActionResult> AjaxCountryStateProvinceOptionsPartial(int countryId)
    {
        ViewBag._Options = GetCountryStateProvinceSelectListItems(countryId); ;
        return PartialView("/Views/Home/_SelectOptionsPartial.cshtml");
    }

    [HttpGet("AjaxProfileList")]
    public async Task<IActionResult> AjaxProfileList(int? page)
    {
        var model = PaginatedList<ProfileItem>.Create(GetProfileItems(), page ?? 1, 10);
        return PartialView("/Views/Home/_ProfileList.cshtml", model);
    }

    private static IQueryable<ProfileItem> GetProfileItems()
    {
        return Enumerable.Range(1, 100).Select(i => new ProfileItem()
        {
            Id = i,
            Name = $"Profile {i}"
        }).AsQueryable();
    }

    private List<SelectListItem> GetCountryStateProvinceSelectListItems(int countryId)
    {
        if (countryId == 1)
        {
            return new() {
                new("Ha Tinh", 1.ToString()),
                new("Ha Noi", 2.ToString()),
            };
        }
        else if (countryId == 2)
        {
            return new() {
                new("Tokyo", 1.ToString()),
                new("Osaka", 2.ToString()),
            };
        }
        else
        {
            return new();
        }
    }

    private List<SelectListItem> GetAddressCountrySelectListItems()
    {
        return new()
        {
            new("Select country", string.Empty),
            new("VietNam", 1.ToString()),
            new("Japan", 2.ToString()),
        };
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
