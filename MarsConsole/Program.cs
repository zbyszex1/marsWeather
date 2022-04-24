using MarsDll;

#region fields
TempBase tempBase = new TempFixed();
bool predefined = false;
bool breakOnFirst = true;
bool ignoreZero = false;
bool errors = false;
#endregion

#region Determining mode of operation
Console.Title = "Mars weather service";
Console.Write("Use predefined 30 element array (y/N)?");
ConsoleKeyInfo key = Console.ReadKey();
if (key.KeyChar == 'y' || key.KeyChar == 'Y')
    predefined = true;
Console.Write("\nIgnore zeros (y/N)?");
key = Console.ReadKey();
if (key.KeyChar == 'y' || key.KeyChar == 'Y')
    ignoreZero = true;
if (!ignoreZero)
{
    Console.Write("\nBreak on first zero (Y/n)?");
    key = Console.ReadKey();
    if (key.KeyChar == 'N' || key.KeyChar == 'n')
        breakOnFirst = false;
}
Console.WriteLine();
#endregion

#region printing data info
Console.WriteLine("finding the temperature closest to °C");
if (predefined)
{
    Console.WriteLine("from predefined short data set");
    tempBase.PrintTemp();
}
else
{
    // initialization of termerature table
    Console.WriteLine("from measurements taken every second");
    Console.WriteLine("at randomly selected points on Mars");
    Console.WriteLine("for a period of one Earth year");
    Console.Write("filling random data ...");
    try
    {
        //tempBase = new TempRandom(-1);
        tempBase = new TempRandom(365 * 24 * 60);
        tempBase.PrintTemp();
    }
    catch (Exception ex)
    {
        Console.Title = "Eror occured!";
        Console.WriteLine("Ups! {0}", ex.Message);
        errors = true;
    }
}
#endregion

#region finding temperature closest to zero
if (!errors)
{
    try
    {
        tempBase.TempFind(ignoreZero, breakOnFirst);
    }
    catch (ArithmeticException ex)
    {
        Console.Title = "Eror occured!";
        Console.WriteLine(ex.Message);
        errors = true;
    }
    catch (Exception ex)
    {
        Console.Title = "Eror occured!";
        Console.WriteLine(ex.Message);
        errors = true;
    }
}
#endregion

#region presenting results
if (!errors)
{
    Console.WriteLine("\rprocessing time: {0:f2} ms", tempBase.Span.TotalMilliseconds);
    if (tempBase.ZeroFound)
    {
        int cntZero = tempBase.CntZero;
        if (cntZero == 1)
            Console.WriteLine("Zero temperature found once");
        else
            Console.WriteLine("Count of zero temperatures: {0}", cntZero);
    }
    else
    {
        int zeroPlus = tempBase.ZeroPlus;
        int zeroMinus = tempBase.ZeroMinus;
        int cntPlus = tempBase.CntPlus;
        int cntMinus = tempBase.CntMinus;
        if (zeroPlus == -zeroMinus)
        {
            Console.WriteLine("Temperatures nearest to 0°C: {0} and {1}", zeroPlus, zeroMinus);
            Console.WriteLine("Count of such temperaures: {0:#,0} and {1:#,0}", cntPlus, cntMinus);
        }
        else // zeroPlus != zeroMinus
        {
            Console.WriteLine("Temperature nearest to 0°C: {0}", zeroPlus < -zeroMinus ? zeroPlus : zeroMinus);
            Console.WriteLine("Count of such temperature: {0:#,0}", zeroPlus < -zeroMinus ? cntPlus : cntMinus);
        }
    }
}
#endregion

Console.WriteLine("\npress any key to exit");
Console.ReadKey();
