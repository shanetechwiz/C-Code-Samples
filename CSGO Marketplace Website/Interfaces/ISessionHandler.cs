using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classes
{
    public interface ISessionHandler
    {
        void SetString(string key, string value);
        void SetInt(string key, int value);
        void SetObjectAsJson(string key, object value);
        string GetString(string key);
        int? GetInt(string key);
        T GetObjectFromJson<T>(string key);
        bool Isset(string key);
        void Delete(string key);
        void DeleteAll();
    }
}
