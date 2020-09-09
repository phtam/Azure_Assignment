﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Azure_Assignment.EF;
using Scrypt;

namespace Azure_Assignment.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private DataPalkia db = new DataPalkia();

        //[Authorize(Roles = "0")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,FirtName,LastName,Password,Gender,Birthday,Phone,Email,Address,Picture,Role,Status,ImageFile")] Users users)
        {
            
            if (ModelState.IsValid)
            {
                
                if (db.Users.Find(users.Username) != null)
                {
                    ViewBag.Error = "Username already exists";
                    return View("Create");
                }

                if (users.Username.Length < 6 || users.Username.Length > 50)
                {
                    ViewBag.Error = "Username must be between 6 to 50 characters.";
                    return View("Create");
                }

                bool isExist = db.Users.ToList().Exists(model => model.Email.Equals(users.Email, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    ViewBag.Error = "Email already exists";
                    return View("Create");
                }

                bool isAgeValid = true;
                if ((DateTime.Now.Year - users.Birthday.Value.Year) == 16)
                {
                    if ((DateTime.Now.Month - users.Birthday.Value.Month) == 0)
                    {
                        if ((DateTime.Now.Day - users.Birthday.Value.Day) > 0)
                        {
                            isAgeValid = false;
                        }
                    }
                    else if ((DateTime.Now.Month - users.Birthday.Value.Month) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Year - users.Birthday.Value.Year) < 16)
                {
                    isAgeValid = false;
                }

                if (!isAgeValid)
                {
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Create");
                }

                if (users.Password.Length < 8 || users.Password.Length > 50)
                {
                    ViewBag.Error = "Password must be between 8 to 50 characters";
                    return View("Create");
                }

                string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);
                string extension = Path.GetExtension(users.ImageFile.FileName);
                if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                {
                    ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                    return View("Create");
                }

                long fileSize = ((users.ImageFile.ContentLength) / 1024);
                if (fileSize > 5120)
                {
                    ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                    return View("Create");
                }

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension; 
                users.Picture = "~/public/uploadedFiles/userPictures/" + fileName;
                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/userPictures/");

                if (Directory.Exists(uploadFolderPath) == false)
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                fileName = Path.Combine(uploadFolderPath, fileName);

                users.ImageFile.SaveAs(fileName);

                /**
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://156.67.222.163:21/NhomHoangTam/" + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("u657022003.ftpuser", "123456789-Aa");
                // Copy the contents of the file to the request stream.
                byte[] fileContents;
                using (Stream inputStream = users.ImageFile.InputStream)
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
                
                **/

                ScryptEncoder encoder = new ScryptEncoder();
                users.Password = encoder.Encode(users.Password);

                db.Users.Add(users);
                db.SaveChanges();
                TempData["Notice_Create_Success"] = true;
                return RedirectToAction("Index");
            }

            return View(users);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            Session["OldImage_User"] = users.Picture;
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,FirtName,LastName,Gender,Birthday,Phone,Email,Address,Picture,Role,Status,ImageFile")] Users users, String imageOldFile_User)
        {
            if (ModelState.IsValid)
            {
                bool isAgeValid = true;
                if ((DateTime.Now.Year - users.Birthday.Value.Year) == 16)
                {
                    if ((DateTime.Now.Month - users.Birthday.Value.Month) == 0)
                    {
                        if ((DateTime.Now.Day - users.Birthday.Value.Day) > 0)
                        {
                            isAgeValid = false;
                        }
                    }
                    else if ((DateTime.Now.Month - users.Birthday.Value.Month) > 0)
                    {
                        isAgeValid = false;
                    }
                }
                else if ((DateTime.Now.Year - users.Birthday.Value.Year) < 16)
                {
                    isAgeValid = false;
                }
                
                if (!isAgeValid)
                {
                    ViewBag.Error = "Age must greater than 16 years old";
                    return View("Edit");
                }

                string uploadFolderPath = Server.MapPath("~/public/uploadedFiles/userPictures/");
                if (users.ImageFile == null)
                {
                    users.Picture = imageOldFile_User;
                }
                else
                {
                    if (!imageOldFile_User.IsEmpty())
                    {
                        System.IO.File.Delete(Server.MapPath(imageOldFile_User));
                    }

                    string fileName = Path.GetFileNameWithoutExtension(users.ImageFile.FileName);

                    string extension = Path.GetExtension(users.ImageFile.FileName);
                    if ((extension == ".png" || extension == ".jpg" || extension == ".jpeg") == false)
                    {
                        ViewBag.Error = String.Format("The File, which extension is {0}, hasn't accepted. Please try again!", extension);
                        return View("Edit");
                    }

                    long fileSize = ((users.ImageFile.ContentLength) / 1024);
                    if (fileSize > 5120)
                    {
                        ViewBag.Error = "The File, which size greater than 5MB, hasn't accepted. Please try again!";
                        return View("Edit");
                    }

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    users.Picture = "~/public/uploadedFiles/userPictures/" + fileName;


                    if (Directory.Exists(uploadFolderPath) == false)
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    fileName = Path.Combine(uploadFolderPath, fileName);

                    users.ImageFile.SaveAs(fileName);
                }

                db.Entry(users).State = EntityState.Modified;
                db.Entry(users).Property(x => x.Password).IsModified = false;

                db.SaveChanges();
                Session.Remove("OldImage_User");
                TempData["Notice_Save_Success"] = true;
                return RedirectToAction("Index");
            }
            return View(users);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Users users = db.Users.Find(id);
                if (!users.Picture.IsEmpty())
                {
                    System.IO.File.Delete(Server.MapPath(users.Picture));
                }
                db.Users.Remove(users);
                db.SaveChanges();
                TempData["Notice_Delete_Success"] = true;
            }
            catch (Exception)
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
