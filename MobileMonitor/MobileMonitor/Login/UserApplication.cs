using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobileMonitor.Models;

namespace MobileMonitor.Login
{
    public class UserApplication
    {
        UserRepository userRepo = new UserRepository();
        public User GetByUsernameAndPassword(User user)
        {
            return userRepo.GetByUsernameAndPassword(user);
        }
    }
}