using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;
[HtmlTargetElement("textarea", Attributes = TinymceEditorAttribute)]
public class CustomTextareaTagHelper : TextAreaTagHelper
{
    private const string TinymceEditorAttribute = "tinymce-editor";
    private readonly ILogger<CustomTextareaTagHelper> _logger;

    public CustomTextareaTagHelper(IHtmlGenerator generator, ILogger<CustomTextareaTagHelper> logger) : base(generator)
    {
        _logger = logger;
    }
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!context.Items.ContainsKey("FormMode"))
        {
            return;
        }
        var formMode = (FormMode)context.Items["FormMode"];
        switch (formMode)
        {
            case FormMode.Edit:
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
        var value = For?.Model;
        output.TagName = null;
        output.Content.SetHtmlContent(value?.ToString());
        var hiddenInput = Generator.GenerateHidden(ViewContext, For?.ModelExplorer, For?.Name, value, false, null);
        output.PostContent.SetHtmlContent(hiddenInput);       
    }
}