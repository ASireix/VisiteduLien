using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Serializer
{
    public static T Load<T>(string base64string) where T: class{
        var bytes = Convert.FromBase64String(base64string);
        var memoryStream = new MemoryStream(bytes);

        BinaryFormatter formatter = new BinaryFormatter();
        return formatter.Deserialize(memoryStream) as T;

    }

    public static string SaveToBase64<T>(T data) where T: class{
        using (MemoryStream stream = new MemoryStream()){
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, data);
            string result = Convert.ToBase64String(stream.ToArray());
            return result;
        }
    }
}
