using System.Web.Mvc;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("_Index");
        }
    }
}