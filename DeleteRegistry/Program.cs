using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteRegistry
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("UseName: " + Environment.UserName);
      string registrySet = "";
      string search = "";
      if (args.Length > 0 )
        registrySet = args[0].ToLower();
      if (args.Length > 1)
      {
        search = args[1];
      }
      if( registrySet.Length == 0)
      {
        Console.Write("Where to search? ");
        registrySet = Console.ReadLine();
      }
      registrySet = registrySet.Replace("hkey", "");
      registrySet = registrySet.Replace("_", "");
      if (registrySet != "localmachine" && registrySet != "currentuser" &&
          registrySet != "classesroot" && registrySet != "currentconfig" && registrySet!="all")
      {
        Console.WriteLine("run with 'LocalMachine', 'CurrentUser', 'CurrentConfig', 'ClassesRoot' or 'ALL'");
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
      if (registrySet == "all")
      {
        Regs regs = new Regs("localmachine");
        regs.List(search.ToLower());
        char always = regs.Always;
        regs.ClearLine();
        regs = new Regs("currentuser");
        regs.Always = always;
        regs.List(search.ToLower());
        always = regs.Always;
        regs.ClearLine();
        regs = new Regs("currentconfig");
        regs.Always = always;
        regs.List(search.ToLower());
        always = regs.Always;
        regs.ClearLine();
        regs = new Regs("classesroot");
        regs.Always = always;
        regs.List(search.ToLower());
        regs.ClearLine();
      }
      else
      {
        Regs regs = new Regs(registrySet);
        regs.List(search.ToLower());
        regs.ClearLine();
      }
      Console.Write("--- done ---");

      Console.ReadKey();
    }
  }
}
