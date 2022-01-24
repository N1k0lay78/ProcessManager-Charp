using System;

namespace ProcessManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var pm = new ProcessManager();
            pm.TrackList();
            pm.GetProcessId(Console.ReadLine());
        }
    }
}
