using DAL;
using DomainModel;

namespace MobileMonitor.Login
{
    public class UserRepository
    {
        StoredProcedureCalls sproc = new StoredProcedureCalls();

        public User GetByUsernameAndPassword(User user)
        {
            return sproc.ReturnUser(user.UserName);
        }
    }
}