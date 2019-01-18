<<<<<<< HEAD
﻿namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardTagHelper : AbpTagHelper<AbpCardTagHelper, AbpCardTagHelperService>
    {
        public AbpCardTagHelper(AbpCardTagHelperService tagHelperService) 
            : base(tagHelperService)
        {
        }
    }
}
=======
﻿namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardTagHelper : AbpTagHelper<AbpCardTagHelper, AbpCardTagHelperService>
    {
        public AbpCardBorderColorType Border { get; set; } = AbpCardBorderColorType.Default;

        public AbpCardTagHelper(AbpCardTagHelperService tagHelperService) 
            : base(tagHelperService)
        {
        }
    }
}
>>>>>>> upstream/master
