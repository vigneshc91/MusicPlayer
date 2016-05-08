using MusicPlayer.Constants;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Models
{

    [ElasticType(IdProperty = ElasticMappingConstants.ID, Name = ElasticMappingConstants.INDEX_NAME)]
    public class User
    {
        [ElasticProperty(Name = ConstUser.ID, Index = FieldIndexOption.NotAnalyzed)]
        public Guid userid { get; set; }

        [ElasticProperty(Name = ConstUser.NAME, Index = FieldIndexOption.Analyzed)]
        public string name { get; set; }

        [ElasticProperty(Name = ConstUser.EMAIL, Index = FieldIndexOption.NotAnalyzed)]
        public string email { get; set; }

        [ElasticProperty(Name = ConstUser.PASSWORD, Index = FieldIndexOption.NotAnalyzed)]
        public string password { get; set; }

        [ElasticProperty(Name = ConstUser.CREATED_AT, Index = FieldIndexOption.NotAnalyzed)]
        public DateTime created_at { get; set; }

    }
}