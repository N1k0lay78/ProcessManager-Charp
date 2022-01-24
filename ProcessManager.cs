using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProcessManager
{
    interface IProcessManager
    {
        public bool KillById(int Id);
        public bool KillByName(string Name);
        public void TrackList();
        public bool ProcessesBy(string Name);
        public bool ProcessBy(int Id);
    }

    public class ProcessManager : IProcessManager
    {
        private const int IdWidth       = 7;
        private const int NameWidth     = -40;
        private const int SizeWidth     = -10;
        private const int StartWidth    = -8;
        private const int ThreadsWidth  = -10;
        private const int PriorityWidth = -12;
        private const int AliveWidth    = -10;

        public ProcessManager() { }

        void WriteHeader()
        {
            Console.WriteLine($"| {"Id",IdWidth} | {"Name",NameWidth} | {"Size",SizeWidth} | {"Start",StartWidth} | {"Threads",ThreadsWidth} | {"Priority",PriorityWidth} | {"Alive",AliveWidth} |");
        }

        bool UsableProcess(Process process)
        {
            try
            {
                if (process != null && process.PeakWorkingSet64 != null && process.Threads.Count != null && process.PriorityClass != null && process.Responding != null) { }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void WriteProcess(Process process)
        {
            try
            {
                var time = "";
                if (process.StartTime.Hour < 10)
                {
                    time += '0';
                }
                time += process.StartTime.Hour + ":";
                if (process.StartTime.Minute < 10)
                {
                    time += '0';
                }
                time += process.StartTime.Minute + ":";
                if (process.StartTime.Second < 10)
                {
                    time += '0';
                }
                time += process.StartTime.Second;
                Console.WriteLine($"| {process.Id,IdWidth} | {process.ProcessName,NameWidth} | {(process.PeakWorkingSet64 / 1024 / 1024) + " MB",-SizeWidth} | {time,-StartWidth} | {process.Threads.Count,-ThreadsWidth} | {process.PriorityClass,PriorityWidth} | {process.Responding,AliveWidth} |");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Console.WriteLine("Access denied");
            }
        }

        public bool KillById(int Id)
        {
            try
            {
                Process.GetProcessById(Id).Kill(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool KillByName(string Name)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(Name))
                {
                    process.Kill(true); 
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ProcessesBy(string Name)
        {
            var Any = false;
            foreach (var process in Process.GetProcessesByName(Name))
            {
                if (UsableProcess(process))
                {
                    Any = true;
                }
            }

            if (Any) { 
                WriteHeader();
                foreach (var process in Process.GetProcessesByName(Name))
                {
                    if (UsableProcess(process))
                    {
                        WriteProcess(process);
                    }
                }
            }
            return Any;
        }

        public bool ProcessBy(int Id)
        {
            if (UsableProcess(Process.GetProcessById(Id)))
            {
                WriteHeader();
                WriteProcess(Process.GetProcessById(Id));
                return true;
            }
            return false;
        }

        public void TrackList()
        {
            WriteHeader();
            var names = new HashSet<string>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.MainWindowHandle != process.Handle && process.ProcessName != "" && !names.Contains(process.ProcessName))
                    {
                        names.Add(process.ProcessName);
                        WriteProcess(process);
                    }
                }
                catch (Exception)
                {
                    // System process or access denied
                }
            }
        }
    }
}
