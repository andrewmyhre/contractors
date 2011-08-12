using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Contractors.Core;
using Contractors.Core.Domain;
using Contractors.Web.Code;
using Contractors.Web.Models;

namespace Contractors.Web.Controllers
{
    [Authorize]
    public class WorkHistoryController : Controller
    {
        private readonly IDbSession _dbSession;
        private readonly HttpContextBase _httpContext;
        private readonly Candidate _candidate;

        public WorkHistoryController(IDbSession dbSession, HttpContextBase httpContext)
        {
            _dbSession = dbSession;
            _httpContext = httpContext;
            if (httpContext.User == null) throw new UnauthorizedAccessException();

            // if the current user isn't linked to a candidate then create one for them
            var user = httpContext.User.Identity as ContractorsIdentity;
            _candidate = GetCandidateForCurrentUser(dbSession, user);

            if (_candidate == null)
            {
                _candidate = new Candidate()
                                {
                                    EmailAddress = user.UserAccount.EmailAddress, 
                                    FullName = string.Format("{0} {1}", user.UserAccount.FirstName, user.UserAccount.LastName),
                                    WorkHistory =  new List<Placement>(),
                                    Skills = new List<Skill>()
                                };
                dbSession.SaveOrUpdate(_candidate);
            }
        }

        private Candidate GetCandidateForCurrentUser(IDbSession dbSession, ContractorsIdentity user)
        {
            return dbSession.Query<Candidate>().Where(
                c => c.EmailAddress == user.UserAccount.EmailAddress)
                .FirstOrDefault();
        }

        //
        // GET: /WorkHistory/

        public ActionResult Index()
        {
            return View(new PlacementModel());
        }

        public ActionResult GetHistory()
        {
            return View("WorkHistory", _candidate.WorkHistory.OrderByDescending(p => p.Started));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPlacement(PlacementModel model)
        {
            if (!model.StillThere && !model.Finished.HasValue)
            {
                ModelState.AddModelError("Finished", "Please provide the date you finished this role");
            }

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(ModelState.ToResponse());
                }

                return View("Index", model);
            }

            if (_candidate.WorkHistory == null)
                _candidate.WorkHistory = new List<Placement>();
            var placement = new Placement()
                                {
                                    CompanyName = model.CompanyName,
                                    Sector = model.Sector,
                                    Finished = model.Finished.HasValue ? model.Finished.Value : DateTime.MaxValue,
                                    StillThere = model.StillThere,
                                    Started = model.Started,
                                    PlacementType = model.PlacementType
                                };
            var skillset = model.SkillSet.Split(',', ';', ' ');
            if (placement.Skills == null)
                placement.Skills = new List<Skill>();
            List<Skill> skills = placement.Skills.ToList();
            foreach(var skill in skillset)
            {
                if (string.IsNullOrWhiteSpace(skill)) continue;
                skills.Add(new Skill() {SkillName = skill});
            }
            placement.Skills = skills;
            _candidate.WorkHistory.Add(placement);
            _dbSession.SaveOrUpdate(_candidate);

            if (Request.IsAjaxRequest())
            {
                return Json("ok");
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditPlacement(string id)
        {
            var placement =
                _candidate.WorkHistory.Where(p => p.Id==id).FirstOrDefault();

            if (placement == null)
            {
                return new HttpNotFoundResult();
            }

            return View("PlacementEdit", placement);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePlacement(string id, PlacementModel model)
        {
            if (!model.StillThere && !model.Finished.HasValue)
            {
                ModelState.AddModelError("Finished", "Please provide the date you finished this role");
            }

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(ModelState.ToResponse());
                }

                return View("Index", model);
            }

            var placement =
                _candidate.WorkHistory.Where(p => p.Id == id).FirstOrDefault();

            placement.CompanyName = model.CompanyName;
            placement.Sector = model.Sector;
            placement.Started = model.Started;
            placement.StillThere = model.StillThere;
            if (!model.StillThere && model.Finished.HasValue)
                placement.Finished = model.Finished.Value;
            else
                placement.Finished = DateTime.MaxValue;

            var skillset = model.SkillSet.Split(',', ';', ' ');
            placement.Skills = new List<Skill>();
            List<Skill> skills = placement.Skills as List<Skill>;
            foreach (var skill in skillset)
            {
                if (string.IsNullOrWhiteSpace(skill)) continue;
                skills.Add(new Skill() { SkillName = skill });
            }
            placement.Skills = skills;

            _dbSession.SaveOrUpdate(placement);

            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(201);
            }

            return new ContentResult(){Content="Ok"};
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult DeletePlacement(string id)
        {
            var placement = _candidate.WorkHistory.Where(p => p.Id == id).FirstOrDefault();
            if (placement == null)
                return new HttpNotFoundResult();

            _candidate.WorkHistory.Remove(placement);
            _dbSession.SaveOrUpdate(_candidate);

            return new HttpStatusCodeResult((int)HttpStatusCode.OK);
        }
    }
}
