using Microsoft.AspNetCore.Razor.TagHelpers;

namespace checkboxlist.TagHelpers
{
    public class CheckBoxTagHelper : TagHelper
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var existingClass = context.AllAttributes.FirstOrDefault(a => a.Name == "class");
            var cssClass = string.Empty;
            if (existingClass != null)
            {
                cssClass = existingClass.Value.ToString();
            }
            cssClass = cssClass + " js-checkbox-checked";
            return base.ProcessAsync(context, output);
        }
    }
}