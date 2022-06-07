
namespace Postify.Shared;

using System.Security.Cryptography;
using System.Text;

public static class Utils 
{

    public static string Hash(string password)
    {

        var md5 = MD5.Create();

        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

        string result = Convert.ToBase64String(hash);

        return result;

    }

    public static bool CompareHash(string password1, string password2)
    {

        return password1.Equals(password2);

    }

}
