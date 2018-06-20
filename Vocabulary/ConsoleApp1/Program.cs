using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private const string _template1 = "{0}. {1}\r\n{2}";
        private const string _template2 = "{0}. {1}\r\nÅ Ä Ö å ä ö\r\n\r\nFörklaring:\r\n\r\nUttalas:\r\n\r\nAdjektiv:\r\nAdverb:\r\nSubstantiv:\r\nVerb:\r\n\r\nExempel:";

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

        public static string[] ReturnRows(string str)
        {
            return str.Split(new string[] { "\r\n" } , StringSplitOptions.None );
        }

        public static string AssenblyRows(string[] rows)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < rows.Length; i++)
            {
                sb.Append(string.Format("{0}\r\n", rows[i].Trim()));
            }

            return sb.ToString();
        }

        public static void CreateNewVersionOfVocabularyFile()
        {
            string fileContent = ReturnFileContents("C:\\Vocabulary\\Vocabulary1.txt");
            string[] rows = ReturnRows(fileContent);

            for(int i = 0; i < rows.Length; i++)
            {
                if ((rows[i].Length > "Exempel: ".Length) && (rows[i].StartsWith("Exempel:")))
                {
                    rows[i] = rows[i].Replace(',', ';');
                }
            }

            string newFileContent = AssenblyRows(rows);

            CreateNewFile("C:\\tmp\\Vocabulary1.txt", newFileContent);
        }

        static void Main(string[] args)
        {
            string fileContents, w, e, str, dir = @"F:\English\";
            string[] v, words = new string[1000];
            int[] intArray;
            int i, j, index, wordNumber;
            string str1, str2, str3;

            //CreateNewVersionOfVocabularyFile();
            //return;

            ArrayList word, explanation, numberWord, wordExplanation;

            word = new ArrayList();
            explanation = new ArrayList();
            numberWord = new ArrayList();
            wordExplanation = new ArrayList();

            for (i = 1; i <= 14; i++)
            {
                fileContents = ReturnFileContents(string.Format("{0}Vocabulary{1}.txt", dir, i.ToString())).Trim();
                v = fileContents.Split(new string[] { "\r\n" }, StringSplitOptions.None );

                for(j = 0; j < v.Length; j++)
                {
                    index = v[j].IndexOf(':');
                    w = v[j].Substring(0, index).Trim();
                    e = v[j].Substring(1 + index).Trim();
                    word.Add(w);
                    explanation.Add(e);
                }
            }

            intArray = new int[]{1, 2, 4, 5, 7, 9, 10, 11, 12, 13, 14 };

            for(i = 0; i < intArray.Length; i++)
            {
                fileContents = ReturnFileContents(string.Format("{0}AVocabulary{1}.txt", dir, intArray[i].ToString())).Trim();
                v = fileContents.Split(new string[] { "----- New word -----\r\n" }, StringSplitOptions.None);

                for (j = 0; j < v.Length; j++)
                {
                    str = v[j].Trim();
                    if (Char.IsDigit(str[0]))
                    {
                        index = str.IndexOf(". ");

                        if ((index < 1) || (index > 3))
                        {
                            throw new Exception("((index < 1) || (index > 3))");
                        }

                        numberWord.Add(int.Parse(str.Substring(0, index)));
                        wordExplanation.Add(str);
                    }
                }
            }

            for(i = 1; i <= word.Count; i++)
            {
                words[i - 1] = string.Format(_template1, i.ToString(), (string)word[i - 1], (string)explanation[i - 1]);
            }

            for (i = (1 + word.Count); i <= 1000; i++)
            {
                words[i - 1] = string.Format(_template2, i.ToString(), "?????");
            }

            for (i = 0; i < numberWord.Count; i++)
            {
                wordNumber = (int)numberWord[i];
                words[wordNumber - 1] = (string)wordExplanation[i];
            }

            StaticMethods.Utility.Print("F:\\Vocabulary\\Vocabulary1.txt", words);
        }
    }
}
