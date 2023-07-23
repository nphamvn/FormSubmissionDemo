using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;

[HtmlTargetElement("select", Attributes = ForAttributeName)]
public class CustomSelectTagHelper : SelectTagHelper
{
    private readonly IHtmlGenerator _generator;
    private readonly ILogger<CustomSelectTagHelper> _logger;
    private const string ForAttributeName = "asp-for";
    public CustomSelectTagHelper(IHtmlGenerator generator, ILogger<CustomSelectTagHelper> logger) : base(generator)
    {
        _generator = generator;
        _logger = logger;
    }

    public override void Init(TagHelperContext context)
    {
        //base.Init(context);
        //_logger.LogInformation("Init for {0}", For?.Name);
    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
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
        var value = For?.Model?.ToString() ?? "";
        var text = Items.FirstOrDefault(i => i.Value == value)?.Text;
        output.Attributes.Clear();
        output.PostContent.Clear();
        
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        var textDiv = new TagBuilder("div");
        textDiv.InnerHtml.Append(text?.ToString() ?? "");
        var hiddenInput = _generator.GenerateHidden(ViewContext, For?.ModelExplorer, For?.Name, value, false, null);
        output.Content.SetHtmlContent(textDiv);
        output.Content.AppendHtml(hiddenInput);
    }
    private async Task GenerateFinish(TagHelperContext context, TagHelperOutput output)
    {

    }
}
