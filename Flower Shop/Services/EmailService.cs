using System.Net;
using System.Net.Mail;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FlowerShopOnlineOrderSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _options;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public Task SendOrderConfirmationAsync(Order order)
        {
            var subject = $"Flower Shop order #{order.OrderId} confirmation";
            var body = $"Thank you for your order. Your total is {order.TotalPrice:C} and the current status is {order.Status}.";
            return SendAsync(order.Customer?.Email, subject, body);
        }

        public Task SendOrderStatusUpdateAsync(Order order)
        {
            var subject = $"Flower Shop order #{order.OrderId} status update";
            var body = $"Your order status is now {order.Status}.";
            return SendAsync(order.Customer?.Email, subject, body);
        }

        private async Task SendAsync(string? toAddress, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toAddress))
            {
                _logger.LogWarning("Skipped email '{Subject}' because the order has no customer email.", subject);
                return;
            }

            if (!_options.EnableSmtp || string.IsNullOrWhiteSpace(_options.Host))
            {
                _logger.LogInformation("Email notification to {Email}: {Subject} - {Body}", toAddress, subject, body);
                return;
            }

            using var message = new MailMessage
            {
                From = new MailAddress(_options.FromAddress, _options.FromName),
                Subject = subject,
                Body = body
            };
            message.To.Add(toAddress);

            using var client = new SmtpClient(_options.Host, _options.Port)
            {
                EnableSsl = _options.EnableSsl
            };

            if (!string.IsNullOrWhiteSpace(_options.UserName))
            {
                client.Credentials = new NetworkCredential(_options.UserName, _options.Password);
            }

            await client.SendMailAsync(message);
        }
    }
}
