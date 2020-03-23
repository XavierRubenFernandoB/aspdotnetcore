using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.Models
{
    public interface IEmployee
    {
        MEmployee GetEmployee(int EmpID);
        IEnumerable<MEmployee> GetAllEmployees();
        MEmployee AddEmployee(MEmployee employee);
        MEmployee UpdateEmployee(MEmployee employeechanges);
        MEmployee DeleteEmployee(int EmpID);
    }
}
