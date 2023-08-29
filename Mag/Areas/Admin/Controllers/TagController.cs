using Mag.Areas.Admin.Models.Dto.Tag;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly DataBaseContext _DbContext;
        private readonly IFileUploadService _fileUpload;
        public TagController(DataBaseContext DbContext, IFileUploadService fileUploadService)
        {
            _DbContext = DbContext;
            _fileUpload = fileUploadService;
        }
        #region Index
        public IActionResult Index()
        {
            var tags = _DbContext.CategoryTags.Where(x => x.Type == "Tag").Select(p => new TagListDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                PicAddress = p.PicAddress,
                PicAlt = p.PicAlt,
                PicTitle = p.PicTitle,
            }).ToList();
            return View(tags);
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(TagAddDto model)
        {
            string repaceSlug = "";
            if (model.Slug != null)
                repaceSlug = model.Slug.Replace(" ", "-").Replace("/", "-");
            else
                repaceSlug = model.Name.Replace(" ", "-").Replace("/", "-");

            var stringPath = "";
            if (model.File != null)
            {
                if (model.File.Length > 1000000)
                {
                    ModelState.AddModelError("PicAddress", "حجم عکس باید زیر یک مگابایت باشد");
                    return View();
                }
                if (model.File.ContentType == "image/png" || model.File.ContentType == "image/jpg"||
                    model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/gif")
                {
                    stringPath = $"Media/Tags/" + await _fileUpload.UploadFileAsync(model.File, model.Name, "Tags");
                }
                else
                {
                    ModelState.AddModelError("PicAddress", "نوع فایل باید به صورت عکس باشد");
                    return View();
                }
            }
            else
                stringPath = null;

            if (ModelState.IsValid)
            {
                var newModel = new CategoryTag
                {
                    Name = model.Name,
                    Slug = repaceSlug,
                    Description = model.Description,
                    ParentId = model.ParentId,
                    PicAddress = stringPath,
                    PicAlt = model.PicAlt == null ? model.Name : model.PicAlt,
                    PicTitle = model.PicTitle == null ? model.Name : model.PicTitle,
                    Type = "Tag",
                };
                await _DbContext.CategoryTags.AddAsync(newModel);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Tag", new { Areas = "Admin" });
            }

            return View(model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var FindTag = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == Id);

            return View(new TagEditDto
            {
                Id = FindTag.Id,
                Name = FindTag.Name,
                Description = FindTag.Description,
                Slug = FindTag.Slug,
                PicAddress = FindTag.PicAddress,
                PicAlt = FindTag.PicAlt,
                PicTitle = FindTag.PicTitle,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TagEditDto model)
        {
            string repaceSlug = "";
            if (model.Slug != null)
                repaceSlug = model.Slug.Replace(" ", "-").Replace("/", "-");
            else
                repaceSlug = model.Name.Replace(" ", "-").Replace("/", "-");

            var FindTag = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == model.Id);
            var stringPath = "";
            if (model.File != null)
            {
                if (model.File.Length > 1000000)
                {
                    ModelState.AddModelError("PicAddress", "حجم عکس باید زیر یک مگابایت باشد");
                    return View(new TagEditDto
                    {
                        Id = FindTag.Id,
                        Name = FindTag.Name,
                        Description = FindTag.Description,
                        Slug = FindTag.Slug,
                        PicAddress = FindTag.PicAddress,
                        PicAlt = FindTag.PicAlt,
                        PicTitle = FindTag.PicTitle,
                    });
                }
                if (model.File.ContentType == "image/png" || model.File.ContentType == "image/jpg" ||
                    model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/gif")
                {
                    //stringPath = $"Media/Tags/" + await _fileUpload.UploadFileAsync(model.File, model.Name, "Tags");
                    stringPath = await _fileUpload.UploadFileAsync(model.File, model.Name, "Tags");
                }
                else
                {
                    ModelState.AddModelError("PicAddress", "نوع فایل باید به صورت عکس باشد");
                    return View(new TagEditDto
                    {
                        Id = FindTag.Id,
                        Name = FindTag.Name,
                        Description = FindTag.Description,
                        Slug = FindTag.Slug,
                        PicAddress = FindTag.PicAddress,
                        PicAlt = FindTag.PicAlt,
                        PicTitle = FindTag.PicTitle,
                    });
                }
                
            }
            if (ModelState.IsValid)
            {
                FindTag.Name = model.Name;
                FindTag.Slug = repaceSlug;
                FindTag.Description = model.Description;
                FindTag.ParentId = model.ParentId;
                FindTag.PicAddress = model.File == null ? FindTag.PicAddress : $"Media/Tags/{stringPath}";
                FindTag.PicAlt = model.PicAlt == null ? model.Name : model.PicAlt;
                FindTag.PicTitle = model.PicTitle == null ? model.Name : model.PicTitle;
                FindTag.Type = "Tag";

                _DbContext.Entry(FindTag).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Tag", new { Areas = "Admin" });
            }

            return View(model);
        }

        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var FindTag = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == Id);

            return View(new TagDeleteDto
            {
                Id = FindTag.Id,
                Name = FindTag.Name,
                Description = FindTag.Description,
                Slug = FindTag.Slug,
                PicAddress = FindTag.PicAddress,
                PicAlt = FindTag.PicAlt,
                PicTitle = FindTag.PicTitle,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TagEditDto model)
        {
            var FindTag = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == model.Id);

            if (ModelState.IsValid)
            {
                _DbContext.CategoryTags.Remove(FindTag);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Tag", new { Areas = "Admin" });
            }

            return View(model);
        }

        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int Id)
        {
            var FindTag = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == Id);

            return View(new TagListDto
            {
                Id = FindTag.Id,
                Name = FindTag.Name,
                Description = FindTag.Description,
                Slug = FindTag.Slug,
                PicAddress = FindTag.PicAddress,
                PicAlt = FindTag.PicAlt,
                PicTitle = FindTag.PicTitle,
            });
        }
        #endregion

    }
}
