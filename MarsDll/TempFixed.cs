using System;

namespace MarsDll
{
    public class TempFixed : TempBase
    {
        #region fixed data
        private readonly sbyte[] fixedData = { 
            -94,  16,  72, -88,  53,  21,   0, -43, -66, -12,
              8, -11, -53,  21,  72,  -2,  -4,   6,  12,  33,
             45, -18,  -4,   2,  12, -18, -55, -23,   0,   5 };
        #endregion

        #region constructor
        public TempFixed() : base()
        {
            tempSize = fixedData.Length;
            tempData = fixedData;
#if DEBUG
            Console.WriteLine("fixed constructor");
#endif
        }
        #endregion
    }
}