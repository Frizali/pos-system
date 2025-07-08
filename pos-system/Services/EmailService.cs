using DotLiquid;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using pos_system.DTOs;
using pos_system.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pos_system.Services
{
    public class EmailService : IEmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "m.fahrizalipradana@gmail.com";
        private readonly string smtpPass = "ueyw dapo autg ycww";

        private async Task SendNotificationAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Poin Of Sale", smtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUser, smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendStockNotification(PartDTO data, string toEmail)
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "MailBody", "StockNotification.txt");
            string templateContent = await System.IO.File.ReadAllTextAsync(templatePath);

            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            var template = Template.Parse(templateContent);
            string htmlBody = template.Render(Hash.FromAnonymousObject(data));

            await SendNotificationAsync(toEmail, "Peringatan: Stok Barang Menipis!", htmlBody);
        }

        public async Task SendPreOrderNotification(PreOrderMailDTO order, string toEmail)
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "MailBody", "PreOrderNotification.txt");
            string templateContent = await System.IO.File.ReadAllTextAsync(templatePath);

            var items = order.items.Select(x => new
            {
                x.ProductName,
                x.Quantity
            }).ToList();

            var liquidData = new
            {
                order.Username,
                order.Email,
                order.ScheduledAt,
                order.TotalPrice,
                items
            };

            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            var template = Template.Parse(templateContent);
            string htmlBody = template.Render(Hash.FromAnonymousObject(liquidData));

            await SendNotificationAsync(toEmail, "Notifikasi: Pre-Order", htmlBody);
        }

        public async Task SendPreOrderFeedbackNotification(TblOrder order, string toEmail)
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "MailBody", "PreOrderFeedbackNotification.txt");
            string templateContent = await System.IO.File.ReadAllTextAsync(templatePath);

            var liquidData = new
            {
                Username = order.Cashier,
                order.ScheduledAt,
                order.TotalPrice,
                PreOrderStatus = order.PreOrderStatus == "Awaiting Payment" ? "Approved" : "Rejected",
                order.Comment
            };

            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            var template = Template.Parse(templateContent);
            string htmlBody = template.Render(Hash.FromAnonymousObject(liquidData));

            await SendNotificationAsync(toEmail, "Notifikasi: Pre-Order", htmlBody);
        }
    }
}
