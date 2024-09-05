using Microsoft.AspNetCore.Mvc;
using RecordingApp.Migrations;
using RecordingApp.Models;
using RecordingApp.Services;

public class AudioController : Controller
{
    private readonly AppDbContext _context;
    private readonly TranscriptionService _transcriptionService;
    private readonly SpeechRecognitionService _speechRecognitionService;
    private readonly RecordingApp.Services.Azure _speechRecognition;
    public AudioController(AppDbContext context, RecordingApp.Services.Azure speechRecognition, TranscriptionService transcriptionService, SpeechRecognitionService speechRecognitionService)
    {
        _context = context;
        _transcriptionService = transcriptionService;
        _speechRecognitionService = speechRecognitionService;
        _speechRecognition = speechRecognition;
    }

    public IActionResult Index()
    {
        var recordings = _context.AudioFiles.ToList();
        return View(recordings);
    }


    [HttpPost]
    public async Task<IActionResult> SaveAudio(IFormFile audioFile)
    {
        if (audioFile != null && audioFile.Length > 0)
        {
            try
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audios", uniqueFileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audios")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audios"));
                }

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    audioFile.CopyTo(stream);
                }

                var transcription = await _speechRecognition.RecognizeSpeechFromFileAsync(filePath);

                // Create and save audio record to the database
                var audio = new AudioModel
                {
                    FileName = uniqueFileName,
                    RecordedOn = DateTime.Now,
                    FilePath = "/audios/" + uniqueFileName,
                    TranscriptionText = transcription,
                    Transcription = transcription
                };

                _context.AudioFiles.Add(audio);
                _context.SaveChanges();

                // Return JSON response including the transcription
                return Json(new { success = true, message = "File saved successfully", transcription });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        return Json(new { success = false, message = "No file was uploaded." });
    }

    //[HttpPost]
    //public async Task<IActionResult> SaveAudio(IFormFile audioFile)
    //{
    //    if (audioFile != null && audioFile.Length > 0)
    //    {
    //        try
    //        {
    //            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName);
    //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audios", uniqueFileName);

    //            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audios")))
    //            {
    //                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/audios"));
    //            }

    //            // Save the file
    //            using (var stream = new FileStream(filePath, FileMode.Create))
    //            {
    //                audioFile.CopyTo(stream);
    //            }

    //            //var transcriptionText = await _transcriptionService.TranscribeAndSaveAsync(filePath);
    //            //var transcription = await _speechRecognitionService.RecognizeSpeechFromFileAsync(filePath);
    //            var transcription = await _speechRecognition.RecognizeSpeechFromFileAsync(filePath);
    //            // Create and save audio record to the database
    //            var audio = new AudioModel
    //            {
    //                FileName = uniqueFileName,
    //                RecordedOn = DateTime.Now,
    //                FilePath = "/audios/" + uniqueFileName,
    //                TranscriptionText = transcription,
    //                Transcription = transcription

    //            };

    //            _context.AudioFiles.Add(audio);
    //            _context.SaveChanges();

    //            // Return JSON response indicating success and the updated list of recordings
    //            var recordings = _context.AudioFiles.ToList();
    //            return Json(new { success = true, message = "File saved successfully", recordings });
    //        }
    //        catch (Exception ex)
    //        {
    //            // Handle exceptions and return an error response
    //            return Json(new { success = false, message = "An error occurred: " + ex.Message });
    //        }
    //    }

    //    return Json(new { success = false, message = "No file was uploaded." });
    //}
    public async Task<IActionResult> TranscribeAudio(int audioFileId)
    {
        // Retrieve the file path from the database (implement this method to suit your database)
        //var filePath = GetAudioFilePathFromDatabase(audioFileId);
        var filePath = "C:/Users/Reem/Downloads/voice1.mp3";
        if (string.IsNullOrEmpty(filePath))
        {
            return NotFound("Audio file not found");
        }
        // Transcribe the audio and save the transcription text to the database
        await _transcriptionService.TranscribeAndSaveAsync(filePath);
        return Ok("Transcription completed and saved to the database");
    }
    private string GetAudioFilePathFromDatabase(int audioFileId)
    {
        // Implement this method to fetch the file path from your database
        return "/path/to/audio/file.mp3";
    }

    [HttpGet]
    public IActionResult GetRecordings()
    {
        var recordings = _context.AudioFiles.ToList();
    
        return Json(recordings);
    }


}