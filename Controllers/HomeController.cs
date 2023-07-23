using System.Diagnostics;
using System.Text.Json;
using FormSubmissionDemo.Data;
using FormSubmissionDemo.Entities;
using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormSubmissionDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHostEnvironment _environment;
    private readonly AppDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger
                        , IHostEnvironment environment
                        , AppDbContext appDbContext)
    {
        _logger = logger;
        _environment = environment;
        _dbContext = appDbContext;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _dbContext.Users.ToListAsync();
        return View(PaginatedList<UserIndexItem>.Create(users.Select(u => new UserIndexItem() {
            Id = u.Id,
            Username = u.Username
        }).AsQueryable(), 1, 10));
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
        await CreateUser(model);
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
        await EditUser(Id, model);
        model.FormMode = FormMode.Finish;
        return await ViewResult(model, Id);
    }

    [HttpGet("{Id:int}/ProfileImage")]
    public async Task<IActionResult> ProfileImage(int Id)
    {
        var user = await GetUser(Id);
        var content = await GetAppFile(user.ProfileFileId);
        return new FileStreamResult(new MemoryStream(content), "image/*");
    }

    [HttpGet("File/{Id:int}")]
    public async Task<IActionResult> File(int Id)
    {
        var content = await GetAppFile(Id);
        return new FileStreamResult(new MemoryStream(content), "image/*");
    }

    [HttpGet("TempFile/{Id:int}")]
    public async Task<IActionResult> TempFile(int Id)
    {
        var content = await GetTempFile(Id);
        return new FileStreamResult(new MemoryStream(content), "image/*");
    }

    private async Task<User> GetUser(int id){
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    private async Task PreProcess(SaveModel model, int? id = null)
    {
        if (model.ProfilePicture is ImageModel imageModel && imageModel.FormFile is IFormFile image && image.Length > 0)
        {
            model.ProfilePicture.TempFileId = await SaveTempFile(image);
        }
        model.FavoriteColors = model.FavoriteColorItems?.Where(x => x.Checked).Select(x => x.Color).ToList() ?? new List<string>();
    }

    private async Task<int> SaveTempFile(IFormFile image)
    {
        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        var file = new TempFile()
        {
            Content = memoryStream.ToArray()
        };
        _dbContext.TempFiles.Add(file);
        await _dbContext.SaveChangesAsync();
        return file.Id;
    }

    private async Task<byte[]> GetAppFile(int id)
    {
        return (await _dbContext.Files.FirstOrDefaultAsync(f => f.Id == id))?.Content ?? null;
    }

    private async Task<int> FromTempFileToAppFile(int id)
    {
        var file = new AppFile()
        {
            Content = await GetTempFile(id)
        };
        _dbContext.Files.Add(file);
        await _dbContext.SaveChangesAsync();
        return file.Id;
    }

    private async Task<byte[]> GetTempFile(int id)
    {
        return (await _dbContext.TempFiles.FirstOrDefaultAsync(f => f.Id == id))?.Content ?? null;
    }
    private void BeforeRender(SaveModel model, int? id = null)
    {
        model.ProfilePicture ??= new();
        if (model.ProfilePicture.TempFileId != null)
        {
            model.ProfilePicture.Src = Url.Action(nameof(TempFile), new { Id = model.ProfilePicture.TempFileId.Value });
        }
        else if (id != null)
        {
            model.ProfilePicture.Src = Url.Action(nameof(ProfileImage), new { Id = id.Value });
        }
        model.ProfilePicture.Src ??= Url.Action(nameof(File), new { Id = ImageModel.DefaultImageFileId });

        var favoriteColorSelectListItems = ViewBag._FavoriteColorSelectListItems as List<SelectListItem>;
        model.FavoriteColorItems = favoriteColorSelectListItems.Select(i => new FavoriteColorItem()
        {
            Color = i.Value,
            Checked = model.FavoriteColors?.Contains(i.Value) ?? false
        }).ToList();

        if (string.IsNullOrEmpty(model.TagsJson))
        {
            model.TagsJson = JsonSerializer.Serialize(model.Tags ?? null);
        }
    }
    private async Task Validate(SaveModel model, ModelStateDictionary modelState, int? id = null)
    {

    }
    private async Task CreateUser(SaveModel model)
    {
        var profileImageId =  await FromTempFileToAppFile(model.ProfilePicture.TempFileId.Value);
        await _dbContext.Users.AddAsync(new () {
            Username = model.Username,
            ProfileFileId = profileImageId
        });
        await _dbContext.SaveChangesAsync();
    }

    private async Task EditUser(int id, SaveModel model)
    {
        var user = await GetUser(id);
        if (model?.ProfilePicture?.TempFileId != null)
        {
            user.ProfileFileId = await FromTempFileToAppFile(model.ProfilePicture.TempFileId.Value);
        }
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<SaveModel> GetSaveModel(int id)
    {
        var user = await GetUser(id);
        return new SaveModel
        {
            Username = user.Username,
            ProfilePicture = new ImageModel()
            {
                AppFileId = user.ProfileFileId
            },
            Address = new Address()
            {
                CountryId = 1,
                StateProvinceId = 1,
                City = "Can Loc"
            },
            Tags = new List<Tag>() {
                new Tag() { Id = 1, Name = "Tag 1"},
                new Tag() { Id = 2, Name = "Tag 2"},
                new Tag() { Id = 3, Name = "Tag 3"},
                new Tag() { Id = 4, Name = "Tag 4"}
            },
            Skills = "I AM GIFTED",
            ProfileIds = Enumerable.Range(1, 2).Select(x => x).ToList(),
            FavoriteColors = new() { "red", "orange" }
        };
    }
    private async Task<IActionResult> ViewResult(SaveModel model, int? id = null)
    {
        ViewBag._FavoriteColorSelectListItems = GetFavoriteColorSelectListItems();
        ViewBag._AddressCountrySelectListItems = GetAddressCountrySelectListItems();
        ViewBag._AddressStateProvinceSelectListItems = GetCountryStateProvinceSelectListItems(model.Address?.CountryId ?? 0);
        ViewBag._ProfileItems = GetProfileItems(model.ProfileIds);
        ViewBag._WhiteListTags = GetWhiteListTags();
        ViewData["Action"] = id == null ? "Create" : "Edit";
        BeforeRender(model, id);
        return View("/Views/Home/Save.cshtml", model);
    }

    private List<SelectListItem> GetWhiteListTags()
    {
        return Enumerable.Range(1, 20).Select(i => new SelectListItem() {
            Text = $"Tag {i}",
            Value = i.ToString()
        }).ToList();
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
