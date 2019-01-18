﻿namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalTagHelper : AbpTagHelper<AbpModalTagHelper, AbpModalTagHelperService>
    {
        public AbpModalSize Size { get; set; } = AbpModalSize.Default;

        public AbpModalTagHelper(AbpModalTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
