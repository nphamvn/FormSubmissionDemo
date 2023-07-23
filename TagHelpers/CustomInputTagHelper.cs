using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;

[HtmlTargetElement("input", Attributes = ForAttributeName)]
public class CustomInputTagHelper : InputTagHelper
{
    private readonly IHtmlGenerator _generator;
    private readonly ILogger<CustomInputTagHelper> _logger;
    private const string ForAttributeName = "asp-for";
    public CustomInputTagHelper(IHtmlGenerator generator, ILogger<CustomInputTagHelper> logger) : base(generator)
    {
        _generator = generator;
        _logger = logger;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        //_logger.LogInformation("InputFor: {0}", For.Name);
        if (!context.Items.ContainsKey("FormMode"))
        {
            //_logger.LogInformation("For: {0}, FormMode key does not exists", For.Name);
            //await base.ProcessAsync(context, output);
            return;
        }
        var formMode = (FormMode)context.Items["FormMode"];
        //_logger.LogInformation("FormMode: {0}", formMode);
        switch (formMode)
        {
            case FormMode.Edit:
                //await base.ProcessAsync(context, output);
                break;
            case FormMode.Confirm:
                await GenerateConfirm(context, output);
                break;
            case FormMode.Finish:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task GenerateConfirm(TagHelperContext context, TagHelperOutput output)
    {
        var type = output.Attributes.FirstOrDefault(a => a.Name == "type")?.Value?.ToString();
        var value = For?.Model;
        if (type == "text")
        {
            output.Attributes.Clear();
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var valueDiv = new TagBuilder("div");
            valueDiv.InnerHtml.Append(value?.ToString() ?? "");
            var hiddenInput = _generator.GenerateHidden(ViewContext, For?.ModelExplorer, For?.Name, value, false, null);
            output.Content.SetHtmlContent(valueDiv);
            output.Content.AppendHtml(hiddenInput);
        }
        else if (type == "checkbox")
        {
            var hiddenInput = _generator.GenerateHidden(ViewContext, For?.ModelExplorer, For?.Name, value, false, null);
            output.Attributes.Add("disabled", "");
            output.PostElement.AppendHtml(hiddenInput);
        }
    }
}
