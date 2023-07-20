using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SubmitCheckBoxListDemo.Models;

namespace SubmitCheckBoxListDemo.Controllers;

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
        var model = new SaveModel{
            Images = new() {
                new(), new(), new()
            }
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
            return new FileStreamResult(new FileStream(id.Value.ToString(), FileMode.Open), "image/*");
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
        if (model.Image is ImageModel imageModel && imageModel.FormFile is IFormFile image && image.Length > 0)
        {
            var filePath = await SaveTempImage(image);
            model.Image.TempImageName = Path.GetFileName(filePath);
        }
        if (model.Image2 is ImageModel imageModel2 && imageModel2.FormFile is IFormFile image2 && image2.Length > 0)
        {
            var filePath = await SaveTempImage(image2);
            model.Image2.TempImageName = Path.GetFileName(filePath);
        }
    }

    private async Task<string> SaveTempImage(IFormFile file) {
        var filePath = Path.Combine(_environment.ContentRootPath, "Images/temp", Path.GetRandomFileName());
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filePath;
    }
    private void BeforeRender(SaveModel model, int? id = null)
    {
        model.Image ??= new();
        model.Image2 ??= new();
        if (!string.IsNullOrEmpty(model.Image.TempImageName))
        {
            model.Image.Src = Url.Action(nameof(Image), new { tempImageName = model.Image.TempImageName });
        }
        else if (id != null)
        {
            model.Image.Src = Url.Action(nameof(Image), new { id = id.Value });
        }
        if (!string.IsNullOrEmpty(model.Image2.TempImageName))
        {
            model.Image2.Src = Url.Action(nameof(Image), new { tempImageName = model.Image2.TempImageName });
        }
        else if (id != null)
        {
            model.Image2.Src = Url.Action(nameof(Image), new { id = id.Value });
        }
        model.Image.Src ??= ImageModel.DefaultSrc;
        model.Image2.Src ??= ImageModel.DefaultSrc;

        foreach (var imageModel in model.Images)
        {
            if (!string.IsNullOrEmpty(imageModel.TempImageName))
            {
                imageModel.Src = Url.Action(nameof(Image), new { tempImageName = imageModel.TempImageName });
            }
            else if (id != null)
            {
                imageModel.Src = Url.Action(nameof(Image), new { id = id.Value });
            }
            imageModel.Src ??= ImageModel.DefaultSrc;
        }
    }
    private async Task Validate(SaveModel model, ModelStateDictionary modelState, int? id = null)
    {

    }
    private async Task Save(SaveModel model)
    {
        if (!string.IsNullOrEmpty(model?.Image?.TempImageName))
        {
            var sourceFile = Path.Combine(_environment.ContentRootPath, "Images/temp", model.Image.TempImageName);
            var destinationFile = Path.Combine(_environment.ContentRootPath, "Images", model.Image.TempImageName);
            await CopyFileAsync(sourceFile, destinationFile);
        }
        if (!string.IsNullOrEmpty(model?.Image2?.TempImageName))
        {
            var sourceFile = Path.Combine(_environment.ContentRootPath, "Images/temp", model.Image2.TempImageName);
            var destinationFile = Path.Combine(_environment.ContentRootPath, "Images", model.Image2.TempImageName);
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

        };
    }
    private async Task<IActionResult> ViewResult(SaveModel model, int? id = null)
    {
        ViewBag._Data = "ViewBagData";
        ViewData["Action"] = id == null ? "Create" : "Edit";
        BeforeRender(model);
        return View("Views/Home/Save.cshtml", model);
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
