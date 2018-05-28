using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ATM.Models;

namespace ATM.Controllers
{
    public class OperationsController : Controller
    {
        private BankDBEntities db = new BankDBEntities();
        
        // GET: Operations

        public ActionResult Index(LoginModel model)
        {
           
            
            var card = db.Card.FirstOrDefault(u => u.CardID == model.CardId && u.PinCode == model.PinCode); //Операции по конкретной карточке.
            var cardID = card.CardID;
            var operations = db.Operations.Include(o => o.Card).Include(o => o.Card1).Where(o => o.InID == cardID  || o.OutID == cardID);
            
            ViewBag.CardID = model.CardId; //Для определения типа операции.
            return View(operations.ToList());
        }

        // GET: Operations/Create
        public ActionResult Create()
        {
            ViewBag.InID = new SelectList(db.Card, "CardID", "CardID");
            ViewBag.OutID = new SelectList(db.Card, "CardID", "CardID");
            return View();
        }

        // POST: Operations/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OperationID,InID,OutID,Amount,OperationTime")] Operations operations)
        {
            try
            {
                Card card = new Card();
                
                if (ModelState.IsValid)
                {
                    if ((db.Card.FirstOrDefault(u => u.CardID == operations.OutID && u.Balance >= operations.Amount) != null)) //Проверка баланса карточки.
                    {

                        operations.OperationTime = System.DateTime.Now;
                        db.Operations.Add(operations);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            ViewBag.InID = new SelectList(db.Card, "CardID", "CardID", operations.InID);
            ViewBag.OutID = new SelectList(db.Card, "CardID", "CardID", operations.OutID);
            return View(operations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } 
    }
}
