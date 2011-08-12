using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailProcessing;
using EmailProcessing.Configuration;

namespace Contractors.Web.Controllers
{
    public class EmailTestController : Controller
    {
        //
        // GET: /EmailTest/

        public string Index()
        {
            IEmailFacade emailFacade = new EmailFacade(ConfigurationManager.GetSection("email") as EmailBuilderConfigurationSection);
            emailFacade.LoadTemplates();
            emailFacade.Send("andrew.myhre@gmail.com", "sample", null);

            return "ok";
        }

    }
}
