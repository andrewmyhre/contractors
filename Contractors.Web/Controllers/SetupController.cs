using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Contractors.Core;
using Contractors.Core.Domain;

namespace Contractors.Web.Controllers
{
    public class SetupController : Controller
    {
        private readonly IDbContext _dbContext;
        private static string[] FirstNames = System.IO.File.ReadAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/App_data/firstnames.txt"));


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
                var candidates = session.Query<Candidate>().ToList();
                foreach (var candidate in candidates)
                {
                    session.Delete(candidate);
                }

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
        private static string DefaultPhoto = "http://localhost:50774/content/img/nerd.jpg";

        private Candidate CreateValidRandomCandidate()
        {
            int id = random.Next(1, 1000);
            //string photo = RandomNerdImage();
            var workHistory = RandomWorkHistory();
            string firstName = FirstNames[random.Next(FirstNames.Length)];
            string surname = Surnames[random.Next(Surnames.Length)];
            string companyName = workHistory[workHistory.Count - 1].CompanyName;
            string email = string.Format("{0}.{1}@{2}.com",
                firstName,
                surname,
                HttpUtility.UrlEncode(companyName.ToLower().Trim())
                );

            return new Candidate()
                       {
                           FullName = string.Format("{0} {1}", firstName, surname),
                           EmailAddress = email,
                           ContactNumber = random.Next(1, 999999999).ToString(),
                           DesiredRate = random.Next(200, 800),
                           DesiredRatePeriod =
                               (RemunerationPeriod)random.Next(Enum.GetValues(typeof(RemunerationPeriod)).Length),
                           EmailMd5Hash = MD5(email.Trim().ToLower()),
                           JobTitle = RandomJobTitle(),
                           WorkHistory = workHistory,
                           Skills = RandomSkillSet(),
                           ContractLengthInMonths=random.Next(12)
                       };

        }

        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        private List<Skill> RandomSkillSet()
        {
            List<Skill> skills = new List<Skill>();
            int skillCount = random.Next(2, 10);
            for (int i = 0; i < skillCount; i++)
            {
                skills.Add(new Skill()
                               {
                                   ExperienceInYears = random.Next(1, 5),
                                   SkillName = "some skill",
                                   Level = (SkillLevel)random.Next(Enum.GetValues(typeof(SkillLevel)).Length - 1)
                               });
            }
            return skills;
        }

        private List<Placement> RandomWorkHistory()
        {
            DateTime started = DateTime.Now.AddMonths(-1 * random.Next(12, 80)).AddDays(random.Next(30));
            List<Placement> history = new List<Placement>();

            RemunerationPeriod payType = (RemunerationPeriod)random.Next(Enum.GetValues(typeof(RemunerationPeriod)).Length - 1);
            decimal remuneration = 0;
            switch (payType)
            {
                case RemunerationPeriod.PerDay:
                    remuneration = random.Next(150, 800);
                    break;
                case RemunerationPeriod.PerWeek:
                    remuneration = random.Next(600, 2500);
                    break;
                case RemunerationPeriod.PerMonth:
                    remuneration = random.Next(2000, 5000);
                    break;
                case RemunerationPeriod.PerYear:
                    remuneration = random.Next(25000, 120000);
                    break;
            }

            while (started <= DateTime.Now)
            {
                int durationInMonths = random.Next(3, 48);
                DateTime finished = started.AddMonths(durationInMonths).AddDays(random.Next(30));
                history.Add(new Placement()
                                {
                                    Started = started,
                                    Finished = finished,
                                    CompanyName = RandomCompanyName(),
                                    Remuneration = remuneration,
                                    RemunerationPeriod = payType,
                                    Sector = (CompanySector)random.Next(Enum.GetValues(typeof(CompanySector)).Length - 1),
                                    Startup = random.Next(3) == 0,
                                    StillThere = finished >= DateTime.Now,

                                });
                started = started.AddMonths(durationInMonths);
            }

            return history;
        }

        private string RandomJobTitle()
        {
            return SampleJobtitles[random.Next(SampleJobtitles.Length - 1)];
        }

        private string RandomNerdImage()
        {
            switch (random.Next(6))
            {
                default:
                case 0:
                    return "nerd.jpg";
                case 1:
                    return "1 nerd.jpg";
                case 2:
                    return "400px-nerdy_guy.jpg";
                case 3:
                    return "istockphoto_5216903-nerd-young-man-isolated-on-white.jpg";
                case 4:
                    return "nerd1.jpg";
                case 5:
                    return "nerd-gamer.jpg";
            }

        }

        private static string RandomCompanyName()
        {
            return FakeCompanyNames[random.Next(FakeCompanyNames.Length - 1)];
        }

        private static string[] FakeCompanyNames = new[] {"Acme, inc.",
"Widget Corp",
"123 Warehousing",
"Demo Company",
"Smith and Co.",
"Foo Bars",
"ABC Telecom",
"Fake Brothers",
"QWERTY Logistics",
"Demo, inc.",
"Sample Company",
"Sample, inc",
"Acme Corp",
"Allied Biscuit",
"Ankh-Sto Associates",
"Extensive Enterprise",
"Galaxy Corp",
"Globo-Chem",
"Mr. Sparkle",
"Globex Corporation",
"LexCorp",
"LuthorCorp",
"North Central Positronics",
"Omni Consimer Products",
"Praxis Corporation",
"Sombra Corporation",
"Sto Plains Holdings",
"Tessier-Ashpool",
"Wayne Enterprises",
"Wentworth Industries",
"ZiffCorp",
"Bluth Company",
"Strickland Propane",
"Thatherton Fuels",
"Three Waters",
"Water and Power",
"Western Gas & Electric",
"Mammoth Pictures",
"Mooby Corp",
"Gringotts",
"Thrift Bank",
"Flowers By Irene",
"The Legitimate Businessmens Club",
"Osato Chemicals",
"Transworld Consortium",
"Universal Export",
"United Fried Chicken",
"Virtucon",
"Kumatsu Motors",
"Keedsler Motors",
"Powell Motors",
"Industrial Automation",
"Sirius Cybernetics Corporation",
"U.S. Robotics and Mechanical Men",
"Colonial Movers",
"Corellian Engineering Corporation",
"Incom Corporation",
"General Products",
"Leeding Engines Ltd.",
"Blammo",
"Input, Inc.",
"Mainway Toys",
"Videlectrix",
"Zevo Toys",
"Ajax",
"Axis Chemical Co.",
"Barrytron",
"Carrys Candles",
"Cogswell Cogs",
"Spacely Sprockets",
"General Forge and Foundry",
"Duff Brewing Company",
"Dunder Mifflin",
"General Services Corporation",
"Monarch Playing Card Co.",
"Krustyco",
"Initech",
"Roboto Industries",
"Primatech",
"Sonky Rubber Goods",
"St. Anky Beer",
"Stay Puft Corporation",
"Vandelay Industries",
"Wernham Hogg",
"Gadgetron",
"Burleigh and Stronginthearm",
"BLAND Corporation",
"Nordyne Defense Dynamics",
"Petrox Oil Company",
"Roxxon",
"McMahon and Tate",
"Sixty Second Avenue",
"Charles Townsend Agency",
"Spade and Archer",
"Megadodo Publications",
"Rouster and Sideways",
"C.H. Lavatory and Sons",
"Globo Gym American Corp",
"The New Firm",
"SpringShield",
"Compuglobalhypermeganet",
"Data Systems",
"Gizmonic Institute",
"Initrode",
"Taggart Transcontinental",
"Atlantic Northern",
"Niagular",
"Plow King",
"Big Kahuna Burger",
"Big T Burgers and Fries",
"Chez Quis",
"Chotchkies",
"The Frying Dutchman",
"Klimpys",
"The Krusty Krab",
"Monks Diner",
"Milliways",
"Minuteman Cafe",
"Taco Grande",
"Tip Top Cafe",
"Moes Tavern",
"Central Perk",
"Chasers"};

        public static string[] SampleJobtitles = new[] {"Developer",
"Analyst",
"Consultant",

".NET Developer",

"Java Developer",

"Business Analyst",

"Architect",

"C# Developer",

"Project Manager",

"Senior Developer",

"Web Developer",

"Support Analyst",

"C# .NET Developer",

"Software Engineer",

"Administrator",

"Software Developer",

"ASP.NET Developer",

"Junior",

"Applications Support",

"Support Engineer",

"Team Leader",

"PHP Developer",

"Senior Analyst",

"Graduate",

"DBA",

"SAP Consultant",

"C# ASP.NET Developer",

"Programmer",

"SQL Developer",

"C++ Developer"};


        private static string[] Surnames = new string[] { "Smith", "Jones", "Williams", "Brown", "Taylor", "Davies", "Evans", "Thomas", "Roberts", "Johnson", "Wilson", "White", "Wright", "Robinson", "Green", "Hall", "Walker", "Lewis", "Edwards", "Hughes", "Turner", "Jackson", "Harris", "Cooper", "Morris", "Martin", "Hill", "Baker", "Moore", "Clark", "King", "Ward", "Morgan", "Phillips", "Allen", "James", "Lee", "Scott", "Watson", "Bennett", "Griffiths", "Price", "Bailey", "Parker", "Young", "Richardson", "Carter", "Cook" };
    }
}