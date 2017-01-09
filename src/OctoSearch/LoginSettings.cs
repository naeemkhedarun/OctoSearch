namespace OctoSearch
{
    class LoginSettings
    {
        public LoginSettings(string octopusUri, string apiKey)
        {
            OctopusUri = octopusUri;
            ApiKey = apiKey;
        }

        public string ApiKey { get; set; }
        public string OctopusUri { get; set; }
    }
}