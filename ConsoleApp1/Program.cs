using System;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    private const string Secret = "0123456789ABCDEF"; // clave de 128 bits en hexadecimal
    private const string OriginalKeyConstant = "Llave original: ";
    private const string EncryptedKeyConstant = "\nLlave encriptada: ";
    private static string encryptedKey;
    private static string decryptedKey;
    private static string originalKey;

    public static void Main(string[] args)
    {
        // Genera la llave y la encripta
        GenerateKey();
        Console.WriteLine(OriginalKeyConstant + originalKey);
        EncryptKey();
        Console.WriteLine(EncryptedKeyConstant + encryptedKey);

        // Desencripta la llave y la compara con la original
        DecryptKey();
        Console.WriteLine("\nDesencriptando " + encryptedKey + ", para ver si la llave desencriptada es: " + originalKey);
        bool keysMatch = string.Equals(originalKey, decryptedKey);
        Console.WriteLine(keysMatch ? "¡Las llaves coinciden!" : "Las llaves no coinciden :(");

        // Espera para que el usuario pueda ver la salida antes de salir
        Console.ReadLine();
    }

    public static void GenerateKey()
    {
        originalKey = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
    }

    public static void EncryptKey()
    {
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Key = Encoding.UTF8.GetBytes(Secret);
            aes.IV = Encoding.UTF8.GetBytes(Secret.Substring(0, 16)); // vector de inicialización
            aes.Mode = CipherMode.CBC;
            var data = Encoding.UTF8.GetBytes(originalKey);
            encryptedKey = Convert.ToBase64String(aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
        }
    }

    public static void DecryptKey()
    {
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Key = Encoding.UTF8.GetBytes(Secret);
            aes.IV = Encoding.UTF8.GetBytes(Secret.Substring(0, 16)); // vector de inicialización
            aes.Mode = CipherMode.CBC;
            var data = Convert.FromBase64String(encryptedKey);
            decryptedKey = Encoding.UTF8.GetString(aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
        }
    }
}