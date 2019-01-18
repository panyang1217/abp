<<<<<<< HEAD
﻿using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpCollapseButtonTagHelper : AbpTagHelper<AbpCollapseButtonTagHelper, AbpCollapseButtonTagHelperService>
    {
        public AbpButtonType ButonType { get; set; } = AbpButtonType.Default;

        public string BodyId { get; set; }

        public AbpCollapseButtonTagHelper(AbpCollapseButtonTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
=======
﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{

    [HtmlTargetElement("abp-button", Attributes = "abp-collapse-id")]
    [HtmlTargetElement("a", Attributes = "abp-collapse-id")]
    public class AbpCollapseButtonTagHelper : AbpTagHelper<AbpCollapseButtonTagHelper, AbpCollapseButtonTagHelperService>
    {
        [HtmlAttributeName("abp-collapse-id")]
        public string BodyId { get; set; }

        public AbpCollapseButtonTagHelper(AbpCollapseButtonTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
>>>>>>> upstream/master
