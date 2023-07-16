using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using checkboxlist.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace checkboxlist.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index(string mode)
    {
        HomeIndexModel model;
        //get select list item from other resourse e.g database
        var checkBoxListItems = GetCheckBoxListItems();
        if (mode == "edit")
        {
            var savedMonsterFeatureItems = new List<MonsterFeatureItem>
            {
                new() { Value = "Horns"}
            };
            model = new HomeIndexModel()
            {
                MonsterFeatureItems = savedMonsterFeatureItems.Select(x => new MonsterFeatureItem
                {
                    Value = x.Value,
                    Checked = true
                }).ToList(),
                ProfileIds = new() { 1 }
            };
        }
        else
        {
            model = new HomeIndexModel
            {
                MonsterFeatureItems = MonsterFeatureItem.CreateListFromSelectListItems(checkBoxListItems),
                ProfileIds = new()
            };
        }
        //
        ViewBag._ProfileSelectListItems = GetProfileSelectListItems();
        //
        ViewBag._CheckBoxListItems = checkBoxListItems;
        return View(model);
    }

    private List<ProfileItem> GetProfileSelectListItems()
    {
        return new(){
            new() {ProfileId = 1, Name = "Nam"},
            new() {ProfileId = 2, Name = "Yen Anh"}
        };
    }

    [HttpGet("ProfilesPartial")]
    public IActionResult ProfilesPartial(int page)
    {
        ViewBag._ProfileSelectListItems = GetProfileSelectListItems();
        return PartialView();
    }

    private List<ProfileItem> GetProfiles()
    {
        return new();
    }

    [HttpPost]
    public IActionResult Index(HomeIndexModel model)
    {
        //get the checked values
        var checkedValues = model.MonsterFeatureItems.Where(i => i.Checked).Select(i => i.Value);

        //display the checked values for demo purpose
        ViewBag._CheckBoxListItems = GetCheckBoxListItems();
        ViewBag._Profiles = GetProfiles(model.ProfileIds);
        ViewBag._ProfileSelectListItems = GetProfileSelectListItems();
        return View(model);
    }

    private List<ProfileItem> GetProfiles(List<int> profileIds)
    {
        if (profileIds?.Any() ?? false)
        {
            var profiles = GetProfileSelectListItems();
            return profileIds.Select(id => new ProfileItem()
            {
                ProfileId = id,
                Name = profiles.First(p => p.ProfileId == id).Name
            }).ToList();
        }
        return Enumerable.Empty<ProfileItem>().ToList();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    private List<SelectListItem> GetCheckBoxListItems()
    {
        return new List<SelectListItem>() {
                new () { Text = "Scales", Value="Scales"},
                new () { Text = "Horns", Value="Horns"}
            };
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
