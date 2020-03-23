using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.Models
{
    public class SQLIEmployeeRepository : IEmployee
    {
        private readonly AppDbContext context;
        private readonly ILogger logger;

        public SQLIEmployeeRepository(AppDbContext context, ILogger<SQLIEmployeeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public MEmployee AddEmployee(MEmployee employee)
        {
            context.EmployeesDBSet.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public MEmployee DeleteEmployee(int EmpID)
        {
            MEmployee delemp = context.EmployeesDBSet.Find(EmpID);
            if (delemp != null)
            {
                context.Remove(delemp);
                context.SaveChanges();
            }
            return delemp;
        }

        public IEnumerable<MEmployee> GetAllEmployees()
        {
            return context.EmployeesDBSet;
        }

        public MEmployee GetEmployee(int EmpID)
        {
            //logger.LogTrace("mTrace Log");
            //logger.LogDebug("mDebug Log");
            //logger.LogInformation("mInformation Log");
            //logger.LogWarning("mWarning Log");
            //logger.LogError("mError Log");
            //logger.LogCritical("mCritical Log");
            return context.EmployeesDBSet.Find(EmpID);
        }

        public MEmployee UpdateEmployee(MEmployee employeechanges)
        {
            var employee = context.EmployeesDBSet.Attach(employeechanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
             
            return employeechanges;
        }
    }
}
