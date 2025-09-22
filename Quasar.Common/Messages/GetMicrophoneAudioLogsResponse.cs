using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class GetMicrophoneAudioLogsResponse : IMessage
    {
        [ProtoMember(1)]
        public string[] AudioFileNames { get; set; }

        [ProtoMember(2)]
        public long[] AudioFileSizes { get; set; }

        [ProtoMember(3)]
        public string[] AudioFileDates { get; set; }
    }
}
