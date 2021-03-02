﻿using Newtonsoft.Json;

/// <summary>
/// Scribble.rs ♯ data namespace
/// </summary>
namespace ScribblersSharp.Data
{
    /// <summary>
    /// A class that describes a sendable "keep-alive" game message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class KeepAliveSendGameMessageData : BaseGameMessageData, ISendGameMessageData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public KeepAliveSendGameMessageData() : base(Naming.GetSendGameMessageDataNameInKebabCase<KeepAliveSendGameMessageData>())
        {
            // ...
        }
    }
}
