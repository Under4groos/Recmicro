using System;
using System.IO;
using System.Threading;

namespace EasyEditorLib
{
    
    public class FileSys
    {
        FileSystemWatcher fileSystemWatcher;
         
        //public FileSystemEventHandler fileSystemEventHandler;
        public Action<string, WatcherChangeTypes> fileSystemEventHandler;
        public Action<string , string , WatcherChangeTypes> fileSystemEventHandlerLocal;
         
        public FileSys()
        {
            
        }
        public void InFileSystemWatcher()
        {
             
            LoclaDirectoryValid(LocalDirectory(), true);
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = LocalDirectory();
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Changed;
            fileSystemWatcher.Renamed += FileSystemWatcher_Changed;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(100);
            if(fileSystemEventHandler != null)
                fileSystemEventHandler(e.FullPath, e.ChangeType);
            if (fileSystemEventHandlerLocal != null)
                fileSystemEventHandlerLocal(e.Name, e.FullPath , e.ChangeType);
        }

        public string LocalPath
        {
            get; set;
        } = "Data";

        public bool LoclaFileValid(string name_file)
        {
            return File.Exists(FullPath(name_file));
        }
        public bool LoclaDirectoryValid(string name_dir , bool cr = false)
        {
            if (!Directory.Exists(name_dir))
            {
                if(cr == true)
                {
                    Directory.CreateDirectory(name_dir);
                    return true;
                }
            }
             
            return false;
        }
        public string LocalDirectory()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + $"\\{LocalPath}";
        }
        public string FullPath(string name_file)
        {
            return Path.Combine(LocalDirectory(), name_file);
        }
        public void write_file(string name_file, string text)
        {
            if (text != string.Empty)
                File.WriteAllText(FullPath(name_file), text);
        }
        public string read_file(string name_file)
        {
             if(!LoclaFileValid(name_file))
                return string.Empty;
             return File.ReadAllText(FullPath(name_file));
        }
        public string[] read_file_lines(string name_file)
        {
            if (!LoclaFileValid(name_file))
                return new string[] { };
            return File.ReadAllLines(FullPath(name_file));
        }
    }
}
