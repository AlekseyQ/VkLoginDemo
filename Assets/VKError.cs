
/// <summary>
/// Class for presenting VK SDK and VK API errors
/// </summary>
public class VKError
{
    /// <summary>
    /// Contains system HTTP error
    /// </summary>
    public string httpError;

    /// <summary>
    /// Describes API error
    /// </summary>
    public VKError apiError;

    /// <summary>
    /// Request which caused error
    /// </summary>
    public VKRequest request;

    /// <summary>
    /// May contains such errors:<br/>
    /// <b>HTTP status code</b> if HTTP error occured;<br/>
    /// <b>VK_API_ERROR</b> if API error occured;<br/>
    /// <b>VK_API_CANCELED</b> if request was canceled;<br/>
    /// <b>VK_API_REQUEST_NOT_PREPARED</b> if error occured while preparing request;
    /// </summary>
    public int errorCode;

    public string errorCodeName;

    /// <summary>
    /// API error message
    /// </summary>
    public string errorMessage;

    /// <summary>
    /// Reason for authorization fail
    /// </summary>
    public string errorReason;

    /// <summary>
    /// Captcha identifier for captcha-check
    /// </summary>
    public string captchaSid;

    /// <summary>
    /// Image for captcha-check
    /// </summary>
    public string captchaImg;

    /// <summary>
    /// Redirection address if validation check required
    /// </summary>
    public string redirectUri;
}
