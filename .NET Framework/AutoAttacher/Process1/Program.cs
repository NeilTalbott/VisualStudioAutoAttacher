namespace Process1
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    
    using EnvDTE80;

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

            Console.WriteLine("Process2 is now running. Press any key to attach:");
            Console.ReadKey();

            var thisVisualStudio = GetCurrentVisualStudioInstance();
            AttachDebugger(thisVisualStudio, Process2Name);

            Console.WriteLine();
            Console.WriteLine("Press any key to kill Process2 and exit:");
            Console.ReadKey();

            if (!process2.HasExited)
            {
                process2.Kill();
            }
        }

        private static DTE2 GetCurrentVisualStudioInstance()
        {
            // Note: "16.0" is for Visual Studio 2019.
            var dte2 = (DTE2)Marshal.GetActiveObject("VisualStudio.DTE.16.0");
            return dte2;
        }

        private static void AttachDebugger(DTE2 dte, string processName)
        {
            var processes = dte.Debugger.LocalProcesses;

            // Note: Consider using an exact match instead of using .IndexOf()
            foreach (var proc in processes.Cast<EnvDTE.Process>().Where(proc => proc.Name.IndexOf(processName) != -1))
            {
                proc.Attach();
            }
        }
    }
}
