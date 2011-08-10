using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Contractors.Core;
using Contractors.Web.Models;

namespace Contractors.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDbContext _dbContext;
        private readonly IUserAccountService _userAccountService;

        public AccountController(IDbContext _dbContext, IUserAccountService userAccountService)
        {
            this._dbContext = _dbContext;
            _userAccountService = userAccountService;
        }

        //
        // GET: /Account/

        public ActionResult SignInOrSignUp()
        {
            return View();
        }

        public ActionResult SignUp(SignUpRequest request)
        {
            var userAccount = _userAccountService.Create(
                request.FirstName,
                request.LastName,
                request.EmailAddress,
                request.Password,
                request.ConfirmPassword);

            FormsAuthentication.SetAuthCookie(request.EmailAddress, false);

            return RedirectToAction("Index", "Candidates");
        }

        public ActionResult SignOut(string returnUrl)
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Candidates");
        }

        public ActionResult SignIn(string emailAddress, string password, string returnUrl)
        {
            if (_userAccountService.CredentialsValid(emailAddress, password))
            {
                FormsAuthentication.SetAuthCookie(emailAddress, false);

                return RedirectToAction("Index", "Candidates");
            }

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}
