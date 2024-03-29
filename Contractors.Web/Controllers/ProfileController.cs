﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Contractors.Core;
using Contractors.Web.Code;
using Contractors.Web.Controllers;
using Contractors.Web.Models;

namespace Contractors.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IDbContext _dbContext;

        public ProfileController(IUserAccountService userAccountService, IDbContext dbContext)
        {
            _userAccountService = userAccountService;
            _dbContext = dbContext;
        }

        //
        // GET: /Profile/
        [Authorize]
        public ActionResult Index()
        {
            var account = _userAccountService.Retrieve(User.Identity.Name);
            if (account == null)
                return RedirectToAction("Index", "Candidates");

            var viewModel = new ProfileIndexViewModel();
            viewModel.BasicInformation = new BasicInformationViewModel()
                                             {
                                                 FirstName = account.FirstName,
                                                 LastName = account.LastName,
                                                 EmailAddress = account.EmailAddress
                                             };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult BasicInformationEdit()
        {
            var account = _userAccountService.Retrieve(User.Identity.Name);
            var viewModel = new BasicInformationViewModel()
                                {
                                    FirstName = account.FirstName,
                                    LastName = account.LastName,
                                    EmailAddress = account.EmailAddress
                                };
            return View("BasicInformationEdit", viewModel);
        }

        [Authorize]
        public ActionResult UpdateBasicInformation(UpdateBasicInformationRequest request)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = 400;
                    return Json(ModelState.ToResponse(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View("BasicInformationEdit", request);
                }
            }

            _userAccountService.UpdateAccount(User.Identity.Name, request.FirstName, request.LastName, request.EmailAddress);

            FormsAuthentication.SetAuthCookie(request.EmailAddress, false);

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

    }

    public class UpdateBasicInformationRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }

    public class ErrorsResponse
    {
        public List<ErrorInformation> Errors { get; set; }
    }

    public class ErrorInformation
    {
        public string Key { get; set; }
        public string[] Messages { get; set; }
    }
}