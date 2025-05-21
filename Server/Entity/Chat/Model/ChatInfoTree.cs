using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using ProtoBuf;

namespace Fantasy;

public partial class ChatInfoTree
{
    [BsonIgnore]
    [JsonIgnore]
    [ProtoIgnore]
    [IgnoreDataMember]
    public Scene Scene { get; set; }
}