using System.Collections.Generic;
using System.Text;

namespace Genetibase.OSwCommon
{
    public interface IFileFilterType
    {
        string[] Extensions
        {
            get;
        }

        string Name
        {
            get;
        }
    }

    /// <summary>
    /// Creates file filters
    /// </summary>
    public sealed class FileFilter
    {
        public static string MakeFilterSortedByName(IEnumerable<IFileFilterType> types, bool asc, bool addAllFiles, bool addAllFilters)
        {
            if (types == null)
                return null;

            // sort
            List<IFileFilterType> sortedList = new List<IFileFilterType>(types);
            if (asc)
                sortedList.Sort(delegate(IFileFilterType t1, IFileFilterType t2) { return t1.Name.CompareTo(t2.Name); });
            else
                sortedList.Sort(delegate(IFileFilterType t1, IFileFilterType t2) { return t2.Name.CompareTo(t1.Name); });

            return MakeFilter(sortedList, addAllFiles, addAllFilters);
        }

        public static string MakeFilter(IEnumerable<IFileFilterType> types, bool addAllFiles, bool addAllFilters)
        {
            if (types == null)
                return null;

            StringBuilder filter = new StringBuilder();
            bool previous = false;
            foreach (IFileFilterType ffType in types)
            {
                if (ffType.Extensions != null && ffType.Extensions.Length > 0)
                {
                    if (previous)
                        filter.Append('|');
                    filter.Append(ffType.Name);

                    StringBuilder exts = new StringBuilder();
                    bool previousExt = false;
                    foreach (string ext in ffType.Extensions)
                    {
                        if (previousExt)
                            exts.Append(";*.");
                        else
                            exts.Append("*.");
                        exts.Append(ext);
                        previousExt = true;
                    }

                    string extsStr = exts.ToString();
                    filter.Append('(');
                    filter.Append(extsStr);
                    filter.Append(")|");

                    filter.Append(extsStr);

                    previous = true;
                }
            }
            if (addAllFiles)
            {
                if (previous)
                    filter.Append('|');
                filter.Append("All Files (*.*)|*.*");
            }
            if (addAllFilters)
            {
                StringBuilder allExtensions = new StringBuilder();
                previous = false;
                foreach (IFileFilterType ffType in types)
                {
                    if (ffType.Extensions != null && ffType.Extensions.Length > 0)
                    {
                        foreach (string ext in ffType.Extensions)
                        {
                            if (previous)
                                allExtensions.Append(";*.");
                            else
                                allExtensions.Append("*.");
                            allExtensions.Append(ext);
                            previous = true;
                        }
                    }
                }
                string exts = allExtensions.ToString();
                filter.Insert(0, '|');
                filter.Insert(0, exts);
                filter.Insert(0, ")|");
                filter.Insert(0, exts);
                filter.Insert(0, "All Supported Formats (");
            }
            return filter.ToString();
        }
    }
}