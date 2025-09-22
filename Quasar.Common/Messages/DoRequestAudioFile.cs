using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoRequestAudioFile : IMessage
    {
        [ProtoMember(1)]
        public string FileName { get; set; }
    }
}
