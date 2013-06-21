using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Twitterizer;
using System.Reflection;

namespace Server.Source.Helper
{
    public static class Configuration
    {
        /// <summary>
        /// Gets the tokens.
        /// </summary>
        /// <returns></returns>
        public static OAuthTokens GetTwitterTokens()
        {
            //ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            //{
            //    ExeConfigFilename = System.IO.Path.Combine(Environment.CurrentDirectory, "/Source/Twitter/TwitterOAuthTokens.config")
            //};
            
            //System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            //OAuthTokens tokens = new OAuthTokens();
            //tokens.AccessToken = config.AppSettings.Settings["AccessToken"].Value;
            //tokens.AccessTokenSecret = config.AppSettings.Settings["AccessTokenSecret"].Value;
            //tokens.ConsumerKey = config.AppSettings.Settings["ConsumerKey"].Value;
            //tokens.ConsumerSecret = config.AppSettings.Settings["ConsumerSecret"].Value;
            OAuthTokens tokens = new OAuthTokens();
            //Your application's OAuth settings.
            tokens.ConsumerKey = "8SlJEl2hwE2j14hHPmS6zA";
            tokens.ConsumerSecret = "2EvMyCKiDQDcZ6Eyk6z32WtkfoOAQpDY0Y8AB1PU54";

            //Use the access token string as your "oauth_token" and the access token secret as your "oauth_token_secret" to sign requests with your own Twitter account.
            tokens.AccessToken = "1386616375-yUi8UxESZL9FUFzDFLncpOKBeMOz7avwb8CuVUO";
            tokens.AccessTokenSecret = "ZRXcjQSl74USwhwHP2McxzY3eRT5SNgGOXFWzeTdGE";

            return tokens;
        }

        public static Source.LinkedIn.OAuthTokens GetLinkedInTokens()
        {
            Source.LinkedIn.OAuthTokens tokens = new Source.LinkedIn.OAuthTokens();
            // API key & Secret key
            tokens.ConsumerKey = "6bpbbe18d3g5";
            tokens.ConsumerSecret = "AXk3MwOFObDkBPMd";

            // OAuth User Token & OAuth User Secret
            tokens.AccessToken = "dd82d23a-5631-458c-9de3-ec8cfe5a8476";
            tokens.AccessTokenSecret = "ed0457b8-b2f4-49ae-9cb6-037545284ee9";

            return tokens;
        }

    }
}

