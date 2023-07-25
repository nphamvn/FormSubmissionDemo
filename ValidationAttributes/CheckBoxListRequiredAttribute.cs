using System.ComponentModel.DataAnnotations;
using FormSubmissionDemo.Models.Shared;

namespace FormSubmissionDemo.ValidationAttributes;

public class CheckBoxListRequiredAttribute : RequiredAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is IEnumerable<CheckBoxItem> list && list.GetEnumerator().MoveNext())
        {
            return list.Any(i => i.Checked);
        }
        return false;
    }
}
