using Mag.Areas.Admin.Models.Dto.Category;
using Mag.Data;
using Mag.Models.Entities;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Mag.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataBaseContext _DbContext;
        private readonly IFileUploadService _fileUpload;
        public CategoryController(DataBaseContext dataBaseContext, IFileUploadService fileUploadService)
        {
            _DbContext = dataBaseContext;
            _fileUpload = fileUploadService;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var dic = new Dictionary<int,string>();

            var FindParentCategoryId = _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => p.ParentId).Distinct().ToList();
            foreach (var item in FindParentCategoryId)
            {
                var FindParentCategoryName = _DbContext.CategoryTags.Where(x => x.Id == item.Value).Select(p => p.Name).ToList();
                dic.Add(Convert.ToInt32(item), FindParentCategoryName.First());
            }

            var categories = _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new CategoryListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ParentId = p.ParentId,
                Slug = p.Slug,
                IsActive = p.IsActive,
                IsShowMainPage = p.IsShowMainPage,
                PicAddress = p.PicAddress,
                PicTitle = p.PicTitle,
                PicAlt = p.PicAlt,
                NameParentId =dic
            }).ToList();

            

            return View(categories);
        }

        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add()
        {
            var categories = new List<SelectListItem>(
                // به صورت استاتیک گرفته شده است ParentId در اینجا 
                _DbContext.CategoryTags.Where(x => x.Type == "Category" && x.ParentId==1).Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());
            return View(new CategoryAddDto
            {
                CategoryTags = categories,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto model)
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
                    var categories = new List<SelectListItem>(
                        _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.Id.ToString()
                        }).ToList());
                    return View(new CategoryAddDto
                    {
                        CategoryTags = categories,
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        IsShowMainPage = model.IsShowMainPage,
                        Slug = model.Slug,
                        PicAlt = model.PicAlt,
                        PicTitle = model.PicTitle
                    });
                }
                if (model.File.ContentType == "image/png" || model.File.ContentType == "image/jpg" ||
                    model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/gif")
                {
                    stringPath = $"Media/Categories/" + await _fileUpload.UploadFileAsync(model.File, model.Name, "Categories");
                }
                else
                {
                    ModelState.AddModelError("PicAddress", "نوع فایل باید به صورت عکس باشد");
                    var categories = new List<SelectListItem>(
                        _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.Id.ToString()
                        }).ToList());
                    return View(new CategoryAddDto
                    {
                        CategoryTags = categories,
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        IsShowMainPage = model.IsShowMainPage,
                        Slug = model.Slug,
                        PicAlt = model.PicAlt,
                        PicTitle = model.PicTitle
                    });
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
                    PicTitle = model.PicTitle==null?model.Name:model.PicTitle,
                    IsActive = model.IsActive,
                    IsShowMainPage = model.IsShowMainPage,
                    Type = "Category",
                };
                await _DbContext.CategoryTags.AddAsync(newModel);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Category", new { Areas = "Admin" });
            }
            return View(model);
        }

        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var Findcategory = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == Id);
            var categories = new List<SelectListItem>(
                _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());
            return View(new CategoryEditDto
            {
                Id = Findcategory.Id,
                Name = Findcategory.Name,
                Description = Findcategory.Description,
                Slug = Findcategory.Slug,
                ParentId = Findcategory.ParentId,
                PicAddress = Findcategory.PicAddress,
                PicAlt = Findcategory.PicAlt,
                PicTitle = Findcategory.PicTitle,
                IsShowMainPage = Findcategory.IsShowMainPage,
                IsActive = Findcategory.IsActive,
                CategoryTags = categories,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditDto model)
        {
            string repaceSlug = "";
            if (model.Slug != null)
                repaceSlug = model.Slug.Replace(" ", "-").Replace("/", "-");
            else
                repaceSlug = model.Name.Replace(" ", "-").Replace("/", "-");

            var Findcategory = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == model.Id);
            var stringPath = "";
            if (model.File != null)
            {
                if (model.File.Length > 1000000)
                {
                    ModelState.AddModelError("PicAddress", "حجم عکس باید زیر یک مگابایت باشد");
                    var categories = new List<SelectListItem>(
                        _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.Id.ToString()
                        }).ToList());
                    return View(new CategoryEditDto
                    {
                        CategoryTags = categories,
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        IsShowMainPage = model.IsShowMainPage,
                        PicAddress = Findcategory.PicAddress,
                        Slug = model.Slug,
                        PicAlt = model.PicAlt,
                        PicTitle = model.PicTitle
                    });

                }
                if (model.File.ContentType == "image/png" || model.File.ContentType == "image/jpg" ||
                    model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/gif")
                {
                    stringPath = await _fileUpload.UploadFileAsync(model.File, model.Name, "Categories");
                }
                else
                {
                    ModelState.AddModelError("PicAddress", "نوع فایل باید به صورت عکس باشد");
                    var categories = new List<SelectListItem>(
                        _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.Id.ToString()
                        }).ToList());
                    return View(new CategoryEditDto
                    {
                        CategoryTags = categories,
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        IsShowMainPage = model.IsShowMainPage,
                        PicAddress = Findcategory.PicAddress,
                        Slug = model.Slug,
                        PicAlt = model.PicAlt,
                        PicTitle = model.PicTitle
                    });
                }
            }
            if (ModelState.IsValid)
            {
                Findcategory.Name = model.Name;
                Findcategory.Slug = repaceSlug;
                Findcategory.Description = model.Description;
                Findcategory.ParentId = model.ParentId;
                Findcategory.PicAddress = model.File == null ? Findcategory.PicAddress : $"Media/Categories/{stringPath}";
                Findcategory.PicAlt = model.PicAlt == null ? model.Name : model.PicAlt;
                Findcategory.PicTitle = model.PicTitle == null ? model.Name : model.PicTitle;
                Findcategory.IsActive = model.IsActive;
                Findcategory.IsShowMainPage = model.IsShowMainPage;
                Findcategory.Type = "Category";

                _DbContext.Entry(Findcategory).State = EntityState.Modified;
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Category", new { Areas = "Admin" });
            }

            return View(model);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var CategoryFind = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == Id);
            var CategoryDetails = new CategoryListDto()
            {
                Name = CategoryFind.Name,
                Slug = CategoryFind.Slug,
                Description = CategoryFind.Description,
                ParentId = CategoryFind.ParentId,
                PicAddress = CategoryFind.PicAddress,
                PicAlt = CategoryFind.PicAlt,
                PicTitle = CategoryFind.PicTitle,
                IsActive = CategoryFind.IsActive,
                IsShowMainPage = CategoryFind.IsShowMainPage,
            };

            return View(CategoryDetails);
        }

        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var Findcategory = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == Id);
            var categories = new List<SelectListItem>(
                _DbContext.CategoryTags.Where(x => x.Type == "Category").Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList());
            return View(new CategoryDeleteDto
            {
                Id = Findcategory.Id,
                Name = Findcategory.Name,
                Description = Findcategory.Description,
                Slug = Findcategory.Slug,
                ParentId = Findcategory.ParentId,
                PicAddress = Findcategory.PicAddress,
                PicAlt = Findcategory.PicAlt,
                PicTitle = Findcategory.PicTitle,
                IsShowMainPage = Findcategory.IsShowMainPage,
                IsActive = Findcategory.IsActive,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteDto model)
        {
            var Findcategory = _DbContext.CategoryTags.FirstOrDefault(p => p.Id == model.Id);

            if (ModelState.IsValid)
            {
                _DbContext.CategoryTags.Remove(Findcategory);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Category", new { Areas = "Admin" });
            }

            return View(model);
        }

        #endregion

    }
}
