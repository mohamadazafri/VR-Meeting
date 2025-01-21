using UnityEngine;
using OpenAI;
using Samples.Whisper;
using System.Threading.Tasks;
using System;

public class TranscriptFacade
{
    private static TranscriptFacade instance;
    private readonly OpenAIApi openai = new OpenAIApi();
    private readonly int sampleRate = 44100;

    public TranscriptFacade() { }

    public static TranscriptFacade Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TranscriptFacade();
            }
            return instance;
        }
    }

    public AudioClip StartRecording(string deviceName, int duration)
    {
        return Microphone.Start(deviceName, false, duration, sampleRate);
    }

    public void StopRecording()
    {
        Microphone.End(null);
    }

    public byte[] SaveAudioClip(string fileName, AudioClip clip)
    {
        return SaveWav.Save(fileName, clip);
    }

    public async Task<string> TranscribeAudio(byte[] audioData, string language = "en")
    {
        var request = new CreateAudioTranscriptionsRequest
        {
            FileData = new FileData { Data = audioData, Name = "audio.wav" },
            Model = "whisper-1",
            Language = language
        };

        try
        {
            var response = await openai.CreateAudioTranscription(request);
            return response.Text;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Transcription Error: {ex.Message}");
            return "Error during transcription.";
        }
    }
}
