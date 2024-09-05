using System.IO;
using System.Threading.Tasks;
using AssemblyAI;
using AssemblyAI.Transcripts;
using RecordingApp.Models;

namespace RecordingApp.Services
{
    public class TranscriptionService
    {
        private readonly string _apiKey;
        //private readonly AppDbContext _context;


        public TranscriptionService(string apiKey)
        {
            _apiKey = apiKey;
            //_context = context;
            // , AppDbContext context
        }

        public async Task<string> TranscribeAndSaveAsync(string filePath)
        {
            var client = new AssemblyAIClient(_apiKey);

            // Upload the file to AssemblyAI
            var uploadedFile = await client.Files.UploadAsync(new FileInfo(filePath));

            // Create the transcription parameters
            var transcriptParams = new TranscriptParams
            {
                AudioUrl = uploadedFile.UploadUrl
            };

            // Transcribe the audio
            var transcript = await client.Transcripts.TranscribeAsync(transcriptParams);

            // Ensure the transcription is completed
            transcript.EnsureStatusCompleted();

            return transcript.Text;

        }
    }
}
