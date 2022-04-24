using System;

namespace MarsDll
{
    public interface ITempPrint
    {
        public void PrintTemp();
    }
    public interface ITempCount
    {
        public void TempFind(bool ignoreZero, bool breakOnFirst);
    }
    public interface ITempProps
    {
        public sbyte[] TempData
        { get; }
        public int TempSize
        { get; }
        public TimeSpan Span
        { get; }
        public bool ZeroFound
        { get; }
        public int CntZero
        { get; }
        public int ZeroPlus
        { get; }
        public int ZeroMinus
        { get; }
        public int CntPlus
        { get; }
        public int CntMinus
        { get; }
    }
}
