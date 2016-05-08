using MusicPlayer.Constants;
using MusicPlayer.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Helper
{
    public class ElasticSearchMappings
    {
        public void CheckMappings(ElasticClient client)
        {
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_USER)).Exists)
            {
                if (!CreateUser(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_ALBUM)).Exists)
            {
                if (!CreateAlbum(client)) throw new Exception();
            }
            if(!client.TypeExists(typeExists => typeExists.Index(ElasticMappingConstants.INDEX_NAME).Type(ElasticMappingConstants.TYPE_SONG)).Exists)
            {
                if (!CreateSong(client)) throw new Exception();
            }
            
        }

        private bool CreateUser(ElasticClient client)
        {
            var response = client.Map<User>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_USER)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateAlbum(ElasticClient client)
        {
            var response = client.Map<Album>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_ALBUM)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

        private bool CreateSong(ElasticClient client)
        {
            var response = client.Map<Song>(u => u
            .Index(ElasticMappingConstants.INDEX_NAME)
            .Type(ElasticMappingConstants.TYPE_SONG)
            .AllField(af => af.Enabled())
            .MapFromAttributes()
            );

            return response.Acknowledged;
        }

       
    }
}