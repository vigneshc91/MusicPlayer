using MusicPlayer.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Models
{
    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class Song
    {
        [ElasticProperty(Name = ConstSong.ID, Index = FieldIndexOption.NotAnalyzed)]
        public Guid songid { get; set; }

        [ElasticProperty(Name = ConstSong.ALBUM_ID, Index = FieldIndexOption.NotAnalyzed)]
        public Guid albumid { get; set; }

        [ElasticProperty(Name = ConstSong.NAME, Index = FieldIndexOption.Analyzed)]
        public string name { get; set; }

        [ElasticProperty(Name = ConstSong.URL, Index = FieldIndexOption.NotAnalyzed)]
        public string url { get; set; }

        [ElasticProperty(Name = ConstSong.ISFAVOURITE, Index = FieldIndexOption.NotAnalyzed)]
        public bool isfavourite { get; set; }

        [ElasticProperty(Name = ConstSong.CREATED_AT, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }

        public virtual Album album { get; set; }
    }
}