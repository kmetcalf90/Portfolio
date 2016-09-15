using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;

namespace WebApplication9
{
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index()
        {
            return View(db.BlogPosts.ToList());
        }
        [HttpPost]
        public ActionResult Index(string searchStr)
        {


            var result = db.BlogPosts.Where(p => p.Body.Contains(searchStr))
                .Union(db.BlogPosts.Where(p => p.Title.Contains(searchStr)))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.Body.Contains(searchStr))))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.Author.DisplayName.Contains(searchStr))))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.Author.FirstName.Contains(searchStr))))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.Author.LastName.Contains(searchStr))))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.Author.UserName.Contains(searchStr))))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.Author.Email.Contains(searchStr))))
                .Union(db.BlogPosts.Where(p => p.Comments.Any(c => c.UpdateReason.Contains(searchStr))));

            return View(result.ToList());

        }


        // GET: BlogPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPosts = db.BlogPosts.Find(id);
            if (blogPosts == null)
            {
                return HttpNotFound();
            }
            return View(blogPosts);
        }

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,Published")] BlogPosts blogPosts)
        {
            if (ModelState.IsValid)
            {
                db.BlogPosts.Add(blogPosts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPosts);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPosts = db.BlogPosts.Find(id);
            if (blogPosts == null)
            {
                return HttpNotFound();
            }
            return View(blogPosts);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,Published")] BlogPosts blogPosts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPosts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPosts);
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPosts = db.BlogPosts.Find(id);
            if (blogPosts == null)
            {
                return HttpNotFound();
            }
            return View(blogPosts);
        }

        // POST: BlogPosts/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPosts blogPosts = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPosts);
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
    }
}
