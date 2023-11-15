using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

public class JSONDataService : IDataService
{
    
    //You can generate your own key using CHATGPT. Simply ask it to generate you an example of an AES256 Key and IV represented as strings

    private const string KEY = "p9yAe48+ul4muZtGww6V/1DeE0J/wTt2p3iBrWM2Ssk=";
    private const string IV = "nmT4KnFsYKJe3hq/XFwncw==";

    List<string> errors = new List<string>();
    public bool SaveData<T>(string RelativePath, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data Exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing file for the first time!");
            }
            using FileStream stream = File.Create(path);
            if(Encrypted)
            {
                WriteEncryptedData(Data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(Data, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
                    ));
            }
            


            return true;

        }
        catch (Exception e)
        {
            Debug.Log($"Unable To Save Data Due To: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>(string RelativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;

        if(!File.Exists(path))
        {
            Debug.LogError($"Cannot Load File at {path}. File does not exist!");
            throw new FileNotFoundException($"{path} does not exist!");
        }

        try
        {
            T data;
            if(Encrypted)
            {
                data = ReadEncryptedData<T>(path);
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    private void WriteEncryptedData<T>(T Data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            stream,
            cryptoTransform,
            CryptoStreamMode.Write);

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data)));
     }

    private T ReadEncryptedData<T>(string Path)
    {
        byte[] fileBytes = File.ReadAllBytes(Path);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key,
            aesProvider.IV);

        using MemoryStream decryptionStream = new MemoryStream(fileBytes);
        using CryptoStream cryptoStream = new CryptoStream(
            decryptionStream,
            cryptoTransform,
            CryptoStreamMode.Read);

        using StreamReader reader = new StreamReader(cryptoStream);

        string result = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<T>(result);
    }
}
