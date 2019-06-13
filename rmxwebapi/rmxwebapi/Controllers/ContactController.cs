using rmxwebapi.Models;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace rmxwebapi.Controllers
{
    public class ContactController : ApiController
    {
        private DataRepository _repo;

        public ContactController()
        {
            _repo = new DataRepository();
        }

        [HttpPost]
        public IHttpActionResult Post(ContactModel contact)
        {
            var location = _repo.GetAllLocations().Where(x => x.Oid == contact.Location).FirstOrDefault();
            if (location == null)
                return null;

            var isReceivedToRenter = SendEmail(location, contact, true);

            if (isReceivedToRenter)
            {
                var isReceivedToPrimaryObject = SendEmail(location, contact, false);
                if (isReceivedToPrimaryObject)
                    return Ok();

                return null;
            }
            return null;
        }

        private bool SendEmail(LocationModel location, ContactModel contact, bool isRenter)
        {
            var toEmail = "";
            var subject = "";
            var body = "";
            if (isRenter)
            {
                toEmail = contact.Email;
                subject = location.Name + " has received your web request";
                body = "Expect a response within 24 hours or less, on normal work days. You are always welcome to contact us directly at " + location.PrimaryPhone;
            }
            else
            {
                toEmail = location.WebEmailContactUsToAddress;
                subject = contact.Name + "'s request has received from " + location.Name;
                var detail = string.IsNullOrEmpty(contact.Comment) == false ? "<br />Detail - " + contact.Comment : "";
                body = "Contact Informations are the followings : <br /><br />Location - " + location.Name + "<br />Renter - " + contact.Name + "<br />Email - " + contact.Email +
                    "<br />Phone - " + contact.Phone + "<br />Subject - " + contact.Subject + detail;
            }
            try
            {
                var client = new SmtpClient(location.OutgoingServerName);

                client.UseDefaultCredentials = false;
                var basicAuthenticationInfo = new NetworkCredential(location.OutgoingUserName, location.OutgoingPassword);
                client.Credentials = basicAuthenticationInfo;
                client.EnableSsl = true;
                client.Port = 587;

                var from = new MailAddress(location.PrimaryEmail, "Rent Agent");
                var to = new MailAddress(toEmail);
                var mail = new MailMessage(from, to);

                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = body;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
