using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.Practices.ServiceLocation;
using OAuth.Net.Common;
using OAuth.Net.Components;
using OAuth.Net.Consumer;

namespace Contractors.Web.Controllers
{
    public class LinkedInController : Controller
    {
        private static readonly OAuthService OAuthService = OAuthService.Create(
            new EndPoint("https://api.linkedin.com/uas/oauth/requestToken"),
            new Uri("https://www.linkedin.com/uas/oauth/authorize"),
            new EndPoint("https://api.linkedin.com/uas/oauth/accessToken"),
            false,
            string.Empty,
            "HMAC-SHA1",
            "1.0",
            new OAuthConsumer("fl2b61wm2dzu", "EXmsOgjuA1SKJjSm"));

        //
        // GET: /LInkedIn/
        private IToken requestToken = null;

        public string Index()
        {
            var request = OAuthRequest.Create(
                new EndPoint("http://api.linkedin.com/v1/people/~/positions", "GET"),
                OAuthService,
                new Uri("http://localhost:50774/linkedin"),
                Session.SessionID);
            
            request.VerificationHandler = AspNetOAuthRequest.HandleVerification;
            OAuthResponse response = request.GetResource();
            if (!response.HasProtectedResource)
            {
                Response.Redirect(OAuthService.BuildAuthorizationUrl(response.Token).AbsoluteUri, true);
            }
            else
            {

                Session["access_token"] = response.Token;

                IEnumerable<Position> workHistory = GetWorkHistoryFromResponseStream(response.ProtectedResource.GetResponseStream());


                return "done";
            }

            return "failed";
        }

        private IEnumerable<Position> GetWorkHistoryFromResponseStream(Stream stream)
        {
            XDocument xml = XDocument.Load(stream);

            return (from e in xml.Element("positions").Elements("position")
                    select new Position()
                               {
                                   Id = e.Element("id").Value,
                                   Title = e.Element("title").Value,
                                   StartDate = new DateTime(
                                       int.Parse(e.Element("start-date").Element("year").Value), 
                                       e.Element("start-date").Element("month") != null 
                                           ? int.Parse(e.Element("start-date").Element("month").Value) 
                                           : 1, 1),
                                   IsCurrent = bool.Parse(e.Element("is-current").Value),
                                   EndDate = e.Element("end-date") != null ?
                                                                               new DateTime?(new DateTime(
                                                                                                 int.Parse(e.Element("end-date").Element("year").Value), 
                                                                                                 e.Element("end-date").Element("month") != null 
                                                                                                     ? int.Parse(e.Element("end-date").Element("month").Value) 
                                                                                                     : 1, 1))
                                                 : null,
                                   Summary = e.Element("summary") != null ? e.Element("summary").Value : "",
                                   Company = e.Element("position") != null ? new PositionCompany()
                                                 {
                                                     Id = e.Element("position").Element("id").Value,
                                                     CompanyType = e.Element("position").Element("type")!= null ? e.Element("position").Element("type").Value : "",
                                                     Industry = e.Element("position").Element("industry") != null ? e.Element("position").Element("industry").Value: "",
                                                     Name = e.Element("position").Element("name").Value != null ? e.Element("position").Element("name").Value : ""
                                                 }
                                                 : null

                               }).ToList();
        }

        public string parse()
        {
            var history =
                GetWorkHistoryFromResponseStream(
                    System.IO.File.OpenRead(
                        System.Web.Hosting.HostingEnvironment.MapPath("~/content/linkedinresponse.xml")));

            return history.Count().ToString();
        }

        void auth_request_OnBeforeGetProtectedResource(object sender, PreProtectedResourceRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        void auth_request_OnBeforeGetAccessToken(object sender, PreAccessTokenRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        void auth_request_OnBeforeGetRequestToken(object sender, PreRequestEventArgs e)
        {
        }

        void auth_request_OnReceiveAccessToken(object sender, AccessTokenReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void auth_request_OnReceiveRequestToken(object sender, RequestTokenReceivedEventArgs e)
        {
            requestToken = e.RequestToken;
        }

        void con_OnReceiveRequestToken(object sender, RequestTokenReceivedEventArgs e)
        {
        }


    }

    public class LinkedInCredentials : IConsumer
    {
        public string Key
        {
            get { return "fl2b61wm2dzu"; }
        }

        public string Secret
        {
            get { return "EXmsOgjuA1SKJjSm"; }
        }

        public ConsumerStatus Status { get; set; }

        public string FriendlyName { get; set; }
    }

    public class RequestStateStore : IRequestStateStore
    {
        private static List<RequestState> _states = new List<RequestState>();
        public void Store(RequestState state)
        {
            _states.Add(state);
        }

        public RequestState Get(RequestStateKey key)
        {
            var state = _states.Where(s=>s.Key==key).FirstOrDefault();

            return state;
        }

        public void Delete(RequestStateKey key)
        {
            _states.Remove(Get(key));
        }
    }
}
