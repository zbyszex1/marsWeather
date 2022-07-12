using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsDll
{
  public class Tree
  {
    public int x;
    public Tree l;
    public Tree r;
    public Tree(int px, Tree pl, Tree pr)
    {
      this.x = px;
      this.l = pl;
      this.r = pr;
    }

  }
  public class Graph
  {
    public static Tree T33 = new Tree(33, null, null);
    public static Tree T34 = new Tree(34, null, null);
    public static Tree T12 = new Tree(12, T33, T34);
    public static Tree T21 = new Tree(121, T12, null);
    public static Tree T20 = new Tree(20, null, null);
    public static Tree T3 = new Tree(3, T20, T21);


    public static Tree T44 = new Tree(44, null, null);
    public static Tree T64 = new Tree(64, null, null);
    public static Tree T82 = new Tree(82, T44, T64);
    public static Tree T7 = new Tree(7, null, T82);
    public static Tree T1 = new Tree(1, null, T7);
    public static Tree T10 = new Tree(10, T1, null);
    public static Tree T5 = new Tree(5, T3, T10);

    public static int result = 0;
    protected static void sol(Tree t, int res)
    {
      int temp1 = res;
      int temp2 = res;
      if (t.l != null)
        sol(t.l, ++temp1);
      if (t.r != null)
        sol(t.r, ++temp2);
      if (temp1 >= temp2 && temp1 > result)
        result = temp1;
      if (temp1 < temp2 && temp2 > result)
        result = temp2;
    }
    public static int solution(Tree T)
    {
      sol(T, 0);
      return result;
    }

    public static int[] weights = new int[1];
    public static ArrayList results = new ArrayList();
    public static ArrayList bitsA = new ArrayList();
    public static ArrayList bitsB = new ArrayList();
    public static ArrayList bitsC = new ArrayList();

    public static int[] Zeros(int[] bits)
    {
      ArrayList zerosAL = new ArrayList();
      for (int i = 0; i < bits.Length; i++)
        if (bits[i] == 0) 
          zerosAL.Add(weights[i]);
      int[] zeros = new int[zerosAL.Count];
      for (int i = 0; i < zerosAL.Count; i++)
        zeros[i] = (int)zerosAL[i];
      return zeros;
    }
  public static int[] Bits(int A)
    {
      int[] bits = new int[30];
      int rest = A;
      for (int i = 30 - 1; i >= 0; i--)
      {
        if (rest >= weights[i])
        {
          rest -= weights[i];
          bits[i] = 1;
        }
        else
          bits[i] = 0;
      }
      return bits;
    }
    public static void Nexty(int X, int[] zerosX)
    {
      int posibs = weights[zerosX.Length+1];
      int newVal = X;
      for (int i = 0; i < posibs; i++)
      {
        newVal = X;
        for (int j = 0; j < zerosX.Length; j++)
        {
          if ((i & weights[j]) != 0)
            newVal += zerosX[j];
        }
        if (!results.Contains(newVal))
          results.Add(newVal);
      }
    }
    public static int conforming(int A, int B, int C)
    {
      weights = new int[30];
      weights[0] = 1;
      for (int i = 1; i < 30; i++)
        weights[i] = weights[i - 1] * 2;
      int[] bitsA = Bits(A);
      int[] bitsB = Bits(B);
      int[] bitsC = Bits(C);
      int[] zerosA = Zeros(bitsA);
      int[] zerosB = Zeros(bitsB);
      int[] zerosC = Zeros(bitsC);
      Nexty(A, zerosA);
      Nexty(B, zerosB);
      Nexty(C, zerosC);
      return results.Count;
    }

    protected static int first(int[] A)
    {
      for (int i = 0; i < A.Length - 1; i++)
      {
        if (A[i] > A[i + 1])
          return i;
      }
      return -1;
    }

    protected static int second(int[] A, int start)
    {
      int v = A[start];
      for (int i = start + 1; i < A.Length; i++)
      {
        if (A[i] >= v)
          return i;
      }
      return 0;
    }
    protected static int unsorts(int[] A)
    {
      int x = 0;
      for (int i = 0; i < A.Length - 1; i++)
      {
        if (A[i] > A[i + 1])
          x++;
      }
      return x;
    }

    public static bool solution_(int[] A)
    {
      // write your code in C# 6.0 with .NET 4.5 (Mono)
      int x = unsorts(A);
      if (x == 0)
        return true;
      if (x >= 2)
        return false;
      int i = first(A);
      if (i == 0)
        return true;
      int j = second(A, i);
      if (j == 0)
        return false;
      return true;
    }



    public static int solution(int[] A, int[] B, int M, int X, int Y)
    {
      int stops = 0;
      int w = 0;
      int c = 0;
      int pp = 0;
      int pl = 0;
      bool full = false;
      for( int p = 0; p < A.Length; p++)
      {
        if (B[p] == 0)
          continue;
        if (w + A[p] <= Y && c < X)
        {
          w += A[p];
          c++;
          pl = p;
        }
        else
        {
          full = true;
          p--;
        }
        if (full || p == A.Length - 1)
        {
          for (int f = 1; f <= M; f++)
          {
            for (int i = pp; i <= pl; i++)
            {
              if (B[i] == f)
              {
                stops++;
                break;
              }
            }
          }
          w = 0;
          c = 0;
          pp = ++pl;
          full = false;
          stops++;
        }
      }
      return stops;
      // write your code in C# 6.0 with .NET 4.5 (Mono)
    }
  }
}
