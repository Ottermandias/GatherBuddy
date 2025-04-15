using System;

namespace GatherBuddy.Keys
{
    public static class SecretKeys
    {
        private const string _apiKey = "00000000-0000-0000-0000-000000000000";
        public static Guid   ApiKey  = Guid.Parse(_apiKey);
    }
}
