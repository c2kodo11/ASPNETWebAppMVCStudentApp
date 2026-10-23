using System.Linq;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserName"] != null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = db.tblUsers.FirstOrDefault(u =>
                u.Username == model.Username && u.Password == model.Password && u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password. Please try again.");
                return View(model);
            }

            Session["UserName"] = user.Username;
            Session["FullName"] = user.FullName ?? user.Username;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}