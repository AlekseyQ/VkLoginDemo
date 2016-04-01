
/// <summary>
/// Class for execution API-requests.
/// </summary>
public class VKRequest
{
    /// <summary>
    /// Selected method name
    /// </summary>
    public string methodName;

    /// <summary>
    /// Use HTTPS requests (by default is YES). If http-request is impossible (user denied no https access), SDK will load https version
    /// </summary>
    public bool secure;
}