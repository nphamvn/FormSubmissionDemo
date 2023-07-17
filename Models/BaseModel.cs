namespace SubmitCheckBoxListDemo.Models;

public class BaseModel
{
    public FormMode FormMode { get; set; } = FormMode.Edit;
    public string? ConfirmBack { get; set; }
    public bool IsConfirmBack => !string.IsNullOrEmpty(ConfirmBack);
}
public enum FormMode
{
    Edit = 1,
    Confirm = 2,
    Finish = 3
}
