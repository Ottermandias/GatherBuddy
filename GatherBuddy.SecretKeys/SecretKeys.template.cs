using System;

namespace GatherBuddy.Keys
{
    public static class SecretKeys
    {
        private const string _apiKey = "__PLACEHOLDER__";
        public static Guid   ApiKey  = Guid.Parse(_apiKey);
    }
}