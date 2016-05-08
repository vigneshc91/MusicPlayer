using MusicPlayer.Constants;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Helper
{
    public class SessionHelper
    {

        public static string SetSession(User session)
        {
            if (session == null) throw new Exception(ErrorConstants.LOGIN_FAILED);

            session.password = null;
            HttpContext.Current.Session["UserProfile"] = session;

            return session.userid.ToString();
        }

        public static void RemoveSession()
        {
            HttpContext.Current.Session.Remove("UserProfile");
        }

        public static User GetLoggedInUser()
        {
            var user = (User) HttpContext.Current.Session["UserProfile"];
            return user;
        }

        }
    }