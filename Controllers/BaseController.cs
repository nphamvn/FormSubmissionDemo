using FormSubmissionDemo.Models.Shared;
using FormSubmissionDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionDemo.Controllers;

[Route("[controller]")]
public class BaseController(IHttpContextAccessor httpContextAccessor) : Controller
{
    protected IAppFileService AppFileService = ServiceProviderServiceExtensions.GetRequiredService<IAppFileService>(httpContextAccessor.HttpContext.RequestServices);
    protected ITempFileService TempFileService = ServiceProviderServiceExtensions.GetRequiredService<ITempFileService>(httpContextAccessor.HttpContext.RequestServices);
    
}