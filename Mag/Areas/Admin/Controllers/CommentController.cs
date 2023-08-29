using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly DataBaseContext _DbContext;
        public CommentController(DataBaseContext dataBaseContext)
        {
            _DbContext = dataBaseContext;
        }
        public IActionResult Index()
        {
            var comments = _DbContext.Comments.Select(p => new CommentListDto
            {
                Id = p.Id,
                NewsId = p.NewsId,
                UserId = p.UserId,
                CommentText = p.CommentText.Length>=50?p.CommentText.Substring(0,40)+" ...":p.CommentText,
                IsActive = p.IsActive,
                IsDelete = p.IsDelete,
                RegisterDate = p.RegisterDate,
                RegisterDatePersian = p.RegisterDatePersian,
                ParentId = p.ParentId,
            }).ToList();
            return View(comments);
        }

        #region Add
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var CommentFind = _DbContext.Comments.Select(p => new CommentEditDto
            {
                Id = p.Id,
                CommentText = p.CommentText,
                IsActive = p.IsActive,
                IsDelete = p.IsDelete
            }).FirstOrDefault(p => p.Id == id);
            return View(CommentFind);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditDto model)
        {
            var FindComment = _DbContext.Comments.FirstOrDefault(p => p.Id == model.Id);
            if (ModelState.IsValid)
            {
                FindComment.CommentText = model.CommentText;
                FindComment.IsActive = model.IsActive;
                FindComment.IsDelete = model.IsDelete;

                _DbContext.Entry(FindComment).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Comment", new { Areas = "Admin" });
            }
            return View(model);
        }
        #endregion


        #region Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var CommentFind = _DbContext.Comments.FirstOrDefault(p => p.Id == id);
            var commentObj = new CommentListDto
            {
                UserId = CommentFind.UserId, 
                NewsId = CommentFind.NewsId,
                CommentText = CommentFind.CommentText,
                IsActive = CommentFind.IsActive,
                IsDelete = CommentFind.IsDelete,
                ParentId = CommentFind.ParentId,
                RegisterDatePersian = CommentFind.RegisterDatePersian,
            };
            return View(commentObj);
        }
        #endregion
    }
}
