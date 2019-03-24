using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace PersonalMailer.Attributes
{
    public class RecaptchaValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var token = value?.ToString();

            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(
                () => new ValidationResult("ReCaptcha validation failed", new[] { validationContext.MemberName }));

            if (string.IsNullOrEmpty(token))
            {
                return errorResult.Value;
            }

            // get secret from configuration
            IConfiguration configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            var secret = configuration.GetValue<string>("Recaptcha:SecretKey");

            // make request
            var url = BuildUrl(token, secret);

            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return errorResult.Value;
            }

            String jsonResponse = response.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            if (jsonData.success != true.ToString().ToLower())
            {
                return errorResult.Value;
            }

            return ValidationResult.Success;
        }

        private string BuildUrl(string token, string secret)
        {
            return $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}";
        }
    }
}