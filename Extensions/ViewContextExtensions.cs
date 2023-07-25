using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormSubmissionDemo.Extensions;
public static class ViewContextExtentions {
    public static T GetViewData<T>(this ViewContext context) {
        return (T)context.ViewBag._ViewData;
    }
}