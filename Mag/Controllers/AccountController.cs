using Mag.Common;
using Mag.Data;
using Mag.Models.Dto.ForgetPassword;
using Mag.Models.Dto.Login;
using Mag.Models.Dto.Register;
using Mag.Models.Entities;
using Mag.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly DataBaseContext _DbContext;
        private readonly EmailService _emailService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<Role> roleManager,DataBaseContext DbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _DbContext = DbContext;
            _emailService = new EmailService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto registerUser)
        {
            var FindUser = await _userManager.FindByEmailAsync(registerUser.Email);
            var FindUserName = await _DbContext.Users.FirstOrDefaultAsync(x => x.UserName == registerUser.UserName);
            var UserPhone = _DbContext.Users.Where(p => p.PhoneNumber == registerUser.PhoneNumber).ToList();
            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                ModelState.AddModelError("", "رمز عبور و تکرار رمز عبور یکی نیست");
            }
            if (FindUser != null)
            {
                ModelState.AddModelError("Email", "این ایمیل قبلا ثبت نام شده است");
            }
            if (FindUserName != null)
            {
                ModelState.AddModelError("UserName", "این نام کاربری قبلا ثبت نام شده است");
            }
            if (registerUser.PhoneNumber.Length != 11)
            {
                ModelState.AddModelError("PhoneNumber", "فرمت وارد شده صحیح نمی باشد");
            }
            if (UserPhone.Count != 0)
            {
                ModelState.AddModelError("PhoneNumber", "این شماره تلفن قبلا ثبت شده");
            }
            if (ModelState.IsValid == false)
            {
                return View(registerUser);
            }

            User NewUser = new User
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                PhoneNumber = registerUser.PhoneNumber,
                DateRegister = DateTime.Now,
                DateRegisterPresian = Utility.ConvertToPersian(DateTime.Now),
                LastLoginDate = DateTime.Now,
                LastLoginDatePersian = Utility.ConvertToPersian(DateTime.Now),
                PicTitle = registerUser.PicTitle == null ? registerUser.FirstName + " " + registerUser.LastName : registerUser.PicTitle,
                PicAlt = registerUser.PicAlt == null ? registerUser.FirstName + " " + registerUser.LastName : registerUser.PicAlt
            };

            var result = await _userManager.CreateAsync(NewUser, registerUser.Password);

            if (result.Succeeded)
            {
                var userForRole = await _userManager.FindByEmailAsync(registerUser.Email);
                await _userManager.AddToRoleAsync(userForRole, "user");

                var UserForId = await _userManager.FindByEmailAsync(registerUser.Email);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(NewUser);
                string callBackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = UserForId.Id, token = token }, protocol: Request.Scheme);
                string body = $"لطفا برای تایید ایمیل خود بر روی لینک زیر کلیک کنید <br/> <a href={callBackUrl}> Link </a>";
                var sendEmail = _emailService.Execute(NewUser.Email, body, "تایید ایمیل");
                if (sendEmail.IsCompletedSuccessfully)
                {
                    return View("SendEmail");
                }

                return RedirectToAction("login", "account");
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }

            ViewBag.Message = message;

            return View();
        }

        #endregion

        #region Login


        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginUserDto
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto loginUser)
        {

            var FindUser = await _userManager.FindByEmailAsync(loginUser.Email);

            if (FindUser == null)
            {
                ModelState.AddModelError("", "پسورد یا نام کاربری اشتباه است");
            }

            if (ModelState.IsValid == false)
            {
                return View(loginUser);
            }
            
            _signInManager.SignOutAsync();

            var resultLogin = await _signInManager.PasswordSignInAsync(FindUser,loginUser.Password,loginUser.IsPersistent,true);

            if (resultLogin.Succeeded)
            {
                FindUser.LastLoginDate = DateTime.Now;
                FindUser.LastLoginDatePersian = Utility.ConvertToPersian(DateTime.Now);

                var resultUpdate = await _userManager.UpdateAsync(FindUser);

                if (resultUpdate.Succeeded)
                    return Redirect(loginUser.ReturnUrl);
                else
                    return View(FindUser);
            }
            if (resultLogin.RequiresTwoFactor == true)
            {
                //
            }
            if (resultLogin.IsLockedOut)
            {
                //
            }

            ModelState.AddModelError("","پسورد یا نام کاربری اشتباه است");

            return View();
        }


        #endregion

        #region Logout

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index","home");
        }

        #endregion

        #region Confirm Email
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
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
                return RedirectToAction("Confirmed", "Account");
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

        #region ForgetPassword & ResetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ViewBag.SendEmailFaile = "ممکن است ایمیل وارد شده معتبر نباشد! ";
                return View(model);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callBackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, protocol: Request.Scheme);
            string Body = $"برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید <br/> <a href={callBackUrl}>لینک ریست کردن رمزعبور</a>";
            _emailService.Execute(user.Email, Body, "فراموشی رمز عبور");

            ViewBag.SendEmailSucces = "لینک تنظیم مجدد رمز عبور برای شما ایمیل شد";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId,string token)
        {
            return View( new ResetPasswordDto
            {
                UserId = userId,
                Token = token
            });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (model.Password != model.ConfirmPassword)
                ModelState.AddModelError("Password", "رمزعبور و تایید رمز عبور یکی نیست");
            if (!ModelState.IsValid)
                return View(model);
            var user =await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
            

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }
            else
            {
                ViewBag.Errors = result.Errors;
                return View(model);
            }
        }
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
        #endregion
    }
}
