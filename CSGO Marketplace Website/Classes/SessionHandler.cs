using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Classes
{
    public class SessionHandler : ISessionHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public class SessionKeys
        {
            
        }

        public SessionKeys Keys => new SessionKeys();
        public SessionHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetString(string key, string value)
        {
            _session.SetString(key, value);
        }
        public void SetInt(string key, int value)
        {
            _session.SetInt32(key, value);
        }

        public void SetObjectAsJson(string key, object value)
        {
            SetString(key, JsonConvert.SerializeObject(value));
        }

        public string GetString(string key)
        {
            return _session.GetString(key);
        }
        public int? GetInt(string key)
        {
            return _session.GetInt32(key);
        }

        public T GetObjectFromJson<T>(string key)
        {
            var value = GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public bool Isset(string key)
        {
            var isset = false;

            if (_session.Keys != null)
            {
                foreach (var keyItem in _session.Keys)
                {
                    if (key == keyItem.ToString()) { isset = true; }
                }
            }

            return isset;
        }

        public void Delete(string key)
        {
            _session.Remove(key);
        }

        public void DeleteAll()
        {
            _session.Clear();
        }
    }
}