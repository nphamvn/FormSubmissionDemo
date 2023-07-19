using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SubmitCheckBoxListDemo.TagHelpers;

[HtmlTargetElement("input", Attributes = ForAttributeName)]
public class CustomInputTagHelper : InputTagHelper
{
    private const string ForAttributeName = "asp-for";
    public CustomInputTagHelper(IHtmlGenerator generator) : base(generator)
    {
    }
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        return base.ProcessAsync(context, output);
    }
    public override void Init(TagHelperContext context)
    {
        base.Init(context);
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
    }
}
