using System;
using System.Net;

namespace Extellect.Utilities.Net
{
    /// <summary>
    /// A web-client that is able to retain session information with cookies.
    /// </summary>
    public class CookieAwareWebClient : WebClient
    {
        private readonly CookieContainer m_container = new CookieContainer();

        /// <summary>
        /// Not called by your code... it's internal to the class
        /// </summary>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            var webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = m_container;
            }
            return request;
        }
    }
}