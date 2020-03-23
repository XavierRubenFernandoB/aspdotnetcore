using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.ViewModels
{
    //Inheriting in order to avoid duplicates and re-use of code. Only EmpID & Existing photo path are required
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public int EmpID { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
