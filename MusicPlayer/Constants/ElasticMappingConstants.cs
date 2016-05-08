using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Constants
{
    public class ElasticMappingConstants
    {
        public const string INDEX_NAME = "musicplayer";
        public const string TYPE_USER = "user";
        public const string TYPE_ALBUM = "album";
        public const string TYPE_SONG = "song";
        public const string ID = "id";
        public const string ELASTIC_SEARCH_URL = "http://127.0.0.1:9200/";
    }

    public class ConstUser
    {
        public const string ID = "id";
        public const string NAME = "name";
        public const string EMAIL = "email";
        public const string PASSWORD = "password";
        public const string CREATED_AT = "created_at";
    }

    public class ConstAlbum
    {
        public const string ID = "album_id";
        public const string USER_ID = "user_id";
        public const string LOGO = "logo";
        public const string NAME = "name";
        public const string GENERE = "genere";
        public const string ARTIST = "artist";
        public const string ISFAVOURITE = "isfavourite";
        public const string CREATED_AT = "created_at";
    }

    public class ConstSong
    {
        public const string ID = "id";
        public const string ALBUM_ID = "album_id";
        public const string NAME = "name";
        public const string URL = "url";
        public const string ISFAVOURITE = "isfavourite";
        public const string CREATED_AT = "created_at";
    }
}