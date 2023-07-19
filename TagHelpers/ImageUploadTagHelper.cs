using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SubmitCheckBoxListDemo.Models;

namespace SubmitCheckBoxListDemo.TagHelpers;

[HtmlTargetElement("image-upload", Attributes = ForAttributeName)]
public class ImageUploadTagHelper: PartialTagHelper
{
    private const string ForAttributeName = "for";

    public ImageUploadTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope)
    {
        
    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Name = "/Views/Shared/_ImageUpload.cshtml";
        await base.ProcessAsync(context, output);
    }
}
