using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FormSubmissionDemo.Models.Shared;

public abstract class BasePostModel
{   
    public const string ConfirmBackFormName = "__ConfirmBack";
    public const string FormModeFormName = "__FormMode";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [FromForm(Name = FormModeFormName)]
    public FormMode FormMode { get; set; } = FormMode.Edit;

    [FromForm(Name = ConfirmBackFormName)]
    [JsonIgnore]
    public string? ConfirmBack { get; set; }
    public bool IsConfirmBack => !string.IsNullOrEmpty(ConfirmBack);
}
public enum FormMode
{
    Edit = 1,
    Confirm = 2,
    Finish = 3
}
