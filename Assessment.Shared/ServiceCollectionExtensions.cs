using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomExceptionHandling(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddProblemDetails();
            serviceCollection.AddExceptionHandler<CustomExceptionHandler>();
        }
    }
}
