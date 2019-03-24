using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

namespace PersonalMailer.Sender
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGrid(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new SendGridConfiguration();
            configuration.Bind(config);

            services.AddTransient<ISendGridClient>(_ => new SendGridClient(config.ApiKey));
            services.AddTransient<ISendGridSender, SendGridSender>();

            services.Configure<SendGridConfiguration>(configuration);

            return services;
        }
    }
}