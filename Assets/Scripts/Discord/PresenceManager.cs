using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace DiscordPresence
{

    public class PresenceManager : MonoBehaviour
    {
        public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
        [SerializeField] string applicationId;
        [SerializeField] string optionalSteamId;
        [SerializeField] int callbackCalls;
        //public int clickCounter;
        [SerializeField] UnityEvent onConnect;
        private UnityEvent onDisconnect;
        private UnityEvent hasResponded;

        [SerializeField] EventSO _eventMainMenu;
        [SerializeField] EventSO _eventStartLevel;

        DiscordRpc.EventHandlers handlers;

        public static PresenceManager instance;

        public void ChangeInformation()
        {
            if (1 == SceneManager.GetActiveScene().buildIndex)
            {
                instance.presence.details = "In Main Menu";
                instance.presence.state = "Looking for the Next Highscore";
                instance.presence.largeImageKey = "default";
                instance.presence.largeImageText = "ByPass - STUDIO GANG";
                instance.presence.smallImageKey = "default";
                instance.presence.smallImageText = "MainMenu";
            }

            else
            {
                if (DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex))
                {
                    SceneSO data = DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex);

                    instance.presence.details = "In Game - MAP : " + data.MapName;
                    instance.presence.largeImageKey = "default";
                    instance.presence.largeImageText = "ByPass - STUDIO GANG";
                    instance.presence.smallImageKey = "default";
                    instance.presence.smallImageText = data.MapName;
                    if (data.BestTime == 0)
                        instance.presence.state = "In TryHard mode to finish the LEVEL !!";
                    else
                        instance.presence.state = "BestTime : " + TimerFormat.FormatTime(data.BestTime);
                }
            }
           
            DiscordRpc.UpdatePresence(instance.presence);
            
        }


        #region Discord Callbacks
        public void ReadyCallback()
        {
            ++callbackCalls;
            Debug.Log("Discord: ready");
            onConnect.Invoke();
            UpdatePresence(null);
        }

        public void DisconnectedCallback(int errorCode, string message)
        {
            ++callbackCalls;
            Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
            onDisconnect.Invoke();
        }

        public void ErrorCallback(int errorCode, string message)
        {
            ++callbackCalls;
            Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
        }


        #endregion

        #region Monobehaviour Callbacks
        // Singleton
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            DiscordRpc.RunCallbacks();
        }

        void OnEnable()
        {
            callbackCalls = 0;

            handlers = new DiscordRpc.EventHandlers();
            handlers.readyCallback = ReadyCallback;
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;
            DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
        }

        void OnDisable()
        {
            DiscordRpc.Shutdown();
        }
        #endregion

        #region Update Presence Method
        public static void UpdatePresence(string detail, string state = null, long start = -1, long end = -1, string largeKey = null,string largeText = null, 
            string smallKey = null, string smallText = null, string partyId = null, int size = -1, int max = -1, string match = null, string join = null, 
            string spectate = null/*, bool instance*/)
        {
            instance.presence.details = detail ?? instance.presence.details;
            instance.presence.state = state ?? instance.presence.state;
            instance.presence.startTimestamp = (start == -1) ? instance.presence.startTimestamp : start;
            instance.presence.endTimestamp = (end == -1) ? instance.presence.endTimestamp : end;
            instance.presence.largeImageKey = largeKey ?? instance.presence.largeImageKey;
            instance.presence.largeImageText = largeText ?? instance.presence.largeImageText;
            instance.presence.smallImageKey = smallKey ?? instance.presence.smallImageKey;
            instance.presence.smallImageText = smallText ?? instance.presence.smallImageText;
            instance.presence.partyId = partyId ?? instance.presence.partyId;
            instance.presence.partySize = (size == -1) ? instance.presence.partySize : size;
            instance.presence.partyMax = (max == -1) ? instance.presence.partyMax : max;
            instance.presence.matchSecret = match ?? instance.presence.matchSecret;
            instance.presence.joinSecret = join ?? instance.presence.joinSecret;
            instance.presence.spectateSecret = spectate ?? instance.presence.spectateSecret;
            //instance.presence.presence.instance =
            DiscordRpc.UpdatePresence(instance.presence);
        }

        public static void ClearPresence()
        {
            instance.presence.details = "";
            instance.presence.state = "";
            instance.presence.startTimestamp = 0;
            instance.presence.endTimestamp = 0;
            instance.presence.largeImageKey = "";
            instance.presence.largeImageText = "";
            instance.presence.smallImageText = "";
            instance.presence.smallImageKey = "";
            instance.presence.partyId = "";
            instance.presence.partySize = 0;
            instance.presence.partyMax = 0;
            instance.presence.matchSecret = "";
            instance.presence.joinSecret = "";
            instance.presence.spectateSecret = "";
            //instance.presence.instance =
        }

        public static void ClearAndUpdate()
        {
            ClearPresence();
            DiscordRpc.UpdatePresence(instance.presence);
        }
        #endregion
    }
}
