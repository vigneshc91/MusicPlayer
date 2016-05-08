using MusicPlayer.Constants;
using MusicPlayer.Helper;
using MusicPlayer.Managers;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPlayer.Controllers
{
    public class UserController : Controller
    {

        UserManager userManager = new UserManager();
        
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult Register()
        {
            
            var name = Request.Params.Get(AppConstants.NAME);
            var email = Request.Params.Get(AppConstants.EMAIL);
            var password = Request.Params.Get(AppConstants.PASSWORD);

            var response = new ServiceResponse();

           
            if(String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            try
            {
                var user = new User()
                {
                    userid = Guid.NewGuid(),
                    name = name,
                    email = email,
                    password = PasswordHash.Encrypt(password),
                    created_at = DateTime.UtcNow
                };

                if (userManager.AddUser(user))
                {
                    response.result = SuccessConstants.USER_ADDED;
                    response.status = true;
                }
                else
                    response.result = ErrorConstants.PROBLEM_ADDING_USER;

                return Json(response);
            }
            catch (Exception e)
            {
                
                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);

        }

        public JsonResult LoginWithEmail()
        {
            var email = Request.Params.Get(AppConstants.EMAIL);
            var password = Request.Params.Get(AppConstants.PASSWORD);

            var response = new ServiceResponse();
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            var loggedInUser = SessionHelper.GetLoggedInUser();
            if (loggedInUser != null)
            {
                response.result = ErrorConstants.SOMEBODY_LOGGEDIN;
                return Json(response);
            }

            try
            {
                var user = userManager.LoginUser(email, PasswordHash.Encrypt(password));
                if (user != null)
                {
                    if (!String.IsNullOrWhiteSpace(SessionHelper.SetSession(user)))
                    {
                        response.result = SuccessConstants.LOGIN_SUCCESS;
                        response.status = true;
                    }
                    else
                        response.result = ErrorConstants.LOGIN_FAILED;

                }
                else
                    response.result = ErrorConstants.INVALID_USERNAME_OR_PASSWORD;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                response.result = e.GetBaseException().Message;
            }

            return Json(response);
        }

        public JsonResult LoginWithFacebook()
        {
            var email = Request.Params.Get(AppConstants.EMAIL);
            var name = Request.Params.Get(AppConstants.NAME);

            var response = new ServiceResponse();
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            var loggedInUser = SessionHelper.GetLoggedInUser();
            if (loggedInUser != null)
            {
                response.result = ErrorConstants.SOMEBODY_LOGGEDIN;
                return Json(response);
            }
            try
            {
                var user = userManager.LoginWithFacebook(name, email);
                if (user != null)
                {
                    if (!String.IsNullOrWhiteSpace(SessionHelper.SetSession(user)))
                    {
                        response.result = SuccessConstants.LOGIN_SUCCESS;
                        response.status = true;
                    }
                    else
                        response.result = ErrorConstants.LOGIN_FAILED;

                }
                else
                    response.result = ErrorConstants.INVALID_USERNAME_OR_PASSWORD;
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public ActionResult Logout()
        {
            try
            {
                var user = SessionHelper.GetLoggedInUser();


                if (user == null)
                {
                    return Redirect("/User/Login");
                }

                var status = userManager.LogoutUser();
                if (status)
                    return Redirect("/User/Login");

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);

            }

            return View();
        }

       
    }
}