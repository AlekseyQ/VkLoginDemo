using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

class LoginVKHandlerScript : MonoBehaviour
{
    private static string RESULT_MAKE_ERROR = "result_failure";

    public LoginVKHandlerDoneHandler loginVKHandlerDoneHandler;
    public LoginVKHandlerFailHandler loginVKHandlerFailHandler;

    public void OnInitVkHandlerDone(string res)
    {
        try
        {
            if (string.IsNullOrEmpty(res))
            {
                Debug.LogError("__OnInitVkHandlerDone: Null result");
                return;
            }
            else if (res.Equals(RESULT_MAKE_ERROR))
            {
                Debug.LogError("__OnInitVkHandlerDone: Internal error");
                return;
            }

            VKAccessToken myObject = JsonMapper.ToObject<VKAccessToken>(res);
            loginVKHandlerDoneHandler(myObject);
        }
        finally
        {
            Destroy(gameObject);
        }
    }

    public void OnInitVkHandlerFail(string res)
    {
        try
        {
            if (string.IsNullOrEmpty(res))
            {
                Debug.LogError("__OnInitVkHandlerFail: Null result");
                return;
            }
            else if (res.Equals(RESULT_MAKE_ERROR))
            {
                Debug.LogError("__OnInitVkHandlerFail: Internal error");
                return;
            }

            VKError myObject = JsonMapper.ToObject<VKError>(res);
            loginVKHandlerFailHandler(myObject);
        }
        finally
        {
            Destroy(gameObject);
        }
    }
}

class InitVkHandlerScript : MonoBehaviour
{
    private static string RESULT_MAKE_ERROR = "result_failure";

    public InitVkHandlerDoneHandler initVkHandlerDoneHandler;
    public InitVkHandlerFailHandler initVkHandlerFailHandler;

    [System.Serializable]
    class VKLoginStateMessage
    {
        public string loginState;
    }

    public void OnInitVkHandlerDone(string res)
    {
        try
        {
            if (string.IsNullOrEmpty(res))
            {
                Debug.LogError("__OnInitVkHandlerDone: Token is null");
                return;
            }
            else if (res.Equals(RESULT_MAKE_ERROR))
            {
                Debug.LogError("__OnInitVkHandlerDone: Internal error");
                return;
            }

            VKLoginStateMessage myObject = JsonMapper.ToObject<VKLoginStateMessage>(res);
            VKLoginStateType stateType = VKLoginStateType.Unknown;
            switch (myObject.loginState)
            {
                case "Unknown": stateType = VKLoginStateType.Unknown; break;
                case "LoggedOut": stateType = VKLoginStateType.LoggedOut; break;
                case "Pending": stateType = VKLoginStateType.Pending; break;
                case "LoggedIn": stateType = VKLoginStateType.LoggedIn; break;
                default: stateType = VKLoginStateType.Unknown; break;
            }

            VKLoginState loginState = new VKLoginState();
            loginState.loginState = stateType;

            initVkHandlerDoneHandler(loginState);
        }
        finally
        {
            Destroy(gameObject);
        }
    }

    public void OnInitVkHandlerFail(string res)
    {
        try
        {
            if (string.IsNullOrEmpty(res))
            {
                Debug.LogError("__OnInitVkHandlerFail: Null result");
                return;
            }
            else if (res.Equals(RESULT_MAKE_ERROR))
            {
                Debug.LogError("__OnInitVkHandlerFail: Internal error");
                return;
            }


            VKError myObject = JsonMapper.ToObject<VKError>(res);
            initVkHandlerFailHandler(myObject);
        }
        finally
        {
            Destroy(gameObject);
        }
    }
}

class RegisterAccessTokenTrackerHandlerScript : MonoBehaviour
{
    private static string RESULT_MAKE_ERROR = "result_failure";
    public VKAccessTokenChangedHandler accessTokenChangedHandler;

    /// <summary>
    /// Use this class for track access token changes.
    /// </summary>
    [System.Serializable]
    class VKAccessTokenPair
    {
        /// <summary>
        /// Token before changes
        /// </summary>
        public VKAccessToken oldToken = null;

        /// <summary>
        /// Actual token after changes
        /// </summary>
        public VKAccessToken newToken = null;
    }

    public void OnVKAccessTokenChanged(string str)
    {
        try
        {
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogError("__OnVKAccessTokenChanged: Null result");
                return;
            }
            else if (str.Equals(RESULT_MAKE_ERROR))
            {
                Debug.LogError("__OnVKAccessTokenChanged: Internal error");
                return;
            }

            try
            {
                VKAccessTokenPair myObject = JsonMapper.ToObject<VKAccessTokenPair>(str);
                accessTokenChangedHandler(myObject.oldToken, myObject.newToken);
            }
            catch (JsonException e)
            {
                Debug.LogError("__OnVKAccessTokenChanged: " + e.Message);
            }
        }
        finally
        {
            //Destroy(gameObject);
        }
    }
}

public class VKUnityPluginAndroid : MonoBehaviour, VKUnityPlugin
{
    private static string NOTIFY_STR = "notify";
    private static string FRIENDS_STR = "friends";
    private static string PHOTOS_STR = "photos";
    private static string AUDIO_STR = "audio";
    private static string VIDEO_STR = "video";
    private static string DOCS_STR = "docs";
    private static string NOTES_STR = "notes";
    private static string PAGES_STR = "pages";
    private static string STATUS_STR = "status";
    private static string WALL_STR = "wall";
    private static string GROUPS_STR = "groups";
    private static string MESSAGES_STR = "messages";
    private static string NOTIFICATIONS_STR = "notifications";
    private static string STATS_STR = "stats";
    private static string ADS_STR = "ads";
    private static string OFFLINE_STR = "offline";
    private static string EMAIL_STR = "email";
    private static string NOHTTPS_STR = "nohttps";
    private static string DIRECT_STR = "direct";

    private static string RESULT_MAKE_ERROR = "result_failure";

    private static int seedValue = 0;
    private VKUnityPluginAndroidNative vkUnity;

    public static VKUnityPluginAndroid MakePlugin(GameObject parentObject)
    {
        GameObject obj = new GameObject("VKUnityAndroid");
        obj.transform.parent = parentObject.transform;
        return obj.AddComponent<VKUnityPluginAndroid>();
    }

    void Awake()
    {
        vkUnity = new VKUnityPluginAndroidNative();
    }

    private static int GetNextSeed()
    {
        return seedValue++;
    }

    // Use this for initialization
    public void InitPlugin()
    {
        vkUnity.InitPlugin();
    }
    
    public void RegisterAccessTokenTracker(VKAccessTokenChangedHandler accessTokenChangedHandler)
    {
        GameObject obj = new GameObject("RegisterAccessTokenTracker" + GetNextSeed());
        obj.transform.parent = this.transform;
        RegisterAccessTokenTrackerHandlerScript script = obj.AddComponent<RegisterAccessTokenTrackerHandlerScript>();
        script.accessTokenChangedHandler = accessTokenChangedHandler;

        vkUnity.RegisterAccessTokenTracker(script.OnVKAccessTokenChanged);
    }

    private List<string> MakeScopeList(List<VKScope> scopeList)
    {
        List<string> res = new List<string>();
        foreach(VKScope scope in scopeList)
            switch(scope)
            {
                case VKScope.FRIENDS: res.Add(FRIENDS_STR); break;
                case VKScope.PHOTOS: res.Add(PHOTOS_STR); break;
                case VKScope.AUDIO: res.Add(AUDIO_STR); break;
                case VKScope.VIDEO: res.Add(VIDEO_STR); break;
                case VKScope.DOCS: res.Add(DOCS_STR); break;
                case VKScope.NOTES: res.Add(NOTES_STR); break;
                case VKScope.PAGES: res.Add(PAGES_STR); break;
                case VKScope.STATUS: res.Add(STATUS_STR); break;
                case VKScope.WALL: res.Add(WALL_STR); break;
                case VKScope.GROUPS: res.Add(GROUPS_STR); break;
                case VKScope.MESSAGES:  res.Add(MESSAGES_STR); break;
                case VKScope.NOTIFICATIONS: res.Add(NOTIFICATIONS_STR); break;
                case VKScope.STATS: res.Add(STATS_STR); break;
                case VKScope.ADS: res.Add(ADS_STR); break;
                case VKScope.OFFLINE: res.Add(OFFLINE_STR); break;
                case VKScope.EMAIL: res.Add(EMAIL_STR); break;
                case VKScope.NOHTTPS: res.Add(NOHTTPS_STR); break;
                case VKScope.DIRECT: res.Add(DIRECT_STR); break;
            }
        return res;
    }

    public void InitUnity(int appId, InitVkHandlerDoneHandler initVkHandlerDoneHandler, InitVkHandlerFailHandler initVkHandlerFailHandler)
    {
        GameObject obj = new GameObject("InitVkHandler" + GetNextSeed());
        obj.transform.parent = this.transform;
        InitVkHandlerScript script = obj.AddComponent<InitVkHandlerScript>();
        script.initVkHandlerDoneHandler = initVkHandlerDoneHandler;
        script.initVkHandlerFailHandler = initVkHandlerFailHandler;

        vkUnity.InitVk(appId, script.OnInitVkHandlerDone, script.OnInitVkHandlerFail);
    }

    public void LoginVK(List<VKScope> scopeList, LoginVKHandlerDoneHandler loginVKHandlerDoneHandler, LoginVKHandlerFailHandler loginVKHandlerFailHandler)
    {
        vkUnity.ClearPermissions();
        foreach (string scope in MakeScopeList(scopeList))
        {
            vkUnity.AddPermission(scope);
        }

        GameObject obj = new GameObject("LoginVKHandler" + GetNextSeed());
        obj.transform.parent = this.transform;
        LoginVKHandlerScript script = obj.AddComponent<LoginVKHandlerScript>();
        script.loginVKHandlerDoneHandler = loginVKHandlerDoneHandler;
        script.loginVKHandlerFailHandler = loginVKHandlerFailHandler;

        vkUnity.LoginVk(script.OnInitVkHandlerDone, script.OnInitVkHandlerFail);
    }

    public void LogoutVK()
    {
        vkUnity.LogoutVK();
    }

    public VKAccessToken GetVKAccessToken()
    {
        string str = vkUnity.GetVKAccessToken();
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        else if (str.Equals(RESULT_MAKE_ERROR))
        {
            Debug.LogError("GetVKAccessToken: Internal error");
            return null;
        }

        VKAccessToken myObject = JsonMapper.ToObject<VKAccessToken>(str);
        return myObject;
    }

    public string GetPackageName()
    {
        return vkUnity.GetPackageName();
    }

    public string GetCertificateFingerprint()
    {
        return vkUnity.GetCertificateFingerprint();
    }

}
