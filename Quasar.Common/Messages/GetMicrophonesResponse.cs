using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class GetMicrophonesResponse : IMessage
    {
        [ProtoMember(1)]
        public string[] MicrophoneNames { get; set; }

        [ProtoMember(2)]
        public string[] MicrophoneIds { get; set; }
    }
}
