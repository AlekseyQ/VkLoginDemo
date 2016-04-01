using System.Collections.Generic;

/// <summary>
/// Presents VK API access token that used for loading API methods and other stuff.
/// </summary>
[System.Serializable]
public class VKAccessToken
{
    /// <summary>
    /// String token for use in request parameters
    /// </summary>
    public string accessToken = null;

    /// <summary>
    /// Time when token expires
    /// </summary>
    public int expiresIn = 0;

    /// <summary>
    /// Current user id for this token
    /// </summary>
    public string userId = null;

    /// <summary>
    /// User secret to sign requests (if nohttps used)
    /// </summary>
    public string secret = null;

    /// <summary>
    /// If user sets "Always use HTTPS" setting in his profile, it will be true
    /// </summary>
    public bool httpsRequired = false;

    /// <summary>
    /// Indicates time of token creation
    /// </summary>
    public long created = 0;

    /// <summary>
    /// User email
    /// </summary>
    public string email = null;

    /// <summary>
    /// Token scope
    /// </summary>
    public List<string> scope = null;

}