using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Android;
using System.Threading.Tasks;


public class VivoxVoiceManager : MonoBehaviour
{

    int m_PermissionAskedCount;

    public void LoginToVivoxService()
    {
        if (IsMicPermissionGranted())
        {
            LoginToVivox();
        }
        else
        {
            if (IsPermissionsDenied())
            {
                m_PermissionAskedCount = 0;
                LoginToVivox();
            }
            else
            {
                AskForPermissions();
            }
        }
    }

    bool IsMicPermissionGranted()
    {
        bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);
        #if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
                if (IsAndroid12AndUp())
                {
                    // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission for all features to work
                    isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
                }
        #endif
        return isGranted;
    }

    async void LoginToVivox()
    {
        try
        {
            string displayName = "John Doe " + Random.Range(2f, 10f).ToString();
            var loginOptions = new LoginOptions()
            {
                DisplayName = displayName,
                ParticipantUpdateFrequency = ParticipantPropertyUpdateFrequency.FivePerSecond
            };
            await Task.Delay(500); // Wait for a short period
            await VivoxService.Instance.LeaveAllChannelsAsync();
            await Task.Delay(500); // Wait for a short period
            await VivoxService.Instance.JoinGroupChannelAsync(VivoxVoiceConfig.LobbyChannelName, ChatCapability.TextAndAudio);
            await Task.Delay(500); // Wait for a short period
            await VivoxService.Instance.SpeechToTextEnableTranscription(VivoxVoiceConfig.LobbyChannelName);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            throw;
        }

    }

    bool IsPermissionsDenied()
    {
        #if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
                // On Android 12 and up, we also need to ask for the BLUETOOTH_CONNECT permission
                if (IsAndroid12AndUp())
                {
                    return m_PermissionAskedCount == 2;
                }
        #endif
        return m_PermissionAskedCount == 1;
    }

    void AskForPermissions()
    {
        string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (m_PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
        m_PermissionAskedCount++;
        Permission.RequestUserPermission(permissionCode);
    }

    Task JoinLobbyChannel()
    {

        return VivoxService.Instance.JoinGroupChannelAsync(VivoxVoiceConfig.LobbyChannelName, ChatCapability.TextAndAudio);
    }

    #if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        bool IsAndroid12AndUp()
        {
            // android12VersionCode is hardcoded because it might not be available in all versions of Android SDK
            const int android12VersionCode = 31;
            AndroidJavaClass buildVersionClass = new AndroidJavaClass("android.os.Build$VERSION");
            int buildSdkVersion = buildVersionClass.GetStatic<int>("SDK_INT");

            return buildSdkVersion >= android12VersionCode;
        }

        string GetBluetoothConnectPermissionCode()
        {
            if (IsAndroid12AndUp())
            {
                // UnityEngine.Android.Permission does not contain the BLUETOOTH_CONNECT permission, fetch it from Android
                AndroidJavaClass manifestPermissionClass = new AndroidJavaClass("android.Manifest$permission");
                string permissionCode = manifestPermissionClass.GetStatic<string>("BLUETOOTH_CONNECT");

                return permissionCode;
            }

            return "";
        }
    #endif

}
