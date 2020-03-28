using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreProj.ViewModels
{
    public class UserClaimsViewModel
    {
        //Just to avoid null reference error
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
