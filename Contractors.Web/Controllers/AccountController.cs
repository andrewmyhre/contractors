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
            return View(new SignUpRequest());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignInOrSignUp(SignUpRequest request)
        {
            var existingAccount = _userAccountService.Retrieve(request.EmailAddress);
            if (existingAccount != null)
            {
                ModelState.AddModelError("EmailAddress", "That email address has already been registered on our system. Do you want to log in?");
            }

            if (!ModelState.IsValid)
            {
                return View("SignInOrSignUp", request);
            }

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

        public ActionResult SignIn()
        {
            return View(new SignInRequest());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(SignInRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (_userAccountService.CredentialsValid(request.EmailAddress, request.Password))
            {
                FormsAuthentication.SetAuthCookie(request.EmailAddress, false);

                return RedirectToAction("Index", "Candidates");
            }

            ModelState.AddModelError("EmailAddress", "An account with that email address or password could not be found");

            return View(request);
        }
    }
}
