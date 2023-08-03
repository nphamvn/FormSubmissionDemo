using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionDemo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() {
        return View();
    }
    [HttpGet("IgnoreValidation")]
    public IActionResult IgnoreValidation() {
        return View();
    }
}
