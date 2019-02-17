using System;
using System.Net;

namespace Extellect.Net
{
    /// <summary>
    /// A web-client that is able to retain session information with cookies.
    /// </summary>
    public class CookieAwareWebClient : WebClient
    {
        private readonly CookieContainer m_container = new CookieContainer();

        /// <summary>
        /// Gets or sets the timeout for getting the response or the request stream.
        /// </summary>
        public TimeSpan? RequestTimeout { get; set; }

        /// <summary>
        /// Gets or sets the timeout for reading from (or writing to) a stream.
        /// </summary>
        public TimeSpan? RequestReadWriteTimeout { get; set; }

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
                if (RequestTimeout.HasValue)
                {
                    webRequest.Timeout = (int)RequestTimeout.Value.TotalMilliseconds;
                }
                if (RequestReadWriteTimeout.HasValue)
                {
                    webRequest.ReadWriteTimeout = (int)RequestReadWriteTimeout.Value.TotalMilliseconds;
                }
            }
            return request;
        }
    }
}