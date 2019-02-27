using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class UserProfile
    {
        [Key]
        public Guid LoginID { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public DateTime LastSuccessfulLogin { get; set; }
    }
}
