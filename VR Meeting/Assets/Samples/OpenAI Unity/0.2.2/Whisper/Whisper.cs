using UnityEngine.UI;
using UnityEngine;

namespace Samples.Whisper
{
    public sealed class TranscriptMeeting : MonoBehaviour
    {
        [SerializeField] private Button recordButton;
        [SerializeField] private Image progressBar;
        [SerializeField] private Text message;
        [SerializeField] private Dropdown dropdown;

        private readonly string fileName = "output.wav";
        private readonly int duration = 5;
        private bool isRecording;
        private float time;

        private AudioClip clip;
        private TranscriptFacade transcriptFacade = new TranscriptFacade();

        private void Start()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
            recordButton.enabled = false;
            #else
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }

            recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);

            var index = PlayerPrefs.GetInt("user-mic-device-index", 0);
            dropdown.SetValueWithoutNotify(index);
            #endif
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        private void StartRecording()
        {
            isRecording = true;
            recordButton.enabled = false;

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            var deviceName = dropdown.options[index].text;

            #if !UNITY_WEBGL
            clip = transcriptFacade.StartRecording(deviceName, duration);
            #endif
        }

        private async void EndRecording()
        {
            message.text = "Transcribing...";
            isRecording = false;

            #if !UNITY_WEBGL
            transcriptFacade.StopRecording();
            #endif

            byte[] audioData = transcriptFacade.SaveAudioClip(fileName, clip);

            Destroy(clip);
            clip = null;

            string transcription = await transcriptFacade.TranscribeAudio(audioData);
            message.text = transcription;

            progressBar.fillAmount = 0;
            recordButton.enabled = true;
        }

        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progressBar.fillAmount = time / duration;

                if (time >= duration)
                {
                    time = 0;
                    EndRecording();
                }
            }
        }
    }
}
