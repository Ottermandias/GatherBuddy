using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace GatherBuddy.Structs;

[JsonConverter(typeof(Converter))]
[InlineArray(Count)]
public partial struct WaymarkSet : IEnumerable<Vector3>
{
    public const int Count = 8;

    private Vector3 _0;

    public static readonly WaymarkSet None = new();

    public WaymarkSet()
    {
        for (var i = 0; i < Count; ++i)
            this[i] = new Vector3(float.NaN);
    }

    private class Converter : JsonConverter<WaymarkSet>
    {
        public override void WriteJson(JsonWriter writer, WaymarkSet value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var item in value)
                serializer.Serialize(writer, item);


            writer.WriteEndArray();
        }

        public override WaymarkSet ReadJson(JsonReader reader, Type objectType, WaymarkSet existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var array = serializer.Deserialize<Vector3[]>(reader);
            if (array == null)
                return existingValue;

            var min = Math.Min(Count, array.Length);
            for (var i = 0; i < min; ++i)
                existingValue[i] = array[i];
            for (var i = min; i < Count; ++i)
                existingValue[i] = new Vector3(float.NaN);
            return existingValue;
        }
    }

    public bool Equals(WaymarkSet other)
    {
        for (var i = 0; i < Count; ++i)
        {
            if (float.IsNaN(this[i].X))
            {
                if (!float.IsNaN(other[i].X))
                    return false;

                continue;
            }

            if (this[i] != other[i])
                return false;
        }

        return true;
    }

    public int CountSet
    {
        get
        {
            var count = 0;
            foreach (ref var item in this)
            {
                if (!float.IsNaN(item.X))
                    ++count;
            }

            return count;
        }
    }

    public bool AnySet
    {
        get
        {
            foreach (ref var item in this)
            {
                if (!float.IsNaN(item.X))
                    return true;
            }

            return false;
        }
    }

    public IEnumerator<Vector3> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
