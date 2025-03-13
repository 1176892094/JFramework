// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-11 18:01:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JFramework
{
    public static partial class Service
    {
        public static class Mail
        {
            public static async void Send(MailData mailData)
            {
                try
                {
                    if (string.IsNullOrEmpty(mailData.senderAddress))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(mailData.senderPassword))
                    {
                        return;
                    }

                    await Task.Run(() =>
                    {
                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(mailData.senderAddress, mailData.senderName),
                            Subject = mailData.mailName,
                            Body = mailData.mailBody,
                            IsBodyHtml = false,
                        };
                        mailMessage.To.Add(mailData.targetAddress);

                        var smtpClient = new SmtpClient(mailData.smtpServer, mailData.smtpPort)
                        {
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(mailData.senderAddress, mailData.senderPassword),
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Timeout = 20000
                        };

                        smtpClient.Send(mailMessage);
                    });
                }
                catch (SmtpException e)
                {
                    throw new SmtpException(e.ToString());
                }
            }
        }
    }
}