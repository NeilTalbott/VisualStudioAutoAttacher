using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process1
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    class Program
    {
        private const string Process2Name = "Process2";

        static void Main(string[] args)
        {
            var currentFolder = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var solutionFolder = new DirectoryInfo(currentFolder).Parent.Parent.Parent;
            var process2Folder = Path.Combine(solutionFolder.FullName, Process2Name, "bin", "Debug");

            var process2Executable = Path.Combine(process2Folder, $"{Process2Name}.exe");

            if (!File.Exists(process2Executable))
            {
                throw new ApplicationException("Executable file for Process2 not found");
            }

            var process2Info = new ProcessStartInfo(process2Executable);
            var process2 = Process.Start(process2Info);

            Console.WriteLine("Process2 is now running. Press any key to attach");
            Console.ReadKey();

            // ToDo: Attach.

            if (!process2.HasExited)
            {
                process2.Kill();
            }
        }
    }
}
