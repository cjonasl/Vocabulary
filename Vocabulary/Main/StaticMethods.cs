using System;
using System.Collections;
using System.IO;
using System.Text;

namespace StaticMethods
{
    public static class Utility
    {
        public static void CreateNewFile(string fileNameFullPath, string fileContent)
        {
            FileStream fileStream = new FileStream(fileNameFullPath, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            streamWriter.Write(fileContent);
            streamWriter.Flush();
            fileStream.Flush();
            streamWriter.Close();
            fileStream.Close();
        }

        public static string ReturnFileContents(string fileNameFullPath)
        {
            FileStream fileStream = new FileStream(fileNameFullPath, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            string str = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();

            return str;
        }

        public static void Print(string fileNameFullPath, string[] words)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < (words.Length - 1); i++)
            {
                sb.Append(string.Format("{0}{1}", words[i], "\r\n----- New word -----\r\n"));
            }

            sb.Append(words[words.Length - 1]);

            CreateNewFile(fileNameFullPath, sb.ToString());
        }

        public static string[] ReturnWordsAndExplanationArray(string fileNameFullPath)
        {
            return ReturnFileContents(fileNameFullPath).Split(new string[] { "\r\n----- New word -----\r\n" }, StringSplitOptions.None);
        }

        public static ArrayList ReturnWordsArray(string[] wordsAndExplanationArray)
        {
            ArrayList wordsArray = new ArrayList();
            int idx1, idx2, idx3, i;

            idx1 = wordsAndExplanationArray[0].IndexOf("?????");
            i = 0;

            while ((idx1 == -1) && (i < 1000))
            {
                idx2 = wordsAndExplanationArray[i].IndexOf(". ");

                if ((idx2 < 1) || (idx2 > 3))
                {
                    throw new Exception("((idx2 < 1) || (idx2 > 3))");
                }

                idx3 = wordsAndExplanationArray[i].IndexOf("\r\n");

                wordsArray.Add(wordsAndExplanationArray[i].Substring(2 + idx2, idx3 - idx2 - 2).Trim());

                i++;
                idx1 = wordsAndExplanationArray[i].IndexOf("?????");
            }

            return wordsArray;
        }

        public static void ReadConfig(string dir, out int choosenFile, out int currentIndex)
        {
            string[] v = ReturnFileContents(dir + "\\Config.txt").Split(new string[] { "\r\n" }, StringSplitOptions.None);
            choosenFile = int.Parse(v[0]);  //If v[0]=1 then it is file Vocabulary1.txt, if v[0]=2 then it is file Vocabulary2.txt etc.
            currentIndex = int.Parse(v[1]);
        }
    }
}

