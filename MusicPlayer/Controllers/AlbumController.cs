using BillingSoftware.Managers;
using MusicPlayer.Constants;
using MusicPlayer.Helper;
using MusicPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillingSoftware.Controllers
{
    public class AlbumController : Controller
    {

        AlbumManager albumManager = new AlbumManager();

        // GET: Album
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Albums()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public JsonResult UploadFile()
        {
            var response = new ServiceResponse();

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }

            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        if (!Directory.Exists(Server.MapPath("~/uploads")))
                            Directory.CreateDirectory(Server.MapPath("~/uploads"));
                        if(!Directory.Exists(Server.MapPath("~/uploads/logo")))
                            Directory.CreateDirectory(Server.MapPath("~/uploads/logo"));

                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        var path = Path.Combine(Server.MapPath("~/uploads/logo/"), fileName);
                        file.SaveAs(path);
                        response.result = Path.Combine("/uploads/logo/", fileName);
                        response.status = true;
                    }
                    return Json(response);
                }
                response.result = "File upload failed";
            }
            catch (Exception e)
            {
               
                response.result = e.GetBaseException().Message;
            }
            return Json(response);

            
        }

        public JsonResult AddAlbum()
        {
            var name = Request.Params.Get(AppConstants.ALBUM_NAME);
            var logo = Request.Params.Get(AppConstants.LOGO);
            var genere = Request.Params.Get(AppConstants.GENERE);
            var artist = Request.Params.Get(AppConstants.ARTIST);

            var response = new ServiceResponse();

            var user = SessionHelper.GetLoggedInUser();
            if(user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }

            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(logo) || String.IsNullOrWhiteSpace(genere) || String.IsNullOrWhiteSpace(artist))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }

            try
            {
                var album = new Album() {
                    albumid = Guid.NewGuid(),
                    userid = user.userid,
                    logo = logo,
                    name = name,
                    genere = genere,
                    artist = artist,
                    isfavourite = false,
                    created_at = DateTime.UtcNow
                };

                if (albumManager.AddAlbum(user, album))
                {
                    response.result = SuccessConstants.ALBUM_CREATED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_CREATING_ALBUM;
                
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult GetAlbumById()
        {
            var albumId = Request.Params.Get(AppConstants.ALBUM_ID);
            var response = new ServiceResponse();

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }
            Guid albumGuid;

            if (!Guid.TryParse(albumId, out albumGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            try
            {
                response.result = albumManager.GetAlbumById(user, albumGuid);
                response.status = true;
                return Json(response);
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult UpdateAlbum()
        {
            var albumId = Request.Params.Get(AppConstants.ALBUM_ID);
            var name = Request.Params.Get(AppConstants.ALBUM_NAME);
            var logo = Request.Params.Get(AppConstants.LOGO);
            var genere = Request.Params.Get(AppConstants.GENERE);
            var artist = Request.Params.Get(AppConstants.ARTIST);

            var response = new ServiceResponse();

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }
            Guid albumGuid;
            
            if(!Guid.TryParse(albumId, out albumGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(logo) && String.IsNullOrWhiteSpace(genere) && String.IsNullOrWhiteSpace(artist))
            {
                response.result = ErrorConstants.NO_CHANGES;
                return Json(response);
            }
            

            try
            {
                var album = new Album()
                {
                    albumid = albumGuid,
                    logo = logo,
                    name = name,
                    genere = genere,
                    artist = artist
                    
                };
                

                if (albumManager.UpdateAlbum(user, album))
                {
                    response.result = SuccessConstants.ALBUM_UPDATED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_UPDATING_ALBUM;

            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult Favourite()
        {
            var albumId = Request.Params.Get(AppConstants.ALBUM_ID);
            var favourite = Request.Params.Get(AppConstants.FAVOURITE);

            var response = new ServiceResponse();
            Guid albumGuid;
            bool favBool;

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }

            if (!Guid.TryParse(albumId, out albumGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(favourite))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if(!bool.TryParse(favourite, out favBool))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            }

            try
            {
                if (albumManager.Favourite(user, albumGuid, favBool))
                {
                    response.result = SuccessConstants.ALBUM_UPDATED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_UPDATING_ALBUM;
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult DeleteAlbum()
        {
            var albumId = Request.Params.Get(AppConstants.ALBUM_ID);
            var response = new ServiceResponse();
            Guid albumGuid;

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }

            if (!Guid.TryParse(albumId, out albumGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            try
            {
                if (albumManager.DeleteAlbum(user, albumGuid))
                {
                    response.result = SuccessConstants.ALBUM_DELETD;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_DELETING_ALBUM;
            }
            catch (Exception e)
            {
                
                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult GetAlbumList()
        {
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            int intStart, intSize;

            var respone = new ServiceResponse();
            var user = SessionHelper.GetLoggedInUser();
            if(user == null)
            {
                respone.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(respone);
            }

            intStart = !(String.IsNullOrWhiteSpace(start)) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !(String.IsNullOrWhiteSpace(size)) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                respone.result = albumManager.GetAlbumList(user, intStart, intSize);
                respone.status = true;
            }
            catch (Exception e)
            {
                
                respone.result = e.GetBaseException().Message;
            }
            return Json(respone);
        }

        public JsonResult SearchAlbumSong()
        {
            var query = Request.Params.Get(AppConstants.QUERY);
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            int intStart, intSize;

            var respone = new ServiceResponse();
            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                respone.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(respone);
            }
            if (String.IsNullOrWhiteSpace(query))
            {
                respone.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(respone);
            }

            intStart = !(String.IsNullOrWhiteSpace(start)) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !(String.IsNullOrWhiteSpace(size)) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                respone.result = albumManager.SearchAlbumSong(user, query, intStart, intSize);
                respone.status = true;
            }
            catch (Exception e)
            {

                respone.result = e.GetBaseException().Message;
            }
            return Json(respone);
        }

    }
}