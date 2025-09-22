using ProtoBuf;

namespace Quasar.Common.Messages
{
    [ProtoContract]
    public class DoStartMicrophoneRecording : IMessage
    {
        [ProtoMember(1)]
        public string MicrophoneId { get; set; }

        [ProtoMember(2)]
        public bool RealTimeStreaming { get; set; }

        [ProtoMember(3)]
        public int Quality { get; set; } // Sample rate in Hz (e.g., 44100, 22050)

        [ProtoMember(4)]
        public int ChunkDurationSeconds { get; set; } // Duration of each recording chunk
    }
}
