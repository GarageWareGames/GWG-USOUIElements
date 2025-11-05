using System;
using System.Collections.Generic;
using UnityEngine;


namespace GWG.UsoUiElements
{
    /// <summary>
    /// - Defines a static utility class for serializing lists to JSON in Unity.
    /// - Provides a generic method that:
    ///   - Wraps the list in a private serializable `Wrapper` class (with a single `Items` field).
    ///   - Uses `JsonUtility.ToJson` to serialize the wrapper,
    ///   - enabling list serialization (which Unity's `JsonUtility` does not support directly).
    ///   - Supports pretty-printing via an optional parameter.
    /// - Simplifies converting lists to JSON strings for storage or transmission in Unity projects.
    /// </summary>
    public static class JsonHelper
    {
        public static string ToJson<T>(List<T> list, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(new Wrapper<T>
            {
                Items = list
            }, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}