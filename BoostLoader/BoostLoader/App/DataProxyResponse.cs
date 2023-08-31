using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MetaBIM
{
    [Serializable]
    public class DataProxyResponse<T>
    {
        public bool result;
        public string message;
        public float complete;

        public List<T> package;

        
        public static DataProxyResponse<T> FromJson(string _json)
        {
            return JsonConvert.DeserializeObject<DataProxyResponse<T>>(_json);
        }
        
        public static List<DataProxyResponse<T>> FromJsonList(string _json)
        {
            return JsonConvert.DeserializeObject<List<DataProxyResponse<T>>>(_json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}
