using MusicPlayer.Constants;
using MusicPlayer.Managers;
using MusicPlayer.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Managers
{
    public class SongManager: ElasticSearchManager
    {
        public bool AddSong(User user, Song song)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (song == null)
                throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);
            try
            {
                var elasticClient = GetElasticClient();

               
                var response = elasticClient.Index<Song>(song, i => i
                 .Index(ElasticMappingConstants.INDEX_NAME)
                 .Type(ElasticMappingConstants.TYPE_SONG)
                );

                return response.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public string GetSongId(Guid songId)
        {
            if (songId == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Song>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_SONG)
                .Filter(f => f.Term(ConstSong.ID, songId))
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

        public bool UpdateSong(User user, Song song)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (song == null)
                throw new Exception(ErrorConstants.NO_CHANGES);

            try
            {
                var elasticClient = GetElasticClient();

                var songExist = GetSongId(song.songid);
                if (String.IsNullOrWhiteSpace(songExist)) throw new Exception(ErrorConstants.SONG_NOT_FOUND);
                

                var songUpdate = new Dictionary<string, object>();
                if (!String.IsNullOrWhiteSpace(song.name))
                    songUpdate[ConstSong.NAME] = song.name;
                if (!String.IsNullOrWhiteSpace(song.url))
                    songUpdate[ConstSong.URL] = song.url;
                

                var response = elasticClient.Update<Song, object>(u => u
               .Index(ElasticMappingConstants.INDEX_NAME)
               .Type(ElasticMappingConstants.TYPE_SONG)
               .Id(songExist)
               .Doc(songUpdate)
               );

                return response.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool Favourite(User user, Guid songid, bool favourite)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (songid == null)
                throw new Exception(ErrorConstants.INVALID_DATA);
            try
            {
                var elasticClient = GetElasticClient();

                var songExist = GetSongId(songid);
                if (String.IsNullOrWhiteSpace(songExist)) throw new Exception(ErrorConstants.ALBUM_NOT_FOUND);

                var favUpdate = new Dictionary<string, object>();
                favUpdate[ConstSong.ISFAVOURITE] = favourite;

                var response = elasticClient.Update<Song, object>(u => u
               .Index(ElasticMappingConstants.INDEX_NAME)
               .Type(ElasticMappingConstants.TYPE_SONG)
               .Id(songExist)
               .Doc(favUpdate)
               );

                return response.RequestInformation.Success;

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool DeleteSong(User user, Guid songid)
        {
            if (user == null)
                throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);
            if (songid == null)
                throw new Exception(ErrorConstants.INVALID_DATA);
            try
            {
                var elasticClient = GetElasticClient();

                var songExist = GetSongId(songid);
                if (String.IsNullOrWhiteSpace(songExist)) throw new Exception(ErrorConstants.ALBUM_NOT_FOUND);

                var response = elasticClient.Delete<Song>(songExist, d => d
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_SONG)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Song> GetSongList(User user, Guid albumid, int start, int size)
        {
            if (user == null) throw new Exception(ErrorConstants.USER_NOT_LOGGED_IN);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<Song>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_SONG)
                .Filter(f => f.Term(ConstSong.ALBUM_ID, albumid))
                .Skip(start)
                .Take(size)
                );

                var songList = new List<Song>();
                if (response.Total > 0)
                    foreach (var item in response.Hits)
                        songList.Add(item.Source);

                return songList;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}