using Microsoft.AspNetCore.Mvc;

namespace SubmitCheckBoxListDemo.Models;

public class BaseModel
{
    [FromForm(Name = "__FormMode")]
    public FormMode FormMode { get; set; } = FormMode.Edit;
    [FromForm(Name = "__ConfirmBack")]
    public string? ConfirmBack { get; set; }
    public bool IsConfirmBack => !string.IsNullOrEmpty(ConfirmBack);
}
public enum FormMode
{
    Edit = 1,
    Confirm = 2,
    Finish = 3
}
