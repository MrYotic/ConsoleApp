using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class User
{
    #region Login
    public static string accesAlphabet = "qwertyuiopasdfghjklzxcvbnm1234567890_";
    public static CheckStringState VerifyUserName(string username)
    {
        if (username.Length < 3)
            return CheckStringState.OverShort;
        else if (username.Length > 16)
            return CheckStringState.OverLong;
        username = username.ToLower();
        if (username.Any(z => !accesAlphabet.Contains(z)))
            return CheckStringState.HasInvalidChars;
        return CheckStringState.Successful;
    }
    public static CheckStringState VerifyPassword(string password)
    {
        if (password.Length < 3)
            return CheckStringState.OverShort;
        else if (password.Length > 256)
            return CheckStringState.OverLong;
        password = password.ToLower();
        if (password.Any(z => !accesAlphabet.Contains(z)))
            return CheckStringState.HasInvalidChars;
        return CheckStringState.Successful;
    }
    public enum CheckStringState
    {
        Successful,
        OverShort,
        OverLong,
        HasInvalidChars,
    }
    #endregion
}