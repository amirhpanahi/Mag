using Mag.Areas.Admin.Models.Dto.Like;
using Mag.Areas.Admin.Models.Dto.News;
using Mag.Common;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Mag.Controllers
{
    public class NewsController : Controller
    {
        private readonly DataBaseContext _DbContext;
        private readonly IFileUploadService _fileUpload;
        public NewsController(DataBaseContext dataBaseContext,IFileUploadService fileUploadService)
        {
            _DbContext = dataBaseContext;
            _fileUpload = fileUploadService;
        }
        [HttpGet]
        [Route("News/index")]
        public IActionResult Index(string slug)
        {
            var model = new NewsListDto
            {
                Slug = slug
            };  
            return View(model);
        }
        #region ShowNews
        [HttpGet]
        [Route("News/{slug}")]
        public async Task<IActionResult> Show(string slug) 
        {
            var findNews = await _DbContext.News.FirstOrDefaultAsync(x => x.Slug == slug);
            var model = new NewsCardDto
            {
                Id = findNews.Id,
                Slug = slug
            };
            return View(model);
        }
        [HttpPost]
        [Route("News/{slug}")]
        public async Task<IActionResult> Show(NewsCardDto model)
        {
            var findNews = _DbContext.News.FirstOrDefault(p => p.Slug == model.Slug);
            if (model.CommentText == null || model.CommentText.Trim().Length == 0)
            {
                ModelState.AddModelError("CommentText","لطفا مقدار خالی وارد ننمایید");
                var CardsModel = new NewsCardDto
                {
                    Slug = model.Slug
                };
                return View(CardsModel);
            }
            if (model.CommentText.Trim().Length > 1000)
            {
                ModelState.AddModelError("CommentText", "اندازه بیشتر از 1000 کاراکتر است");
                var CardsModel = new NewsCardDto
                {
                    Slug = model.Slug
                };
                return View(CardsModel);
            }

            if (ModelState.IsValid)
            {
                var NewComment = new Comment
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    NewsId = findNews.Id,
                    CommentText = model.CommentText,
                    RegisterDate = DateTime.Now,
                    RegisterDatePersian = Utility.ConvertToPersian(DateTime.Now),
                    ParentId = 0,
                    Status = Comment.StatusName.WaitingForConfirm
                };

                await _DbContext.Comments.AddAsync(NewComment);
                await _DbContext.SaveChangesAsync();
                return Redirect($"/News/{model.Slug}");
            }

            var Cards = new NewsCardDto
            {
                Slug = model.Slug
            };
            return View(Cards);
        }
        #endregion

        #region Category
        [HttpGet]
        [Route("News/Category/{slug}")] 
        public IActionResult Category(string slug)
        {
            var model = new NewsListDto
            {
                Slug = slug
            };
            return View(model);
        }
        #endregion

        #region Add
        [HttpGet]
        [Route("User/News/Add")]
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
                // به صورت استاتیک گرفته شده است ParentId در اینجا 
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
        [Route("User/News/Add")]
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 111148393)]//110
        public async Task<IActionResult> Add(NewsAddDto model, string? Draft, string? Publish)
        {
            if (model == null)
            {
                return Redirect("/User/News/Add");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var TagsId = new StringBuilder();
            var CategoriesId = new StringBuilder();

            if (model.TagId != null)
            {
                TagsId.Append(",");
                foreach (var item in model.TagId)
                {
                    TagsId.Append($"{item},");
                }
            }
            if (model.CategoryId != null)
            {
                CategoriesId.Append(",");
                foreach (var item in model.CategoryId)
                {
                    CategoriesId.Append($"{item},");
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
                if (model.VideoFile.Length > 52428487)
                {
                    ModelState.AddModelError("VideoFile", "حجم ویدیو باید زیر 50 مگابایت باشد");
                    return View(model);
                }
                if (model.VideoFile.ContentType == "video/mp4" || model.VideoFile.ContentType == "video/wmv")
                {
                    stringVideoPath = $"Media/News/Video" + await _fileUpload.UploadFileAsync(model.VideoFile, model.Title, "News", "Video");
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
                    DraftNewsDate = Draft == "Draft" ? DateTime.Now : null,
                    DraftNewsDatePersian = Draft == "Draft" ? Utility.ConvertToPersian(DateTime.Now) : null,
                    WriterId = userId,
                    IsActive = model.IsActive,
                    Status = Publish == "Publish" ? StatusName.WaitingForConfirm : StatusName.Draft,
                    CountSeeNews = model.CountSeeNews,
                    NewsSummary = model.NewsSummary,
                };

                await _DbContext.News.AddAsync(NewNews);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Profile");
            }
            return View(model);
        }
        #endregion

        #region Edit
        [HttpGet]
        [Route("User/NewsEdit/{id}")]
        public IActionResult Edit(int id)
        {

            var FindNews = _DbContext.News.FirstOrDefault(p => p.Id == id);

            if (FindNews.Status == StatusName.Publish || FindNews.Status == StatusName.Draft)
            { }
            else
            {
                return Redirect("/Profile/index");
            }

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
        [Route("User/NewsEdit")]
        [RequestFormLimits(MultipartBodyLengthLimit = 111148393)]//110
        public async Task<IActionResult> Edit(NewsEditDto model, string? Draft, string? Publish)
        {
            if (model == null)
            {
                return Redirect($"/Profile/Index");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var NewsFind = _DbContext.News.FirstOrDefault(p => p.Id == model.Id);

            var TagsId = new StringBuilder();
            var CategoriesId = new StringBuilder();

            if (model.TagId != null )
            {
                TagsId.Append(",");
                foreach (var item in model.TagId)
                {
                    TagsId.Append($"{item},");
                }
            }
            if (model.CategoryId != null)
            {
                CategoriesId.Append(",");
                foreach (var item in model.CategoryId)
                {
                    CategoriesId.Append($"{item},");
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
                if (model.VideoFile.Length > 52428487)
                {
                    ModelState.AddModelError("VideoFile", "حجم عکس باید زیر پنجاه مگابایت باشد");
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
                NewsFind.VideoAddress = model.VideoFile == null ? NewsFind.VideoAddress : $"Media/News/Video/{stringVideoPath}";
                NewsFind.IndexImageAddress = model.indexImageFile == null ? NewsFind.IndexImageAddress : $"Media/News/{stringImagePath}";
                NewsFind.IndexImageAddressAlt = model.IndexImageAlt == null ? model.Title : model.IndexImageAlt;
                NewsFind.IndexImageAddressTitle = model.IndexImageTitle == null ? model.Title : model.IndexImageTitle;
                NewsFind.DraftNewsDate = Draft == "Draft" && NewsFind.DraftNewsDate == null ? DateTime.Now : NewsFind.DraftNewsDate;
                NewsFind.DraftNewsDatePersian = Draft == "Draft" && NewsFind.DraftNewsDatePersian == null ? Utility.ConvertToPersian(DateTime.Now) : NewsFind.DraftNewsDatePersian;
                NewsFind.WriterId = userId;
                NewsFind.IsActive = model.IsActive;
                NewsFind.Status = Publish == "Publish" ? StatusName.WaitingForConfirm : StatusName.Draft;
                NewsFind.CountSeeNews = model.CountSeeNews;
                NewsFind.NewsSummary = model.NewsSummary;

                _DbContext.Entry(NewsFind).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Profile");
            }
            return View(model);
        }
        #endregion

        #region LikeNewsAjax
        [HttpPost]
        [Route("Like/News")]
        public async Task<string> LikeNews(int NewsId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var FindLike =await _DbContext.Likes.FirstOrDefaultAsync(p => p.NewsId == NewsId && p.UserId == userId);

            if (FindLike != null)
            {
                if (FindLike.StatusLike == StatusLike.Like)
                {
                    FindLike.StatusLike = StatusLike.None;
                    _DbContext.Entry(FindLike).State = EntityState.Modified;
                    await _DbContext.SaveChangesAsync();
                    return "fa-regular fa-heart text-danger";
                }
                else
                {
                    FindLike.StatusLike = StatusLike.Like;
                    _DbContext.Entry(FindLike).State = EntityState.Modified;
                    await _DbContext.SaveChangesAsync();
                    return "fa-solid fa-heart text-danger";
                }
            }
            else
            {
                var AddLike = new Like
                {
                    NewsId = NewsId,
                    UserId = userId,
                    StatusLike = StatusLike.Like
                };
                await _DbContext.Likes.AddAsync(AddLike);
                await _DbContext.SaveChangesAsync();
                return "fa-solid fa-heart text-danger";
            }
        }
        #endregion

    }
}
