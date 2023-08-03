using FormSubmissionDemo.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormSubmissionDemo.Services;

public interface IOrderService
{
    Task Edit(int orderId, OrderSaveViewModel model);
    Task<int> Create(OrderSaveViewModel model);
    Task<OrderSaveViewModel> GetCreateViewModel();
    Task<OrderSaveViewModel> GetEditViewModel(int orderId);
    Task Validate(int? orderId, OrderSaveViewModel model, ModelStateDictionary modelState);
}
public class OrderService : IOrderService
{
    public Task<int> Create(OrderSaveViewModel model)
    {
        throw new NotImplementedException();
    }

    public Task Edit(int orderId, OrderSaveViewModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderSaveViewModel> GetCreateViewModel()
    {
        var vm = new OrderSaveViewModel
        {
            TaskNo = "2023072901",
            PublishRequestDate = DateTime.Today,
            FistcalYear = 50,
            BasicPlanId = 500001,
            Options = new() { new(), new() }
        };
        vm.FistcalYearSelectListItems = FistcalYears.Select(y => new SelectListItem() { Value = y.ToString(), Text = y.ToString() })
                                                    .Prepend(new SelectListItem() { Value = "", Text = "Select" })
                                                    ;
        vm.BasicPlanIdSelectListItems = GetPlanIds(vm.FistcalYear).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() })
                                            .Prepend(new SelectListItem() { Value = "", Text = "Select" })
                                            ;
        vm.OptionPlanIdSelectListItems = GetOptionPlanIds(vm.FistcalYear).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() })
                                            .Prepend(new SelectListItem() { Value = "", Text = "Select" })
                                            ;
        return vm;
    }
    private static List<int> FistcalYears
        => new() { 50, 51, 52 };
    private static List<int> GetPlanIds(int fistcalYear)
        => Enumerable.Range(1, 10).Select(i => fistcalYear * 10000 + i).ToList();
        private static List<int> GetOptionPlanIds(int fistcalYear)
        => Enumerable.Range(1, 10).Select(i => i).ToList();
    public Task<OrderSaveViewModel> GetEditViewModel(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task Validate(OrderSaveViewModel model, ModelStateDictionary modelState)
    {
        throw new NotImplementedException();
    }

    public Task Validate(int orderId, OrderSaveViewModel model, ModelStateDictionary modelState)
    {
        throw new NotImplementedException();
    }

    public Task Validate(OrderSaveViewModel model, ModelStateDictionary modelState, int? orderId = null)
    {
        throw new NotImplementedException();
    }

    public async Task Validate(int? orderId, OrderSaveViewModel model, ModelStateDictionary modelState)
    {
        
    }
}
