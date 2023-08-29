using Mag.Areas.Admin.Models.Dto.News;
using Mag.Data;
using Mag.Models.Dto.Profile;
using Mag.Models.Entities;
using Mag.Services;
using Mag.Services.FileUploadService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Mag.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataBaseContext _DbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IFileUploadService _fileUpload;
        private readonly EmailService _emailService;
        public ProfileController(DataBaseContext dataBaseContext,UserManager<User> userManager,IFileUploadService fileUploadService,RoleManager<Role> roleManager)
        {
            _DbContext = dataBaseContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _fileUpload = fileUploadService;
            _emailService = new EmailService();
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var ListNews = _DbContext.News.Where(p => p.WriterId == userId).Select(p => new NewsListDto
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IndexImageAddress = p.IndexImageAddress,
                IndexImageAddressAlt = p.IndexImageAddressAlt,
                IndexImageAddressTitle = p.IndexImageAddressTitle,
                WriterId = p.WriterId,
                Categories = p.Categories,
                IsActive = p.IsActive,
                Status = p.Status,
            }).ToList();

            var role = await _roleManager.FindByNameAsync("writer");
            var RoleUserWriter = _DbContext.UserRoles.Where(p => p.UserId == userId && p.RoleId == role.Id).ToList().Count();

            var roles = _DbContext.UserRoles.Where(p => p.UserId == userId).ToList();
            var roleNme = new StringBuilder();
            foreach (var item in roles)
            {
                var a = await _roleManager.FindByIdAsync(item.RoleId);
                roleNme.Append($"{a.Name},");
            }
            var RetVal = new Tuple<User, List<NewsListDto>,string?,int?>(user,ListNews,roleNme.ToString(), RoleUserWriter);

            return View(RetVal);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var userEdit = new PUserEditDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PicAddress = user.PicAddress,
            };
            return View(userEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PUserEditDto model)
        {
            var stringPath = "";
            var FindUser = await _userManager.FindByIdAsync(model.Id);
            if (model.FirstName == null)
            {
                ModelState.AddModelError("FirstName", "فرمت وارد شده صحیح نمی باشد");
            }
            if (model.LastName == null)
            {
                ModelState.AddModelError("LastName", "فرمت وارد شده صحیح نمی باشد");
            }
            if (model.PhoneNumber == null || model.PhoneNumber.Length != 11)
            {
                ModelState.AddModelError("PhoneNumber", "فرمت وارد شده صحیح نمی باشد");
            }
            if (model.File != null)
            {
                if (model.File.Length > 1000000)
                {
                    ModelState.AddModelError("PicAddress", "حجم عکس باید زیر یک مگابایت باشد");
                    var userEdit = new PUserEditDto()
                    {
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        PicAddress = FindUser.PicAddress,
                    };
                    return View(userEdit);
                }
                if (model.File.ContentType == "image/png" || model.File.ContentType == "image/jpg" ||
                    model.File.ContentType == "image/jpeg" || model.File.ContentType == "image/gif")
                {
                    stringPath = await _fileUpload.UploadFileAsync(model.File, model.FirstName + " " + model.LastName, "Users");
                }
                else
                {
                    ModelState.AddModelError("PicAddress", "نوع فایل باید به صورت عکس باشد");
                    var userEdit = new PUserEditDto()
                    {
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        PicAddress = FindUser.PicAddress,
                    };
                    return View(userEdit);
                }
            }
            if (ModelState.IsValid == false)
            {
                model.PicAddress = FindUser.PicAddress;
                return View(model);
            }

            FindUser.FirstName = model.FirstName;
            FindUser.LastName = model.LastName;
            FindUser.PhoneNumber = model.PhoneNumber;
            FindUser.PicAddress = model.File == null ? FindUser.PicAddress : $"Media/Users/{stringPath}";

            var result = await _userManager.UpdateAsync(FindUser);

            if (result.Succeeded)
            {
                return RedirectToAction("index", "Profile");
            }

            return View();
        }
        #endregion

        #region EditEmail
        public async Task<IActionResult> EditEmail(PUserEditDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var FindUser = await _userManager.FindByIdAsync(model.Id);
            var ExistUser = await _userManager.FindByEmailAsync(model.NewEmail);
            if (ExistUser != null)
            {
                ModelState.AddModelError("NewEmail", "این ایمیل قبلا ثبت شده است");
            }
            if (model.NewEmail != model.NewEmailConfirm)
            {
                ModelState.AddModelError("NewEmail", "مقادیر وارد شده یکسان نیست");
            }
            if (model.NewEmail == null)
            {
                ModelState.AddModelError("NewEmail", "فرمت وارد شده صحیح نمی باشد");
            }
            if (model.NewEmailConfirm == null)
            {
                ModelState.AddModelError("NewEmailConfirm", "فرمت وارد شده صحیح نمی باشد");
            }
            if (ModelState.IsValid == false)
            {
                model.PicAddress = FindUser.PicAddress;
                model.FirstName = FindUser.FirstName;
                model.LastName = FindUser.LastName;
                model.PhoneNumber = FindUser.PhoneNumber;
                return View("Edit",model);
            }

            FindUser.Email = model.NewEmail;
            var result = await _userManager.UpdateAsync(FindUser);

            if (result.Succeeded)
            {
                return RedirectToAction("index", "Profile");
            }

            return View();
        }
        #endregion

        #region Confirm Email
        public async Task<IActionResult> SendEmail()
        { 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string callBackUrl = Url.Action("ConfirmEmail", "Profile", new { UserId = userId, token = token }, protocol: Request.Scheme);
            string body = $"لطفا برای تایید ایمیل خود بر روی لینک زیر کلیک کنید <br/> <a href={callBackUrl}> Link </a>";
            _emailService.Execute(user.Email, body, "تایید ایمیل");

            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Confirmed", "Profile");
            }
            else
            {
                return View("Error");
            }
        }
        public IActionResult Confirmed()
        {
            return View();
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(PUserEditDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var FindUser = await _userManager.FindByIdAsync(userId);
            var result = _userManager.PasswordHasher.VerifyHashedPassword(FindUser,FindUser.PasswordHash, model.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("CurrentPassword", "مقدار وارد شده صحیح نمی باشد");
            }
            if (model.CurrentPassword == null || model.NewPassword == null || model.ConfirmNewPassword == null)
            {
                ModelState.AddModelError("", "مقدار وارد شده صحیح نمی باشد");
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError("NewPassword", "رمز عبور و تایید صحیح نمیباشد");
            }
            if (ModelState.IsValid == false)
            {
                model.PicAddress = FindUser.PicAddress;
                model.FirstName = FindUser.FirstName;
                model.LastName = FindUser.LastName;
                model.PhoneNumber = FindUser.PhoneNumber;
                return View("Edit", model);
            }
            if (result == PasswordVerificationResult.Success)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(FindUser);
                var Changepassword = await _userManager.ResetPasswordAsync(FindUser, token, model.NewPassword);
                if (Changepassword.Succeeded)
                {
                    return Redirect("/Account/ResetPasswordConfirm");
                }
            }

            //if (userId == null || token == null)
            //{
            //    return BadRequest();
            //}
            //var user = await _userManager.FindByIdAsync(userId);
            //if (user == null)
            //{
            //    return View("Error");
            //}

            //var result = await _userManager.ConfirmEmailAsync(user, token);
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Confirmed", "Profile");
            //}
            //else
            //{
            //    return View("Error");
            //}
            return View();
        }
        #endregion

        #region RequestForWriting
        public async Task<IActionResult> RequestForWriting(string userId)
        {
            var userForRole = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(userForRole, "writer");
            return RedirectToAction("Index", "Profile");
        }
        #endregion

    }
}
