using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PulseSecure
{
  public class Regs
  {
    protected List<String> paths;
    protected List<RegistryKey> keys;
    protected String fullPath;
    protected RegistryKey rootKey;
    protected RegistryKey currentKey;
    // -------------------------------------------------------------------------------
    public Regs(string registrySet)
    {
      paths = new List<string>();
      switch (registrySet) {
        case "localmachine":
          rootKey = Registry.LocalMachine;
          paths.Add("HKEY_LOCAL_MACHINE");
          break;
        case "currentconfig":
          rootKey = Registry.CurrentConfig;
          paths.Add("HKEY_CURRENT_CONFIG");
          break;
        case "classesroot":
          rootKey = Registry.ClassesRoot;
          paths.Add("HKEY_CLASSES_ROOT");
          break;
        default:
          rootKey = Registry.CurrentUser;
          paths.Add("HKEY_CURRENT_USER");
          break;

      }
      currentKey = rootKey;
      keys = new List<RegistryKey>();
      keys.Add(currentKey);
      fullPath = "";
      fullPath = GetPath();
    }
    // -------------------------------------------------------------------------------
    public void List(string toSearch)
    {
      string[] subKeysNames;
      string[] names = currentKey.GetValueNames();
      foreach (string name in names)
      {
        try
        {
          RegistryValueKind kind = currentKey.GetValueKind(name);
          if (kind != RegistryValueKind.String && kind != RegistryValueKind.ExpandString)
            continue;
          string value = (string)currentKey.GetValue(name);
          if (fullPath.ToLower().Contains(toSearch) || name.ToLower().Contains(toSearch) || value.ToLower().Contains(toSearch))
            Console.WriteLine("{0} {1}:{2}", fullPath, name, value);
        }
        catch(Exception ex)
        {
          //Console.WriteLine("*** value of '{0}' {1} ***", name, ex.Message);
          continue;
        }
      }
      try
      {
        subKeysNames = currentKey.GetSubKeyNames();
      }
      catch (Exception ex)
      {
        //Console.WriteLine("*** subkeys of '{0}' {1} ***", currentKey.Name, ex.Message);
        subKeysNames = new string[0];
      }
      foreach (string subKeyName in subKeysNames)
      {
        try
        {
          currentKey = currentKey.OpenSubKey(subKeyName);
        }
        catch(Exception ex)
        {
          //Console.WriteLine("*** subkey '{0}' {1} ***",fullPath+subKeyName, ex.Message);
          continue;
        }
        keys.Add(currentKey);
        paths.Add(subKeyName);
        fullPath = GetPath();
        List(toSearch);
        int last = paths.Count - 1;
        paths.RemoveAt(last);
        last = keys.Count - 1;
        keys.RemoveAt(last);
        currentKey = keys[--last];
      }
    }
    // -------------------------------------------------------------------------------
    //public Regs(String[] _paths)
    //{
    //  paths = _paths;
    //}
    // -------------------------------------------------------------------------------
    public Regs(RegistryKey _root)
    {
      rootKey = _root;
    }
    // -------------------------------------------------------------------------------
    //public Regs(String[] _paths, RegistryKey _root)
    //{
    //  paths = _paths;
    //  rootKey = _root;
    //}
    // -------------------------------------------------------------------------------
    protected String GetPath()
    {
      String path = "";
      int p = paths.Count;
      for (int i = 0; i < p; i++)
      {
	      if (i > 0)
	        path += "\\";
	      path += paths[i];
      }
      path += "\\";
      return path;
    }
    // -------------------------------------------------------------------------------
    public Int32 GetNumber(RegistryKey key, String val, Int32 def=100)
    {
      Int32 nVal;
      String nTxt;
      nTxt = key.GetValue(val, def.ToString()).ToString();
      if (!Int32.TryParse(nTxt, out nVal))
	      nVal = def;
      return nVal;
    }
    // -------------------------------------------------------------------------------
    public void Close()
    {
      if (rootKey != null)
        rootKey.Close();
    }
    // -------------------------------------------------------------------------------
  }
}
