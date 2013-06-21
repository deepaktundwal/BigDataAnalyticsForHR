using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Server.OAuth
{

    public class OAuthUtility
    {
        #region Public Methods
        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="callbackAddress">The callback address. For PIN-based authentication "oob" should be supplied.</param>
        /// <returns></returns>
        public static OAuthTokenResponse GetRequestToken(string consumerKey, string consumerSecret, string callbackAddress)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException("consumerSecret");
            }

            if (string.IsNullOrEmpty(callbackAddress))
            {
                throw new ArgumentNullException("callbackAddress", @"You must always provide a callback url when obtaining a request token. For PIN-based authentication, use ""oob"" as the callback url.");
            }

            WebRequestBuilder builder = new WebRequestBuilder(
                new Uri("https://api.twitter.com/oauth/request_token"),
                HTTPVerb.POST,
                new OAuthTokens { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret });

            if (!string.IsNullOrEmpty(callbackAddress))
            {
                builder.Parameters.Add("oauth_callback", callbackAddress);
            }

            string responseBody = null;

            try
            {
                HttpWebResponse webResponse = builder.ExecuteRequest();
                Stream responseStream = webResponse.GetResponseStream();
                if (responseStream != null) responseBody = new StreamReader(responseStream).ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new OAuthTokenResponse
            {
                Token = ParseQuerystringParameter("oauth_token", responseBody),
                TokenSecret = ParseQuerystringParameter("oauth_token_secret", responseBody),
                VerificationString = ParseQuerystringParameter("oauth_verifier", responseBody)
            };
        }

        /// <summary>
        /// Tries to the parse querystring parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="text">The text.</param>
        /// <returns>The value of the parameter or an empty string.</returns>
        /// <remarks></remarks>
        private static string ParseQuerystringParameter(string parameterName, string text)
        {
            Match expressionMatch = Regex.Match(text, string.Format(@"{0}=(?<value>[^&]+)", parameterName));

            if (!expressionMatch.Success)
            {
                return string.Empty;
            }

            return expressionMatch.Groups["value"].Value;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="requestToken">The request token.</param>
        /// <param name="verifier">The pin number or verifier string.</param>
        /// <returns>
        /// An <see cref="OAuthTokenResponse"/> class containing access token information.
        /// </returns>
        public static OAuthTokenResponse GetAccessToken(string consumerKey, string consumerSecret, string requestToken, string verifier)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException("consumerSecret");
            }

            if (string.IsNullOrEmpty(requestToken))
            {
                throw new ArgumentNullException("requestToken");
            }

            WebRequestBuilder builder = new WebRequestBuilder(
                new Uri("https://api.twitter.com/oauth/access_token"),
                HTTPVerb.GET,
                new OAuthTokens { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret });

            if (!string.IsNullOrEmpty(verifier))
            {
                builder.Parameters.Add("oauth_verifier", verifier);
            }

            builder.Parameters.Add("oauth_token", requestToken);

            string responseBody;

            try
            {
                HttpWebResponse webResponse = builder.ExecuteRequest();

                responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            OAuthTokenResponse response = new OAuthTokenResponse();
            response.Token = Regex.Match(responseBody, @"oauth_token=([^&]+)").Groups[1].Value;
            response.TokenSecret = Regex.Match(responseBody, @"oauth_token_secret=([^&]+)").Groups[1].Value;
            response.UserId = long.Parse(Regex.Match(responseBody, @"user_id=([^&]+)").Groups[1].Value, CultureInfo.CurrentCulture);
            response.ScreenName = Regex.Match(responseBody, @"screen_name=([^&]+)").Groups[1].Value;
            return response;
        }

        #endregion

        /// <summary>
        /// Builds the authorization URI.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <returns>A new <see cref="Uri"/> instance.</returns>
        public static Uri BuildAuthorizationUri(string requestToken)
        {
            return BuildAuthorizationUri(requestToken, false);
        }

        /// <summary>
        /// Builds the authorization URI.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="authenticate">if set to <c>true</c>, the authenticate url will be used. (See: "Sign in with Twitter")</param>
        /// <returns>A new <see cref="Uri"/> instance.</returns>
        public static Uri BuildAuthorizationUri(string requestToken, bool authenticate)
        {
            StringBuilder parameters = new StringBuilder("https://twitter.com/oauth/");

            if (authenticate)
            {
                parameters.Append("authenticate");
            }
            else
            {
                parameters.Append("authorize");
            }

            parameters.AppendFormat("?oauth_token={0}", requestToken);

            return new Uri(parameters.ToString());
        }

    }
}
