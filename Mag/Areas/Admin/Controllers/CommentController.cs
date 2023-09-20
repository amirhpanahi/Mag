using Mag.Areas.Admin.Models.Dto.Comment;
using Mag.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Mag.Models.Entities.Comment;

namespace Mag.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly DataBaseContext _DbContext;
        public CommentController(DataBaseContext dataBaseContext)
        {
            _DbContext = dataBaseContext;
        }

        // PublishComment WatingConfirmComment RejectedComment DeletedComment


        public IActionResult Index()
        {
            var comments = _DbContext.Comments.Where(p => p.Status == StatusName.Publish).Select(p => new CommentListDto
            {
                Id = p.Id,
                NewsId = p.NewsId,
                UserId = p.UserId,
                CommentText = p.CommentText.Length>=50?p.CommentText.Substring(0,40)+" ...":p.CommentText,
                RegisterDate = p.RegisterDate,
                RegisterDatePersian = p.RegisterDatePersian,
                ParentId = p.ParentId,
            }).ToList();
            return View(comments);
        }

        #region ConfirmComment
        public async Task<IActionResult> ConfirmByAdmin(int Id)
        {
            var NewsComment = _DbContext.Comments.FirstOrDefault(p => p.Id == Id);
            if (NewsComment != null)
            {
                NewsComment.Status = StatusName.Publish;

                _DbContext.Entry(NewsComment).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("WatingConfirmComment", "Comment", new { Areas = "Admin" });
            }
            return View();
        }
        #endregion

        #region WatingConfirmComment
        public IActionResult WatingConfirmComment()
        {
            var comments = _DbContext.Comments.Where(p => p.Status == StatusName.WaitingForConfirm).Select(p => new CommentListDto
            {
                Id = p.Id,
                NewsId = p.NewsId,
                UserId = p.UserId,
                CommentText = p.CommentText.Length >= 50 ? p.CommentText.Substring(0, 40) + " ..." : p.CommentText,
                RegisterDate = p.RegisterDate,
                RegisterDatePersian = p.RegisterDatePersian,
                ParentId = p.ParentId,
            }).ToList();
            return View(comments);
        }
        #endregion

        #region RejectedComment
        public IActionResult RejectedComment()
        {
            var comments = _DbContext.Comments.Where(p => p.Status == StatusName.RejectedByAdmin).Select(p => new CommentListDto
            {
                Id = p.Id,
                NewsId = p.NewsId,
                UserId = p.UserId,
                CommentText = p.CommentText.Length >= 50 ? p.CommentText.Substring(0, 40) + " ..." : p.CommentText,
                RegisterDate = p.RegisterDate,
                RegisterDatePersian = p.RegisterDatePersian,
                ParentId = p.ParentId,
            }).ToList();
            return View(comments);
        }

        public async Task<IActionResult> RejectedByAdmin(int Id)
        {
            var NewsComment = _DbContext.Comments.FirstOrDefault(p => p.Id == Id);
            if (NewsComment != null)
            {
                NewsComment.Status = StatusName.RejectedByAdmin;

                _DbContext.Entry(NewsComment).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("RejectedComment", "Comment", new { Areas = "Admin" });
            }
            return View();
        }
        #endregion

        #region DeletedComment
        public IActionResult DeletedComment()
        {
            var comments = _DbContext.Comments.Where(p => p.Status == StatusName.Delete).Select(p => new CommentListDto
            {
                Id = p.Id,
                NewsId = p.NewsId,
                UserId = p.UserId,
                CommentText = p.CommentText.Length >= 50 ? p.CommentText.Substring(0, 40) + " ..." : p.CommentText,
                RegisterDate = p.RegisterDate,
                RegisterDatePersian = p.RegisterDatePersian,
                ParentId = p.ParentId,
            }).ToList();
            return View(comments);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var CommentFind = _DbContext.Comments.Select(p => new CommentEditDto
            {
                Id = p.Id,
                CommentText = p.CommentText,
            }).FirstOrDefault(p => p.Id == id);
            return View(CommentFind);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditDto model)
        {
            if (model == null)
            {
                return Redirect($"/Admin/Comment/index");
            }
            var FindComment = _DbContext.Comments.FirstOrDefault(p => p.Id == model.Id);
            if (ModelState.IsValid)
            {
                FindComment.CommentText = model.CommentText;

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
                ParentId = CommentFind.ParentId,
                RegisterDatePersian = CommentFind.RegisterDatePersian,
            };
            return View(commentObj);
        }
        #endregion
    }
}
