
public enum VKLoginStateType
{
    Unknown,
    LoggedOut,
    Pending,
    LoggedIn
}

public class VKLoginState
{
    public VKLoginStateType loginState = VKLoginStateType.Unknown;
}
