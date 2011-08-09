using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using OAuth.Net.Common;
using OAuth.Net.Components;
using OAuth.Net.Consumer;

namespace Contractors.Web.Controllers
{
    public class LinkedInController : Controller
    {
        //
        // GET: /LInkedIn/
        static string linkedin_basepath = "https://api.linkedin.com";
        static Uri linkedin_requestToken = new Uri(linkedin_basepath + "/uas/oauth/requestToken");
        static Uri linkedin_accessToken = new Uri(linkedin_basepath + "/uas/oauth/accessToken");
        static Uri linkedin_RequestAuthorizationPath = new Uri("https://www.linkedin.com/uas/oauth/authorize");
        static EndPoint epRequest = new EndPoint(linkedin_requestToken);
        static EndPoint epAccess = new EndPoint(linkedin_accessToken);
        private OAuthConsumer consumer = null;
        OAuthService serviceDefinition;
        private IToken requestToken = null;

        public LinkedInController()
        {
            consumer = new OAuthConsumer("fl2b61wm2dzu", "EXmsOgjuA1SKJjSm");


            serviceDefinition = OAuthService.Create(epRequest, linkedin_RequestAuthorizationPath, epAccess, "HMAC-SHA1", consumer);
        }

        public string Index()
        {

            var con = OAuthConsumerRequest.Create(epRequest, serviceDefinition);
            con.OnReceiveRequestToken += new EventHandler<RequestTokenReceivedEventArgs>(con_OnReceiveRequestToken);

            var auth_request = OAuthRequest.Create(new EndPoint("http://api.linkedin.com/v1/people/~"),
                                                   serviceDefinition,new Uri("http://localhost:50774/linkedin/callback"), "1");

            auth_request.OnReceiveRequestToken += new EventHandler<RequestTokenReceivedEventArgs>(auth_request_OnReceiveRequestToken);
            auth_request.OnReceiveAccessToken += new EventHandler<AccessTokenReceivedEventArgs>(auth_request_OnReceiveAccessToken);
            auth_request.OnBeforeGetRequestToken += new EventHandler<PreRequestEventArgs>(auth_request_OnBeforeGetRequestToken);
            auth_request.OnBeforeGetAccessToken += new EventHandler<PreAccessTokenRequestEventArgs>(auth_request_OnBeforeGetAccessToken);
            auth_request.OnBeforeGetProtectedResource += new EventHandler<PreProtectedResourceRequestEventArgs>(auth_request_OnBeforeGetProtectedResource);

            var response = auth_request.GetResource();

            if (!response.HasProtectedResource)
            {
                Response.Redirect(linkedin_RequestAuthorizationPath.ToString()+"?oauth_token="+response.Token.Token);
            }

            return "ok";
        }

        public string Callback(string oauth_token, string oauth_verifier)
        {
            var auth_request = OAuthRequest.Create(new EndPoint("http://api.linkedin.com/v1/people/~"),
                                                   serviceDefinition, "1");

            var response = auth_request.GetResource();

            return "wicked";
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
            Uri requestTokenUri = new Uri(string.Format("{0}{1}", linkedin_basepath, linkedin_requestToken));
            Uri accessTokenUri = new Uri(string.Format("{0}{1}", linkedin_basepath, linkedin_accessToken));
            var con = OAuth.Net.Consumer.OAuthRequest.Create(new EndPoint(requestTokenUri), serviceDefinition,
                                                             e.RequestToken);
            
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
