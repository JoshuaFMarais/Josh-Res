using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.IO;

public static class DataManaginigMethods
{   
    public static string createFilePathname(string FileName)
    {
        return @"C:\Users\user\Desktop\" + FileName + ".txt";
    }
    public static void SaveSomeData(object data, string fileName, string hashkey)
    {      
        string path = createFilePathname(fileName);
         string jsonString = JsonSerializer.Serialize(data);

         string finalSave = Encript(jsonString, hashkey);       
         using (StreamWriter streamWriter = File.CreateText(path))
         {
             streamWriter.Write(finalSave);
         }
    }

    //load Data
    public static object loadData<type>(string path, string hashkey)
    {
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            string finalLoad = Decript(jsonString, hashkey);

            return JsonSerializer.Deserialize<type>(finalLoad);
        }
    }
    //For the most part you should only need to use the save and load methods instead of direectly useing this
    public static string Encript(string input, string hashkey)
    {
        byte[] encripteddata = UTF8Encoding.UTF8.GetBytes(input);

        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashkey));

            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider()
            {
                Key
                = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                ICryptoTransform i = trip.CreateEncryptor();
                byte[] result = i.TransformFinalBlock(encripteddata, 0, encripteddata.Length);

                return Convert.ToBase64String(result, 0, result.Length);
            }

        }

    }

    //For the most part you should only need to use the SAVE and LOAD methods instead of direectly useing this
    public static string Decript(string input, string hashkey)
    {
        byte[] encripteddata = Convert.FromBase64String(input);

        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashkey));

            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider()
            {
                Key
                = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                ICryptoTransform i = trip.CreateDecryptor();
                byte[] result = i.TransformFinalBlock(encripteddata, 0, encripteddata.Length);

                return UTF8Encoding.UTF8.GetString(result);
            }
        }
    }
    //use this to set up a new class of data
    /*the structure is
     classInstance= (class type)SetUpAndLoadDataFile<class type>(
              class.fileName,
              class.fileHash,
              classInstance);
    */

    //you would call this function in another class before useing the file
    public static object SetUpAndLoadDataFile<DataType>(string fileName, string filehash, object dataInput)
    {
        string Path = DataManaginigMethods.createFilePathname(fileName);

        if (File.Exists(Path))
        {
            DataType Data = (DataType)
                loadData<DataType>(Path, filehash);

            return Data;
        }
        else
        {
            DataManaginigMethods.SaveSomeData(dataInput, fileName, filehash);

            DataType Data = (DataType)
                 loadData<DataType>(Path, filehash);

            return Data;
        }
    }
}
