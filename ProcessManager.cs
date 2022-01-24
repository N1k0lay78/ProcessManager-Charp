using System;
using System.Diagnostics;

namespace ProcessManager
{
    interface IProcessManager
    {
        public bool KillById(int Id);
        public bool KillByName(string Name);
        public bool TrackList();
        public bool ProcessId(string Name);
        public bool ProcessName(int Id);
    }

    public class ProcessManager : IProcessManager
    {
        public ProcessManager() { }

        public bool KillById(int Id)
        {
            throw new NotImplementedException();
        }

        public bool KillByName(string Name)
        {
            throw new NotImplementedException();
        }

        public bool ProcessId(string Name)
        {
            throw new NotImplementedException();
        }

        public bool ProcessName(int Id)
        {
            throw new NotImplementedException();
        }

        public bool TrackList()
        {
            var proceses = Process.GetProcesses();
            Console.WriteLine($"| {"Id",7} | {"Name",-25} | {"Size",-10} | {"Start",-8} | {"Threads",-10} | {"Priority",-10} | {"Alive",-10} |");
            foreach (var proces in proceses)
            {
                if (proces.MainWindowTitle != "")
                {
                    var time = "";
                    if (proces.StartTime.Hour < 10)
                    {
                        time += '0';
                    }
                    time += proces.StartTime.Hour + ":";
                    if (proces.StartTime.Minute < 10)
                    {
                        time += '0';
                    }
                    time += proces.StartTime.Minute + ":";
                    if (proces.StartTime.Second < 10)
                    {
                        time += '0';
                    }
                    time += proces.StartTime.Second;
                    Console.WriteLine($"| {proces.Id,7} | {proces.ProcessName,-25} | {(proces.PeakWorkingSet64 / 1024 / 1024) + " MB",10} | {time,8} | {proces.Threads.Count,-10} | {proces.PriorityClass,-10} | {proces.Responding,-10} |");
                }
            }
            return true;
        }
    }
}
