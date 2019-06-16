using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services 
{
    public class UserServices
    {
        private readonly SessionHandler _session;
        private readonly CommonServices _commonServices;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServices(IHttpContextAccessor httpContextAccessor,
        CommonServices commonServices, IConfiguration configuration)
        {
            _session = new SessionHandler(httpContextAccessor);
            _commonServices = commonServices;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Item>> GetUserInventory(int appId, int contextId)
        {
            try
            {
                if(_session.Isset(_session.Keys.UserToken))
                {
                    var token = _session.GetString(_session.Keys.UserToken);
                    var requestObject = new { token = token, game_app_id = appId, game_context_id = contextId };
                    var url = _configuration.GetValue<string>("BackendSettings:APIHost") 
                    + _configuration.GetValue<string>("BackendSettings:Endpoints:GetUserInventory");
                    var response = await _commonServices.MakeWebRequest(url, JsonConvert.SerializeObject(requestObject));
                    
                    if(response.IsSuccess) 
                    {
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse>(response.PayLoad);

                        if(apiResponse != null && apiResponse.success)
                        {
                            var userInventory = JsonConvert.DeserializeObject<List<Item>>(apiResponse.data);
                            _session.SetObjectAsJson(_session.Keys.UserInventory, userInventory);
                            return userInventory;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"GetUserInventory Exception: {ex.Message}");
            }

            return new List<Item>();
        }
    }
}