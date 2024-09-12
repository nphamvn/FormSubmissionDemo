using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormSubmission.Models;
using Type = FormSubmission.Models.Type;

namespace FormSubmission.Controllers;

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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpGet("Save")]
    public IActionResult Save()
    {
        return View(new SaveModel()
        {
            Blocks = new List<Block>()
            {
                new Block()
                {
                    Name = "Block 1",
                    Fields = new List<Field>()
                    {
                        new Field()
                        {
                            Name = "Field 1",
                            Type = Type.Image
                        }
                    }
                }
            }
        });
    }
    
    [HttpPost("Save")]
    public IActionResult Save(SaveModel model)
    {
        return View(model);
    }
    
    [HttpGet("Block")]
    public IActionResult Block(int blockIndex)
    {
        var model = new Block();
        ViewData["blockIndex"] = blockIndex;
        return PartialView("/Views/Home/_Block.cshtml", model);
    }
    
    [HttpGet("Field")]
    public IActionResult Field(int blockIndex, int fieldIndex)
    {
        var model = new Field();
        ViewData["blockIndex"] = blockIndex;
        ViewData["fieldIndex"] = fieldIndex;
        return PartialView("/Views/Home/_Field.cshtml", model);
    }
}