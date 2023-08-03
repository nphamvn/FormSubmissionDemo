using FormSubmissionDemo.Controllers;
using FormSubmissionDemo.Models.Orders;
using FormSubmissionDemo.Models.Shared;
using FormSubmissionDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionDemo;

[Route("Orders")]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;

    public IActionResult Index() {
        return View();
    }
    public OrdersController(IHttpContextAccessor httpContextAccessor
    , IOrderService orderService) : base(httpContextAccessor)
    {
        _orderService = orderService;
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        var vm = await _orderService.GetCreateViewModel();
        return await OrderSaveView(vm, null);
    }

    [HttpGet("{OrderId:int}/Edit")]
    public async Task<IActionResult> Edit(int OrderId)
    {
        var vm = await _orderService.GetEditViewModel(OrderId);
        return await OrderSaveView(vm, OrderId);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(OrderSaveViewModel model)
    {
        await PreProcess(model);
        await _orderService.Validate(null, model, ModelState);
        if (model.IsConfirmBack || !ModelState.IsValid)
        {
            model.FormMode = FormMode.Edit;
            return await OrderSaveView(model);
        }
        if (model.FormMode == FormMode.Edit)
        {
            model.FormMode = FormMode.Confirm;
            return await OrderSaveView(model);
        }

        var orderId = await _orderService.Create(model);
        model.FormMode = FormMode.Finish;
        return await OrderSaveView(model);
    }

    [HttpPost("{OrderId:int}/Edit")]
    public async Task<IActionResult> Edit(int OrderId, OrderSaveViewModel model)
    {
        await PreProcess(model, OrderId);
        await _orderService.Validate(OrderId, model, ModelState);
        if (model.IsConfirmBack || !ModelState.IsValid)
        {
            model.FormMode = FormMode.Edit;
            return await OrderSaveView(model, OrderId);
        }
        if (model.FormMode == FormMode.Edit)
        {
            model.FormMode = FormMode.Confirm;
            return await OrderSaveView(model, OrderId);
        }
        await _orderService.Edit(OrderId, model);
        model.FormMode = FormMode.Finish;
        return await OrderSaveView(model, OrderId);
    }
    private async Task PreProcess(OrderSaveViewModel model, int? orderId = null)
    {
        
    }
    private async Task<IActionResult> OrderSaveView(OrderSaveViewModel vm, int? orderId = null)
    {
        return View("Views/Orders/SaveOrder.cshtml", vm);
    }
}
