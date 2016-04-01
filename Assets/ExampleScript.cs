using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

public class ExampleScript : MonoBehaviour {

    public VKUnity vkUnity;

    public Text infoText;

    public Toggle NOTIFY;
    public Toggle FRIENDS;
    public Toggle PHOTOS;
    public Toggle AUDIO;
    public Toggle VIDEO;
    public Toggle DOCS;
    public Toggle NOTES;
    public Toggle PAGES;
    public Toggle STATUS;
    public Toggle WALL;
    public Toggle GROUPS;
    public Toggle MESSAGES;
    public Toggle NOTIFICATIONS;
    public Toggle STATS;
    public Toggle ADS;
    public Toggle OFFLINE;
    public Toggle EMAIL;
    public Toggle NOHTTPS;
    public Toggle DIRECT;

    // Use this for initialization
    void Start () {
        NOTIFY.isOn = vkUnity.CheckScope(VKScope.NOTIFY);
        FRIENDS.isOn = vkUnity.CheckScope(VKScope.FRIENDS);
        PHOTOS.isOn = vkUnity.CheckScope(VKScope.PHOTOS);
        AUDIO.isOn = vkUnity.CheckScope(VKScope.AUDIO);
        VIDEO.isOn = vkUnity.CheckScope(VKScope.VIDEO);
        DOCS.isOn = vkUnity.CheckScope(VKScope.DOCS);
        NOTES.isOn = vkUnity.CheckScope(VKScope.NOTES);
        PAGES.isOn = vkUnity.CheckScope(VKScope.PAGES);
        STATUS.isOn = vkUnity.CheckScope(VKScope.STATUS);
        WALL.isOn = vkUnity.CheckScope(VKScope.WALL);
        GROUPS.isOn = vkUnity.CheckScope(VKScope.GROUPS);
        MESSAGES.isOn = vkUnity.CheckScope(VKScope.MESSAGES);
        NOTIFICATIONS.isOn = vkUnity.CheckScope(VKScope.NOTIFICATIONS);
        STATS.isOn = vkUnity.CheckScope(VKScope.STATS);
        ADS.isOn = vkUnity.CheckScope(VKScope.ADS);
        OFFLINE.isOn = vkUnity.CheckScope(VKScope.OFFLINE);
        EMAIL.isOn = vkUnity.CheckScope(VKScope.EMAIL);
        NOHTTPS.isOn = vkUnity.CheckScope(VKScope.NOHTTPS);
        DIRECT.isOn = vkUnity.CheckScope(VKScope.DIRECT);

        vkUnity.RegisterAccessTokenTracker((VKAccessToken oldToken, VKAccessToken newToken) => {
            infoText.text += "\nOnVKAccessTokenChanged >> old > " + (oldToken != null ? LitJson.JsonMapper.ToJson(oldToken) : "null") + " new > " + (newToken != null ? LitJson.JsonMapper.ToJson(newToken) : "null");
        });
    }

    public void PrintToken()
    {
        VKAccessToken token = vkUnity.GetVKAccessToken();
        if (token == null)
        {
            infoText.text += "\nPrintToken >> Token is null";
            return;
        }

        infoText.text += "\nPrintToken >> " + JsonMapper.ToJson(token);
    }

    public void InitUnity()
    {
        vkUnity.InitUnity((VKLoginState loginState) => {
            infoText.text += "\nInitUnity >> " + loginState.loginState.ToString();
        }, (VKError error) => {
            infoText.text += "\nInitUnity >> error! > " + (error != null ? LitJson.JsonMapper.ToJson(error) : "null");
        });
    }

    public void LoginVK()
    {
        vkUnity.SetScope(VKScope.NOTIFY, NOTIFY.isOn);
        vkUnity.SetScope(VKScope.FRIENDS, FRIENDS.isOn);
        vkUnity.SetScope(VKScope.PHOTOS, PHOTOS.isOn);
        vkUnity.SetScope(VKScope.AUDIO, AUDIO.isOn);
        vkUnity.SetScope(VKScope.VIDEO, VIDEO.isOn);
        vkUnity.SetScope(VKScope.DOCS, DOCS.isOn);
        vkUnity.SetScope(VKScope.NOTES, NOTES.isOn);
        vkUnity.SetScope(VKScope.PAGES, PAGES.isOn);
        vkUnity.SetScope(VKScope.STATUS, STATUS.isOn);
        vkUnity.SetScope(VKScope.WALL, WALL.isOn);
        vkUnity.SetScope(VKScope.GROUPS, GROUPS.isOn);
        vkUnity.SetScope(VKScope.MESSAGES, MESSAGES.isOn);
        vkUnity.SetScope(VKScope.NOTIFICATIONS, NOTIFICATIONS.isOn);
        vkUnity.SetScope(VKScope.STATS, STATS.isOn);
        vkUnity.SetScope(VKScope.ADS, ADS.isOn);
        vkUnity.SetScope(VKScope.OFFLINE, OFFLINE.isOn);
        vkUnity.SetScope(VKScope.EMAIL, EMAIL.isOn);
        vkUnity.SetScope(VKScope.NOHTTPS, NOHTTPS.isOn);
        vkUnity.SetScope(VKScope.DIRECT, DIRECT.isOn);

        vkUnity.LoginVK((VKAccessToken token) => {
            infoText.text += "\nLoginVK >> " + (token != null ? LitJson.JsonMapper.ToJson(token) : "null");
        }, (VKError error) => { 
            infoText.text += "\nLoginVK >> error! > " + (error != null ? LitJson.JsonMapper.ToJson(error) : "null");
        });
    }

    public void LogoutVK()
    {
        vkUnity.LogoutVK();
        infoText.text += "\nLogoutVK";
    }

}
