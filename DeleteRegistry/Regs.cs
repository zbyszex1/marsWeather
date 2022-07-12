using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DeleteRegistry
{
  public class Regs
  {
    protected List<String> paths;
    protected List<RegistryKey> keys;
    protected String fullPath;
    protected RegistryKey rootKey;
    protected RegistryKey currentKey;
    protected int lineLen = 0;
    protected int sbLen = 0;
    public char Always = ' ';
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
      int last = paths.Count - 1;
      string subName = paths[last];

      string marker = GetPath();
      if (marker.Length > 76)
        marker = marker.Substring(0, 76) + "...";
      Console.Write("\r" + marker);
      int oldLine = lineLen;
      lineLen = marker.Length;
      if (oldLine > lineLen)
        Console.Write(new string(' ', oldLine - lineLen));

      if (subName.ToLower().Contains(toSearch))
      {
        ClearLine();
        Console.WriteLine(fullPath);
        char key = ' ';
        if (Always != 'Y' && Always != 'N')
        {
          Console.Write("Remove Registry Key (y/N)?");
          key = Console.ReadKey().KeyChar;
        }
        try
        {
          if (key == 'Y' || key == 'N')
            Always = key;
          if (Always == 'Y' || key == 'y')
          {
            last = keys.Count - 1;
            RegistryKey parent = keys[last - 1];
            parent.DeleteSubKeyTree(subName, false);
            ClearLine();
            Console.WriteLine();
            return;
          }
          Console.WriteLine();
          if (Always == 'N')
          {
            WriteRegistryPath(currentKey);
            return;
          }
        }
        catch (Exception ex)
        {
          Log.WriteError(String.Format("*** can't delete key '{0}' {1} ***", fullPath, ex.Message));
          return;
        }
      }
      foreach (string name in names)
      {
        try
        {
          RegistryValueKind kind = currentKey.GetValueKind(name);
          if (kind != RegistryValueKind.String && kind != RegistryValueKind.ExpandString)
            continue;
          string value = (string)currentKey.GetValue(name);
          if (fullPath.ToLower().Contains(toSearch) || name.ToLower().Contains(toSearch) || value.ToLower().Contains(toSearch))
            //Console.WriteLine("{0} {1}:{2}", fullPath, name, value);
            Log.WriteLog(String.Format("{0} {1}:{2}", fullPath, name, value));
          if (name.ToLower().Contains(toSearch) || value.ToLower().Contains(toSearch))
          {
            ClearLine();
            Console.WriteLine(String.Format("{0} {1}:{2}", fullPath, name, value));
            char key = ' ';
            if (Always != 'Y' && Always != 'N')
            {
              Console.Write("Remove Registry Entry (y/N)?");
              key = Console.ReadKey().KeyChar;
            }
            if (key == 'Y' || key == 'N')
              Always = key;
            if (Always == 'Y' || key == 'y')
              currentKey.DeleteValue(name);
            Console.WriteLine();
            if (Always == 'N')
            {
              WriteRegistryPath(currentKey);
              return;
            }
          }
        }
        catch (Exception ex)
        {
          Log.WriteError(String.Format("*** value of '{0}' {1} ***", name, ex.Message));
          continue;
        }
      }
      try
      {
        subKeysNames = currentKey.GetSubKeyNames();
      }
      catch (Exception ex)
      {
        Log.WriteError(String.Format("*** subkeys of '{0}' {1} ***", currentKey.Name, ex.Message));
        subKeysNames = new string[0];
      }
      foreach (string subKeyName in subKeysNames)
      {
        try
        {
          currentKey = currentKey.OpenSubKey(subKeyName, true);
          if (currentKey == null)
            return;
        }
        catch (Exception ex)
        {
          Log.WriteError(String.Format("*** subkey '{0}' {1} ***", fullPath + subKeyName, ex.Message));
          continue;
        }
        keys.Add(currentKey);
        paths.Add(subKeyName);
        fullPath = GetPath();
        List(toSearch);
        int postLast = paths.Count - 1;
        paths.RemoveAt(postLast);
        last = keys.Count - 1;
        keys.RemoveAt(last);
        currentKey = keys[--postLast];
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
    protected void WriteRegistryPath(RegistryKey lKey)
    {
      string[] subKeysNames;
      Log.WriteReg("[" + lKey.Name + "]");
      string value = (string)lKey.GetValue(null);
      if (value != null)
        Log.WriteReg("@=\"" + value + "\"");
      string[] names = lKey.GetValueNames();
      foreach (string name in names)
      {
        if (name.Length == 0)
          continue;
        RegistryValueKind kind = lKey.GetValueKind(name);
        switch (kind)
        {
          case RegistryValueKind.String:
            string strVal = (string)lKey.GetValue(name);
            strVal = strVal.Replace("\"", "\\\"");
            Log.WriteReg(string.Format("\"{0}\"=\"{1}\"", name, strVal));
            break;
          case RegistryValueKind.ExpandString:
            string expandVal = (string)lKey.GetValue(name);
            string expandHdr = string.Format("\"{0}\"=hex(2):", name);
            string[] expandStrings = new string[1];
            expandStrings[0] = expandVal;
            Log.WriteReg(WriteStrings(expandHdr, expandStrings));
            break;
          case RegistryValueKind.MultiString:
            string[] multiVal = (string[])lKey.GetValue(name);
            string multiHdr = string.Format("\"{0}\"=hex(7):", name);
            Log.WriteReg(WriteStrings(multiHdr, multiVal));
            break;
          case RegistryValueKind.DWord:
            Int32 intVal = (Int32)lKey.GetValue(name);
            Log.WriteReg(string.Format("\"{0}\"=dword:{1}", name, intVal));
            break;
          case RegistryValueKind.QWord:
            Int64 longVal = (Int64)lKey.GetValue(name);
            string longHdr = string.Format("\"{0}\"=hex(b):", name);
            byte[] bajty = new byte[8];
            for (int i = 0; i< 8; i++)
            {
              bajty[i] = (byte)(longVal & 0xff);
              longVal >>= 8;
            }
            Log.WriteReg(WriteBytes(longHdr, bajty));
            break;
          case RegistryValueKind.Binary:
            string binaryHdr = string.Format("\"{0}\"=hex:", name);
            byte[] binaryVal = (byte[])lKey.GetValue(name);
            Log.WriteReg(WriteBytes(binaryHdr, binaryVal));
            break;
          default:
            break;
        }
      }
      Log.WriteReg("");
      try
      {
        subKeysNames = lKey.GetSubKeyNames();
      }
      catch (Exception ex)
      {
        Log.WriteError(String.Format("*** subkeys of '{0}' {1} ***", currentKey.Name, ex.Message));
        subKeysNames = new string[0];
        Log.WriteReg("");
        return;
      }
      foreach (string subKeyName in subKeysNames)
      {
        try
        {
          RegistryKey sKey = lKey.OpenSubKey(subKeyName, false);
          if (lKey == null)
            return;
          WriteRegistryPath(sKey);
        }
        catch (Exception ex)
        {
          Log.WriteError(String.Format("*** subkey '{0}' {1} ***", fullPath + subKeyName, ex.Message));
          continue;
        }

      }
    }
    // -----------------------------------------------------------------------------
    protected string WriteStrings(string header, String[] stringi)
    {
      StringBuilder sb = new StringBuilder(header);
      sbLen = header.Length;
      int size = 0;
      for (int i = 0; i < stringi.Length; i++)
        size += 2* stringi[i].Length + 2;
      if (stringi.Length > 1)
        size += 2;
      byte[] bytes = new byte[size];
      int c = 0;
      for (int j = 0; j < stringi.Length; j++)
      {
        char[] chars = stringi[j].ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
          bytes[c++] = (byte)(chars[i] & 0xff);
          bytes[c++] = (byte)((chars[i] >> 8) & 0xff);
        }
        if (stringi.Length > 1)
        {
          bytes[c++] = 0;
          bytes[c++] = 0;
        }
      }
      if (stringi.Length > 1)
      {
        bytes[c++] = 0;
        bytes[c++] = 0;
      }
      for( c=0; c<bytes.Length; c++)
      sb.Append(Byte2String(bytes[c], c == bytes.Length - 1));
      return sb.ToString();
    }
    // -----------------------------------------------------------------------------
    protected string WriteBytes(string header, byte[] bajty)
    {
      StringBuilder sb = new StringBuilder(header);
      sbLen = header.Length;
      for (int i = 0; i < bajty.Length; i++)
      {
        sb.Append(Byte2String(bajty[i], i == bajty.Length - 1));
      }
      return sb.ToString();
    }
    // -----------------------------------------------------------------------------
    protected string Byte2String(byte bajt, bool last = false)
    {
      sbLen += 3;
      if (sbLen <= 76)
        return String.Format("{0:x2}{1}", bajt, last ? "" : ",");
      sbLen = 2;
      return String.Format("{0:x2}{1}\\\r\n  ", bajt, last ? "" : ",");
    }
    // -----------------------------------------------------------------------------
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
