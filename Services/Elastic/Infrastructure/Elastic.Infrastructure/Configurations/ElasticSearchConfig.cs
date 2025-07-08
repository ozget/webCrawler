using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Elastic.Infrastructure.Configurations
{
    public static class ElasticSearchConfig
    {
        public static void AddElasticSearch(this IServiceCollection services, string uri)
        {
            var settings = new ConnectionSettings(new Uri(uri)).DefaultIndex("news");
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }
    }
}
