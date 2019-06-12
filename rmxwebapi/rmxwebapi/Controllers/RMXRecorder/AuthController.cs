using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rmxwebapi.Controllers.RMXRecorder
{
    public class AuthController : ApiController
    {
        private string _userName;
        private string _password;

        public AuthController()
        {
            _userName = Properties.Resources.RMX_RECORDER_AUTH_USER_NAME;
            _password = Properties.Resources.RMX_RECORDER_AUTH_PASSWORD;
        }

        public void Get()
        {

        }

        public string Get(string user, string pass)
        {
            if (user.Equals(_userName) && pass.Equals(_password))
                return Properties.Resources.SUCCESS_OF_RMX_RECORDER_CONN_REQUEST;

            return Properties.Resources.FAIL_OF_RMX_RECORDER_CONN_REQUEST;
        }

        public void Get(string id, string password, string token)
        {

        }
    }
}
