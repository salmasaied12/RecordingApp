namespace RecordingApp.Models
{
    public class AudioModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime RecordedOn { get; set; }
        public string FilePath { get; set; }
        public string Transcription { get; set; } // whisper
    }
}
