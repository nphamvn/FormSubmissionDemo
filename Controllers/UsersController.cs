using FormSubmissionDemo.Models.Shared;
using FormSubmissionDemo.Models.Users;
using FormSubmissionDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionDemo.Controllers;

public class UsersController : BaseController
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;

    public UsersController(ILogger<UsersController> logger
        , IUserService userService
        , IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _userService.GetUserIndexViewModel();
        return View("Index", viewModel);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var vm = await _userService.GetCreateViewModel();
        return await UserSaveView(vm);
    }

    [HttpGet("{UserId:int}/Edit")]
    public async Task<IActionResult> Edit(int UserId)
    {
        var vm = await _userService.GetEditViewModel(UserId);
        return await UserSaveView(vm, UserId);
    }

    [HttpGet("{UserId:int}/DownloadJson")]
    public async Task<IActionResult> DownloadJson(int UserId)
    {
        return new EmptyResult();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(UserSaveViewModel model)
    {
        await PreProcess(model);
        await _userService.ValidateCreate(model, ModelState);
        if (model.IsConfirmBack || !ModelState.IsValid)
        {
            model.FormMode = FormMode.Edit;
            return await UserSaveView(model);
        }
        if (model.FormMode == FormMode.Edit)
        {
            model.FormMode = FormMode.Confirm;
            return await UserSaveView(model);
        }
        
        var userId = await _userService.Create(model);
        model.FormMode = FormMode.Finish;
        return await UserSaveView(model);
    }

    [HttpPost("{UserId:int}/Edit")]
    public async Task<IActionResult> Edit(int UserId, UserSaveViewModel model)
    {
        await PreProcess(model, UserId);
        await _userService.ValidateEdit(UserId, model, ModelState);
        if (model.IsConfirmBack || !ModelState.IsValid)
        {
            model.FormMode = FormMode.Edit;
            return await UserSaveView(model, UserId);
        }
        if (model.FormMode == FormMode.Edit)
        {
            model.FormMode = FormMode.Confirm;
            return await UserSaveView(model, UserId);
        }
        await _userService.Edit(UserId, model);
        model.FormMode = FormMode.Finish;
        return await UserSaveView(model, UserId);
    }

    [HttpGet("{UserId:int}/ProfileImage")]
    public async Task<IActionResult> ProfileImage(int UserId)
    {
        var user = await _userService.GetUserModel(UserId);
        var file = await AppFileService.Get(user.ProfileImageFileId);
        return new FileStreamResult(new MemoryStream(file.Content), "image/*");
    }
    
    [HttpGet("AjaxCountryStateProvinceOptionsPartial")]
    public async Task<IActionResult> AjaxCountryStateProvinceOptionsPartial(int countryId)
    {
        return PartialView("_SelectOptionsPartial");
    }

    [HttpGet("AjaxProfileList")]
    public async Task<IActionResult> AjaxProfileList(int? page)
    {
        return PartialView("_ProfileList");
    }
    
    private async Task PreProcess(UserSaveViewModel model, int? userId = null)
    {
        if (model.ProfilePicture is { FormFile: { Length: > 0 } image })
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var file = new TempFile()
            {
                Content = memoryStream.ToArray()
            };
            model.ProfilePicture.TempFileId = await TempFileService.Save(file);
        }
        model.FavoriteColors = model.FavoriteColorItems?.Where(x => x.Checked).Select(x => x.Color).ToList() ?? new List<string>();
    }
    
    private Task PreRender(UserSaveViewModel model, int? userId = null)
    {
        ViewData["Action"] = userId == null ? "Create" : "Edit";
        
        model.ProfilePicture ??= new();
        if (model.ProfilePicture.TempFileId != null)
        {
            model.ProfilePicture.Src = Url.Action(nameof(FileController.TempFile), "File", new { FileId = model.ProfilePicture.TempFileId });
        }
        else if (userId != null)
        {
            model.ProfilePicture.Src = Url.Action(nameof(ProfileImage), new { UserId = userId });
        }
        model.ProfilePicture.Src ??= Url.Action(nameof(FileController.File), "File", new { FileId = FormImage.DefaultImageFileId });
        return Task.CompletedTask;
    }

    private async Task<IActionResult> UserSaveView(UserSaveViewModel model, int? id = null)
    {
        await PreRender(model, id);
        return View("Save", model);
    }
}
