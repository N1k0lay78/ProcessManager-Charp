using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProcessManager
{
    public enum WindowsNum
    {
        Help = -1,
        Main = 0,
        Processes = 1,
        Process = 2,
        Kill = 3
    }

    public class GraphicConsole : IUserInterface
    {
        bool Running = true;
        int i = 0;
        int Window = (int)WindowsNum.Help;
        int Id;
        int status = 0;
        string Name;
        ProcessManager PM;
        List<Process> Processes;

        public GraphicConsole()
        {
            PM = new ProcessManager();
        }

        void KillWindow(int Action)
        {
            if (Action == 0)
            {
                i--;
            }
            else if (Action == 2)
            {
                i++;
            }
            else if (Action == 1 && status == 0)
            {
                if (i % 2 == 0)
                {
                    if (PM.KillById(Id))
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 2;
                    }
                }
                else
                {
                    status = 3;
                }
            }
            else if (Action == 1)
            {
                status = 0;
                ChangeWindow(true);
                return;
            }
            else if (Action == 3)
            {
                var kill = status == 1;
                status = 0;
                ChangeWindow(kill);
                return;
            }
            i = i % 2;
            if (status == 0)
            {
                Console.Write("U need to close process: ");
                if (i % 2 == 0)
                {
                    Console.WriteLine("Yes");
                }
                else
                {
                    Console.WriteLine("No");
                }
                Console.WriteLine("*Press Right if make choice");
                Console.WriteLine("*Press Up/Down to change choice");
            }
            else if (status == 1)
            {
                Console.WriteLine("Process killed successfully");
            }
            else if (status == 2)
            {
                Console.WriteLine("The process escaped being killed!!!!");
            }
            else
            {
                Console.WriteLine("Kill canseled");
            }
        }

        void ProcessWindow(int Action)
        {
            if (Action == 1)
            {
                ChangeWindow(true);
                return;
            }
            else if (Action == 3)
            {
                ChangeWindow(false);
                return;
            }
            if (i >= Processes.Count)
            {
                i = Processes.Count - 1;
            }
            PM.FullInfo(Id);
            Console.WriteLine("Press Right if need to close this process");
        }

        void ProcessesWindow(int Action)
        {
            if (Action == 0 && i > 0)
            {
                i--;
            }
            else if (Action == 2)
            {
                i++;
            }
            else if (Action == 1)
            {
                Id = Processes[i].Id;
                ChangeWindow(true);
                return;
            }
            else if (Action == 3)
            {
                ChangeWindow(false);
                return;
            }
            if (i >= Processes.Count)
            {
                i = Processes.Count - 1;
            }
            PM.WriteProcesses(Processes, i);
        }

        void HelpWindow(int Action)
        {
            if (Action == 1)
            {
                ChangeWindow(Action == 1);
                return;
            }
            Console.WriteLine("Help Page");
            Console.WriteLine("Press Q/Escape to close");
            Console.WriteLine("To controll on page use key up/down");
            Console.WriteLine("up - move up / down - move down");
            Console.WriteLine("To move between pages use key left/right");
            Console.WriteLine("left - back to previous page (update) / down - next page");
        }

        void MainWindow(int Action)
        {
            if (Action == 0 && i > 0)
            {
                i--;
            }
            else if (Action == 2)
            {
                i++;
            }
            else if (Action == 1)
            {
                Name = Processes[i].ProcessName;
                ChangeWindow(Action == 1);
                return;
            }
            else if (Action == 3)
            {
                UpdateListOfProcesses();
            }
            if (i >= Processes.Count)
            {
                i = Processes.Count - 1;
            }
            PM.WriteProcesses(Processes, i);
        }

        void Update(int Action)
        {
            Console.Clear();
            if (Action == 4) 
            {
                Running = false;
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }
            if (Window == (int)WindowsNum.Main)
            {
                MainWindow(Action);
            }
            else if (Window == (int)WindowsNum.Help)
            {
                HelpWindow(Action);
            }
            else if (Window == (int)WindowsNum.Processes)
            {
                ProcessesWindow(Action);
            }
            else if (Window == (int)WindowsNum.Process)
            {
                ProcessWindow(Action);
            }
            else if (Window == (int)WindowsNum.Kill)
            {
                KillWindow(Action);
            }
            if (Action == -1)
            {
                Console.Write('\a');
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void Run()
        {
            Update(-2);
            while (Running)
            {
                Update(GetAction(Console.ReadKey(true)));
            }
        }

        void ChangeWindow(bool isUp)
        {
            if (Window == (int)WindowsNum.Main && isUp)
            {
                Window = (int)WindowsNum.Processes;
                UpdateListOfProcesses(Name);
            }
            else if (Window == (int)WindowsNum.Main && !isUp)
            {
                Window = (int)WindowsNum.Help;
            }
            else if (Window == (int)WindowsNum.Processes && !isUp)
            {
                Window = (int)WindowsNum.Main;
                UpdateListOfProcesses();
            }
            else if (Window == (int)WindowsNum.Processes && isUp)
            {
                Window = (int)WindowsNum.Process;
                UpdateListOfProcesses();
            }
            else if (Window == (int)WindowsNum.Process && isUp)
            {
                Window = (int)WindowsNum.Kill;
                UpdateListOfProcesses();
            }
            else if (Window == (int)WindowsNum.Process && !isUp)
            {
                Window = (int)WindowsNum.Process;
                UpdateListOfProcesses();
            }
            else if (Window == (int)WindowsNum.Kill && isUp)
            {
                Window = (int)WindowsNum.Main;
                UpdateListOfProcesses();
            }
            else if (Window == (int)WindowsNum.Kill && !isUp)
            {
                Window = (int)WindowsNum.Process;
                UpdateListOfProcesses();
            }
            else if (Window == (int)WindowsNum.Help && isUp)
            {
                Window = (int)WindowsNum.Main;
                UpdateListOfProcesses();
            }
            i = 0;
            Update(-2);
        }

        void UpdateListOfProcesses()
        {
            Processes = PM.GetProcessesList();
        }

        void UpdateListOfProcesses(string Name)
        {
            Processes = PM.GetProcessesList(Name);
        }

        void UpdateListOfProcesses(int Id)
        {
            Processes = PM.GetProcessesList(Id);
        }

        int GetAction(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    return 4;
                case ConsoleKey.UpArrow:
                    return 0;
                case ConsoleKey.RightArrow:
                    return 1;
                case ConsoleKey.DownArrow:
                    return 2;
                case ConsoleKey.LeftArrow:
                    return 3;
                default:
                    return -1;
            }
        }
    }
}
 