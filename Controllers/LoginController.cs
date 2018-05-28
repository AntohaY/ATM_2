using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ATM.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Card card = null;

                using(BankDBEntities db = new BankDBEntities())
                {
                    card = db.Card.FirstOrDefault(c => c.CardID == model.CardId && c.PinCode == model.PinCode); //Ищем карточку с таким ID и PinCode.
                }
                if (card != null)
                {
                    FormsAuthentication.SetAuthCookie(model.CardId, false);
                    return RedirectToAction("Index", "Operations",model);
                }
                else
                {
                    ModelState.AddModelError("", "Login information was incorrect or non existent");
                }
            }
                return View(model);
        }
    }
}