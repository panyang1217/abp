<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavItemTagHelperService : AbpTagHelperService<AbpNavItemTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.Attributes.AddClass("nav-item");

            SetDropdownClass(context, output);
            SetActiveClass(context, output);
        }

        protected virtual void SetDropdownClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Dropdown ?? false)
            {
                output.Attributes.AddClass("dropdown");
            }
        }

        protected virtual void SetActiveClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Active ?? false)
            {
                output.Attributes.AddClass("active");
            }
        }
    }
=======
﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavItemTagHelperService : AbpTagHelperService<AbpNavItemTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.Attributes.AddClass("nav-item");

            SetDropdownClass(context, output);
        }

        protected virtual void SetDropdownClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Dropdown ?? false)
            {
                output.Attributes.AddClass("dropdown");
            }
        }
    }
>>>>>>> upstream/master
}