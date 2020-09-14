﻿using Azure_Assignment.EF;
using Azure_Assignment.Providers;
using Azure_Assignment.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Azure_Assignment.DAO
{
    public class CategoryDAO
    {
        DataPalkia db = new DataPalkia();
        FTPServerProvider ftp = new FTPServerProvider();
        string ftpChild = "imgCategories";

        public List<CategoryViewModel> Get()
        {
            var list = (from cate in db.Categories
                             select new CategoryViewModel
                             {
                                 CategoryID = cate.CategoryID,
                                 CategoryName = cate.CategoryName,
                                 Description = cate.Description,
                                 Picture = cate.Picture
                             }).ToList();
            foreach (var item in list)
            {
                item.Picture = ftp.Get(item.Picture, ftpChild);
            }
            return list;
        }

        public List<CategoryViewModel> GetNewCategories()
        {
            var list = (from cate in db.Categories
                             orderby cate.CategoryID descending
                             select new CategoryViewModel
                             {
                                 CategoryID = cate.CategoryID,
                                 CategoryName = cate.CategoryName,
                                 Description = cate.Description,
                                 Picture = cate.Picture
                             }).ToList();
            foreach (var item in list)
            {
                item.Picture = ftp.Get(item.Picture, ftpChild);
            }
            return list;
        }
    }
}