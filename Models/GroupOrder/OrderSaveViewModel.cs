using System.ComponentModel.DataAnnotations;
using FormSubmissionDemo.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormSubmissionDemo.Models.Orders;

public class OrderSaveViewModel : BasePostModel
{
    [Required]
    public string TaskNo { get; set; }
    [Required]
    public DateTime? PublishRequestDate { get; set; }
    [Required]
    public int FistcalYear { get; set; }
    [Required]
    public int BasicPlanId { get; set; }
    public List<OptionSaveModel> Options { get; set; }

    public IEnumerable<SelectListItem>? FistcalYearSelectListItems { get; set; }
    public IEnumerable<SelectListItem>? BasicPlanIdSelectListItems { get; set; }
    public IEnumerable<SelectListItem>? OptionPlanIdSelectListItems { get; set; }
}
public class OptionSaveModel {
    public string TaskNo { get; set; }
    public int PlanId { get; set; }
}
