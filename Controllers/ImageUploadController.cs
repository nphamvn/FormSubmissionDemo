using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SubmitCheckBoxListDemo.Models;
using SubmitCheckBoxListDemo.Models.ImageUpload;

namespace SubmitCheckBoxListDemo.Controllers;

[Route("[controller]")]
public class ImageUploadController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View("Views/ImageUpload/Index.cshtml");
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var model = new ImageUploadSaveModel();
        return await ViewResult(model);
    }

    [HttpGet("Edit/{Id:int}")]
    public async Task<IActionResult> Edit(int Id)
    {
        var model = await GetSaveModel(Id);
        return await ViewResult(model, Id);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(ImageUploadSaveModel model)
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
    public async Task<IActionResult> Edit(int Id, ImageUploadSaveModel model)
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

    private async Task PreProcess(ImageUploadSaveModel model, int? id = null)
    {
      
    }
    private async Task Validate(ImageUploadSaveModel model, ModelStateDictionary modelState, int? id = null)
    {

    }
    private async Task Save(ImageUploadSaveModel model)
    {

    }
    private async Task<ImageUploadSaveModel> GetSaveModel(int id)
    {
        return new ImageUploadSaveModel
        {

        };
    }
    private async Task<IActionResult> ViewResult(ImageUploadSaveModel model, int? id = null)
    {
        ViewBag._Data = "ViewBagData";
        ViewData["Action"] = id == null ? "Create" : "Edit";
        return View("Views/ImageUpload/Save.cshtml", model);
    }
}
