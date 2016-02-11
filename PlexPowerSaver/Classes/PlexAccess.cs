using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Diagnostics;

namespace PlexPowerSaver
{
    internal class PlexAccess
    {
        private User _user;
        private string _clientId;

        public PlexAccess(string clientId)
        {
            _clientId = clientId;
            GetPlexToken();
        }

        /// <summary>
        /// Check if plex is running that there are no active streams
        /// </summary>
        /// <returns>true if plex is not running or no active stream connection playing</returns>
        public bool NoActiveStreams()
        {
            if (string.IsNullOrEmpty(_user.authenticationToken))
            {
                throw new Exception("No authentication token.");
            }

            var client = new RestClient("http://localhost:32400/status/sessions");
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("X-Plex-Token", _user.authenticationToken);
            IRestResponse<MediaContainer> response = client.Execute<MediaContainer>(request);

            uint activeStreams = response.Data.size;
            Trace.WriteLine("Active streams: " + activeStreams);

            return activeStreams == 0;
        }

        /// <summary>
        /// Check if the plexIdleTimeout has elapsed since the last play
        /// </summary>
        /// <returns>true if no videos were marked watched since the timer interval</returns>
        public bool NoRecentPlay()
        {
            int plexIdleTimeout;
            int.TryParse(Properties.Settings.Default.PlexIdleTimeout, out plexIdleTimeout);

            if (string.IsNullOrEmpty(_user.authenticationToken))
            {
                throw new Exception("No authentication token.");
            }

            var client = new RestClient("http://localhost:32400/status/sessions/history/all");
            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("X-Plex-Token", _user.authenticationToken);
            request.AddQueryParameter("X-Plex-Container-Start", "0");
            request.AddQueryParameter("X-Plex-Container-Size", "1");
            IRestResponse<PlexHistoryObjects.MediaContainer> response = client.Execute<PlexHistoryObjects.MediaContainer>(request);

            if (response.Data.Video != null)
            {
                var lastStream = response.Data.Video;
                DateTime lastStreamDateTime = UnixTimeStampToDateTime(double.Parse(lastStream.ViewedAt));
                TimeSpan timeSpan = DateTime.Now - lastStreamDateTime;
                var msSinceLastStream = timeSpan.TotalMilliseconds;
                Trace.WriteLine("Last stream: " + lastStream.GrandparentTitle + " at " +
                                lastStreamDateTime.ToUniversalTime());

                return msSinceLastStream >= (plexIdleTimeout * Global.Minutes);
            }
            else
            {
                return true;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Obtain plex authentication token, sets the _user object
        /// </summary>
        /// <returns>authenticationToken</returns>
        private string GetPlexToken()
        {
            string username = Properties.Settings.Default.Username;
            string password = Settings.ToInsecureString(Settings.DecryptString(Properties.Settings.Default.Password));
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                if (!Environment.UserInteractive)
                {
                    throw new Exception("No user or password is configured, run application in console.");
                }
                // if no user or password is stored yet we need to collect them in the console
                Trace.WriteLine("Plex Username:");
                username = Console.ReadLine();
                Trace.WriteLine("Plex Password:");
                password = Console.ReadLine();
                Settings.SaveUserSettings(username, password);
            }

            var client = new RestClient("https://plex.tv/users/sign_in.xml");
            client.Authenticator = new HttpBasicAuthenticator(username, password);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-plex-client-identifier", _clientId);
            request.AddHeader("x-plex-product", "PlexStatusCheck");
            request.AddHeader("x-plex-version", "1.0.0");
            IRestResponse<User> response = client.Execute<User>(request);
            _user = response.Data;


            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Invalid Plex login credentials.");
            }
            else
            {
                Trace.WriteLine("Authenticated with Plex as " + _user.username + "(" + _user.email + ")");
            }

            return _user.authenticationToken;
        }
    }
}