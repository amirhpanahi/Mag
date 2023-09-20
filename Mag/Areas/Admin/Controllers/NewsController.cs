using Mag.Areas.Admin.Models.Dto.News;
using Mag.Common;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Mag.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly DataBaseContext _DbContext;
        private readonly IFileUploadService _fileUpload;
        private readonly UserManager<User> _userManager;
        public NewsController(DataBaseContext dataBaseContext, IFileUploadService fileUploadService,
            UserManager<User> userManager)
        {
            _DbContext = dataBaseContext;
            _fileUpload = fileUploadService;
            _userManager = userManager;
        }

        #region Index
        public IActionResult Index()
        {
            var ListNews = _DbContext.News.Where(p => p.Status == StatusName.Publish).Select(p => new NewsListDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                WriterId = p.WriterId,
                WriterName = _DbContext.Users.Where(q => q.Id == p.WriterId).Select(q => new FullnameUser { FirstName = q.FirstName,LastName = q.LastName}).First(),
                Categories = p.Categories,
                IsActive = p.IsActive,
                Status = p.Status,
            }).ToList();

            List<int> Categories = new List<int>();
            foreach (var item in ListNews)
            {
                if (item.Categories != "")
                {
                    item.Categories = item.Categories.Trim(',');
                    var splitcat = item.Categories.Split(",").Select(int.Parse).ToList();
                    Categories.AddRange(splitcat);
                }
            }
            var Cats = _DbContext.CategoryTags.Where(p => Categories.Contains(p.Id)).ToList();
            ViewBag.Categories = Cats;
            return View(ListNews);

        }
        #endregion 

        #region Details
        public IActionResult Details(int id)
        {
            var FindNews = _DbContext.News.FirstOrDefault(p => p.Id == id);
            var DetailsNews = new NewsListDto
            {
                Title = FindNews.Title,
                Slug = FindNews.Slug,
                IndexImageAddress = FindNews.IndexImageAddress,
                KeyWords = FindNews.KeyWords,
                IndexImageAddressAlt = FindNews.IndexImageAddressAlt,
                IndexImageAddressTitle = FindNews.IndexImageAddressTitle,
                WriterId = FindNews.WriterId,
                Categories = FindNews.Categories,
                Tags = FindNews.Tags,
                IsActive = FindNews.IsActive,
                Status = FindNews.Status,
                DraftTimePersain = FindNews.DraftNewsDatePersian,
                RegisterDatePersian = FindNews.RegisterNewsDatePersian,
                DescriptionSeo = FindNews.DescriptionSeo,
                DescriptionHtmlEditor = FindNews.DescriptionHtmlEditor,
                NewsSummary = FindNews.NewsSummary == null ? " " : FindNews.NewsSummary
            };

            ViewBag.StatusName = FindNews.Status.Value.ToString();
            return View(DetailsNews);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            var categories = new List<SelectListItem>(
                // به صورت استاتیک گرفته شده است ParentId در اینجا 
                _DbContext.CategoryTags.Where(x => x.Type == "Category" && x.Id != 1).Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());

            var Tags = new List<SelectListItem>(
                _DbContext.CategoryTags.Where(x => x.Type == "Tag").Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());

            return View(new NewsAddDto
            {
                Categories = categories,
                Tags = Tags
            });
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 111148393)]//110
        public async Task<IActionResult> Add(NewsAddDto model, string? Draft, string? Publish)
        {
            if (model == null)
            {
                return Redirect("/Admin/News/Add");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var TagsId = new StringBuilder();
            var CategoriesId = new StringBuilder();

            if (model.CategoryId != null)
            {
                CategoriesId.Append(",");
                foreach (var item in model.CategoryId)
                {
                    CategoriesId.Append($"{item},");
                }
            }
            if (model.TagId != null)
            {
                TagsId.Append(",");
                foreach (var item in model.TagId)
                {
                    TagsId.Append($"{item},");
                }
            }

            string repaceSlug = "";
            if (model.Slug != null)
                repaceSlug = model.Slug.Replace(" ", "-").Replace("/", "-");
            else
                repaceSlug = model.Title.Replace(" ", "-").Replace("/", "-");

            var stringImagePath = "";
            if (model.indexImageFile != null)
            {
                if (model.indexImageFile.Length > 5242848)
                {
                    ModelState.AddModelError("indexImageFile", "حجم عکس باید زیر پنج مگابایت باشد");
                    return View(model);
                }
                if (model.indexImageFile.ContentType == "image/png" || model.indexImageFile.ContentType == "image/jpg" ||
                    model.indexImageFile.ContentType == "image/jpeg" || model.indexImageFile.ContentType == "image/gif")
                {
                    stringImagePath = $"Media/News/" + await _fileUpload.UploadFileAsync(model.indexImageFile, model.Title, "News");
                }
                else
                {
                    ModelState.AddModelError("indexImageFile", "نوع فایل باید به صورت عکس باشد");
                    return View(model);
                }
            }
            else
                stringImagePath = null;

            var stringVideoPath = "";
            if (model.VideoFile != null)
            {
                if (model.VideoFile.Length > 104856975)
                {
                    ModelState.AddModelError("VideoFile", "حجم ویدیو باید زیر 100 مگابایت باشد");
                    return View(model);
                }
                if (model.VideoFile.ContentType == "video/mp4" || model.VideoFile.ContentType == "video/wmv")
                {
                    stringVideoPath = $"Media/News/" + await _fileUpload.UploadFileAsync(model.VideoFile, model.Title, "News", "Video");
                }
                else
                {
                    ModelState.AddModelError("VideoFile", "نوع فایل باید به صورت ویدیو باشد");
                    return View(model);
                }
            }
            else
                stringVideoPath = null;


            if (ModelState.IsValid)
            {
                var NewNews = new News
                {
                    Title = model.Title,
                    Slug = repaceSlug,
                    Categories = CategoriesId.ToString(),
                    Tags = TagsId.ToString(),
                    DescriptionHtmlEditor = model.DescriptionHtmlEditor,
                    DescriptionSeo = model.DescriptionSeo == null ? model.Title : model.DescriptionSeo,
                    KeyWords = model.KeyWords == null ? null : model.KeyWords,
                    VideoAddress = stringVideoPath,
                    IndexImageAddress = stringImagePath,
                    IndexImageAddressAlt = model.Title,
                    IndexImageAddressTitle = model.Title,
                    RegisterNewsDate = DateTime.Now,
                    RegisterNewsDatePersian = Utility.ConvertToPersian(DateTime.Now),
                    PublishNewsDate = Publish == "Publish" ? DateTime.Now : null,
                    PublishNewsDatePersian = Publish == "Publish" ? Utility.ConvertToPersian(DateTime.Now) : null,
                    DraftNewsDate = Draft == "Draft" ? DateTime.Now : null,
                    DraftNewsDatePersian = Draft == "Draft" ? Utility.ConvertToPersian(DateTime.Now) : null,
                    WriterId = userId,
                    IsActive = model.IsActive,
                    Status = Draft == null ? StatusName.Publish : StatusName.Draft,
                    CountSeeNews = model.CountSeeNews,
                    NewsSummary = model.NewsSummary,
                };

                await _DbContext.News.AddAsync(NewNews);
                await _DbContext.SaveChangesAsync();

                return Redirect("/Admin/News/index");
            }
            return View(model);
        }
        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            var FindNews = _DbContext.News.FirstOrDefault(p => p.Id == id);


            var CategoryId = new List<int>();
            if (FindNews.Categories != null)
            {
                var splitCategories = FindNews.Categories.Split(",");
                foreach (var item in splitCategories)
                {
                    if (item != "")
                    {
                        CategoryId.Add(Convert.ToInt32(item));
                    }
                }
            }

            var TagId = new List<int>();
            if (FindNews.Tags != null)
            {
                var splitTags = FindNews.Tags.Split(",");
                foreach (var item in splitTags)
                {
                    if (item != "")
                    {
                        TagId.Add(Convert.ToInt32(item));
                    }
                }
            }
            var categories = new List<SelectListItem>(
                // به صورت استاتیک گرفته نشده است ParentId در اینجا 
                _DbContext.CategoryTags.Where(x => x.Type == "Category" && x.Id != 1).Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());

            var Tags = new List<SelectListItem>(
                _DbContext.CategoryTags.Where(x => x.Type == "Tag").Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());

            var EditNews = new NewsEditDto
            {
                Title = FindNews.Title,
                Slug = FindNews.Slug,
                IndexImageAddress = FindNews.IndexImageAddress,
                KeyWords = FindNews.KeyWords,
                WriterId = FindNews.WriterId,
                CategoryId = CategoryId,
                Categories = categories,
                TagId = TagId,
                Tags = Tags,
                IsActive = FindNews.IsActive,
                IsSelectBychiefEditor = FindNews.IsSelectBychiefEditor,
                Status = FindNews.Status,
                DraftTimePersain = FindNews.DraftNewsDatePersian,
                RegisterDatePersian = FindNews.RegisterNewsDatePersian,
                DescriptionSeo = FindNews.DescriptionSeo,
                DescriptionHtmlEditor = FindNews.DescriptionHtmlEditor,
                IndexImageAlt = FindNews.IndexImageAddressAlt,
                IndexImageTitle = FindNews.IndexImageAddressTitle,
                NewsSummary = FindNews.NewsSummary == null ? " " : FindNews.NewsSummary
            };
            return View(EditNews);
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 111148393)]//110
        public async Task<IActionResult> Edit(NewsEditDto model, string? Draft, string? Publish)
        {
            if (model == null)
            {
                return Redirect($"/Admin/News/index");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var NewsFind = _DbContext.News.FirstOrDefault(p => p.Id == model.Id);

            var TagsId = new StringBuilder();
            var CategoriesId = new StringBuilder();

            if (model.CategoryId != null)
            {
                CategoriesId.Append(",");
                foreach (var item in model.CategoryId)
                {
                    CategoriesId.Append($"{item},");
                }
            }
            if (model.TagId != null)
            {
                TagsId.Append(",");
                foreach (var item in model.TagId)
                {
                    TagsId.Append($"{item},");
                }
            }

            string repaceSlug = "";
            if (model.Slug != null)
                repaceSlug = model.Slug.Replace(" ", "-").Replace("/", "-");
            else
                repaceSlug = model.Title.Replace(" ", "-").Replace("/", "-");

            var stringImagePath = "";
            if (model.indexImageFile != null)
            {
                if (model.indexImageFile.Length > 5242848)
                {
                    ModelState.AddModelError("indexImageFile", "حجم عکس باید زیر پنج مگابایت باشد");
                    return View(model);
                }
                if (model.indexImageFile.ContentType == "image/png" || model.indexImageFile.ContentType == "image/jpg" ||
                    model.indexImageFile.ContentType == "image/jpeg" || model.indexImageFile.ContentType == "image/gif")
                {
                    stringImagePath = await _fileUpload.UploadFileAsync(model.indexImageFile, model.Title, "News");
                }
                else
                {
                    ModelState.AddModelError("indexImageFile", "نوع فایل باید به صورت عکس باشد");
                    return View(model);
                }
            }
            else
                stringImagePath = null;

            var stringVideoPath = "";
            if (model.VideoFile != null)
            {
                if (model.VideoFile.Length > 70856975)
                {
                    ModelState.AddModelError("VideoFile", "حجم ویدیو باید زیر 100 مگابایت باشد");
                    return View(model);
                }
                if (model.VideoFile.ContentType == "video/mp4" || model.VideoFile.ContentType == "video/wmv")
                {
                    stringVideoPath = await _fileUpload.UploadFileAsync(model.VideoFile, model.Title, "News", "Video");
                }
                else
                {
                    ModelState.AddModelError("VideoFile", "نوع فایل باید به صورت ویدیو باشد");
                    return View(model);
                }
            }
            else
                stringVideoPath = null;


            if (ModelState.IsValid)
            {
                NewsFind.Title = model.Title;
                NewsFind.Slug = repaceSlug;
                NewsFind.Categories = CategoriesId.ToString();
                NewsFind.Tags = TagsId.ToString();
                NewsFind.DescriptionHtmlEditor = model.DescriptionHtmlEditor;
                NewsFind.DescriptionSeo = model.DescriptionSeo == null ? model.Title : model.DescriptionSeo;
                NewsFind.KeyWords = model.KeyWords == null ? null : model.KeyWords;
                NewsFind.VideoAddress = model.VideoFile == null ? NewsFind.VideoAddress : $"Media/News/video/{stringVideoPath}";
                NewsFind.IndexImageAddress = model.indexImageFile == null ? NewsFind.IndexImageAddress : $"Media/News/{stringImagePath}";
                NewsFind.IndexImageAddressAlt = model.IndexImageAlt == null ? model.Title : model.IndexImageAlt;
                NewsFind.IndexImageAddressTitle = model.IndexImageTitle == null ? model.Title : model.IndexImageTitle;
                NewsFind.PublishNewsDate = Publish == "Publish" && NewsFind.PublishNewsDate == null ? DateTime.Now : NewsFind.RegisterNewsDate;
                NewsFind.PublishNewsDatePersian = Publish == "Publish" && NewsFind.PublishNewsDatePersian == null ? Utility.ConvertToPersian(DateTime.Now) : NewsFind.RegisterNewsDatePersian;
                NewsFind.DraftNewsDate = Draft == "Draft" && NewsFind.DraftNewsDate == null ? DateTime.Now : NewsFind.DraftNewsDate;
                NewsFind.DraftNewsDatePersian = Draft == "Draft" && NewsFind.DraftNewsDatePersian == null ? Utility.ConvertToPersian(DateTime.Now) : NewsFind.DraftNewsDatePersian;
                NewsFind.WriterId = userId;
                NewsFind.IsActive = model.IsActive;
                NewsFind.IsSelectBychiefEditor = model.IsSelectBychiefEditor;
                NewsFind.Status = Draft == null ? StatusName.Publish : StatusName.Draft;
                NewsFind.CountSeeNews = model.CountSeeNews;
                NewsFind.NewsSummary = model.NewsSummary;

                _DbContext.Entry(NewsFind).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return Redirect("/Admin/News/Index");
            }
            return View(model);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var FindNews = _DbContext.News.FirstOrDefault(p => p.Id == id);

            var categories = new List<SelectListItem>(
                // به صورت استاتیک گرفته نشده است ParentId در اینجا 
                _DbContext.CategoryTags.Where(x => x.Type == "Category" && x.Id != 1).Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());

            var Tags = new List<SelectListItem>(
                _DbContext.CategoryTags.Where(x => x.Type == "Tag").Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());

            var CategoryId = new List<int>();
            if (FindNews.Categories != null)
            {
                var splitCategories = FindNews.Categories.Split(",");
                foreach (var item in splitCategories)
                {
                    if (item != "")
                    {
                        CategoryId.Add(Convert.ToInt32(item));
                    }
                }
            }
            var TagId = new List<int>();
            if (FindNews.Tags != null)
            {
                var splitTags = FindNews.Tags.Split(",");
                foreach (var item in splitTags)
                {
                    if (item != "")
                    {
                        TagId.Add(Convert.ToInt32(item));
                    }
                }
            }
            var EditNews = new NewsDeleteDto
            {
                Id = FindNews.Id,
                Title = FindNews.Title,
                IndexImageAddress = FindNews.IndexImageAddress,
                KeyWords = FindNews.KeyWords,
                CategoryId = CategoryId,
                TagId = TagId,
                DescriptionHtmlEditor = FindNews.DescriptionHtmlEditor,
                IndexImageAlt = FindNews.IndexImageAddressAlt,
                IndexImageTitle = FindNews.IndexImageAddressTitle,
                Categories = categories,
                Tags = Tags
            };
            return View(EditNews);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(NewsDeleteDto model)
        {
            var FindNews = _DbContext.News.FirstOrDefault(p => p.Id == model.Id);

            if (ModelState.IsValid)
            {
                FindNews.Status = StatusName.Delete;
                FindNews.DeleteNewsDate = DateTime.Now;
                FindNews.DeleteNewsDatePersian = Utility.ConvertToPersian(DateTime.Now);

                _DbContext.Entry(FindNews).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return Redirect("Admin/News/DeletedNewses");
            }

            return View();
        }
        #endregion

        #region ConfirmNews
        public async Task<IActionResult> ConfirmByAdmin(int Id)
        {
            var NewsFind = _DbContext.News.FirstOrDefault(p => p.Id == Id);
            if (NewsFind != null)
            {
                NewsFind.Status = StatusName.Publish;
                NewsFind.PublishNewsDate = DateTime.Now;
                NewsFind.PublishNewsDatePersian = Utility.ConvertToPersian(DateTime.Now);

                _DbContext.Entry(NewsFind).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("WatingConfirmNews", "News", new { Areas = "Admin" });
            }
            return View();
        }
        #endregion

        #region WatingForConfirmNews
        [HttpGet]
        public IActionResult WatingConfirmNews()
        {
            var ListNews = _DbContext.News.Where(p => p.Status == StatusName.WaitingForConfirm).Select(p => new NewsListDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                WriterId = p.WriterId,
                WriterName = _DbContext.Users.Where(q => q.Id == p.WriterId).Select(q => new FullnameUser { FirstName = q.FirstName, LastName = q.LastName }).First(),
                Categories = p.Categories,
                IsActive = p.IsActive,
                Status = p.Status,
            }).ToList();

            List<int> Categories = new List<int>();
            foreach (var item in ListNews)
            {
                if (item.Categories != "")
                {
                    item.Categories = item.Categories.Trim(',');
                    var splitcat = item.Categories.Split(",").Select(int.Parse).ToList();
                    Categories.AddRange(splitcat);
                }
            }
            var Cats = _DbContext.CategoryTags.Where(p => Categories.Contains(p.Id)).ToList();
            ViewBag.Categories = Cats;
            return View(ListNews);
        }
        #endregion

        #region RejectingNewses
        [HttpGet]
        public IActionResult RejectedNewses()
        {
            var ListNews = _DbContext.News.Where(p => p.Status == StatusName.RejectedByAdmin).Select(p => new NewsListDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                WriterId = p.WriterId,
                WriterName = _DbContext.Users.Where(q => q.Id == p.WriterId).Select(q => new FullnameUser { FirstName = q.FirstName, LastName = q.LastName }).First(),
                Categories = p.Categories,
                IsActive = p.IsActive,
                Status = p.Status,
            }).ToList();

            List<int> Categories = new List<int>();
            foreach (var item in ListNews)
            {
                if (item.Categories != "")
                {
                    item.Categories = item.Categories.Trim(',');
                    var splitcat = item.Categories.Split(",").Select(int.Parse).ToList();
                    Categories.AddRange(splitcat);
                }
            }
            var Cats = _DbContext.CategoryTags.Where(p => Categories.Contains(p.Id)).ToList();
            ViewBag.Categories = Cats;
            return View(ListNews);
        }
        public async Task<IActionResult> RejectedByAdmin(int Id)
        {
            var NewsFind = _DbContext.News.FirstOrDefault(p => p.Id == Id);
            if (NewsFind != null)
            {
                NewsFind.Status = StatusName.RejectedByAdmin;
                NewsFind.RejecteNewsDate = DateTime.Now;
                NewsFind.RejecteNewsDatePersian = Utility.ConvertToPersian(DateTime.Now);

                _DbContext.Entry(NewsFind).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("RejectedNewses", "News", new { Areas = "Admin" });
            }
            return View();
        }
        #endregion

        #region DraftNewses
        [HttpGet]
        public IActionResult DraftNewses()
        {
            var ListNews = _DbContext.News.Where(p => p.Status == StatusName.Draft).Select(p => new NewsListDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                WriterId = p.WriterId,
                WriterName = _DbContext.Users.Where(q => q.Id == p.WriterId).Select(q => new FullnameUser { FirstName = q.FirstName, LastName = q.LastName }).First(),
                Categories = p.Categories,
                IsActive = p.IsActive,
                Status = p.Status,
            }).ToList();

            List<int> Categories = new List<int>();
            foreach (var item in ListNews)
            {
                if (item.Categories != "")
                {
                    item.Categories = item.Categories.Trim(',');
                    var splitcat = item.Categories.Split(",").Select(int.Parse).ToList();
                    Categories.AddRange(splitcat);
                }
            }
            var Cats = _DbContext.CategoryTags.Where(p => Categories.Contains(p.Id)).ToList();
            ViewBag.Categories = Cats;
            return View(ListNews);
        }
        #endregion 

        #region DeletedNewses
        [HttpGet]
        public IActionResult DeletedNewses()
        {
            var ListNews = _DbContext.News.Where(p => p.Status == StatusName.Delete).Select(p => new NewsListDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                WriterId = p.WriterId,
                WriterName = _DbContext.Users.Where(q => q.Id == p.WriterId).Select(q => new FullnameUser { FirstName = q.FirstName, LastName = q.LastName }).First(),
                Categories = p.Categories,
                IsActive = p.IsActive,
                Status = p.Status,
            }).ToList();

            List<int> Categories = new List<int>();
            foreach (var item in ListNews)
            {
                if (item.Categories != "")
                {
                    item.Categories = item.Categories.Trim(',');
                    var splitcat = item.Categories.Split(",").Select(int.Parse).ToList();
                    Categories.AddRange(splitcat);
                }
            }
            var Cats = _DbContext.CategoryTags.Where(p => Categories.Contains(p.Id)).ToList();
            ViewBag.Categories = Cats;
            return View(ListNews);
        }
        #endregion 

        private List<string> GetCategoriesName(string Cats)
        {
            List<string> listCat = new List<string>();
            var splitcat = Cats.Split(",");
            int[] intCat = splitcat.Select(int.Parse).ToArray();

            var findtag = _DbContext.CategoryTags.Where(p => intCat.Contains(p.Id)).ToList();
            return listCat;
        }
    }
}
