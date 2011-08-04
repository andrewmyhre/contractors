using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contractors.Core;
using Contractors.Core.Domain;
using Contractors.Web.Models;

namespace Contractors.Web.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly IDbContext _dbContext;

        public CandidatesController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /CandidateDetail/

        public ActionResult Index(int? page)
        {
            var viewModel = new CandidateListViewModel();

            int startIndex = (page ?? 0)*20;
            
            using (var session = _dbContext.OpenSession())
            {
                viewModel.Candidates = session.Query<Candidate>()
                    .Skip(startIndex)
                    .Take(20);
            }

            return View(viewModel);
        }

        public ActionResult Calendar(int? month, int? year)
        {
            var viewModel = new CandidateListViewModel();


            viewModel.CalendarActualStartDate = viewModel.CalendarMonthStartDate = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);
            while (viewModel.CalendarActualStartDate.DayOfWeek != DayOfWeek.Monday)
                viewModel.CalendarActualStartDate = viewModel.CalendarActualStartDate.AddDays(-1);

            viewModel.CalendarActualEndDate = viewModel.CalendarMonthEndDate = viewModel.CalendarMonthStartDate.AddMonths(1);
            while (viewModel.CalendarActualEndDate.DayOfWeek != DayOfWeek.Sunday)
                viewModel.CalendarActualEndDate = viewModel.CalendarActualEndDate.AddDays(1);

            IEnumerable<Candidate> candidates = null;
            using (var session = _dbContext.OpenSession())
            {
                candidates = session.Query<Candidate>();
            }

            viewModel.CandidatesByDay = new Dictionary<string, IEnumerable<Candidate>>();

            for (DateTime day = viewModel.CalendarActualStartDate; day <= viewModel.CalendarActualEndDate; day=day.AddDays(1) )
            {
                DateTime day1 = day;
                var candidatesThisDay = candidates.Where(c => c.AvailableDate >= day1 && c.AvailableDate < day1.AddDays(1));
                viewModel.CandidatesByDay.Add(day.ToString("dd-MM-yyyy"),
                    candidatesThisDay);
            }

            viewModel.CandidatesNextMonth =
                candidates.Where(c => c.AvailableDate >= viewModel.CalendarMonthStartDate.AddMonths(1)
                                                && c.AvailableDate < viewModel.CalendarMonthStartDate.AddMonths(2)).Count();
            viewModel.CandidatesLastMonth =
                candidates.Where(c => c.AvailableDate >= viewModel.CalendarMonthStartDate.AddMonths(-1)
                                                && c.AvailableDate < viewModel.CalendarMonthStartDate).Count();
            
            return View(viewModel);
        }

        [ActionName("cal")]
        public ActionResult CalendarView(int? month, int? year)
        {
            var viewModel = new CandidateListViewModel();


            viewModel.CalendarActualStartDate = viewModel.CalendarMonthStartDate = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);
            while (viewModel.CalendarActualStartDate.DayOfWeek != DayOfWeek.Monday)
                viewModel.CalendarActualStartDate = viewModel.CalendarActualStartDate.AddDays(-1);

            viewModel.CalendarActualEndDate = viewModel.CalendarMonthEndDate = viewModel.CalendarMonthStartDate.AddMonths(1);
            while (viewModel.CalendarActualEndDate.DayOfWeek != DayOfWeek.Sunday)
                viewModel.CalendarActualEndDate = viewModel.CalendarActualEndDate.AddDays(1);

            IEnumerable<Candidate> candidates = null;
            using (var session = _dbContext.OpenSession())
            {
                candidates = session.Query<Candidate>();
            }

            viewModel.CandidatesByDay = new Dictionary<string, IEnumerable<Candidate>>();

            for (DateTime day = viewModel.CalendarActualStartDate; day <= viewModel.CalendarActualEndDate; day = day.AddDays(1))
            {
                DateTime day1 = day;
                var candidatesThisDay = candidates.Where(c => c.AvailableDate >= day1 && c.AvailableDate < day1.AddDays(1));
                viewModel.CandidatesByDay.Add(day.ToString("dd-MM-yyyy"),
                    candidatesThisDay);
            }

            viewModel.CandidatesNextMonth =
                candidates.Where(c => c.AvailableDate >= viewModel.CalendarMonthStartDate.AddMonths(1)
                                                && c.AvailableDate < viewModel.CalendarMonthStartDate.AddMonths(2)).Count();
            viewModel.CandidatesLastMonth =
                candidates.Where(c => c.AvailableDate >= viewModel.CalendarMonthStartDate.AddMonths(-1)
                                                && c.AvailableDate < viewModel.CalendarMonthStartDate).Count();

            return View("~/views/candidates/CalendarView.cshtml", viewModel);
        }

        //
        // GET: /CandidateDetail/Details/5

        public ActionResult Details(int id)
        {
            var viewModel = new CandidateDetailsViewModel();
            using (var session = _dbContext.OpenSession())
            {
                viewModel.Candidate = session.Query<Candidate>().Where(c => c.Id.Equals(id)).FirstOrDefault();

            }
            return View(viewModel);
        }

        //
        // GET: /CandidateDetail/Create

        public ActionResult Create()
        {
            var candidate = new Candidate();
            return View(candidate);
        } 

        //
        // POST: /CandidateDetail/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var candidate = new Candidate();
            try
            {
                // TODO: Add insert logic here
                using (var session = _dbContext.OpenSession())
                {
                    TryUpdateModel(candidate);
                    session.SaveOrUpdate(candidate);
                    session.Commit();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /CandidateDetail/Edit/5
 
        public ActionResult Edit(int id)
        {
            var viewModel = new CandidateDetailsViewModel();
            using (var session = _dbContext.OpenSession())
            {
                viewModel.Candidate = session.Query<Candidate>().Where(c => c.Id.Equals(id)).FirstOrDefault();

            }
            return View(viewModel);
        }

        //
        // POST: /CandidateDetail/Edit/5

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
        // GET: /CandidateDetail/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CandidateDetail/Delete/5

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
