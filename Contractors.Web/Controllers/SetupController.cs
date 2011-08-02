using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contractors.Core;
using Contractors.Core.Domain;

namespace Contractors.Web.Controllers
{
    public class SetupController : Controller
    {
        private readonly IDbContext _dbContext;

        public SetupController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /Setup/

        public ActionResult Index()
        {
            using (var session = _dbContext.OpenSession())
            {
                for (int i = 0; i < 100; i++)
                {
                    Candidate candidate = CreateValidRandomCandidate();
                    session.SaveOrUpdate(candidate);
                }
                session.Commit();
            }

            return RedirectToAction("Index", "Candidates");
        }

        private static Random random = new Random();
        private Candidate CreateValidRandomCandidate()
        {
            int id = random.Next(1, 1000);
            return new Candidate()
                                {
                                    FullName = "test candidate " + id,
                                    EmailAddress = "test.candidate."+id+"@testcandidates.com",
                                    ContactNumber = random.Next(1,999999999).ToString(),
                                    DesiredRate = random.Next(200,800),
                                    DesiredRatePeriod = (RemunerationPeriod)random.Next(Enum.GetValues(typeof(RemunerationPeriod)).Length),
            };

        }

        //
        // GET: /Setup/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Setup/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Setup/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Setup/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Setup/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Setup/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Setup/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
