using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreProj.Models;
using NetCoreProj.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreProj.Controllers
{
    [Authorize]
    //my comments for branch testing
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger logger;

        //Constructor Injection
        public HomeController(IEmployee IEmpRepository, IWebHostEnvironment hostEnvironment, ILogger<HomeController> logger)
        {
            _emp_repository = IEmpRepository;
            _hostEnvironment = hostEnvironment;
            this.logger = logger;
        }

        //Attribute routing not working
        //[Route("")]
        //[Route("Home")]
        //[Route("Home/Index")]
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

        //Attribute routing not working
        //[Route("Home/DetailsView/{id?}")]
        //To get XML output
        [AllowAnonymous]
        public ViewResult DetailsView(int? id)
        {
            //throw new Exception("Error in details view");

            //logger.LogTrace("mTrace Log");
            //logger.LogDebug("mDebug Log");
            //logger.LogInformation("mInformation Log");
            //logger.LogWarning("mWarning Log");
            //logger.LogError("mError Log");
            //logger.LogCritical("mCritical Log");

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

            MEmployee emp = _emp_repository.GetEmployee(id.Value);

            if (emp == null)
            {
                //throw new Exception("Error in details view");

                Response.StatusCode = 404;
                return View("EmployeeNotFoundView", id.Value);
            }

            //View Model
            HomeDetailsViewModel oVM = new HomeDetailsViewModel()
            {
                vmEmployee = _emp_repository.GetEmployee(id ?? 1),
                vmPageTitle = "Employee Data using Vm"
            };
            return View(oVM);
        }

        //List View
        [AllowAnonymous]
        public ViewResult Listview()
        {
            var model = _emp_repository.GetAllEmployees();
            return View(model);
        }

        [HttpGet]
        public ViewResult Createview()
        {
            return View();
        }

        [HttpPost]
        //FOR IN-MEMORY
        //public IActionResult Createview(MEmployee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MEmployee newemployee = _emp_repository.AddEmployee(employee);
        //        return RedirectToAction("detailsview", new { id = newemployee.EmpID });
        //    }
        //    return View();
        //}

        //FOR SQL
        public IActionResult Createview(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniquefilename = null;
                if (model.Photo != null)
                {
                    string uploadFolderPath = ProcessUploadedFile(model);
                    uniquefilename = uploadFolderPath;
                    //string uploadFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                    //uniquefilename = Guid.NewGuid().ToString() + "-" + model.Photo.FileName;
                    //string filepath = Path.Combine(uploadFolderPath, uniquefilename);
                    //model.Photo.CopyTo(new FileStream(filepath, FileMode.Create));
                }

                MEmployee newemployee = new MEmployee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Dept = model.Dept,
                    PhotoPath = uniquefilename
                };

                _emp_repository.AddEmployee(newemployee);

                return RedirectToAction("detailsview", new { id = newemployee.EmpID });
            }
            return View();
        }

        [HttpGet]
        public ViewResult EditView(int id)
        {
            MEmployee emp = _emp_repository.GetEmployee(id);

            if (emp == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFoundView", id);
            }

            EmployeeEditViewModel empeditvm = new EmployeeEditViewModel
            {
                EmpID = emp.EmpID,
                Name = emp.Name,
                Email = emp.Email,
                Dept = emp.Dept,
                ExistingPhotoPath = emp.PhotoPath
            };

            return View(empeditvm);
        }

        [HttpPost]
        public IActionResult EditView(EmployeeEditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                MEmployee employee = _emp_repository.GetEmployee(model.EmpID);
                // Update the employee object with the data in the model object
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Dept = model.Dept;

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Photo != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath,
                            "uploads", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                MEmployee updatedEmployee = _emp_repository.UpdateEmployee(employee);

                return RedirectToAction("listview");
            }

            return View(model);
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult DeleteView(int id) 
        {
            //self written code. need to cover delete of photo too.
            MEmployee deletedEmployee = _emp_repository.DeleteEmployee(id);
            return RedirectToAction("listview");
        }
    }
}
