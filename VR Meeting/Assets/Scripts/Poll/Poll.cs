using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Poll : NetworkBehaviour
{

    [Header("View Elements")]
    [SerializeField] private GameObject setupView;
    [SerializeField] private GameObject answeringView;

    [Header("Host UI Elements")]
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_InputField[] optionInputs;
    [SerializeField] private Button startPollButton;
    [SerializeField] private Button endPollButton;

    [Header("Client UI Elements")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TMP_Text[] votesTexts;
    [SerializeField] private TMP_Text noPollText;

    [SerializeField] private NetworkVariable<FixedString64Bytes> currentQuestion = new NetworkVariable<FixedString64Bytes>();
    [SerializeField] private NetworkList<FixedString32Bytes> currentOptions = new NetworkList<FixedString32Bytes>();
    [SerializeField] private NetworkList<int> voteCounts = new NetworkList<int>();
    [SerializeField] private NetworkVariable<bool> isPollActive = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {

        // Client-side vote buttons
            for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Capture index for the button
            optionButtons[i].onClick.AddListener(() => SubmitVote(index));
        }

        // Subscribe to NetworkVariable changes
        currentQuestion.OnValueChanged += OnQuestionChanged;
        currentOptions.OnListChanged += OnOptionsChanged;
        voteCounts.OnListChanged += OnVoteCountsChanged;
        isPollActive.OnValueChanged += OnPollActiveChanged;

        UpdateUI(false);
    }

    public void StartPoll()
    {
        if (string.IsNullOrEmpty(questionInput.text)) return;

        currentQuestion.Value = questionInput.text;

        currentOptions.Clear();
        foreach (var input in optionInputs)
        {
            if (!string.IsNullOrEmpty(input.text))
            {
                currentOptions.Add(input.text);
            }
        }

        if (currentOptions.Count < 2) return;

        voteCounts.Clear();
        for (int i = 0; i < currentOptions.Count; i++)
        {
            voteCounts.Add(0);
        }

        isPollActive.Value = true;
    }

    public void EndPoll()
    {
        isPollActive.Value = false;
    }

    private void SubmitVote(int index)
    {
        if (!isPollActive.Value || index < 0 || index >= voteCounts.Count) return;

        SubmitVoteServerRpc(index);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitVoteServerRpc(int index)
    {
        voteCounts[index]++;
    }

    private void OnQuestionChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        questionText.text = newValue.ToString();
    }

    private void OnOptionsChanged(NetworkListEvent<FixedString32Bytes> changeEvent)
    {
        for (int i = 0; i < currentOptions.Count; i++)
        {
            if (i < optionButtons.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                TMP_Text buttonText = optionButtons[i].GetComponentInChildren<TMP_Text>();
                buttonText.text = currentOptions[i].ToString();
                votesTexts[i].gameObject.SetActive(true);
                votesTexts[i].text = "Votes: 0";
            }
        }

        for (int i = currentOptions.Count; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
            votesTexts[i].gameObject.SetActive(false);
        }
    }

    private void OnVoteCountsChanged(NetworkListEvent<int> changeEvent)
    {
        for (int i = 0; i < voteCounts.Count; i++)
        {
            if (i < votesTexts.Length)
            {
                votesTexts[i].text = $"Votes: {voteCounts[i]}";
            }
        }
    }

    private void OnPollActiveChanged(bool oldValue, bool newValue)
    {
        UpdateUI(newValue);
    }

    private void UpdateUI(bool pollActive)
    {
        // Host UI
        if (IsHost)
        {
            switch (pollActive)
            {
                case true:
                    //questionText.gameObject.SetActive(true);
                    //endPollButton.gameObject.SetActive(true);
                    //foreach (var button in optionButtons)
                    //{
                    //    button.gameObject.SetActive(true);
                    //}
                    //foreach (var text in votesTexts)
                    //{
                    //    text.gameObject.SetActive(true);
                    //}
                    answeringView.gameObject.SetActive(true);
                    setupView.gameObject.SetActive(false);
                    startPollButton.gameObject.SetActive(false);
                    endPollButton.gameObject.SetActive(true);
                    break;
                default:
                    //questionInput.gameObject.SetActive(true);
                    //foreach (var input in optionInputs)
                    //{
                    //    input.gameObject.SetActive(true);
                    //}
                    //startPollButton.gameObject.SetActive(true);
                    //endPollButton.gameObject.SetActive(false);
                    noPollText.gameObject.SetActive(false);
                    answeringView.gameObject.SetActive(false);
                    setupView.gameObject.SetActive(true);
                    startPollButton.gameObject.SetActive(true);
                    endPollButton.gameObject.SetActive(false);
                    break;
            }
            return;
        } else
        {
            // Client UI
            switch (pollActive)
            {
                case true:
                    //questionText.gameObject.SetActive(true);
                    //noPollText.gameObject.SetActive(false);
                    //foreach (var button in optionButtons)
                    //{
                    //    button.gameObject.SetActive(true);
                    //}
                    //foreach (var text in votesTexts)
                    //{
                    //    text.gameObject.SetActive(true);
                    //}
                    noPollText.gameObject.SetActive(false);
                    answeringView.gameObject.SetActive(true);
                    setupView.gameObject.SetActive(false);
                    endPollButton.gameObject.SetActive(false);
                    break;
                default:
                    noPollText.gameObject.SetActive(true);
                    answeringView.gameObject.SetActive(false);
                    setupView.gameObject.SetActive(false);
                    break;
            }


        }
        

    }

    void OnDestroy()
    {
        //currentQuestion.Dispose();
        //currentOptions.Dispose();
        //voteCounts.Dispose();
        //isPollActive.Dispose();

        currentQuestion.OnValueChanged -= OnQuestionChanged;
        currentOptions.OnListChanged -= OnOptionsChanged;
        voteCounts.OnListChanged -= OnVoteCountsChanged;
        isPollActive.OnValueChanged -= OnPollActiveChanged;

        base.OnDestroy();
    }

    
}
