using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Azure_Assignment.EF;
using Microsoft.Ajax.Utilities;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private DataPalkia db = new DataPalkia();

        // GET: Admin/Categories
        public ActionResult Index()
        {

            //System.Net.FtpWebRequest tmpReq = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create("ftp://domain.com/file.txt");
            //tmpReq.Credentials = new System.Net.NetworkCredential("userName", "Password");

            //using (System.Net.WebResponse tmpRes = tmpReq.GetResponse())
            //{
            //    using (System.IO.Stream tmpStream = tmpRes.GetResponseStream())
            //    {
            //        using (System.IO.TextReader tmpReader = new System.IO.StreamReader(tmpStream))
            //        {
            //            string fileContents = tmpReader.ReadToEnd();
            //        }
            //    }
            //}
            return View(db.Categories.ToList());
        }

        

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Description,Picture,ImageFile")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(categories.ImageFile.FileName);
                string extension = Path.GetExtension(categories.ImageFile.FileName);
                if((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                {
                    ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                    ViewBag.ImageFileCategory = categories.ImageFile;
                    return View("Create");
                }

                long fileSize = ((categories.ImageFile.ContentLength) / 1024);
                if (fileSize > 5120)
                {
                    ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                    ViewBag.ImageFileCategory = categories.ImageFile;
                    return View("Create");
                }
                categories.Picture = categories.ImageFile.FileName;  //categories.ImageFile.FileName;
                /**
                fileName = fileName+ DateTime.Now.ToString("yymmssfff")  + extension; 
                categories.Picture = "~/public/uploadedFiles/categoryPictures/" + fileName;  //categories.ImageFile.FileName;
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/categoryPictures/");

                 if (Directory.Exists(uploadFolderPath) == false)
                 {
                     Directory.CreateDirectory(uploadFolderPath);
                 }

                 fileName = Path.Combine(uploadFolderPath, fileName);

                 categories.ImageFile.SaveAs(fileName);**/


                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://156.67.222.163:21/NhomHoangTam/imgCategories" + categories.ImageFile.FileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential("u657022003.ftpuser", "123456789-Aa");

                byte[] fileContents;
                using (Stream inputStream = categories.ImageFile.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    fileContents = memoryStream.ToArray();
                }

                //using (StreamReader sourceStream = new StreamReader(fileName))
                //{
                //    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                //}

                request.ContentLength = fileContents.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
                

                db.Categories.Add(categories);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                ModelState.Clear();
                return RedirectToAction("Index");
                
            }

            return View(categories);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            Session["OldImage"] = categories.Picture;
            return View(categories);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description,Picture,ImageFile")] Categories categories, String imageOldFile)
        {
            if (ModelState.IsValid)
            {
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/categoryPictures/");
                if (categories.ImageFile == null)
                {
                    categories.Picture = imageOldFile;
                }
                else
                {
                    if (!imageOldFile.IsEmpty())
                    {
                        System.IO.File.Delete(Server.MapPath(imageOldFile));
                    }

                    string fileName = Path.GetFileNameWithoutExtension(categories.ImageFile.FileName);

                    string extension = Path.GetExtension(categories.ImageFile.FileName);
                    if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                    {
                        ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                        return View("Edit");
                    }

                    long fileSize = ((categories.ImageFile.ContentLength) / 1024);
                    if (fileSize > 5120)
                    {
                        ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                        return View("Edit");
                    }

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    categories.Picture = "~/public/uploadedFiles/categoryPictures/" + fileName;


                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    categories.ImageFile.SaveAs(fileName);
                }
                db.Entry(categories).State = EntityState.Modified;
                db.SaveChanges();
                Session.Remove("OldImage");
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
                
            }
            return View(categories);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = db.Categories.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Categories categories = db.Categories.Find(id);
                /**
                if (!categories.Picture.IsEmpty())
                {
                    System.IO.File.Delete(Server.MapPath(categories.Picture));
                }
                **/

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://156.67.222.163:21/NhomHoangTam/imgCategories" + categories.Picture);
                request.Credentials = new NetworkCredential("u657022003.ftpuser", "123456789-Aa");
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //Console.WriteLine("Delete status: {0}", response.StatusDescription);
                response.Close();

                db.Categories.Remove(categories);
                db.SaveChanges();
                TempData["Notice_Delete_Success"] = true;
            }
            catch (ArgumentException)
            {
                TempData["Notice_Delete_Fail"] = true;
            }
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
