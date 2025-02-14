﻿using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
     public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message) 
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);

        }

        private MimeMessage CreateEmailMessage (Message message)
        {
            var emailMessage = new MimeMessage();
            string nameEmail = string.Empty;
            emailMessage.From.Add(new MailboxAddress(nameEmail , _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch 
                {
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }
    }
}
