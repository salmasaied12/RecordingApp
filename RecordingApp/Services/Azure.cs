//using Microsoft.CognitiveServices.Speech;
//using Microsoft.CognitiveServices.Speech.Audio;
//using System.Text;
//using System.Threading.Tasks;

//namespace RecordingApp.Services
//{
//    public class SpeechRecognitionService
//    {
//        private readonly SpeechConfig _config;

//        public SpeechRecognitionService(string subscriptionKey, string region)
//        {
//            _config = SpeechConfig.FromSubscription(subscriptionKey, region);
//        }

//        public async Task<string> RecognizeSpeechFromFileAsync(string audioFilePath)
//        {
//            try
//            {
//                //string test = @"C:\Users\owner\source\repos\RecordingApp\RecordingApp\wwwroot\audios\daec46d4-e05c-4f2a-b4bd-f6c6519faf1b.wav";
//                //var audioConfig = AudioConfig.FromWavFileInput(test);
//                var audioConfig = AudioConfig.FromWavFileInput(audioFilePath);
//                var recognizer = new SpeechRecognizer(_config, audioConfig);


//                var finalText = new StringBuilder();

//                recognizer.Recognizing += (s, e) => { /* Handle interim results if needed */ };

//                recognizer.Recognized += (s, e) =>
//                {
//                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
//                    {
//                        finalText.Append(e.Result.Text + " ");
//                    }
//                };

//                recognizer.Canceled += (s, e) =>
//                {
//                    var cancellation = CancellationDetails.FromResult(e.Result);
//                    Console.WriteLine($"Recognition canceled: {cancellation.Reason}");
//                };

//                await recognizer.StartContinuousRecognitionAsync();
//                //await Task.Delay(5000); // Adjust as necessary

//                await Task.Delay(TimeSpan.FromSeconds(30));
//                await recognizer.StopContinuousRecognitionAsync();

//                return finalText.ToString().Trim();
//            }
//            catch(Exception ex)  {
//                Console.WriteLine($"Exception: {ex.Message}");
//                return "An error occurred during transcription.";
//            }


//        }
//    }
//}





using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Text;
using System.Threading.Tasks;

namespace RecordingApp.Services
{
    public class Azure
    {
        private readonly SpeechConfig _config;

        public Azure(string subscriptionKey, string region)
        {
            _config = SpeechConfig.FromSubscription(subscriptionKey, region);
        }

        public async Task<string> RecognizeSpeechFromFileAsync(string audioFilePath)
        {
           
                //string test = @"C:\Users\owner\source\repos\RecordingApp\RecordingApp\wwwroot\audios\daec46d4-e05c-4f2a-b4bd-f6c6519faf1b.wav";
                //var audioConfig = AudioConfig.FromWavFileInput(test);
                var audioConfig = AudioConfig.FromWavFileInput(audioFilePath);
                var recognizer = new SpeechRecognizer(_config, audioConfig);


                var finalText = new StringBuilder();

                recognizer.Recognizing += (s, e) => { /* Handle interim results if needed */ };

                recognizer.Recognized += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        finalText.Append(e.Result.Text + " ");
                    }
                };

                recognizer.Canceled += (s, e) =>
                {
                    var cancellation = CancellationDetails.FromResult(e.Result);
                    Console.WriteLine($"Recognition canceled: {cancellation.Reason}");
                };

                await recognizer.StartContinuousRecognitionAsync();
                //await Task.Delay(5000); // Adjust as necessary

                await Task.Delay(TimeSpan.FromSeconds(30));
                await recognizer.StopContinuousRecognitionAsync();

                return finalText.ToString().Trim();
            



        }
    }
}
