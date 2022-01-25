using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager
{
    public enum WindowsNum
    {
        Help = -1,
        Main = 0,
        Processes = 1,
        Process = 2,
    }

    public class GraphicConsole : IUserInterface
    {
        bool Running = true;
        int i = 0;
        int window = (int)WindowsNum.Help;
        int id = 0;

        public GraphicConsole() 
        {
            if (!Console.KeyAvailable)
            {
                Console.WriteLine("Sorry, your console does not support Console.Key");
                Console.ReadLine();
            }
        }

        void ProcessesWindow(int Action)
        {
            if (Action == 3)
            {
                ChangeWindow(Action == 1);
                return;
            }
            Console.WriteLine("Processes Page id = " + id);
        }

            void HelpWindow(int Action)
        {
            if (Action == 1)
            {
                ChangeWindow(Action == 1);
                return;
            }
            Console.WriteLine("Help Page");
            Console.WriteLine("To controll on page use key up/down");
            Console.WriteLine("up - move up / down - move down");
            Console.WriteLine("To move between pages use key left/right");
            Console.WriteLine("left - back to previous page / down - next page");
        }

        void MainWindow(int Action)
        {
            if (Action == 0 && i > 0)
            {
                i--;
            }
            else if (Action == 2 && i < 9)
            {
                i++;
            }
            else if (Action == 1 || Action == 3)
            {
                id = i;
                ChangeWindow(Action == 1);
                return;
            }
            for (var j=0; j<10; ++j)
            {
                if (j != i)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                }
                Console.WriteLine(j + " - Number of line");
            }
        }

        void Update(int Action)
        {
            Console.Clear();
            if (window == (int)WindowsNum.Main)
            {
                MainWindow(Action);
            }
            else if (window == (int)WindowsNum.Help)
            {
                HelpWindow(Action);
            }
            else if (window == (int)WindowsNum.Processes)
            {
                ProcessesWindow(Action);
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
            if (window == 0 && isUp)
            {
                window = (int)WindowsNum.Processes;
            }
            else if (window == 1 && !isUp)
            {
                window = (int)WindowsNum.Main;
            }
            else if (window == 0 && !isUp)
            {
                window = (int)WindowsNum.Help;
            }
            else if (window == -1 && isUp)
            {
                window = (int)WindowsNum.Main; // mainPage
            }
            Update(-2);
        }

        int GetAction(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    return 0;
                case ConsoleKey.RightArrow:
                    return 1;
                case ConsoleKey.DownArrow:
                    return 2;
                case ConsoleKey.LeftArrow:
                    if (window != 0)
                    {
                        return 3;
                    }
                    return -1;
                default:
                    return -1;
            }
        }
    }
}
 