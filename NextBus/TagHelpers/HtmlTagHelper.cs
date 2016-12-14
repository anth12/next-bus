using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NextBus.TagHelpers
{
    public class HtmlTagHelper : TagHelper
    {
        private IHostingEnvironment env;

        public HtmlTagHelper(IHostingEnvironment env)
        {
            this.env = env;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!env.IsDevelopment())
            {
                output.Attributes.Add("manifest", "index.appcache");
            }
        }
    }
}
