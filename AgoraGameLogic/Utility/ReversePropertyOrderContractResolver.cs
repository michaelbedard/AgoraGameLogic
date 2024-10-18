using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AgoraGameLogic.Utility;

public class ReversePropertyOrderContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        // Get the properties as usual
        var properties = base.CreateProperties(type, memberSerialization);

        // Reverse the order of properties
        return properties.Reverse().ToList(); // 
    }
}