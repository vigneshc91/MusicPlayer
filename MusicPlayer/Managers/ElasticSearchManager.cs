using MusicPlayer.Constants;
using MusicPlayer.Helper;
using MusicPlayer.Models;
using Elasticsearch.Net.ConnectionPool;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Managers
{
    public class ElasticSearchManager
    {
        static Uri node = new Uri(ElasticMappingConstants.ELASTIC_SEARCH_URL);
        static bool isDbInitialized = false;
        static SniffingConnectionPool connectionPool = new SniffingConnectionPool(new[] { node });

        public ElasticSearchManager()
        {
            if (!isDbInitialized)
            {
                InitializeDb();
                isDbInitialized = true;
            }
        }

        private void InitializeDb()
        {
            var mappings = new ElasticSearchMappings();
            var elasticClient = GetElasticClient();
            try
            {
                if (!elasticClient.IndexExists(ElasticMappingConstants.INDEX_NAME).Exists)
                {
                    var indexCreationResponse = elasticClient.CreateIndex(i => i
                                                    .Index(ElasticMappingConstants.INDEX_NAME));

                    if(indexCreationResponse.ConnectionStatus.HttpStatusCode != 200)
                    {
                        throw new Exception(ErrorConstants.INDEX_CREATE_FAILED);
                    }

                    mappings.CheckMappings(elasticClient);
                }
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
            }
        }

       

        public ElasticClient GetElasticClient()
        {
            ConnectionSettings settings = new ConnectionSettings(connectionPool, ElasticMappingConstants.INDEX_NAME);
            settings.ThrowOnElasticsearchServerExceptions(true);
            return new ElasticClient(settings);
        }
    }
}