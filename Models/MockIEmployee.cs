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
                new MEmployee() { EmpID=1, Name="Calvyn", Email="calvyn.xavier@gmail.com", Dept="Executive" },
                new MEmployee() { EmpID=2, Name="Madhumitha", Email="madhu.xavier@gmail.com", Dept="HR" },
                new MEmployee() { EmpID=2, Name="Xavier", Email="ruben.xavier@gmail.com", Dept="IT" }
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
    }
}
