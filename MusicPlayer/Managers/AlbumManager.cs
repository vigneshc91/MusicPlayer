using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicPlayer.Models;
using MusicPlayer.Managers;
using MusicPlayer.Constants;
using Nest;

namespace BillingSoftware.Managers
{
    public class AlbumManager : ElasticSearchManager
    {
        public bool AddAlbum(User user, Album album)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (album == null)
                throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            try
            {
                var elasticClient = GetElasticClient();

                var existingAlbum = GetAlbumByName(album.name);
                if (existingAlbum != null) throw new Exception(ErrorConstants.ALBUM_ALREADY_EXIST);

                var response = elasticClient.Index<Album>(album, i => i
                 .Index(ElasticMappingConstants.INDEX_NAME)
                 .Type(ElasticMappingConstants.TYPE_ALBUM)
                );

                return response.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public Album GetAlbumByName(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Album>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ALBUM)
                .Filter(f => f.Term(ConstAlbum.NAME, name))
                .Size(1));

                Album retAlbum = null;

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        retAlbum = item.Source;

                return retAlbum;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Album GetAlbumById(User user, Guid albumId)
        {
            if (user == null) throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (albumId == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Album>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ALBUM)
                .Filter(f => f.Term(ConstAlbum.ID, albumId))
                .Size(1));

                Album retAlbum = null;

                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        retAlbum = item.Source;

                return retAlbum;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public string GetAlbumId(Guid albumID)
        {
            if (albumID == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Album>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ALBUM)
                .Filter(f => f.Term(ConstAlbum.ID, albumID))
                .Size(1));


                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        return item.Id;

                return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool UpdateAlbum(User user, Album album)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (album == null)
                throw new Exception(ErrorConstants.NO_CHANGES);

            try
            {
                var elasticClient = GetElasticClient();

                var albumExist = GetAlbumId(album.albumid);
                if (String.IsNullOrWhiteSpace(albumExist)) throw new Exception(ErrorConstants.ALBUM_NOT_FOUND);
                var existingAlbum = GetAlbumByName(album.name);
                if (existingAlbum != null) throw new Exception(ErrorConstants.ALBUM_ALREADY_EXIST);

                var albumUpdate = new Dictionary<string, object>();
                if (!String.IsNullOrWhiteSpace(album.name))
                    albumUpdate[ConstAlbum.NAME] = album.name;
                if (!String.IsNullOrWhiteSpace(album.logo))
                    albumUpdate[ConstAlbum.LOGO] = album.logo;
                if (!String.IsNullOrWhiteSpace(album.genere))
                    albumUpdate[ConstAlbum.GENERE] = album.genere;
                if (!string.IsNullOrWhiteSpace(album.artist))
                    albumUpdate[ConstAlbum.ARTIST] = album.artist;

                var response = elasticClient.Update<Album, object>(u => u
               .Index(ElasticMappingConstants.INDEX_NAME)
               .Type(ElasticMappingConstants.TYPE_ALBUM)
               .Id(albumExist)
               .Doc(albumUpdate)
               );

                return response.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool Favourite(User user, Guid albumid, bool favourite)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (albumid == null)
                throw new Exception(ErrorConstants.INVALID_DATA);
            try
            {
                var elasticClient = GetElasticClient();

                var albumExist = GetAlbumId(albumid);
                if (String.IsNullOrWhiteSpace(albumExist)) throw new Exception(ErrorConstants.ALBUM_NOT_FOUND);

                var favUpdate = new Dictionary<string, object>();
                favUpdate[ConstAlbum.ISFAVOURITE] = favourite;

                var response = elasticClient.Update<Album, object>(u => u
               .Index(ElasticMappingConstants.INDEX_NAME)
               .Type(ElasticMappingConstants.TYPE_ALBUM)
               .Id(albumExist)
               .Doc(favUpdate)
               );

                return response.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool DeleteAlbum(User user, Guid albumid)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (albumid == null)
                throw new Exception(ErrorConstants.INVALID_DATA);
            try
            {
                var elasticClient = GetElasticClient();

                var albumExist = GetAlbumId(albumid);
                if (String.IsNullOrWhiteSpace(albumExist)) throw new Exception(ErrorConstants.ALBUM_NOT_FOUND);

                var response = elasticClient.Delete<Album>(albumExist, d => d
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ALBUM)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Album> GetAlbumList(User user, int start, int size)
        {
            if (user == null) throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Album>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ALBUM)
                .Filter(f => f.Term(ConstAlbum.USER_ID, user.userid))
                .Skip(start)
                .Take(size)
                );

                var albumList = new List<Album>();
                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        albumList.Add(item.Source);

                return albumList;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Tuple<List<Album>, List<Song>> SearchAlbumSong(User user, string query, int start, int size)
        {
            if (user == null) throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (String.IsNullOrWhiteSpace(query)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            try
            {
                var elasticClient = GetElasticClient();

                var albumResponse = elasticClient.Search<Album>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_ALBUM)
                .Filter(f => f.Term(ConstAlbum.USER_ID, user.userid))
                .Query(q => q.Prefix(ConstAlbum.NAME, query))
                .Skip(start)
                .Take(size)
                );

                var songResponse = elasticClient.Search<Song>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_SONG)
                .Query(q => q.Prefix(ConstSong.NAME, query))
                .Skip(start)
                .Take(size)
                );

                var albumList = new List<Album>();
                if (albumResponse.Total > 0)
                    foreach (var item in albumResponse.Hits)
                        albumList.Add(item.Source);

                var songList = new List<Song>();
                if (songResponse.Total > 0)
                    foreach (var item in songResponse.Hits)
                        songList.Add(item.Source);

                var retResponse = new Tuple<List<Album>, List<Song>>(albumList, songList);
                
                return retResponse;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


    }
}