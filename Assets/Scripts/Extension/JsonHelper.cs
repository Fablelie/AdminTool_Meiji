// From https://forum.unity3d.com/threads/how-to-load-an-array-with-jsonutility.375735/
// Author: ffleurey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class JsonHelper : MonoBehaviour
{
    public static T[] FromJsonArray<T>(string json)
    {
        string newJson = new StringBuilder().Append("{\"array\": ").Append(json).Append("}").ToString();
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] array;
    }
}
