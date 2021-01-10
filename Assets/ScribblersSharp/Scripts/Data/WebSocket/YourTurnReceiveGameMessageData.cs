﻿using Newtonsoft.Json;
/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// Your turn receive game message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class YourTurnReceiveGameMessageData : GameMessageData<string[]>, IReceiveGameMessageData
    {
        // ...
    }
}