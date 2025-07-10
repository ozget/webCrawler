using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Elastic.Infrastructure.Configurations
{
    public static class ElasticSearchConfig
    {
        public static void AddElasticSearch(this IServiceCollection services, string uri)
        {

            var pool = new SingleNodeConnectionPool(new Uri(uri));
            var settings = new ConnectionSettings(pool)
                .BasicAuthentication("elastic", "elastic123")
                .ServerCertificateValidationCallback((sender, certificate, chain, errors) => true) // Sertifika bypass
                .DefaultIndex("news")
                .DisableDirectStreaming()
                .EnableDebugMode()
                .ThrowExceptions()
                .PrettyJson()
                .PingTimeout(TimeSpan.FromSeconds(2));

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }
    }
}
