using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;

[HtmlTargetElement("form", Attributes = FormModeAttributeName)]
public class CustomFormTagHelper : FormTagHelper
{
    private readonly ILogger<CustomFormTagHelper> _logger;
    private const string FormModeAttributeName = "form-mode";
    [HtmlAttributeName(FormModeAttributeName)]
    public FormMode FormMode { get; set; }
    public CustomFormTagHelper(IHtmlGenerator generator, ILogger<CustomFormTagHelper> logger) : base(generator)
    {
        _logger = logger;
    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await base.ProcessAsync(context, output);
        var input = new TagBuilder("input");
        input.Attributes.Add("type", "hidden");
        input.Attributes.Add("name", BasePostModel.FormModeFormName);
        input.Attributes.Add("value", FormMode.ToString());
        output.PostContent.AppendHtml(input);
        context.Items["FormMode"] = FormMode;
    }
}
