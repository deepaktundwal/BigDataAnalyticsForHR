﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.OAuth
{
    /// <summary>
    /// Values returned by Twitter when getting a request token or an access token.
    /// </summary>
    public class OAuthTokenResponse
    {        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token secret.
        /// </summary>
        /// <value>The token secret.</value>
        public string TokenSecret { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public decimal UserId { get; set; }

        /// <summary>
        /// Gets or sets the screenname.
        /// </summary>
        /// <value>The screenname.</value>
        public string ScreenName { get; set; }

        /// <summary>
        /// Gets or sets the verification string.
        /// This is required when overriding the application's callback url.
        /// </summary>
        /// <value>The verification string.</value>
        public string VerificationString { get; set; }
    }
}
