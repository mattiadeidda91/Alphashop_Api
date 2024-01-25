using System.Net.Http.Headers;
using System.Text.Json;

namespace Alphashop_ArticoliWebService.Services
{
    public class HttpService : IHttpService
    {
        public async Task<T> Get<T>(string token, string url) where T : class
        {
            using var client = new HttpClient();

            //Passiamo il token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Opzionale: consenti la corrispondenza delle proprietà JSON anche se le maiuscole e minuscole non corrispondono esattamente
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };

                var result = JsonSerializer.Deserialize<T>(content, options);

                return result;
            }
            else
                return null;
        }
    }
}
