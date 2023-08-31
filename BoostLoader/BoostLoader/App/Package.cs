
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


[BsonIgnoreExtraElements]
[Serializable]
public class Package: IModel
{
    public string key           = "xVgsbCuuxkCGFDwLk1EpEQ";
    public string profileGuid   = "admin";
    public string itemGuid      = "default";
    public string endpoint      = "0.0.0.0";
    public string target        = "Profile";
    public string package       = "{}";

    public List<Filter> filters = 
        new List<Filter> { new Filter("status", Config.DevelopmentStage) };




    public static string ToJson(Package _item)
    {
        return JsonConvert.SerializeObject(_item);
    }

    public static Package FromJson(string _json)
    {
        return JsonConvert.DeserializeObject<Package>(_json);
    }

    public static List<Package> FromJsonList(string _json)
    {
        return JsonConvert.DeserializeObject<List<Package>>(_json);
    }
}

[Serializable]
public class Filter
{
    public string key;
    public string value;
    public string condition = "eq";

    public Filter(string key, string value, string condition = "eq")
    {
        this.key = key;
        this.value = value;
        this.condition = condition;
    }
}


