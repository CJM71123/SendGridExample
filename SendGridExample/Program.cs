using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendGridExample
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            SendEmail().Wait();
            BuildWebHost(args).Run();
        }

        static async Task SendEmail()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            var sendGridKey = Configuration["SendGridApiKey"];
            var client = new SendGridClient(sendGridKey);

            var message = new SendGridMessage
            {
                From = new EmailAddress("test@members1st.org", "Members1st - Test"),
                Subject = "This is a test to try out SendGrid",
                HtmlContent = "<strong>This is a test of sending HTML content!</strong> <u>I hope this works!</u>",
            };

            message.AddTo(new EmailAddress("CJM71123@gmail.com", "Clinton McClure"));

            var response = await client.SendEmailAsync(message);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
