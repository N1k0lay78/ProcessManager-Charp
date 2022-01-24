using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager
{
    public class UI
    {
        bool running = true;
        public UI()
        {
            var pm = new ProcessManager();

        }

        string[] ask_comand()
        {
            Console.Write("command> ");
            var ans = Console.ReadLine();
            return ans.Split();
        }

        void update()
        {
            var command = ask_comand();
            switch (command[0].ToLower())
            {
                case "quit":
                    running = false;
                    break;
                case "help":
                    Console.WriteLine("\tQuit              - Выйти из приложения");
                    Console.WriteLine("\tKill <process>    - Убить процесс (name/id)");
                    Console.WriteLine("\tTaskList          - Список выполняющихся задач");
                    Console.WriteLine("\tProcess <process> - Показать процессы (name/id)");
                    Console.WriteLine("\tInfo <id>         - Полную информацию о процессе");
                    break;
                case "kill":
                    if (command.Length > 1)
                    {
                        Console.WriteLine($"*kill process {command[1]}*");
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'process' (name/id)");
                    }
                    break;
                case "tasklist":
                    Console.WriteLine($"*taskList of processes*");
                    break;
                case "process":
                    if (command.Length > 1)
                    {
                        Console.WriteLine($"*processes {command[1]}*");
                    }
                            else
                    {
                        Console.WriteLine("missing required attribute 'process' (name/id)");
                    }
                    break;
                case "info":
                    if (command.Length > 1)
                    {
                        Console.WriteLine($"*info about process id +{command[1]}*");
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'id' (id)");
                    }
                    break;
                default:
                    Console.WriteLine("write help to get help");
                    break;
            }
        }

        public void run()
        {
            while (running)
            {
                update();
            }
        }
    }
}
