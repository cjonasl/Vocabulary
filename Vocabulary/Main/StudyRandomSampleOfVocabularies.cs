using System;
using System.Collections;
using System.Text;

namespace Main
{
    public class StudyRandomSampleOfVocabularies
    {
        private string[] _first, _second;
        private bool _isFirst;
        private RandomSequenceOfIntegers _randomSequenceOfIntegers;
        private int _currentIndex, _currentNumber;

        public StudyRandomSampleOfVocabularies(string[] wordsAndExplanationArray, ArrayList wordsArray, int from, int to, bool showWordFirst)
        {
            ArrayList v1, v2;
            int i, n, sequenceNr;

            n = wordsArray.Count;

            if (showWordFirst)
            {
                v1 = new ArrayList();
                v2 = new ArrayList();

                for (i = 0; i < n; i++)
                {
                    if (wordsAndExplanationArray[i].IndexOf("Exempel: ") >= 0)
                    {
                        v1.Add(wordsAndExplanationArray[i]);
                        v2.Add((string)wordsArray[i]);
                    }
                }

                n = to - from + 1;
                _first = new string[n];
                _second = new string[n];
                sequenceNr = 0;

                for (i = (from - 1); i <= (to - 1); i++)
                {
                    _first[sequenceNr] = (string)v2[i];
                    _second[sequenceNr] = (string)v1[i];
                    sequenceNr++;
                }
            }
            else
            {
                v1 = new ArrayList();

                for (i = 0; i < n; i++)
                {
                    if (wordsAndExplanationArray[i].IndexOf("Exempel: ") >= 0)
                    {
                        v1.Add(wordsAndExplanationArray[i]);
                    }
                }

                n = to - from + 1;
                _first = new string[n];
                _second = new string[n];
                sequenceNr = 0;

                for (i = (from - 1); i <= (to - 1); i++)
                {
                    _first[sequenceNr] = ReturnEntry((string)v1[i]);
                    _second[sequenceNr] = (string)v1[i];
                    sequenceNr++;
                }
            }

            _isFirst = true;
            _randomSequenceOfIntegers = new RandomSequenceOfIntegers(0, n - 1);
        }

        public string Next(out int n, out int total, out bool isFinished)
        {
            int index;

            if (_isFirst)
            {
                index = _randomSequenceOfIntegers.Next(out n);
                _currentIndex = index;
                _currentNumber = n;
            }
            else
            {
                index = _currentIndex;
                n = _currentNumber;
            }

            isFinished = _randomSequenceOfIntegers.AllIntegersAreTaken && !_isFirst;
            total = _randomSequenceOfIntegers.NumberOfIntegers;

            if (_isFirst)
            {
                _isFirst = false;
                return _first[index];
            }
            else
            {
                _isFirst = true;
                return _second[index];
            }
        }

        private string ReturnEntry(string str)
        {
            StringBuilder tmpSb, sb = new StringBuilder();
            string[] tmp, rows = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int i, j, index1, index2, n;

            for(i = 0; i < rows.Length; i++)
            {
                if ((i == 0) || (string.IsNullOrEmpty(rows[i])))
                {
                    sb.Append("\r\n");
                }
                else if (rows[i].IndexOf("Uttalas: ") >= 0)
                {
                    sb.Append("Uttalas:\r\n");
                }
                else if ((rows[i].IndexOf("Exempel: ") >= 0))
                {
                    index1 = rows[i].IndexOf(';');
                    index2 = rows[i].IndexOf('(');
                    n = int.Parse(rows[i].Substring(2 + index1, index2 - index1 - 3));
                    tmp = rows[i].Substring(9).Split(' ');
                    tmpSb = new StringBuilder("Exempel:");

                    for(j = 1; j <= tmp.Length; j++)
                    {
                        if (j == n)
                        {
                            tmpSb.Append(" XXXXX");
                        }
                        else
                        {
                            tmpSb.Append(" " + tmp[j - 1]);
                        }
                    }

                    sb.Append(tmpSb.ToString() + "\r\n");
                }
                else
                {
                    sb.Append(rows[i] + "\r\n");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
