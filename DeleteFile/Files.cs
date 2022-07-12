using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteFile
{
  internal class Files
  {
    protected List<String> dirs;
    protected String fullPath;
    protected String rootDir;
    protected String currentDir;
    protected int lineLen = 0;
    protected char Always = ' ';
    // -------------------------------------------------------------------------------
    public Files(string root)
    {
      dirs = new List<string>();
      rootDir = "C:\\" + root;
      dirs.Add(rootDir);
      currentDir = rootDir;
      fullPath = GetPath();
    }
    // -------------------------------------------------------------------------------
    public void List(string toSearch)
    {
      string[] subDirNames;
      string[] curFiles;
      string marker = GetPath();
      if (marker.Length > 76)
        marker = marker.Substring(0, 76) + "...";
      Console.Write("\r" + marker);
      int oldLine = lineLen;
      lineLen = marker.Length;
      if(oldLine > lineLen)
        Console.Write(new string(' ', oldLine - lineLen));
      try
      {
        curFiles = Directory.GetFiles(currentDir);
      }
      catch (Exception ex)
      {
        Log.WriteError(string.Format("{0} : {1}", currentDir, ex.Message));
        return;
      }
      int last = dirs.Count - 1;
      string subName = dirs[last];
      if (subName.ToLower().Contains(toSearch) || currentDir.EndsWith("Application Data\\Application Data"))
      {
        ClearLine();
        Console.WriteLine(fullPath);
        Log.WriteLog(String.Format("{0}", fullPath));
        char key = ' ';
        if (Always != 'Y' && Always != 'N')
        {
          Console.Write("Remove Directory (y/N)?");
          key = Console.ReadKey().KeyChar;
        }
        Console.WriteLine();
        try
        {
          if (key == 'Y' || key == 'N')
            Always = key;
          if (Always == 'Y' || key == 'y')
          {
            last = dirs.Count - 1;
            string parent = dirs[last - 1];
            Directory.Delete(currentDir, true);
            return;
          }
        }
        catch (Exception ex)
        {
          string message = String.Format("can't delete directory '{0}' {1} ***", fullPath, ex.Message);
          ClearLine();
          Console.WriteLine("*** " + message);
          Log.WriteError(message);
          return;
        }
      }
      foreach (string name in curFiles)
      {
        try
        {
          if (Path.GetFileName(name).ToLower().Contains(toSearch))
          {
            Log.WriteLog(String.Format("{0}: {1}", fullPath, name));
            ClearLine();
            Console.WriteLine(String.Format("{0}: {1}", fullPath, name));
            Char key = ' ';
            if (Always != 'Y' && Always != 'N')
            {
              Console.Write("Remove File (y/N)?");
              key = Console.ReadKey().KeyChar;
            }
            Console.WriteLine();
            try
            {
              if (key == 'Y' || key == 'N')
                Always = key;
              if (Always == 'Y' || key == 'y')
              {
                File.Delete(Path.Combine(currentDir, name));
              }
            }
            catch(Exception ex)
            {
              string message = String.Format("can't delete file '{0}' {1}", Path.Combine(currentDir, name), ex.Message);
              ClearLine();
              Console.WriteLine("*** " + message);
              Log.WriteError(message);
            }
          }
        }
        catch (Exception ex)
        {
          Log.WriteError(String.Format("*** file '{0}' {1} ***", name, ex.Message));
          continue;
        }
      }
      try
      {
        subDirNames = Directory.GetDirectories(currentDir, "*", SearchOption.TopDirectoryOnly);
      }
      catch (Exception ex)
      {
        Log.WriteError(String.Format("*** subdirectories of '{0}' {1} ***", currentDir, ex.Message));
        subDirNames = new string[0];
      }
      foreach (string subDir in subDirNames)
      {
        string[] currentEndings = currentDir.Split('\\');
        string[] subEndings = subDir.Split('\\');
        if (currentEndings.Contains("Application Data") && subEndings.Last() == "Application Data")
        {
          string subSub = subDir;
          Log.WriteLog(subDir);
        }
        if (subEndings.Contains("Application Data"))
        {
          if (currentEndings.Contains("Application Data"))
          {
            string subSub = subDir;
          }
        }
        int li = subDir.LastIndexOf('\\');
        string sub = li > 0 ? subDir.Substring(++li) : subDir;
        dirs.Add(sub);
        currentDir = Path.Combine(currentDir, sub);
        fullPath = GetPath();
        List(toSearch);
        int postLast = dirs.Count - 1;
        dirs.RemoveAt(postLast);
        currentDir = GetPath();
      }
    }
    // -------------------------------------------------------------------------------
    public void ClearLine()
    {
      if (lineLen == 0)
        return;
      Console.Write("\r" + new string(' ', lineLen) + "\r");
      lineLen = 0;
    }
    // -------------------------------------------------------------------------------
    protected String GetPath()
    {
      String path = "";
      int p = dirs.Count;
      for (int i = 0; i < p; i++)
      {
        if (i > 0)
          path += "\\";
        path += dirs[i];
      }
      path += "\\";
      return path;
    }
    // -------------------------------------------------------------------------------
  }
}
