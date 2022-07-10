using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteFile
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine(Environment.UserName);
      string directorySet = "";
      string search = "";
      if (args.Length > 0)
        directorySet = args[0].ToLower();
      if (args.Length > 1)
      {
        search = args[1];
      }
      if (directorySet.Length == 0)
      {
        Console.Write("Where to search? ");
        directorySet = Console.ReadLine();
      }
      if (directorySet.ToLower() != "program files" && directorySet.ToLower() != "program files(x86)" &&
          directorySet.ToLower() != "users" && directorySet.ToLower() != "all")
      {
        Console.WriteLine("run with 'Program Files', 'Program Filse (x86)', 'Users', or 'ALL'");
        Console.ReadKey();
        return;
      }
      if (search.Length == 0)
      {
        Console.Write("What to search? ");
        search = Console.ReadLine();
      }
      search = search.Replace("\"", "");
      if (search.Length == 0)
      {
        Console.Write("Nothing to do");
        return;
      }
      Log.Init();
      Console.WriteLine("--- start ---");
      if (directorySet == "all")
      {
        Files files = new Files("Program Files");
        files.List(search.ToLower());
        files.ClearLine();
        files = new Files("Program Files (x86)");
        files.List(search.ToLower());
        files.ClearLine();
        files = new Files("Users\\" + Environment.UserName);
        files.List(search.ToLower());
        files.ClearLine();
      }
      else
      {
        string file = directorySet;
        if (directorySet.ToLower() == "users")
          file = directorySet + "\\" + Environment.UserName;
        Files files = new Files(file);
        files.List(search.ToLower());
        files.ClearLine();
      }
      Console.Write("--- done ---");

      Console.ReadKey();
    }
  }
}
