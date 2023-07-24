using System.Text.Encodings.Web;
using System.Text.Json;
using FormSubmissionDemo.Models;
using FormSubmissionDemo.Models.Common;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FormSubmissionDemo.TagHelpers;

[HtmlTargetElement("tagify")]
public class TagifyTagHelper(IHtmlGenerator generator
    , ILogger<TagifyTagHelper> logger) : InputTagHelper(generator)
{
    private const string ForAttribute = "asp-for";
    private const string OptionsAttribute = "options";

    [HtmlAttributeName(ForAttribute)]
    public ModelExpression For { get; set; }

    [HtmlAttributeName(OptionsAttribute)]
    public TagifyOptions Options { get; set; }

    public List<Tag> Tags => For.Model as List<Tag>;

    private readonly ILogger<TagifyTagHelper> _logger = logger;

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
        output.TagName = null;
        output.Attributes.Clear();
        output.Content.Clear();
        //output.Attributes.TryGetAttributes("class", out var classes);
        var json = JsonSerializer.Serialize(Tags?.Select(t => new { Value = t.Name, Id = t.Id }), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var input = Generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, json, null, null);
        input.AddCssClass("js-tagify");
        input.Attributes.Add("data-my-whitelist", JsonSerializer.Serialize(Tags?.Select(t => new { Value = t.Name, Id = t.Id}), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
        output.Content.SetHtmlContent(input);
    }

    private async Task ProcessConfirmMode(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("d-flex", HtmlEncoder.Default);
        for (int i = 0; i < Tags.Count; i++)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("badge bg-secondary p-2 me-2");
            span.InnerHtml.AppendHtml(Tags[i].Name);
            output.Content.AppendHtml(span);

            var name = Generator.GenerateHidden(ViewContext, For.ModelExplorer, For.Name + $"[{i}].Name", Tags[i].Name, false, null);
            output.Content.AppendHtml(name);
            
            var id = Generator.GenerateHidden(ViewContext, For.ModelExplorer, For.Name + $"[{i}].Id", Tags[i].Id, false, null);
            output.Content.AppendHtml(id);
        }
    }
}
