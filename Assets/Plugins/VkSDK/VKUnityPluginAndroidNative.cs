using UnityEngine;
using System.Collections;

public class VKUnityPluginAndroidNative
{
    public delegate void VKCallback(string res);

    public delegate void LoginVKCallbackDone(string res);
    public delegate void LoginVKCallbackFail(string res);

    public delegate void InitVKCallbackDone(string res);
    public delegate void InitVKCallbackFail(string res);

    public delegate void VKAccessTokenTrackerHandler(string res);

    private AndroidJavaClass javaUnityPlayer;
    private AndroidJavaObject currentActivity;
    private AndroidJavaObject androidPlugin;

    public void InitPlugin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            androidPlugin = new AndroidJavaObject("com.vk.sdk.unity.VKUnity", currentActivity);
        }
    }

    public void RegisterAccessTokenTracker(VKAccessTokenTrackerHandler handler)
    {
        if (!(handler.Target is MonoBehaviour))
        {
            throw new System.Exception();
        }

        MonoBehaviour obj = (MonoBehaviour)handler.Target;
        GameObject gameObject = obj.gameObject;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                androidPlugin.Call("registerAccessTokenTracker", gameObject.name, handler.Method.Name);
            }
        }
    }

    public string GetVKAccessToken()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                return androidPlugin.Call<string>("getVKAccessToken");
            }
        }
        return null;
    }

    public void InitVk(int vkAppId, InitVKCallbackDone doneCallback, InitVKCallbackFail failCallback)
    {
        if (!(doneCallback.Target is MonoBehaviour))
        {
            throw new System.Exception();
        }

        if (!(failCallback.Target is MonoBehaviour))
        {
            throw new System.Exception();
        }

        MonoBehaviour doneObj = (MonoBehaviour)doneCallback.Target;
        GameObject doneGameObject = doneObj.gameObject;

        MonoBehaviour failObj = (MonoBehaviour)failCallback.Target;
        GameObject failGameObject = failObj.gameObject;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                androidPlugin.Call("init", currentActivity, vkAppId, doneGameObject.name, doneCallback.Method.Name, failGameObject.name, failCallback.Method.Name);
            }
        }
    }

    public void ClearPermissions()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                androidPlugin.Call("clearPermissions");
            }
        }
    }

    public void AddPermission(string permission)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                androidPlugin.Call("addPermission", permission);
            }
        }
    }

    public bool LoginVk(LoginVKCallbackDone doneCallback, LoginVKCallbackDone failCallback)
    {
        if (!(doneCallback.Target is MonoBehaviour))
        {
            throw new System.Exception();
        }

        if (!(failCallback.Target is MonoBehaviour))
        {
            throw new System.Exception();
        }

        MonoBehaviour doneObj = (MonoBehaviour)doneCallback.Target;
        GameObject doneGameObject = doneObj.gameObject;

        MonoBehaviour failObj = (MonoBehaviour)failCallback.Target;
        GameObject failGameObject = failObj.gameObject;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                return androidPlugin.Call<bool>("login", currentActivity, doneGameObject.name, doneCallback.Method.Name, failGameObject.name, failCallback.Method.Name);
            }
        }
        return false;
    }

    public bool LogoutVK()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (androidPlugin != null)
            {
                return androidPlugin.Call<bool>("logout", currentActivity);
            }
        }
        return false;
    }

    public string GetCertificateFingerprint()
    {
        if (androidPlugin != null)
        {
            using (AndroidJavaObject obj = androidPlugin.Call<AndroidJavaObject>("getCertificateFingerprint", currentActivity))
            {
                if (obj.GetRawObject().ToInt32() != 0)
                {
                    // String[] returned with some data!
                    string strTotal = "";
                    string[] result = AndroidJNIHelper.ConvertFromJNIArray<string[]>(obj.GetRawObject());
                    foreach (string str in result)
                    {
                        Debug.Log(str);
                        strTotal += str;
                        strTotal += '\n';
                    }
                    return strTotal;
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }

    public string GetPackageName()
    {
        if (androidPlugin != null)
        {
            string str = androidPlugin.Call<string>("getPackageName", currentActivity);
            return str;
        }
        return "androidPlugin == null";
    }

    public void DoUsersGetRequest(VKCallback completeCallback, VKCallback errorCallback, VKCallback attemptFailedCallback, params string[] args)
    {
        if (androidPlugin != null)
        {
            if (!(completeCallback.Target is MonoBehaviour))
            {
                throw new System.Exception();
            }

            if (!(errorCallback.Target is MonoBehaviour))
            {
                throw new System.Exception();
            }

            if (!(attemptFailedCallback.Target is MonoBehaviour))
            {
                throw new System.Exception();
            }

            MonoBehaviour completeObj = (MonoBehaviour)completeCallback.Target;
            GameObject completeGameObject = completeObj.gameObject;

            MonoBehaviour errorObj = (MonoBehaviour)errorCallback.Target;
            GameObject errorGameObject = errorObj.gameObject;

            MonoBehaviour attemptFailedObj = (MonoBehaviour)attemptFailedCallback.Target;
            GameObject attemptFailedGameObject = attemptFailedObj.gameObject;

            object[] arrgs = new object[] { currentActivity,
                completeGameObject.name, completeCallback.Method.Name,
                errorGameObject.name, errorCallback.Method.Name,
                attemptFailedGameObject.name, attemptFailedCallback.Method.Name,
                args };
            androidPlugin.Call("doUsersGetRequest", arrgs);
        }
    }

}
