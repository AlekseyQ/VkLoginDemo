using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class VKUnityEmptyPlugin : VKUnityPlugin
{
    public VKAccessToken GetVKAccessToken()
    {
        return null;
    }

    public void InitPlugin()
    {
    }

    public void InitUnity(int appId, InitVkHandlerDoneHandler initVkHandlerDoneHandler, InitVkHandlerFailHandler initVkHandlerFailHandler)
    {
        initVkHandlerFailHandler(new VKError());
    }

    public void LoginVK(List<VKScope> scopeList, LoginVKHandlerDoneHandler loginVKHandlerDoneHandler, LoginVKHandlerFailHandler loginVKHandlerFailHandler)
    {
        loginVKHandlerFailHandler(new VKError());
    }

    public void LogoutVK()
    {
    }

    public void RegisterAccessTokenTracker(VKAccessTokenChangedHandler accessTokenChangedHandler)
    {
    }
}
