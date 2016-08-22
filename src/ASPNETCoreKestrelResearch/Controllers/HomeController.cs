using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreKestrelResearch.Data;
using ASPNETCoreKestrelResearch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ASPNETCoreKestrelResearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerFactory _loggerFactory;
        public HomeController(ApplicationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _loggerFactory = loggerFactory;
        }
        public IActionResult Index()
        {
            ViewData["IPAddress"] = this.ControllerContext.HttpContext.Connection.LocalIpAddress.ToString();
            ViewData["Port"] = this.ControllerContext.HttpContext.Connection.LocalPort.ToString();
            return View();
        }

        [Authorize]
        public IActionResult TestOIDC()
        {
            var logger = _loggerFactory.CreateLogger("TestOIDC Controller");
            logger.LogInformation("entered testoidc action");
            List<string> messages = new List<string>()
            {
                $"Username: {User.Identity.Name}",
                $"Authenticated: {User.Identity.IsAuthenticated}",
                $"Claims:"
            };
            messages.AddRange(User.Claims.Select(c => $"{c.Type}: {c.Value}"));

            OIDCUser user = _dbContext.OIDCUsers.FirstOrDefault(u => u.Subject == User.Claims.First(c => c.Type == "sub").Value);
            if(user!=null)
            {
                logger.LogInformation("user found!");
                messages.Add($"Database ID: {user.Id}");
            }
            else
                logger.LogInformation("user not found");
            return View(messages);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
