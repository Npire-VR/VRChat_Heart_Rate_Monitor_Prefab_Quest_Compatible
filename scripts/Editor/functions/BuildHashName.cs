#if UNITY_EDITOR

using System.Security.Cryptography;
using System.Text;

public static class BuildHashName
{
    public static string FromPayload(string payload, string prefix = "")
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(payload));
            StringBuilder builder = new StringBuilder(16);

            for (int i = 0; i < 8; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            
            if (prefix == "") return builder.ToString();
            return prefix + "_" + builder;
            
        }
    }
}

#endif
