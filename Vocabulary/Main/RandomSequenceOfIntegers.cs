using System;

namespace Main
{
    public class RandomSequenceOfIntegers
    {
        private int[] _v;
        private bool[] _numberIsTaken;
        private int _numberOfIntegers, _numberOfInegersTaken;
        private Random random;

        public RandomSequenceOfIntegers(int startIntegerInclusive, int endIntegerInclusive)
        {
            int i1, i2;
            long longInteger;
            string guid;
            int i, n;

            if (startIntegerInclusive > endIntegerInclusive)
            {
                throw new Exception("start integer must be less than or equal to end integer!");
            }

            _numberOfIntegers = endIntegerInclusive - startIntegerInclusive + 1;
            _numberOfInegersTaken = 0;

            _v = new int[_numberOfIntegers];
            _numberIsTaken = new bool[_numberOfIntegers];

            for (i = 0; i < _numberOfIntegers; i++)
            {
                _v[i] = startIntegerInclusive + i;
                _numberIsTaken[i] = false;
            }

            i1 = (int)(DateTime.Now.Ticks % (long)65423);

            guid = Guid.NewGuid().ToString();
            n = guid.Length;

            for (i = 0; i < n; i++)
            {
                if (char.IsDigit(guid[i]))
                {
                    i2 = int.Parse(guid[i].ToString());

                    longInteger = (long)i1 * (long)i2;

                    if ((longInteger > 0) && (longInteger <= 2147483647))
                    {
                        i1 = i1 * i2;
                    }
                }
            }

            random = new Random(i1);
        }

        public bool AllIntegersAreTaken { get { return _numberOfIntegers == _numberOfInegersTaken; } }

        public int NumberOfIntegers { get { return _numberOfIntegers; } }

        private int ReturnNearestIndexNotTaken(int index)
        {
            int n = random.Next(0, 2);
            int returnIndex = -1;

            if (n == 0) //Search upwards
            {
                while (returnIndex == -1)
                {
                    index--;
                    index = (index == -1) ? (_numberOfIntegers - 1) : index;
                    returnIndex = _numberIsTaken[index] ? -1 : index;
                }
            }
            else //Search downwards
            {
                while (returnIndex == -1)
                {
                    index++;
                    index = (index == _numberOfIntegers) ? 0 : index;
                    returnIndex = _numberIsTaken[index] ? -1 : index;
                }
            }

            return returnIndex;
        }

        public int Next(out int n)
        {
            if (AllIntegersAreTaken)
            {
                throw new Exception("All integers are taken!");
            }

            int index = random.Next(0, _numberOfIntegers);

            if (_numberIsTaken[index])
            {
                index = ReturnNearestIndexNotTaken(index);
            }

            _numberIsTaken[index] = true;
            _numberOfInegersTaken++;

            n = _numberOfInegersTaken;

            return _v[index];
        }
    }
}
