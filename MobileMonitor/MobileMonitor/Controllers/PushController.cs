using BLL;
using System.Threading.Tasks;
using System.Web.Http;

namespace MobileMonitor.Controllers
{
    public class PushController : ApiController
    {
        [HttpGet]
        public void Get(int serverID, string hardware)
        {
            SendWarning warning = new SendWarning();
            warning.SendEmail(serverID, hardware);
        }
    }
}