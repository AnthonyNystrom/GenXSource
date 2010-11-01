using System;
using System.IO;

namespace Genetibase.Debug
{
  /// <summary>
  /// Summary description for fwatch.
  /// </summary>
  public class fwatch : System.IO.FileSystemWatcher
  {
    public FileSystemWatcher watcher;
    public string path = "C:\\";

    public fwatch()
    {
      watcher = new FileSystemWatcher();
      watcher.Path = path;
    }
  }
}
