using rmxwebapi.Models;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class TestimonialController : ApiController
    {
        private DataRepository _repo;

        public TestimonialController()
        {
            _repo = new DataRepository();
        }

        [HttpPost]
        public IHttpActionResult Post(TestimonialModel tes)
        {
            var name = tes.Name;
            var city = tes.City;
            var country = tes.Country;
            var state = tes.State;
            var testimonial = tes.Testimonial;
            var locationOid = tes.Location;

            var location = _repo.GetAllLocations().Where(x => x.Oid == locationOid).FirstOrDefault();
            var locationName = location.Name;
            var outgoingUserName = location.OutgoingUserName;
            var outgoingPassword = location.OutgoingPassword;
            var outgoingServerName = location.OutgoingServerName;
            var outgoingServerPort = location.OutgoingServerPort;
            var emailAddress = location.EmailAddress;

            try
            {
                var client = new SmtpClient(outgoingServerName);

                client.UseDefaultCredentials = false;
                var basicAuthenticationInfo = new NetworkCredential(outgoingUserName, outgoingPassword);
                client.Credentials = basicAuthenticationInfo;
                client.EnableSsl = true;
                client.Port = 587; //port

                // add from,to mailaddresses
                var from = new MailAddress(emailAddress, "Renter");
                var to = new MailAddress("sun.kiss.me@yandex.com");
                var mail = new MailMessage(from, to);

                // set subject and encoding
                mail.Subject = locationName + " has received your web request";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                mail.Body = "Expect a response within 24 hours or less, on normal work days. You are always welcome to contact us directly at " + location.PrimaryPhone;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(mail);
                return Ok();
            }
            catch
            {
                return null;
            }
        }
    }
}
