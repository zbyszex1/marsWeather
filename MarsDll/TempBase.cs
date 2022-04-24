using System;
using System.Text;

namespace MarsDll
{
    public abstract class TempBase :ITempPrint, ITempCount, ITempProps
    {
        #region fields
        private const int rowCnt = 8;
        private const int colCnt = 15;
        protected sbyte[] tempData;
        protected int tempSize;
        protected sbyte zeroPlus = 100;
        protected sbyte zeroMinus = -100;
        protected int cntPlus = 0;
        protected int cntMinus = 0;
        protected int cntZero = 0;
        protected TimeSpan ts;
        protected bool zeroFound;
        #endregion

        #region constructor
        public TempBase()
        {
            zeroFound = false;
            tempSize = 1;
            tempData = new sbyte[tempSize];
            ts = new TimeSpan();
#if DEBUG
            Console.WriteLine("base constructor");
#endif
        }
        #endregion

        #region methods
        public void PrintTemp()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                for (int i = 0; i < rowCnt * colCnt && i < tempSize; i++)
                {
                    sb.AppendFormat("{0}{1,4}", i % colCnt != 0 ? ", " : i == 0 ? "\r" : "\n", tempData[i]);
                    if (i == rowCnt * colCnt - 1)
                        sb.AppendLine("\nand so on ...");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ups! somthing went wrong: {0}", ex.Message);
            }
            Console.WriteLine("{0}", sb.ToString());
            Console.WriteLine("data size: {0:#,0}", tempSize);

        }

        public void TempFind(bool ignoreZero, bool breakOnFirst)
        {
            DateTime dt0 = DateTime.Now;
            // program loop
            foreach (sbyte temp in tempData)
            {
                if (temp == 0 && !ignoreZero)
                {
                    if (!zeroFound)
                        zeroFound = true;
                    cntZero++;
                    if (breakOnFirst)
                        break;
                }
                if (cntZero > 0)
                    continue;
                if (temp > 0)
                {
                    if (temp == zeroPlus)
                        cntPlus++;
                    if (temp < zeroPlus)
                    {
                        zeroPlus = temp;
                        cntPlus = 1;
                    }
                }
                if (temp < 0)
                {
                    if (temp == zeroMinus)
                        cntMinus++;
                    if (temp > zeroMinus)
                    {
                        zeroMinus = temp;
                        cntMinus = 1;
                    }
                }
            }
            DateTime dt1 = DateTime.Now;
            ts = dt1 - dt0;
        }
        #endregion

        #region properties
        public int TempSize
        {
            get { return tempSize; }
        }
        public sbyte[] TempData
        {
            get { return tempData; }
        }
        public TimeSpan Span
        {
            get { return ts; }
        }
        public bool ZeroFound
        {
            get { return zeroFound; }
        }
        public int CntZero
        {
            get { return cntZero; }
        }
        public int ZeroPlus
        {
            get { return zeroPlus; }
        }
        public int ZeroMinus
        {
            get { return zeroMinus; }
        }
        public int CntPlus
        {
            get { return cntPlus; }
        }
        public int CntMinus
        {
            get { return cntMinus; }
        }
        #endregion
    }
}
