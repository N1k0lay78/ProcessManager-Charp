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
        public bool FullInfo(int Id);
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
                Console.WriteLine($"| {process.Id,IdWidth} | {process.ProcessName,NameWidth} | {(process.PrivateMemorySize64 / 1024 / 1024) + " MB",-SizeWidth} | {time,-StartWidth} | {process.Threads.Count,-ThreadsWidth} | {process.PriorityClass,PriorityWidth} | {process.Responding,AliveWidth} |");
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

        public bool FullInfo(int Id)
        {
            var process = Process.GetProcessById(Id);
            if (UsableProcess(process)) 
            {
                Console.WriteLine($"Id:");
                Console.WriteLine($"Id                         {process.Id}");
                Console.WriteLine($"SessionId                  {process.SessionId}");
                
                Console.WriteLine($"Info:");
                Console.WriteLine($"ProcessName                {process.ProcessName}");
                Console.WriteLine($"MainModule                 {process.MainModule}");
                Console.WriteLine($"MainWindowTitle            {process.MainWindowTitle}");
                Console.WriteLine($"PriorityClass              {process.PriorityClass}");
                Console.WriteLine($"PriorityBoostEnabled       {process.PriorityBoostEnabled}");
                Console.WriteLine($"BasePriority               {process.BasePriority}");
                Console.WriteLine($"Responding(Alive)          {process.Responding}");
                Console.WriteLine($"MachineName                {process.MachineName}");
                Console.WriteLine($"Modules                    {process.Modules}");
                Console.WriteLine($"HasExited                  {process.HasExited}");

                Console.WriteLine($"Handle:");
                Console.WriteLine($"Handle                     {process.Handle}");
                Console.WriteLine($"HandleCount                {process.HandleCount}");
                Console.WriteLine($"MainWindowHandle           {process.MainWindowHandle}");

                Console.WriteLine($"Memory:");
                Console.WriteLine($"VirtualMemorySize64        {process.VirtualMemorySize64} Byte");
                Console.WriteLine($"NonpagedSystemMemorySize64 {process.NonpagedSystemMemorySize64} Byte");
                Console.WriteLine($"PrivateMemorySize64        {process.PrivateMemorySize64} Byte");

                try
                {
                    Console.WriteLine($"Standard input/output/error/info:");
                    Console.WriteLine($"StandardInput              {process.StandardInput}");
                    Console.WriteLine($"StandardOutput             {process.StandardOutput}");
                    Console.WriteLine($"StandardError              {process.StandardError}");
                    Console.WriteLine($"StartInfo                  {process.StartInfo}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"Cant get info about Standard IO");
                }
                Console.WriteLine($"Threads:");
                Console.WriteLine($"Threads.Count              {process.Threads.Count}");
                Console.WriteLine($"Time:");
                Console.WriteLine($"StartTime                  {process.StartTime}");
                Console.WriteLine($"TotalProcessorTime         {process.TotalProcessorTime}");
                Console.WriteLine($"UserProcessorTime          {process.UserProcessorTime}");
                Console.WriteLine($"PrivilegedProcessorTime    {process.PrivilegedProcessorTime}");
                return true;
            }
            return false;
        }
        
        public List<Process> GetProcessesList(int Id)
        {
            
            if (UsableProcess(Process.GetProcessById(Id)))
            {
                return new List<Process>() { Process.GetProcessById(Id) };
            }
            return null;
        }

        public List<Process> GetProcessesList(string Name)
        {
            var names = new HashSet<string>();
            List<Process> Processes = new List<Process>();
            foreach (var process in Process.GetProcessesByName(Name))
            {
                if (UsableProcess(process))
                {
                    names.Add(process.ProcessName);
                    Processes.Add(process);
                }
            }
            return Processes;
        }

        public List<Process> GetProcessesList()
        {
            var names = new HashSet<string>();
            List<Process> Processes = new List<Process>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.MainWindowHandle != process.Handle && process.ProcessName != "" && !names.Contains(process.ProcessName))
                    {
                        names.Add(process.ProcessName);
                        Processes.Add(process);
                    }
                }
                catch (Exception)
                {
                    // System process or access denied
                }
            }
            return Processes;
        }

        public void WriteProcesses(List<Process> Processes, int Index)
        {
            WriteHeader();
            for (var i=0; i<Processes.Count; ++i)
            {
                if (i == Index)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                WriteProcess(Processes[i]);
            }
        }

        public void TrackList()
        {
            WriteHeader();
            var names = new HashSet<string>();
            var processes = Process.GetProcesses();
            for (var i=0; i < processes.Length; ++i)
            {
                try
                {
                    if (processes[i].MainWindowHandle != processes[i].Handle && processes[i].ProcessName != "" && !names.Contains(processes[i].ProcessName))
                    {
                        names.Add(processes[i].ProcessName);
                        WriteProcess(processes[i]);
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
