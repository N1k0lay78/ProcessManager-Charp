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
            pm.KillById(Convert.ToInt32(Console.ReadLine()));
            pm.KillByName(Console.ReadLine());
            pm.GetProcessName(Convert.ToInt32(Console.ReadLine()));
        }
    }
}
