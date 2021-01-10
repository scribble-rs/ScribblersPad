﻿using Newtonsoft.Json;
using System;
using System.Drawing;

/// <summary>
/// Scribble.rs ♯ JSON converters namespace
/// </summary>
namespace ScribblersSharp.JSONConverters
{
    /// <summary>
    /// A class used for convert colors to JSON and vice versa
    /// </summary>
    internal class ColorJSONConverter : JsonConverter<Color>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Color</returns>
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Color ret = existingValue;
            if (reader.Value is string value && (value.Length == 7))
            {
                if
                (
                    value[0] == '#' &&
                    byte.TryParse(value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber, null, out byte red) &&
                    byte.TryParse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber, null, out byte green) &&
                    byte.TryParse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber, null, out byte blue)
                )
                {
                    ret = Color.FromArgb(0xFF, red, green, blue);
                }
            }
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer) => writer.WriteValue($"#{ value.R:X2}{ value.G:X2}{ value.B:X2}");
    }
}
