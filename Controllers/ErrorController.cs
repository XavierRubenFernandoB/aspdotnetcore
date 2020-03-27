using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreProj.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger; 

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // GET: /<controller>/
        [Route("Error/{statuscode}")]
        public IActionResult MyHttpStatusCodeHandler(int statuscode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page could not be found";

                    _logger.LogWarning($"404 Error Occured. Path " + $"{statusCodeResult.OriginalPath}" + 
                        " and Query String " + $"{statusCodeResult.OriginalQueryString}");

                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QS = statusCodeResult.OriginalQueryString;

                    break;
            }
            return View("Notfound");
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            // Retrieve the exception Details
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            _logger.LogError($"The path { exceptionHandlerPathFeature.Path } threw an exception " + $"{ exceptionHandlerPathFeature.Error }");

            return View("Error");
        }
    }
}
