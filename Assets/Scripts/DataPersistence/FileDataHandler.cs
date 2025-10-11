using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;
    private string profileId = "Jimpu";                
    private static readonly string encryptionKey = "78e83f38S7fd6dW@"; // 16 Zeichen für AES-128
    private bool useEncryption;

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption = true)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Save file not found: " + fullPath);
            return null;
        }
        try
        {
            if (useEncryption)
            {
                byte[] fileData = File.ReadAllBytes(fullPath);
                // Die ersten 16 Bytes sind der IV
                byte[] iv = new byte[16];
                Array.Copy(fileData, 0, iv, 0, 16);
                byte[] encryptedData = new byte[fileData.Length - 16];
                Array.Copy(fileData, 16, encryptedData, 0, encryptedData.Length);

                string decryptedJson = DecryptStringFromBytes(encryptedData, encryptionKey, iv);
                return JsonUtility.FromJson<GameData>(decryptedJson);
            }
            else
            {
                string json = File.ReadAllText(fullPath);
                return JsonUtility.FromJson<GameData>(json);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data: " + fullPath + "\n" + e);
            return null;
        }
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string json = JsonUtility.ToJson(data, true);
            if (useEncryption)
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    aesAlg.GenerateIV();
                    byte[] iv = aesAlg.IV;
                    byte[] encryptedData = EncryptStringToBytes(json, aesAlg.Key, iv);

                    // Schreibe IV + verschlüsselte Daten in die Datei
                    using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(iv, 0, iv.Length);
                        fs.Write(encryptedData, 0, encryptedData.Length);
                    }
                }
            }
            else
            {
                File.WriteAllText(fullPath, json);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {fullPath + "\n" + e}");
        }
    }

    // AES Encryption mit IV
    private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (StreamWriter sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
                sw.Close();
                return ms.ToArray();
            }
        }
    }

    private static string DecryptStringFromBytes(byte[] cipherText, string keyString, byte[] iv)
    {
        byte[] key = Encoding.UTF8.GetBytes(keyString);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream ms = new MemoryStream(cipherText))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
