using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SubmitCheckBoxListDemo.Models;

namespace SubmitCheckBoxListDemo.TagHelpers;

[HtmlTargetElement("form", Attributes = FormModeAttributeName)]
public class CustomFormTagHelper : FormTagHelper
{
    private const string FormModeAttributeName = "form-mode";
    [HtmlAttributeName(FormModeAttributeName)]
    public FormMode FormMode { get; set; }
    public CustomFormTagHelper(IHtmlGenerator generator) : base(generator)
    {

    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await base.ProcessAsync(context, output);
        output.PostContent.AppendHtml($"""
        <input type="hidden" value="{FormMode}" name="__FormMode">
        """);
    }
}
