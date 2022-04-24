using System.Text;

// Predefined temperature in range -100 - +100 °C
using System;

sbyte[] tempData = { -94,  16,  72, -88,  53,  21,   0, -43, -66, -12,
                       8, -11, -53,  21,  72,  -2,  -4,   6,  12,  33,
                      45, -18,  -4,   2,  12, -18, -55, -23,   0,   5 };

// variables
const int rowCnt = 8;
const int colCnt = 15;
const int randSize = 365 * 24 * 60 * 60;
sbyte zeroPlus = 100;
sbyte zeroMinus = -100;
int cntPlus = 0;
int cntMinus = 0;
int cntZero = 0;
int tempSize;
bool zeroFound = false;
bool predefined = false;
bool breakOnFirst = true;
bool ignoreZero = false;

try
{
    // Checking mode of operaion
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

    // printing info
    Console.WriteLine("finding the temperature closest to °C");
    if (predefined)
        Console.WriteLine("from predefined short data set");
    else
    {
        // initialization of termerature table
        Console.WriteLine("from measurements taken every second");
        Console.WriteLine("at randomly selected points on Mars");
        Console.WriteLine("for a period of one Earth year");
        Console.Write("filling random data ...");
        tempData = new sbyte[randSize];
        Random random = new Random();
        for (int i = 0; i < randSize; i++)
            tempData[i] = (sbyte)random.Next(-100, 100);
    }

    // printing data
    tempSize = tempData.Length;
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < rowCnt * colCnt && i < tempSize; i++)
    {
        sb.AppendFormat("{0}{1,4}", i % colCnt != 0 ? ", " : i == 0 ? "\r" : "\n", tempData[i]);
        if (i == rowCnt * colCnt - 1)
            sb.AppendLine("\nand so on ...");
    }
    Console.WriteLine("{0}", sb.ToString());
    Console.WriteLine("data size: {0:#,0}", tempSize);
    Console.Write("selecting ...");

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
    TimeSpan ts = dt1 - dt0;

    // presenting results
    Console.WriteLine("\rprocessing time: {0:f2} ms", ts.TotalMilliseconds);
    if (zeroFound)
    {
        if (cntZero == 1)
            Console.WriteLine("Zero temperature found");
        else
            Console.WriteLine("Count of zero temperatures: {0}", cntZero);
    }
    else if (zeroPlus == -zeroMinus)
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
catch (ArithmeticException ex)
{
    Console.Title = "Eror occured!";
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.Title = "Eror occured!";
    Console.WriteLine(ex.Message);
}
finally
{
    Console.WriteLine("\npress any key to exit");
    Console.ReadKey();
}

