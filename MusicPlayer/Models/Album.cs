using MusicPlayer.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class Album
    {
        [ElasticProperty(Name = ConstAlbum.ID, Index = FieldIndexOption.NotAnalyzed)]
        public Guid albumid { get; set; }

        [ElasticProperty(Name = ConstAlbum.USER_ID, Index = FieldIndexOption.NotAnalyzed)]
        public Guid userid { get; set; }

        [ElasticProperty(Name = ConstAlbum.LOGO, Index = FieldIndexOption.NotAnalyzed)]
        public string logo { get; set; }

        [ElasticProperty(Name = ConstAlbum.NAME, Index = FieldIndexOption.Analyzed)]
        public string name { get; set; }

        [ElasticProperty(Name = ConstAlbum.GENERE, Index = FieldIndexOption.Analyzed)]
        public string genere { get; set; }

        [ElasticProperty(Name = ConstAlbum.ARTIST, Index = FieldIndexOption.Analyzed)]
        public string artist { get; set; }

        [ElasticProperty(Name = ConstAlbum.ISFAVOURITE, Index = FieldIndexOption.NotAnalyzed)]
        public bool isfavourite { get; set; }

        [ElasticProperty(Name = ConstAlbum.CREATED_AT, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }
    }
}