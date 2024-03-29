﻿using Mag.Areas.Admin.Models.Dto.Ads;
using Mag.Areas.Admin.Models.Dto.Home;
using Mag.Areas.Admin.Models.Dto.Like;
using Mag.Areas.Admin.Models.Dto.News;
using Mag.Common;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Authorization;
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
        public NewsController(DataBaseContext dataBaseContext, IFileUploadService fileUploadService)
        {
            _DbContext = dataBaseContext;
            _fileUpload = fileUploadService;
        }
        [HttpGet]
        [Route("News/index")]
        public async Task<IActionResult> Index(string slug)
        {
            var baner1 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner1");
            var baner2 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner2");
            var baner3 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner3");
            var baner4 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner4");
            var baner5 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner5");
            var baner6 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner6");
            var baner7 = await _DbContext.Banners.FirstOrDefaultAsync(p => p.BannerName == "Banner7");

            var banner = new BannersDto
            {
                Banner1Address = baner1 == null ? "" : baner1.BannerAddress,
                Banner1PhotoText = baner1.PhotoText == null ? "" : baner1.PhotoText,
                Banner1PhotoLink = baner1.PhotoLink == null ? "" : baner1.PhotoLink,
                Banner2Address = baner2 == null ? "" : baner2.BannerAddress,
                Banner2PhotoText = baner2.PhotoText == null ? "" : baner2.PhotoText,
                Banner2PhotoLink = baner2.PhotoLink == null ? "" : baner2.PhotoLink,
                Banner3Address = baner3 == null ? "" : baner3.BannerAddress,
                Banner3PhotoText = baner3.PhotoText == null ? "" : baner3.PhotoText,
                Banner3PhotoLink = baner3.PhotoLink == null ? "" : baner3.PhotoLink,
                Banner4Address = baner4 == null ? "" : baner4.BannerAddress,
                Banner4PhotoText = baner4.PhotoText == null ? "" : baner4.PhotoText,
                Banner4PhotoLink = baner4.PhotoLink == null ? "" : baner4.PhotoLink,
                Banner5Address = baner5 == null ? "" : baner5.BannerAddress,
                Banner5PhotoText = baner5.PhotoText == null ? "" : baner5.PhotoText,
                Banner5PhotoLink = baner5.PhotoLink == null ? "" : baner5.PhotoLink,
                Banner6Address = baner6 == null ? "" : baner6.BannerAddress,
                Banner6PhotoText = baner6.PhotoText == null ? "" : baner6.PhotoText,
                Banner6PhotoLink = baner6.PhotoLink == null ? "" : baner6.PhotoLink,
                Banner7Address = baner7 == null ? "" : baner7.BannerAddress,
                Banner7PhotoText = baner7.PhotoText == null ? "" : baner7.PhotoText,
                Banner7PhotoLink = baner7.PhotoLink == null ? "" : baner7.PhotoLink,
            };

            var Ads1 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP1");
            var Ads2 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP2");
            var Ads3 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP3");
            var Ads4 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP4");
            var Ads5 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP5");
            var Ads6 = await _DbContext.Ads.FirstOrDefaultAsync(p => p.Name == "MP6");

            var Ads = new AdsDto
            {
                Address1 = Ads1 == null ? "" : Ads1.Address,
                Alt1 = Ads1 == null ? "" : Ads1.Alt,
                Title1 = Ads1 == null ? "" : Ads1.Title,
                Address2 = Ads2 == null ? "" : Ads2.Address,
                Alt2 = Ads2 == null ? "" : Ads2.Alt,
                Title2 = Ads2 == null ? "" : Ads2.Title,
                Address3 = Ads3 == null ? "" : Ads3.Address,
                Alt3 = Ads3 == null ? "" : Ads3.Alt,
                Title3 = Ads3 == null ? "" : Ads3.Title,
                Address4 = Ads4 == null ? "" : Ads4.Address,
                Alt4 = Ads4 == null ? "" : Ads4.Alt,
                Title4 = Ads4 == null ? "" : Ads4.Title,
                Address5 = Ads5 == null ? "" : Ads5.Address,
                Alt5 = Ads5 == null ? "" : Ads5.Alt,
                Title5 = Ads5 == null ? "" : Ads5.Title,
                Address6 = Ads6 == null ? "" : Ads6.Address,
                Alt6 = Ads6 == null ? "" : Ads6.Alt,
                Title6 = Ads6 == null ? "" : Ads6.Title,
            };

            var Result = new Tuple<BannersDto, AdsDto>(banner, Ads);

            return View(Result);
        }

        #region TagsNews
        [HttpGet]
        [Route("News/Tag/{Id}")]
        public async Task<IActionResult> Tag(int Id)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    var userIdVisitor = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //    ViewBag.userIdVisitor = userIdVisitor;
            //}
            //var findNews = await _DbContext.CategoryTags.FirstOrDefaultAsync(x => x.Id == Id);
            //if (findNews == null)
            //{
            //    return Redirect("/NotFound/News");
            //}
            //var model = new NewsCardDto
            //{
            //    Id = findNews.Id,
            //    Slug = findNews.Slug,
            //    CountOfLike = await _DbContext.Likes.Where(p => p.NewsId == findNews.Id && p.StatusLike == StatusLike.Like).CountAsync()
            //};
            //return View(model);
            var model = new NewsListDto
            {
                Id = Id
            };
            return View(model);
        }
        #endregion

        #region NotFound
        [HttpGet]
        [Route("NotFound/News/Not")]
        public IActionResult NotFound()
        {
            return View();
        }
        #endregion

        #region ShowNews
        [HttpGet]
        [Route("News/{slug}")]
        public async Task<IActionResult> Show(string slug)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdVisitor = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.userIdVisitor=userIdVisitor;
            }
            var findNews = await _DbContext.News.FirstOrDefaultAsync(x => x.Slug == slug);
            if (findNews == null)
            {
                return Redirect("/NotFound/News");
            }
            var model = new NewsCardDto
            {
                Id = findNews.Id,
                Slug = slug,
                CountOfLike = await _DbContext.Likes.Where(p => p.NewsId == findNews.Id && p.StatusLike == StatusLike.Like).CountAsync()
            };
            return View(model);
        }
        #endregion

        #region submitComment
        [HttpPost]
        [Route("api/submitComment")]
        public async Task<string> SubmitComment(string IdCommentWriter,string TextComment,int NewsId)
        {
            var findNews = _DbContext.News.FirstOrDefault(p => p.Id == NewsId);
            if (TextComment == null || TextComment.Trim().Length == 0)
            {
                return "error";
            }
            if (TextComment.Length > 1000)
            {
                return "error";
            }

            if (ModelState.IsValid)
            {
                var NewComment = new Comment
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    NewsId = findNews.Id,
                    CommentText = TextComment,
                    RegisterDate = DateTime.Now,
                    RegisterDatePersian = Utility.ConvertToPersian(DateTime.Now),
                    ParentId = 0,
                    Status = Comment.StatusName.WaitingForConfirm
                };
                await _DbContext.Comments.AddAsync(NewComment);
                await _DbContext.SaveChangesAsync();
                return "دیدگاه شما ثبت شد لطفا منتظر تایید آن بمانید";
            }
            return "UnKnow";
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
        [Authorize(Roles = "writer")]
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
        [Authorize(Roles = "writer")]
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
        [Authorize(Roles = "writer")]
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
        [Authorize(Roles = "writer")]
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

                return Redirect("/Profile/Index/User"); 
            }
            return View(model);
        }
        #endregion

        #region LikeNewsAjax
        [HttpPost]
        [Route("Like/News")]
        public async Task<string> LikeNews(int NewsId,int CountOfLike)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var FindLike = await _DbContext.Likes.FirstOrDefaultAsync(p => p.NewsId == NewsId && p.UserId == userId);

            if (FindLike != null)
            {
                if (FindLike.StatusLike == StatusLike.Like)
                {
                    FindLike.StatusLike = StatusLike.None;
                    _DbContext.Entry(FindLike).State = EntityState.Modified;
                    await _DbContext.SaveChangesAsync();
                    return $"fa-regular fa-heart text-danger,{CountOfLike-=1}";
                }
                else
                {
                    FindLike.StatusLike = StatusLike.Like;
                    _DbContext.Entry(FindLike).State = EntityState.Modified;
                    await _DbContext.SaveChangesAsync();
                    return $"fa-solid fa-heart text-danger,{CountOfLike+1}";
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
                return $"fa-solid fa-heart text-danger,{CountOfLike+1}";
            }
        }
        #endregion

        #region Details
        [HttpGet]
        [Authorize(Roles = "writer")]
        [Route("User/NewsDetails/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var FindNews = _DbContext.News.FirstOrDefault(p => p.Id == id);

            if (FindNews.Status == StatusName.Publish || FindNews.Status == StatusName.Draft)
            { }
            else
            {
                return Redirect("/Profile/index");
            }

            var WriterName = await _DbContext.Users.Where(p => p.Id == FindNews.WriterId).Select(p => new FullnameUser
            {
                FirstName = p.FirstName,
                LastName = p.LastName
            }).FirstAsync();

            var DetailsNews = new NewsListDto
            {
                Title = FindNews.Title,
                Slug = FindNews.Slug,
                IndexImageAddress = FindNews.IndexImageAddress,
                KeyWords = FindNews.KeyWords,
                IndexImageAddressAlt = FindNews.IndexImageAddressAlt,
                IndexImageAddressTitle = FindNews.IndexImageAddressTitle,
                WriterName = WriterName,
                IsActive = FindNews.IsActive,
                IsSelectBychiefEditor = FindNews.IsSelectBychiefEditor,
                Status = FindNews.Status,
                DraftTimePersain = FindNews.DraftNewsDatePersian,
                RegisterDatePersian = FindNews.RegisterNewsDatePersian,
                PublishTimePersain = FindNews.PublishNewsDatePersian,
                DescriptionSeo = FindNews.DescriptionSeo,
                DescriptionHtmlEditor = FindNews.DescriptionHtmlEditor,
                NewsSummary = FindNews.NewsSummary == null ? " " : FindNews.NewsSummary,
                VideoAddress = FindNews.VideoAddress
            };

            List<int> Categoies = new List<int>();
            if (FindNews.Categories != "")
            {
                var TrimItem = FindNews.Categories.Trim(',');
                var SplitCategoeies = TrimItem.Split(",").Select(int.Parse).ToList();
                Categoies.AddRange(SplitCategoeies);
            }
            var Cats = _DbContext.CategoryTags.Where(p => Categoies.Contains(p.Id)).Select(p => p.Name).ToList();
            ViewBag.Categories = Cats;



            List<int> Tags = new List<int>();
            if (FindNews.Tags != "")
            {
                var trimItem = FindNews.Tags.Trim(',');
                var SplitTags = trimItem.Split(',').Select(int.Parse).ToList();
                Tags.AddRange(SplitTags);
            }
            var Tag = _DbContext.CategoryTags.Where(p => Tags.Contains(p.Id)).Select(p => p.Name).ToList();
            ViewBag.Tags = Tag;

            return View(DetailsNews);
        }
        #endregion

    }
}
