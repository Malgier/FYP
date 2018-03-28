using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using DAL;
using DomainModel;
using System.Net;

namespace BLL
{
    public class SendWarning
    {
        StoredProcedureCalls sprocs;

        public SendWarning()
        {
            sprocs = new StoredProcedureCalls();
        }

        public void SendEmail(int serverID, string hardware)
        {
            List<int> userIds = sprocs.ReturnUsersByServer(serverID);
            List<User> users = new List<User>();
            Server server = sprocs.ReturnServer(serverID);
            string warningPoint = "";

            if (hardware == "CPU")
                warningPoint = "over " + server.CPUWarningPoint.ToString() + "%";
            else if (hardware == "RAM")
                warningPoint = "under " + server.RAMWarningPoint.ToString() + "MB";
            else if (hardware == "Net")
                warningPoint = "over " + server.NetworkWarningPoint.ToString() + "%";

            foreach (int id in userIds)
            {
                users.Add(sprocs.ReturnUser("", id));
            }

            foreach (User user in users)
            {

                var fromAddress = new MailAddress("mobilemonitorfyp@gmail.com", "Mobile Monitor");
                var toAddress = new MailAddress(user.Email, user.UserName);
                const string fromPassword = "Cheesebiscuit123";
                string subject = "Warning for server" + server.ServerName + "!";
                string body = "This email is to warn you that server " + server.ServerName + "'s " + hardware + " has been running " + warningPoint + " for " + server.TimeWarning + " seconds!";
                ;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
        }
    }
}
