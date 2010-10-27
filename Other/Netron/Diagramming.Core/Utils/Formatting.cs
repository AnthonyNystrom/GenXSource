
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace Netron.Diagramming.Core
{
  /// <summary>
  /// <i>(Describe usage of "L:300" format string.)</i>
  /// </summary>
  public interface IShowable : IFormattable
  {
    //TODO: wonder if we should use TextWriters instead of StringBuilders?
    /// <summary>
    /// Format <code>this</code> using at most approximately <code>rest</code> chars and 
    /// append the result, possibly truncated, to stringbuilder.
    /// Subtract the actual number of used chars from <code>rest</code>.
    /// </summary>
    /// <param name="stringbuilder"></param>
    /// <param name="rest"></param>
    /// <param name="formatProvider"></param>
    /// <returns>True if the appended formatted string was complete (not truncated).</returns>
    bool Show(StringBuilder stringbuilder, ref int rest, IFormatProvider formatProvider);
  }
  // ------------------------------------------------------------

  // Static helper methods for Showing collections 

  /// <summary>
  /// 
  /// </summary>
  public static class Showing
  {
    /// <summary>
    /// Show  <code>Object obj</code> by appending it to <code>stringbuilder</code>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="stringbuilder"></param>
    /// <param name="rest"></param>
    /// <param name="formatProvider"></param>
    /// <returns>True if <code>obj</code> was shown completely.</returns>
    public static bool Show(Object obj, StringBuilder stringbuilder, ref int rest, IFormatProvider formatProvider)
    {
      if (rest <= 0)
        return false;
      else if (obj is IShowable)
        return ((IShowable)obj).Show(stringbuilder, ref rest, formatProvider);
      int oldLength = stringbuilder.Length;
      stringbuilder.AppendFormat(formatProvider, "{0}", obj);
      rest -= (stringbuilder.Length - oldLength);
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="showable"></param>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public static String ShowString(IShowable showable, String format, IFormatProvider formatProvider)
    {
      int rest = maxLength(format);
      StringBuilder sb = new StringBuilder();
      showable.Show(sb, ref rest, formatProvider);
      return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    static int maxLength(String format)
    {
      //TODO: validate format string
      if (format == null)
        return 80;
      if (format.Length > 1 && format.StartsWith("L"))
      {
        return int.Parse(format.Substring(1));
      }
      else
        return int.MaxValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="stringbuilder"></param>
    /// <param name="rest"></param>
    /// <param name="formatProvider"></param>
    /// <returns>True if collection was shown completely</returns>
    public static bool ShowCollectionValue<T>(ICollectionBase<T> items, StringBuilder stringbuilder, ref int rest, IFormatProvider formatProvider)
    {
      string startdelim = "{ ", enddelim = " }";
      bool showIndexes = false;
      
      //TODO: do not test here at run time, but select code at compile time
      //      perhaps by delivering the print type to this metod
      if (items is IList<T>)
      {
        startdelim = "[ ";
        enddelim = " ]";
       
      }
      else if (items is ICollection<T>)
      {
       
          startdelim = "{{ ";
          enddelim = " }}";
       
      }

      stringbuilder.Append(startdelim);
      rest -= 2 * startdelim.Length;
      bool first = true;
      bool complete = true;
      int index = 0;

     
      {
        foreach (T x in items)
        {
          complete = false;
          if (rest <= 0)
            break;
          if (first)
            first = false;
          else
          {
            stringbuilder.Append(", ");
            rest -= 2;
          }
          if (showIndexes)
          {
            string indexString = string.Format("{0}:", index++);
            stringbuilder.Append(indexString);
            rest -= indexString.Length;
          }
          complete = Showing.Show(x, stringbuilder, ref rest, formatProvider);
        }
      }
      if (!complete)
      {
        stringbuilder.Append("...");
        rest -= 3;
      }
      stringbuilder.Append(enddelim);
      return complete;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// 
    /// <param name="dictionary"></param>
    /// <param name="stringbuilder"></param>
    /// <param name="formatProvider"></param>
    /// <param name="rest"></param>
    /// <returns></returns>
    public static bool ShowDictionary<K, V>(IDictionary<K, V> dictionary, StringBuilder stringbuilder, ref int rest, IFormatProvider formatProvider)
    {
      stringbuilder.Append("{ ");
      rest -= 4;				   // Account for "( " and " )"
      bool first = true;
      bool complete = true;

      foreach (KeyValuePair<K, V> p in dictionary)
      {
        complete = false;
        if (rest <= 0)
          break;
        if (first)
          first = false;
        else
        {
          stringbuilder.Append(", ");
          rest -= 2;
        }
        complete = Showing.Show(p, stringbuilder, ref rest, formatProvider);
      }
      if (!complete)
      {
        stringbuilder.Append("...");
        rest -= 3;
      }
      stringbuilder.Append( " }");
      return complete;
    }
  }
}