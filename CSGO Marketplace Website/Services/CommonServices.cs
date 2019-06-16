using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CommonServices
    {
        public async Task<RequestResponse> MakeWebRequest(string url, string jsonRequestObject = null)
        {
            RequestResponse requestResponse = new RequestResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.Timeout = TimeSpan.FromMinutes(10);

                    var response = (String.IsNullOrEmpty(jsonRequestObject)) ? await client.GetAsync(url)
                        : await client.PostAsync("", new StringContent(jsonRequestObject, Encoding.UTF8, "application/json"));

                    requestResponse.ResponseStatus = response.StatusCode;
                    requestResponse.IsSuccess = response.IsSuccessStatusCode;

                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        requestResponse.ErrorMessage = errorResponse;
                        throw new Exception(errorResponse);
                    }

                    var stringResponse = await response.Content.ReadAsStringAsync();
                    requestResponse.Data = stringResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while attempting to process your request. {ex}");
                requestResponse.IsSuccess = false;
                requestResponse.ErrorMessage = ex.Message;
                requestResponse.Data = null;
            }

            return requestResponse;
        }
    }
}
