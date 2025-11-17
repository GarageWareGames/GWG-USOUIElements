using System;
using System.Collections.Generic;
using UnityEngine;


namespace GWG.UsoUIElements.Utilities
{
    /// <summary>
    /// - Defines a static utility class for serializing lists to JSON in Unity.
    /// </summary>
    /// <remarks>
    /// Provides a generic method that:
    /// <list type="bullet">
    /// - Wraps the list in a private serializable `Wrapper` class (with a single `Items` field).
    /// - Uses `JsonUtility.ToJson` to serialize the wrapper,
    /// - enabling list serialization (which Unity's `JsonUtility` does not support directly).
    /// - Supports pretty-printing via an optional parameter.
    /// - Simplifies converting lists to JSON strings for storage or transmission in Unity projects.
    /// </list>
    /// </remarks>
    public static class JsonHelper
    {
        public static string ToJson<T>(List<T> list, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(new Wrapper<T>
            {
                Items = list
            }, prettyPrint);
        }

        /// <summary>
        /// - A private serializable wrapper class for lists of generic type `T`.
        /// </summary>
        /// <typeparam name="T">Generic Wrapper Type T</typeparam>
        [Serializable]
        private class Wrapper<T>
        {
            public List<T> Items;
        }
    }
}