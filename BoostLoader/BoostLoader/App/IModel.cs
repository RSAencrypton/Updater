using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;


[BsonIgnoreExtraElements]
[Serializable]
public  class IModel
{
    public string guid;
    public string status;       // dev/production/pending/testing/other
    public string created;
    public string updated;

    public IModel()
    {
        guid = Guid.NewGuid().ToString("N");
        status = Config.DevelopmentStage;
        created = DateTime.Now.Ticks.ToString();
        updated = DateTime.Now.Ticks.ToString();
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

}