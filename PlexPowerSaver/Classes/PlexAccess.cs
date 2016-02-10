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

            int activeStreams = response.Data.size;
            Trace.WriteLine("Active streams: " + activeStreams);

            return activeStreams == 0;
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

            Trace.WriteLine("Authenticated with Plex as " + _user.username + "(" + _user.email + ")");

            return _user.authenticationToken;
        }
    }
}