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

        public static bool IsMatch(string row, string pattern)
        {
            if (row.IndexOf(pattern) >= 0)
                return true;
            else
                return false;
        }

        public static bool isNumberOfOccurenciesOfCharInRowFulfilled(string row, char c, int expectedNumberOfOccurencies)
        {
            int i, n, numberOfOccurencies = 0;
            bool returnValue = true;

            n = row.Length;
            i = 0;

            while ((i < n) && returnValue)
            {
                if (row[i] == c)
                {
                    numberOfOccurencies++;
                }

                if (numberOfOccurencies > expectedNumberOfOccurencies)
                {
                    returnValue = false;
                }

                i++;
            }

            if (numberOfOccurencies < expectedNumberOfOccurencies)
            {
                returnValue = false;
            }

            return returnValue;
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

        public static string ReturnString(ArrayList v)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < v.Count; i++)
            {
                sb.Append(string.Format("{0}\r\n", (string)v[i]));
            }

            return sb.ToString().TrimEnd();
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

                if ((idx2 < 1) || (idx2 > 4))
                {
                    throw new Exception("((idx2 < 1) || (idx2 > 4))");
                }

                idx3 = wordsAndExplanationArray[i].IndexOf("\r\n");

                wordsArray.Add(wordsAndExplanationArray[i].Substring(2 + idx2, idx3 - idx2 - 2).Trim());

                i++;
                idx1 = wordsAndExplanationArray[i].IndexOf("?????");
            }

            return wordsArray;
        }

        public static void ReadConfig(string dir, out string targetVocabularyFileFullPath, out string fileNameInfoFileFullPath, out int currentIndex)
        {
            string[] v = ReturnFileContents(dir + "\\Config.txt").Split(new string[] { "\r\n" }, StringSplitOptions.None);
            targetVocabularyFileFullPath = v[0];
            fileNameInfoFileFullPath = v[1];
            currentIndex = int.Parse(v[2]);
        }

        public static bool HasEmptyWord(string[] v)
        {
            bool hasEmptyWord = false;
            int i, n;

            n = v.Length;
            i = 0;

            while ((i < n) && (!hasEmptyWord))
            {
                if (string.IsNullOrEmpty(v[i]))
                {
                    hasEmptyWord = true;
                }
                else
                {
                    i++;
                }
            }
        
            return hasEmptyWord;
        }

        public static bool HasOneParticularChar(string str, char c, out int indexFirstOccurence)
        {
            int i, n, numberOfOccurencies = 0;
            bool returnValue;

            indexFirstOccurence = -1;
            n = str.Length;

            for(i = 0; i < n; i++)
            {
                if (str[i] == c)
                {
                    numberOfOccurencies++;

                    if (numberOfOccurencies == 1)
                    {
                        indexFirstOccurence = i;
                    }
                }
            }


            returnValue = numberOfOccurencies == 1 ? true : false;

            return returnValue;
        }

        public static string[] ReturnEnglishWords(string[] v)
        {
            string[] returnArray;
            int i = 1, numberOfWords = 0;
            bool commaFound = false;

            while ((i <= v.Length) && (!commaFound))
            {
                if (v[i].EndsWith(","))
                {
                    commaFound = true;
                }

                i++;
                numberOfWords++;
            }

            returnArray = new string[numberOfWords];

            for(i = 1; i <= numberOfWords; i++)
            {
                if (i == numberOfWords)
                {
                    returnArray[i - 1] = v[i].Substring(0, v[i].Length - 1);
                }
                else
                {
                    returnArray[i - 1] = v[i];
                }
                
            }

            return returnArray;
        }

        public static string ReturnExempel(string[] v, int indexWord, bool masked)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < v.Length; i++)
            {
                if ((i == indexWord) && (masked))
                {
                    sb.Append("[XXXXX] ");
                }
                else if ((i == indexWord) && (!masked))
                {
                    sb.Append(string.Format("[{0}] ", v[i]));
                }
                else
                {
                    sb.Append(v[i] + " ");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static int ReturnNumber(string row)
        {
            int returnValue = -1;

            if ((string.IsNullOrEmpty(row)) || (!char.IsDigit(row[0])))
            {
                throw new Exception("Error in method ReturnNumber!");
            }

            int dotIndex = row.IndexOf('.');

            if (dotIndex == -1)
            {
                throw new Exception("Error in method ReturnNumber! Can't find a dot!");
            }

            string str = row.Substring(0, dotIndex);

            if (!int.TryParse(str, out returnValue))
            {
                throw new Exception("Error in method ReturnNumber! Can't find a valid number!");
            }

            return returnValue;
        }

        public static bool ExempelsAreOk(string text, out string errorMessage, out ArrayList exempelunmasked, out ArrayList exempelMasked, out ArrayList exempelTranslated)
        {
            string[] v, englishWords, rows = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string str;
            bool oneComma, oneLeftparentheses, oneRightparentheses, returnValue;
            int i, indexComma, indexLeftparentheses, indexRightparentheses, indexWord, numberOfErrors = 0, currentNumber = 0;
            bool exempelExists = true;
            StringBuilder sb = new StringBuilder("");

            exempelunmasked = new ArrayList();
            exempelMasked = new ArrayList();
            exempelTranslated = new ArrayList();

            i = 1;

            while (exempelExists)
            {
                if (char.IsDigit(rows[i - 1][0]))
                {
                    currentNumber = ReturnNumber(rows[i - 1]);
                }

                if (rows[i - 1].ToLower().IndexOf("exempel") >= 0)
                {
                    if ((rows[i - 1].Length < 10) || (rows[i - 1].Substring(0, 9) != "Exempel: "))  //Exempel: StartWord
                    {
                        sb.Append(string.Format("Row {0}, Exempel error: ((rows[i - 1].Length < 10) || (rows[i - 1].Substring(0, 9) != \"Exempel: \"))\r\n", i.ToString()));
                        numberOfErrors++;
                    }
                    else if (rows[i - 1].ToLower().Substring(1).IndexOf("exempel") >= 0)
                    {
                        sb.Append(string.Format("Row {0}, Exempel error: (rows[i - 1].ToLower().Substring(1).IndexOf(\"Exempel\") >= 0)\r\n", i.ToString()));
                        numberOfErrors++;
                    }
                    else
                    {
                        v = rows[i - 1].Split(' ');
                        if (HasEmptyWord(v))
                        {
                            sb.Append(string.Format("Row {0}, Exempel error: Empty word exists\r\n", i.ToString()));
                            numberOfErrors++;
                        }
                        else
                        {
                            oneComma = HasOneParticularChar(rows[i - 1], ',', out indexComma);
                            oneLeftparentheses = HasOneParticularChar(rows[i - 1], '(', out indexLeftparentheses);
                            oneRightparentheses = HasOneParticularChar(rows[i - 1], ')', out indexRightparentheses);

                            if (!oneComma)
                            {
                                sb.Append(string.Format("Row {0}, Exempel error: Not exactly one comma\r\n", i.ToString()));
                                numberOfErrors++;
                            }
                            else if (!oneLeftparentheses)
                            {
                                sb.Append(string.Format("Row {0}, Exempel error: Not exactly one left parentheses\r\n", i.ToString()));
                                numberOfErrors++;
                            }
                            else if (!oneRightparentheses)
                            {
                                sb.Append(string.Format("Row {0}, Exempel error: Not exactly one right parentheses\r\n", i.ToString()));
                                numberOfErrors++;
                            }
                            else
                            {
                                bool b = ((indexComma < indexLeftparentheses) && (indexComma < indexRightparentheses) && (indexLeftparentheses < indexRightparentheses));

                                if (!b)
                                {
                                    sb.Append(string.Format("Row {0}, Exempel error: comma, left parentheses and right parentheses do not come in right order: ,()\r\n", i.ToString()));
                                    numberOfErrors++;
                                }
                                else if (rows[i - 1][1 + indexComma] != ' ')
                                {
                                    sb.Append(string.Format("Row {0}, Exempel error: Not blank after comma\r\n", i.ToString()));
                                    numberOfErrors++;
                                }
                                else if (rows[i - 1][indexLeftparentheses - 1] != ' ')
                                {
                                    sb.Append(string.Format("Row {0}, Exempel error: Not blank before left parentheses\r\n", i.ToString()));
                                    numberOfErrors++;
                                }
                                else if (rows[i - 1].Length != (indexRightparentheses + 1))
                                {
                                    sb.Append(string.Format("Row {0}, Exempel error: Not new line after right parentheses\r\n", i.ToString()));
                                    numberOfErrors++;
                                }
                                else
                                {
                                    str = rows[i - 1].Substring(2 + indexComma, indexLeftparentheses - 1 - 2 - indexComma);
                                    if (!int.TryParse(str, out indexWord))
                                    {
                                        sb.Append(string.Format("Row {0}, Exempel error: Not an integer after comma\r\n", i.ToString()));
                                        numberOfErrors++;
                                    }
                                    else
                                    {
                                        englishWords = ReturnEnglishWords(v);
                                        indexWord--;
                                        if ((indexWord < 0) || (indexWord > (englishWords.Length - 1)))
                                        {
                                            sb.Append(string.Format("Row {0}, Exempel error: ((idx < 0) || (idx > (englishWords.Length - 1)))\r\n", i.ToString()));
                                            numberOfErrors++;
                                        }
                                        else
                                        {
                                            if (numberOfErrors == 0)
                                            {
                                                str = ReturnExempel(englishWords, indexWord, false);

                                                if (str != "[Word]")
                                                {
                                                    exempelunmasked.Add(string.Format("{0}. {1}", currentNumber.ToString(), str));
                                                    exempelMasked.Add(string.Format("{0}. {1}", currentNumber.ToString(), ReturnExempel(englishWords, indexWord, true)));
                                                    exempelTranslated.Add(string.Format("{0}. {1}", currentNumber.ToString(), rows[i - 1].Substring(1 + indexLeftparentheses, indexRightparentheses - 1 - indexLeftparentheses).Trim()));
                                                }
                                                else
                                                {
                                                    exempelExists = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                i++;

                if (i > rows.Length)
                {
                    exempelExists = false;
                }
            }

            if (numberOfErrors == 0)
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }

            errorMessage = sb.ToString().TrimEnd();

            return returnValue;
        }
    }
}

