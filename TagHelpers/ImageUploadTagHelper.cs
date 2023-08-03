using FormSubmissionDemo.Models;
using FormSubmissionDemo.Models.Shared;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;

[HtmlTargetElement("image-upload", Attributes = ForAttributeName)]
public class ImageUploadTagHelper: PartialTagHelper
{
    private const string ForAttributeName = "for";

    public ImageUploadTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
    {
        Name = "/Views/Shared/_ImageUpload.cshtml";
    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.ContainsKey("FormMode"))
        {
            var formMode = (FormMode)context.Items["FormMode"];
            ViewContext.ViewData["FormMode"] = formMode;
        }
        else
        {
            ViewContext.ViewData["FormMode"] = FormMode.Edit;
        }
        await base.ProcessAsync(context, output);
    }
}
