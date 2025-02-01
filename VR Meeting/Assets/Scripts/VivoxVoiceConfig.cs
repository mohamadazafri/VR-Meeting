
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;
using System.Threading.Tasks;
#if AUTH_PACKAGE_PRESENT
using Unity.Services.Authentication;
#endif

public class VivoxVoiceConfig : MonoBehaviour
{
    public const string LobbyChannelName = "vrMeetChannel";

    static object m_Lock = new object();
    static VivoxVoiceConfig m_Instance;

    [SerializeField]
    string _key;
    [SerializeField]
    string _issuer;
    [SerializeField]
    string _domain;
    [SerializeField]
    string _server;

    public static VivoxVoiceConfig Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (VivoxVoiceConfig)FindObjectOfType(typeof(VivoxVoiceConfig));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<VivoxVoiceConfig>();
                        singletonObject.name = typeof(VivoxVoiceConfig).ToString() + " (Singleton)";
                    }
                }
                // Make instance persistent even if its already in the scene
                DontDestroyOnLoad(m_Instance.gameObject);
                return m_Instance;
            }
        }
    }

    async void Awake()
    {
        if (m_Instance != this && m_Instance != null)
        {
            Debug.LogWarning(
                "Multiple VivoxVoiceConfig detected in the scene. Only one VivoxVoiceConfig can exist at a time. The duplicate VivoxVoiceConfig will be destroyed.");
            Destroy(this);
        }
        var options = new InitializationOptions();
        if (CheckManualCredentials())
        {
            options.SetVivoxCredentials(_server, _domain, _issuer, _key);
        }

        await UnityServices.InitializeAsync(options);
        await VivoxService.Instance.InitializeAsync();

    }

    public async Task InitializeAsync(string playerName)
    {
        #if AUTH_PACKAGE_PRESENT
                if (!CheckManualCredentials())
                {
                    AuthenticationService.Instance.SwitchProfile(playerName);
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
        #endif
    }

    bool CheckManualCredentials()
    {
        return !(string.IsNullOrEmpty(_issuer) && string.IsNullOrEmpty(_domain) && string.IsNullOrEmpty(_server));
    }
}
