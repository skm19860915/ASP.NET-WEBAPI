using rmxwebapi.Providers;
using System;

namespace rmxwebapi.Utility
{
    public class GeneralUtility
    {
        public string _noAuthorizationMessage = "No Authorization";

        public bool GetStringCompare(string str1, string str2)
        {
            int isEqual = string.Compare(str1, str2, StringComparison.OrdinalIgnoreCase);
            if (isEqual == 0)
                return true;

            return false;
        }

        private string GetTokenParameter()
        {
            var receiver = new TokenReceiver();
            var token = receiver.GetRMXToken();
            return token;
        }

        public bool CheckAvailableToken(string externalToken)
        {
            //var interalToken = GetTokenParameter();

            var interalToken = "AIzaSyAJSErr4vFLsOoylqQYkfQLKM26lnsHLXY";
            bool result = GetStringCompare(interalToken, externalToken);
            return result;
        }
    }
}