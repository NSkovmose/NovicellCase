namespace ImporterFunction.Helpers;

public static class EtagHelper
{
    public static string ToSha256Hash(this string input)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hash);
    }
}