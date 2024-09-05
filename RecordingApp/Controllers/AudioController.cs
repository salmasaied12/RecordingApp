using Microsoft.AspNetCore.Mvc;
using RecordingApp.Models;

public class AudioController : Controller
{
    private readonly AppDbContext _context;

    public AudioController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var recordings = _context.AudioFiles.ToList();
        return View(recordings);
    }

    [HttpPost]
    public IActionResult SaveAudio(IFormFile audioFile)
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

                // Create and save audio record to the database
                var audio = new AudioModel
                {
                    FileName = uniqueFileName,
                    RecordedOn = DateTime.Now,
                    FilePath = "/audios/" + uniqueFileName
                };

                _context.AudioFiles.Add(audio);
                _context.SaveChanges();

                // Return JSON response indicating success and the updated list of recordings
                var recordings = _context.AudioFiles.ToList();
                return Json(new { success = true, message = "File saved successfully", recordings });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        return Json(new { success = false, message = "No file was uploaded." });
    }

    [HttpGet]
    public IActionResult GetRecordings()
    {
        var recordings = _context.AudioFiles.ToList();
        return Json(recordings);
    }


}