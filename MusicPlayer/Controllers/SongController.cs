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
    public class SongController : Controller
    {

        SongManager songManager = new SongManager();

        // GET: Song
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Songs()
        {
            return View();
        }

        public JsonResult AddSong()
        {
            var id = Request.Params.Get(AppConstants.ALBUM_ID);
            var name = Request.Params.Get(AppConstants.SONG_NAME);
            var url = Request.Params.Get(AppConstants.SONG_URL);

            var response = new ServiceResponse();
            Guid albumGuid;
            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }
            if(String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(url))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if(!Guid.TryParse(id, out albumGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }

            try
            {
                var song = new Song()
                {
                    songid = Guid.NewGuid(),
                    albumid = albumGuid,
                    name = name,
                    url = url,
                    isfavourite = false,
                    created_at = DateTime.UtcNow
                    
                };

                if (songManager.AddSong(user, song))
                {
                    response.result = SuccessConstants.SONG_ADDED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_ADDING_SONG;

            }
            catch (Exception e)
            {
                
                response.result = e.GetBaseException().Message;
            }
            return Json(response);

        }

        public JsonResult UpdateSong()
        {
            var id = Request.Params.Get(AppConstants.SONG_ID);
            var name = Request.Params.Get(AppConstants.SONG_NAME);
            var url = Request.Params.Get(AppConstants.SONG_URL);

            var response = new ServiceResponse();
            Guid songGuid;
            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(url))
            {
                response.result = ErrorConstants.NO_CHANGES;
                return Json(response);
            }
            if (!Guid.TryParse(id, out songGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }

            try
            {
                var song = new Song() {
                    songid = songGuid,
                    name = name,
                    url = url,
                };

                if (songManager.UpdateSong(user, song))
                {
                    response.result = SuccessConstants.SONG_UPDATED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_UPDATING_SONG;
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);

        }

        public JsonResult Favourite()
        {
            var songId = Request.Params.Get(AppConstants.SONG_ID);
            var favourite = Request.Params.Get(AppConstants.FAVOURITE);

            var response = new ServiceResponse();
            Guid songGuid;
            bool favBool;

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }

            if (!Guid.TryParse(songId, out songGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            if (String.IsNullOrWhiteSpace(favourite))
            {
                response.result = ErrorConstants.REQUIRED_FIELD_EMPTY;
                return Json(response);
            }
            if (!bool.TryParse(favourite, out favBool))
            {
                response.result = ErrorConstants.INVALID_DATA;
                return Json(response);
            }

            try
            {
                if (songManager.Favourite(user, songGuid, favBool))
                {
                    response.result = SuccessConstants.SONG_UPDATED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_UPDATING_SONG;
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult DeleteSong()
        {
            var songId = Request.Params.Get(AppConstants.SONG_ID);
            var response = new ServiceResponse();
            Guid songGuid;

            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                response.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(response);
            }

            if (!Guid.TryParse(songId, out songGuid))
            {
                response.result = ErrorConstants.INVALID_ID;
                return Json(response);
            }
            try
            {
                if (songManager.DeleteSong(user, songGuid))
                {
                    response.result = SuccessConstants.SONG_DELETED;
                    response.status = true;
                    return Json(response);
                }
                else
                    response.result = ErrorConstants.PROBLEM_DELETING_SONG;
            }
            catch (Exception e)
            {

                response.result = e.GetBaseException().Message;
            }
            return Json(response);
        }

        public JsonResult GetSongList()
        {
            var albumid = Request.Params.Get(AppConstants.ALBUM_ID);
            var start = Request.Params.Get(AppConstants.START);
            var size = Request.Params.Get(AppConstants.SIZE);

            int intStart, intSize;
            Guid albumGuid;
            var respone = new ServiceResponse();
            var user = SessionHelper.GetLoggedInUser();
            if (user == null)
            {
                respone.result = ErrorConstants.USER_NOT_LOGGED_IN;
                return Json(respone);
            }
            if(!Guid.TryParse(albumid, out albumGuid))
            {
                respone.result = ErrorConstants.INVALID_ID;
                return Json(respone);
            }
            intStart = !(String.IsNullOrWhiteSpace(start)) && int.TryParse(start, out intStart) ? Math.Max(0, intStart) : AppConstants.START_VALUE;
            intSize = !(String.IsNullOrWhiteSpace(size)) && int.TryParse(size, out intSize) ? Math.Max(0, intSize) : AppConstants.SIZE_VALUE;

            try
            {
                respone.result = songManager.GetSongList(user, albumGuid, intStart, intSize);
                respone.status = true;
            }
            catch (Exception e)
            {

                respone.result = e.GetBaseException().Message;
            }
            return Json(respone);
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
                        if (!Directory.Exists(Server.MapPath("~/uploads/songs")))
                            Directory.CreateDirectory(Server.MapPath("~/uploads/songs"));

                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        var path = Path.Combine(Server.MapPath("~/uploads/songs/"), fileName);
                        file.SaveAs(path);
                        response.result = Path.Combine("/uploads/songs/", fileName);
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
    }
}