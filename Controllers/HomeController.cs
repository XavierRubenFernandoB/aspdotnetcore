using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreProj.Models;
using NetCoreProj.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreProj.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/

        //public string Index()
        //{
        //    return "Hellow MVC";
        //}

        //public JsonResult Index()
        //{
        //    return Json(new { id = 1 });
        //}

        private readonly IEmployee _emp_repository; //readonly to make it secure

        //Constructor Injection
        public HomeController(IEmployee IEmpRepository)
        {
            _emp_repository = IEmpRepository;
        }

        public string Index()
        {
            return _emp_repository.GetEmployee(1).Name;
        }

        public JsonResult DetailsJson()
        {
            MEmployee model = _emp_repository.GetEmployee(1);
            return Json(model);
        }

        //To get XML output
        public ObjectResult DetailsXml()
        {
            MEmployee model = _emp_repository.GetEmployee(1);
            return new ObjectResult(model);
        }

        //To get XML output
        public ViewResult DetailsView()
        {
            //MEmployee model = _emp_repository.GetEmployee(1);
            //return View(model);

            //View Data
            //MEmployee model = _emp_repository.GetEmployee(1);
            //ViewData["vdHeader"] = "Employee Data";
            //ViewData["vdEmpDetails"] = model;
            //return View("../Test/Test");

            //View Bag
            //MEmployee model = _emp_repository.GetEmployee(1);
            //ViewBag.vbHeader = "Employee Data";
            //ViewBag.vbEmpDetails = model;
            //return View("../Test/ViewBag");

            //Strongly Typed View
            //MEmployee omodel = _emp_repository.GetEmployee(1);
            //ViewBag.vbHeader = "Employee Data";
            //return View(omodel);

            //View Model
            HomeDetailsViewModel oVM = new HomeDetailsViewModel()
            {
                vmEmployee = _emp_repository.GetEmployee(1),
                vmPageTitle = "Employee Data using Vm"
            };
            return View(oVM);
        }

        //List View
        public ViewResult Listview()
        {
            var model = _emp_repository.GetAllEmployees();
            return View(model);
        }
    }
}
