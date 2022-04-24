using System;

namespace MarsDll
{
    public class TempRandom : TempBase
    {
        #region constructor
        public TempRandom(int size) : base()
        {
#if DEBUG
            Console.WriteLine("random constructor");
#endif
            tempSize = size;
                tempData = new sbyte[tempSize];
                Random random = new Random();
                for (int i = 0; i < tempSize; i++)
                    tempData[i] = (sbyte)random.Next(-100, 100);
        }
        #endregion
    }
}
