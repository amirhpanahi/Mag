﻿using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.ViewComponents
{
    public class TenSelectedByAdminViewComponent : ViewComponent
    {
        private readonly DataBaseContext _dbContext;
        public TenSelectedByAdminViewComponent(DataBaseContext dataBaseContex)
        {
            _dbContext = dataBaseContex;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listNews = _dbContext.News.OrderByDescending(p => p.PublishNewsDate).Where(p => p.Status == StatusName.Publish && p.IsSelectBychiefEditor == true).Take(10).ToList().Select(p=>new NewsListDto
            {
                Title = p.Title,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                Slug = p.Slug
            }).ToList();
            return View(listNews);
        }
    }
}
