using System.IO;
using UnityEngine;

public class textFromFile : MonoBehaviour
{
    public string GetTextFromFile(string file_path)
    {
        string fileText = "";

        StreamReader inputStream = new StreamReader(file_path);

        while (!inputStream.EndOfStream)
        {
            fileText += inputStream.ReadLine();
        }

        inputStream.Close();
        return fileText;
    }
}
