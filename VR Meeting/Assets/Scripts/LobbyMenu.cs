using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    // UI Pages
    public GameObject loginPage;
    public GameObject mainMenu;
    public GameObject createPage;
    public GameObject joinPage;
    public GameObject viewMeetingPage;
    public GameObject scheduleMeetingPage;

    // Buttons in Login Page
    //public Button loginSubmitButton;

    // Buttons in Register Page
    //public Button registerSubmitButton;

    // Buttons in Main Menu
    //public Button createMeetingButton;
    //public Button joinMeetingButton;
    //public Button viewMeetingButton;
    //public Button scheduleMeetingButton;

    //// Buttons in Create Page
    //public Button createMeetingSubmitButton;

    //// Buttons in Join Page
    //public Button joinMeetingSubmitButton;

    //// Buttons in Schedule Page
    //public Button scheduleMeetingSubmitButton;

    // Other Components
    //public NetworkConnect networkConnect;
    public CloudSave cloudSave;
    public MeetingsDisplay meetingDisplay;

    public List<Button> returnButtons; // Use a list of return buttons


    // Start is called before the first frame update
    void Start()
    {
        EnableLoginPage();

        //loginSubmitButton.onClick.AddListener(LoginSubmit);
        //registerSubmitButton.onClick.AddListener(RegisterSubmit);

        //scheduleMeetingButton.onClick.AddListener(EnableScheduleMeetingPage);
        //createMeetingButton.onClick.AddListener(EnableCreatePage);
        //joinMeetingButton.onClick.AddListener(EnableJoinPage);
        //viewMeetingButton.onClick.AddListener(EnableViewMeetingPage);

        //createMeetingSubmitButton.onClick.AddListener(CreateMeetingSubmit);

        //joinMeetingSubmitButton.onClick.AddListener(JoinMeetingSubmit);

        //scheduleMeetingSubmitButton.onClick.AddListener(ScheduleMeetingSubmit);

        foreach (var returnButton in returnButtons)
        {
            returnButton.onClick.AddListener(EnablePreviousPage);
        }
    }

    void EnablePreviousPage()
{
    if (mainMenu.activeSelf)
        EnableLoginPage();
    else if (joinPage.activeSelf)
        EnableMainMenu();
    else if (viewMeetingPage.activeSelf)
        EnableMainMenu();
    else if (scheduleMeetingPage.activeSelf)
        EnableMainMenu();
    else if (createPage.activeSelf)
        EnableMainMenu();
}
 


   public void EnableLoginPage()
    {
        loginPage.SetActive(true);
        mainMenu.SetActive(false);
        createPage.SetActive(false);
        joinPage.SetActive(false);
        viewMeetingPage.SetActive(false);
        scheduleMeetingPage.SetActive(false);
    }

    public void EnableMainMenu()
    {
        loginPage.SetActive(false);
        mainMenu.SetActive(true);
        createPage.SetActive(false);
        joinPage.SetActive(false);
        viewMeetingPage.SetActive(false);
        scheduleMeetingPage.SetActive(false);
    }

    public void EnableScheduleMeetingPage()
    {
        mainMenu.SetActive(false);
        createPage.SetActive(false);
        joinPage.SetActive(false);
        viewMeetingPage.SetActive(false);
        scheduleMeetingPage.SetActive(true);
    }
    public void EnableCreatePage()
    {
        mainMenu.SetActive(false);
        createPage.SetActive(true);
        joinPage.SetActive(false);
        viewMeetingPage.SetActive(false);
        scheduleMeetingPage.SetActive(false);
        //createMeetingSubmitButton.interactable = true;
    }

    public void EnableJoinPage()
    {
        mainMenu.SetActive(false);
        createPage.SetActive(false);
        joinPage.SetActive(true);
        viewMeetingPage.SetActive(false);
        scheduleMeetingPage.SetActive(false);
    }

    public async void EnableViewMeetingPage()
    {
        await meetingDisplay.InitializeMeetingsDisplayAsync(); // Use await here

        mainMenu.SetActive(false);
        createPage.SetActive(false);
        joinPage.SetActive(false);
        viewMeetingPage.SetActive(true);
        scheduleMeetingPage.SetActive(false);
    }

    public void ScheduleMeetingSubmit()
    {
        cloudSave.SaveData("", "");
    }
}
