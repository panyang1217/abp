<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.DocsTestApp.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            return Redirect("/Documents/");
        }
    }
}
=======
﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.DocsTestApp.Controllers
{
    public class HomeController : AbpController
    {
        public void Index()
        {

        }
    }
}
>>>>>>> upstream/master
