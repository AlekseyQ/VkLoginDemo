using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class VKUnity : MonoBehaviour
{
    [HideInInspector]
    public int appId;

#if UNITY_EDITOR
    [HideInInspector]
    public string __;
#endif

    [HideInInspector]
    public bool[] scope = new bool[VKScope.GetNames(typeof(VKScope)).Length];

    private VKUnityPlugin vkUnity;

    // Use this for initialization
    void Awake()
    {
#if UNITY_EDITOR
        vkUnity = new VKUnityEmptyPlugin();
#elif UNITY_ANDROID
        vkUnity = VKUnityPluginAndroid.MakePlugin(gameObject);
#elif UNITY_IOS
        vkUnity = new VKUnityEmptyPlugin();
#else
        vkUnity = new VKUnityEmptyPlugin();
#endif

        vkUnity.InitPlugin();
    }

    public VKUnityPlugin GetPlugin()
    {
        return vkUnity;
    }

    public void RegisterAccessTokenTracker(VKAccessTokenChangedHandler accessTokenChangedHandler)
    {
        vkUnity.RegisterAccessTokenTracker(accessTokenChangedHandler);
    }

    public void InitUnity(InitVkHandlerDoneHandler initVkHandlerDoneHandler, InitVkHandlerFailHandler initVkHandlerFailHandler)
    {
        vkUnity.InitUnity(appId, initVkHandlerDoneHandler, initVkHandlerFailHandler);
    }

    public void LoginVK(LoginVKHandlerDoneHandler loginVKHandlerDoneHandler, LoginVKHandlerFailHandler loginVKHandlerFailHandler)
    {
        vkUnity.LoginVK(MakeScopeList(), loginVKHandlerDoneHandler, loginVKHandlerFailHandler);
    }

    public void LogoutVK()
    {
        vkUnity.LogoutVK();
    }

    public VKAccessToken GetVKAccessToken()
    {
        return vkUnity.GetVKAccessToken();
    }

    public void ClearScope()
    {
        foreach (VKScope s in System.Enum.GetValues(typeof(VKScope)))
        {
            SetScope(s, false);
        }
    }

    public bool CheckScope(VKScope s)
    {
        return scope[(int)s];
    }

    public void SetScope(VKScope s, bool b)
    {
        scope[(int)s] = b;
    }

    public List<VKScope> MakeScopeList()
    {
        List<VKScope> res = new List<VKScope>();
        foreach(VKScope s in System.Enum.GetValues(typeof(VKScope)))
        {
            if (CheckScope(s)) res.Add(s);
        }
        return res;
    }

    public void DoUsersGetRequest(DoUsersGetRequestCompleteHandler completeCallback, DoUsersGetRequestErrorHandler errorCallback, DoUsersGetRequestAttemptFailHandler attemptFailedCallback, params string[] args)
    {
        vkUnity.DoUsersGetRequest(completeCallback, errorCallback, attemptFailedCallback, args);
    }


}
