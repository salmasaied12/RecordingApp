using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            // Generate a unique file name using a GUID
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

            // Create and save audio record to database
            var audio = new AudioModel
            {
                FileName = uniqueFileName,
                RecordedOn = DateTime.Now,
                FilePath = "/audios/" + uniqueFileName
            };

           

            _context.AudioFiles.Add(audio);
            _context.SaveChanges();

        }

        return BadRequest("file saved.");
    }

}


