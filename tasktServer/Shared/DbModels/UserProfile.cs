using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Shared.DbModels
{
    public class UserProfile
    {
        [Key]
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LastSuccessfulLogin { get; set; }
    }
}
