using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MetaBIM
{
    [BsonIgnoreExtraElements]
    [Serializable]
    public class admin : IModel
    {

        public string adminType = "";
        public string versionNumber = "";
        public string updatePackageUrl = "";
        public string updatePackageDescription = "";


        public admin()
        {

        }



        public static string ToJson(admin _item)
        {
            return JsonConvert.SerializeObject(_item);
        }

        public static string ToJsonList(List<admin> _item)
        {
            return JsonConvert.SerializeObject(_item);
        }

        public static admin FromJson(string _json)
        {
            return JsonConvert.DeserializeObject<admin>(_json);
        }

        public static List<admin> FromJsonList(string _json)
        {
            return JsonConvert.DeserializeObject<List<admin>>(_json);
        }
    }
}