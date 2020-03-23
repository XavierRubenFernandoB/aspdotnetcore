using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.Models
{
    public class MockIEmployee : IEmployee
    {
        private List<MEmployee> _employeeList;

        public MockIEmployee()
        {
            _employeeList = new List<MEmployee>()
            {
                new MEmployee() { EmpID=1, Name="Calvyn", Email="calvyn.xavier@gmail.com", Dept=eDept.Executive },
                new MEmployee() { EmpID=2, Name="Madhumitha", Email="madhu.xavier@gmail.com", Dept=eDept.HR },
                new MEmployee() { EmpID=3, Name="Xavier", Email="ruben.xavier@gmail.com", Dept=eDept.IT }
            };
        }

        public MEmployee GetEmployee(int EmpID)
        {
            try
            {
                return _employeeList.FirstOrDefault(e => e.EmpID == EmpID);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<MEmployee> GetAllEmployees()
        {
            return _employeeList;
        }

        public MEmployee AddEmployee(MEmployee employee)
        {
            employee.EmpID = _employeeList.Max(e => e.EmpID) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public MEmployee UpdateEmployee(MEmployee employeechanges)
        {
            MEmployee modifyemp = _employeeList.FirstOrDefault(e => e.EmpID == employeechanges.EmpID);
            if (modifyemp != null)
            {
                modifyemp.Name = employeechanges.Name;
                modifyemp.Email = employeechanges.Email;
                modifyemp.Dept = employeechanges.Dept;
            }
            return modifyemp;
        }

        public MEmployee DeleteEmployee(int EmpID)
        {
            MEmployee delemp =_employeeList.FirstOrDefault(e => e.EmpID == EmpID);
            if (delemp != null)
            {
                _employeeList.Remove(delemp);
            }
            return delemp;
        }
    }
}
