using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void VKAccessTokenChangedHandler(VKAccessToken oldToken, VKAccessToken newToken);

public delegate void InitVkHandlerDoneHandler(VKLoginState loginState);
public delegate void InitVkHandlerFailHandler(VKError error);

public delegate void LoginVKHandlerDoneHandler(VKAccessToken token);
public delegate void LoginVKHandlerFailHandler(VKError error);

public delegate void DoUsersGetRequestErrorHandler(VKError error);
public delegate void DoUsersGetRequestCompleteHandler(VKResponse response);
public delegate void DoUsersGetRequestAttemptFailHandler(VKRequest request, int attemptNumber, int totalAttempts);

public interface VKUnityPlugin {
    // Use this for initialization
    void InitPlugin();
    void RegisterAccessTokenTracker(VKAccessTokenChangedHandler accessTokenChangedHandler);
    void InitUnity(int appId, InitVkHandlerDoneHandler initVkHandlerDoneHandler, InitVkHandlerFailHandler initVkHandlerFailHandler);
    void LoginVK(List<VKScope> scopeList, LoginVKHandlerDoneHandler loginVKHandlerDoneHandler, LoginVKHandlerFailHandler loginVKHandlerFailHandler);
    void LogoutVK();
    VKAccessToken GetVKAccessToken();
    void DoUsersGetRequest(DoUsersGetRequestCompleteHandler completeCallback, DoUsersGetRequestErrorHandler errorCallback, DoUsersGetRequestAttemptFailHandler attemptFailedCallback, params string[] args);
}
