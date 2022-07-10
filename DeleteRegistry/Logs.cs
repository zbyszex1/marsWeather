using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DeleteRegistry
{
  public class Log
  {
    public static string LogName = "log.txt";
    public static string ErrorName = "error.txt";

    //---------------------------------------------------------------------------
    public static void Init()
    {
      DateTime now = DateTime.Now;
      Object[] args = { };
      string lName = String.Format("regs_{0}_{1}.txt", now.ToString("yyyy-MM-dd"), now.ToLongTimeString());
      LogName = Path.Combine(GetPath(), lName.Replace(':', '_'));
      ErrorName = Path.Combine(GetPath(), "refs_error.txt");
    }
    // ---------------------------------------------------------------------------
    public static void WriteLog(String lString)
    {
      Write(LogName, lString, new DateTime(0));
    }
    //---------------------------------------------------------------------------
    public static void WriteLog(Byte[] lData)
    {
      Write(LogName, Bytes2String(lData), new DateTime(0));
    }
    //---------------------------------------------------------------------------
    public static void WriteLog(String lString, Byte[] lData)
    {
      Write(LogName, lString + "; " + Bytes2String(lData), new DateTime(0));
    }
    // ---------------------------------------------------------------------------
    public static void WriteLog(String lString, DateTime now)
    {
      Write(LogName, lString, now );
    }
    // ---------------------------------------------------------------------------
    public static void WriteError(String lString)
    {
      Write(ErrorName, lString, DateTime.Now);
    }
    //---------------------------------------------------------------------------
    protected static string GetPath()
    {
      String exPath = Application.ExecutablePath.ToLower();
      return Path.GetDirectoryName(exPath);
    }
    //---------------------------------------------------------------------------
    protected static void Write(String lName, String lString, DateTime now)
    {
      FileStream fs = null;
      StreamWriter sw = null;
      Int16 ms;
      Boolean lOK;
      String post;
      post = "";
      for (lOK = false; !lOK;)
      {
        try
        {
          fs = new FileStream(lName, FileMode.OpenOrCreate);
          fs.Seek(0, System.IO.SeekOrigin.End);
          sw = new StreamWriter(fs);
          if (now.Ticks > 0)
          {
            ms = (Int16)((now.Millisecond + 5) / 10);
            if (ms >= 100)
              ms = 0;
            Object[] args = { now.ToString("yyyy-MM-dd"), now.ToLongTimeString(), ms % 100, lString, post };
            sw.WriteLine(String.Format("{0} {1}:{2:d2}; {3}{4}", args));
          }
          else
            sw.WriteLine(lString);
          sw.Flush();
          sw.Close();
          sw = null;
          fs.Close();
          fs = null;
          lOK = true;
        }
        catch (System.Exception e)
        {
          String lMessage = e.Message;
          post = String.Concat(post, " .");
          if (sw != null)
            sw.Close();
          sw = null;
          if (fs != null)
            fs.Close();
          fs = null;
          Random r = new Random();
          Thread.Sleep(10 + r.Next(50));
        }
      }
      post = null;
    }
    //---------------------------------------------------------------------------
    protected static String Bytes2String(Byte[] lData)
    {
      StringBuilder sb = new StringBuilder();
      for (Int16 li = 0; lData != null && li < lData.Length; li++)
        sb.Append((lData[li] >= 0x20 && lData[li] < 0x7f) ? FromBytes(lData, li, 1) :
                                    String.Format("[{0:X2}]", lData[li] & 0xff));
      return sb.ToString();
    }
    //---------------------------------------------------------------------------
    protected static String FromBytes(Byte[] lBytes, Int32 lOffset, Int32 lSize)
    {
      Int32 lo = lOffset;
      Int32 ls = lSize;
      if (lo < 0 || lo > lBytes.Length)
        lo = lBytes.Length;
      if (ls < 0 || ls > lBytes.Length - lo)
        ls = lBytes.Length - lo;
      Char[] chars = new Char[ls];
      for (int li = 0; li < ls; li++)
        chars[li] = (char)lBytes[lo + li];
      String lString = new String(chars);
      chars = null;
      return (lString);
    }
  }
}
