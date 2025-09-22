using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class MicrophoneAudioData : IMessage
    {
        [ProtoMember(1)]
        public byte[] AudioData { get; set; }

        [ProtoMember(2)]
        public string FileName { get; set; }

        [ProtoMember(3)]
        public bool IsRealTime { get; set; }

        [ProtoMember(4)]
        public long FileSize { get; set; }

        [ProtoMember(5)]
        public bool IsComplete { get; set; }

        [ProtoMember(6)]
        public int ChunkIndex { get; set; }
    }
}
