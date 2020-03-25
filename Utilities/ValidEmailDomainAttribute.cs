using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedmyDomain)
        {
            this.allowedDomain = allowedmyDomain;
        }

        public override bool IsValid(object value)
        {
            string[] arrstring = value.ToString().Split("@");
            return arrstring[1].ToUpper() == this.allowedDomain.ToUpper();
        }
    }
}