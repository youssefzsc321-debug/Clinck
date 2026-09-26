using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Clinck.Web.Helpers
{
    [HtmlTargetElement("a", Attributes = "active-when")]
    public class ActiveTag : TagHelper
    {
        public string? ActiveWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ActiveWhen))
                return;

            var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();

            if (!string.IsNullOrEmpty(currentController) && currentController.Equals(ActiveWhen, StringComparison.OrdinalIgnoreCase))
            {
                if (output.Attributes.ContainsName("class"))
                {
                    var existingClass = output.Attributes["class"].Value.ToString();
                    if (!existingClass.Contains("active"))
                    {
                        output.Attributes.SetAttribute("class", $"{existingClass} active");
                    }
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
        }
    }
}