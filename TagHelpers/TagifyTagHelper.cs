using System.Text.Json;
using System.Text.Json.Serialization;
using FormSubmissionDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;

[HtmlTargetElement("input", Attributes = "tagify", TagStructure = TagStructure.Unspecified)]
public class TagifyTagHelper(IHtmlGenerator generator
    , ILogger<TagifyTagHelper> logger) : InputTagHelper(generator)
{
    private readonly ILogger<TagifyTagHelper> _logger = logger;
    [HtmlAttributeName("tagify")]
    public bool IsTagify { get; set; }
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
                await ProcessEditMode(context, output);
                break;
            case FormMode.Confirm:
                await ProcessConfirmMode(context, output);
                break;
            case FormMode.Finish:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task ProcessEditMode(TagHelperContext context, TagHelperOutput output)
    {
        
    }

    private async Task ProcessConfirmMode(TagHelperContext context, TagHelperOutput output)
    {
        //await base.ProcessAsync(context, output);
        output.Attributes.SetAttribute("type", "hidden");
        var json = For.Model?.ToString();
        _logger.LogInformation("Name: {0}, Value: {1}", For?.Name, json);
        var tags = JsonSerializer.Deserialize<List<Tag>>(json);
        var postElementDiv = new TagBuilder("div");
        postElementDiv.AddCssClass("post-element");
        foreach (var tag in tags)
        {
            //_logger.LogInformation("Name: {0}, Value: {1}", For?.Name, tag.Value);
            postElementDiv.InnerHtml.AppendHtml($"<span>{tag.Value}</span>");
        }
        output.PostElement.SetHtmlContent(postElementDiv);
    }
}
public class Tag {

    [JsonPropertyName("value")]
    public string Value { get; set; }
}
