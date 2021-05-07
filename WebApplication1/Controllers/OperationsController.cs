using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class OperationsController : Controller
    {
        private DataContext db = new DataContext();

        //GET: Operations
        public ActionResult Index(string sortOrder, string sortingField, string searchFilter)
        {
           
            ViewData["SortingField"] = sortingField;
            ViewData["SortOrder"] = sortOrder;
            IEnumerable<Operation> operations = db.Operations;
            if (searchFilter != null)
            {
                operations = operations.ToList().Where(s => s.name.Contains(searchFilter));
            }
            switch(sortingField)
            {
                case "name":
                    if (sortOrder == "ascending")
                        return View(operations.OrderByDescending(s => s.name).Reverse());
                    else
                        return View(operations.OrderByDescending(s => s.name));
                case "creationDate":
                    if (sortOrder == "ascending")
                        return View(operations.OrderByDescending(s => s.creationDate).Reverse());
                    else
                        return View(operations.OrderByDescending(s => s.creationDate));
                case "amount":
                    if (sortOrder == "ascending")
                        return View(operations.OrderByDescending(s => s.amount).Reverse());
                    else
                        return View(operations.OrderByDescending(s => s.amount));
                case "type":
                    if (sortOrder == "ascending")
                        return View(operations.OrderByDescending(s => s.type).Reverse());
                    else
                        return View(operations.OrderByDescending(s => s.type));
                default:
                    return View(operations.OrderByDescending(s => s.Id).Reverse());
            }
            
        }

        ////public async Task<ActionResult> Index(string sortOrder)
        //public ActionResult Index(string sortOrder)
        //{
        //    //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
        //    //ViewData["DateSortParm"] = sortOrder == "Date" ? "creationDate" : "Date";
        //    ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
        //    ViewData["DateSortParm"] = sortOrder == "Date" ? "creationDate" : "";
        //    Console.WriteLine(sortOrder);
        //    var operations = from s in db.Operations.ToList()
        //                   select s;
        //    switch (sortOrder)
        //    {
        //        case "name":
        //            operations = operations.OrderByDescending(s => s.name);
        //            break;
        //        case "creationDate":
        //            operations = operations.OrderBy(s => s.creationDate);
        //            break;
        //        default:
        //            operations = operations.OrderBy(s => s.Id);
        //            break;
        //    }
        //    //return View(await operatoins.AsNoTracking().ToListAsync());
        //    return View(operations);

        //}

        public ActionResult CreateDocument()
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();

            //Add values to list
            Dictionary<string, int> x = CalculateMonthlyExpense();
            List<object> data = new List<object>();
            int total = 0;
            foreach (KeyValuePair<string, int> entry in x)
            {
                data.Add(new
                {
                    category = entry.Key,
                    amount = entry.Value
                });
                total += entry.Value;
            }
            data.Add(new
            {
                category = "Total",
                amount = total
            });

            //Add list to IEnumerable
            IEnumerable<object> dataTable = data;
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
            //Save the PDF document to stream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            doc.Close(true);
            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";
            //Define the file name.
            string fileName = "MONTHLY_REPORT.pdf";

            Report r = new Report();
            r.Content = data.ToString();
            r.Title = "Monthly report " + DateTime.Now.Month;
            r.fromDate = DateTime.Now.AddMonths(-1);
            r.toDate = DateTime.Now;
            r.UserId = 1;
            db.Reports.Add(r);
            db.SaveChanges();

            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return File(stream, contentType, fileName);
        }


// GET: Operations/Details/5
public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // GET: Operations/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        [HttpGet]
        public JsonResult GetMonthlyExpense()
        {
            Dictionary<string, int> monthlyExpense = this.CalculateMonthlyExpense();
            System.Diagnostics.Debug.WriteLine(monthlyExpense);
            //            return new JsonResult(monthlyExpense);
            return Json(monthlyExpense);
        }

        [HttpGet]
        public JsonResult GetWeeklyExpense()
        {
            Dictionary<string, int> weeklyExpense = this.CalculateWeeklyExpense();

            System.Diagnostics.Debug.WriteLine(weeklyExpense["Food"]);
            //return new JsonResult(weeklyExpense);
            return Json(weeklyExpense.ToString());
        }

        // POST: Operations/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,fromAccountId,toAccountId,UserId,amount,creationDate,type")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                db.Operations.Add(operation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(operation);
        }

        // GET: Operations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // POST: Operations/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,fromAccountId,toAccountId,UserId,amount,creationDate,type")] Operation operation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(operation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(operation);
        }

        // GET: Operations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = db.Operations.Find(id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Operation operation = db.Operations.Find(id);
            db.Operations.Remove(operation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public Dictionary<string, int> CalculateMonthlyExpense()
        {
            List<Operation> lstEmployee = new List<Operation>();

            Dictionary<string, int> dictMonthlySum = new Dictionary<string, int>();

            int foodSum = db.Operations.Where
                (cat => cat.type == "Food" && (cat.creationDate > EntityFunctions.AddMonths(DateTime.Now, -1)))
                .Sum(cat => (int?)cat.amount) ?? 0;

            int shoppingSum = db.Operations.Where
               (cat => cat.type == "Shopping" && (cat.creationDate > EntityFunctions.AddMonths(DateTime.Now, -1)))
               .Sum(cat => (int?)cat.amount) ?? 0;

            int travelSum = db.Operations.Where
               (cat => cat.type == "Travel" && (cat.creationDate > EntityFunctions.AddMonths(DateTime.Now, -1)))
               .Sum(cat => (int?)cat.amount) ?? 0;

            int healthSum = db.Operations.Where
               (cat => cat.type == "Health" && (cat.creationDate > EntityFunctions.AddMonths(DateTime.Now, -1)))
               .Sum(cat => (int?)cat.amount) ?? 0;


            dictMonthlySum.Add("Food", foodSum);
            dictMonthlySum.Add("Shopping", shoppingSum);
            dictMonthlySum.Add("Travel", travelSum);
            dictMonthlySum.Add("Health", healthSum);

            return dictMonthlySum;
        }

        public Dictionary<string, int> CalculateWeeklyExpense()
        {
            List<Operation> lstEmployee = new List<Operation>();

            Dictionary<string, int> dictWeeklySum = new Dictionary<string, int>();

            int foodSum = db.Operations.Where
                (cat => cat.type == "Food" && (cat.creationDate > EntityFunctions.AddDays(DateTime.Now, -7)))
                .Sum(cat => (int?)cat.amount) ?? 0;

            int shoppingSum = db.Operations.Where
               (cat => cat.type == "Shopping" && (cat.creationDate > EntityFunctions.AddDays(DateTime.Now, -7)))
               .Sum(cat => (int?)cat.amount) ?? 0;

            int travelSum = db.Operations.Where
               (cat => cat.type == "Travel" && (cat.creationDate > EntityFunctions.AddDays(DateTime.Now, -7)))
               .Sum(cat => (int?)cat.amount) ?? 0;

            int healthSum = db.Operations.Where
               (cat => cat.type == "Health" && (cat.creationDate > EntityFunctions.AddDays(DateTime.Now, -7)))
               .Sum(cat => (int?)cat.amount) ?? 0;

            //int travelSum = db.Operations.Where
            //   (cat => cat.type == "Travel" && (cat.creationDate > DateTime.Now.AddDays(-28)))
            //   .Select(cat => cat.amount)
            //   .Sum();

            //int healthSum = db.Operations.Where
            //   (cat => cat.type == "Health" && (cat.creationDate > DateTime.Now.AddDays(-28)))
            //   .Select(cat => cat.amount)
            //   .Sum();

            dictWeeklySum.Add("Food", foodSum);
            dictWeeklySum.Add("Shopping", shoppingSum);
            dictWeeklySum.Add("Travel", travelSum);
            dictWeeklySum.Add("Health", healthSum);

            return dictWeeklySum;
        }

        [HttpPost]
        [ActionName("filter")]
        public ActionResult filter(string SearchString)
        {
            if (SearchString == null)
            {
                return RedirectToAction("Index");
            }
            //return View(db.Operations.ToList().Where(o => o.name.Contains(SearchString)));
            return View("Index", db.Operations.ToList().Where(o => o.name.Contains(SearchString)));
        }

    }
}
