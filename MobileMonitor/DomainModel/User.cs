namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
   
        public int UserID { get; set; }
        [Required()]
        public string UserName { get; set; }
        [Required()]
        public string Email { get; set; }
        [Required()]
        public string Password { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<UserServer> Servers { get; set; }
    }
}
