using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SubmitCheckBoxListDemo.Models;

namespace SubmitCheckBoxListDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var model = new SaveModel();
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

    private async Task PreProcess(SaveModel model, int? id = null)
    {

    }
    private async Task Validate(SaveModel model, ModelStateDictionary modelState, int? id = null)
    {

    }
    private async Task Save(SaveModel model)
    {

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
