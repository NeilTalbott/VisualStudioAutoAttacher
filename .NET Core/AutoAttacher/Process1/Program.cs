namespace Process1
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security;
    using EnvDTE80;

    class Program
    {
        // Note: "16.0" is for Visual Studio 2019. This value need to be edited for other versions.
        private const string VisualStudioVersion = "VisualStudio.DTE.16.0";

        private const string Process2Name = "Process2";

        static void Main(string[] args)
        {
            // Note: This code assumes no instances of Process2 are running at startup.
            //  Since Process2 is started programmatically, it will not terminate if 'Stop' is pressed in Visual Studio.
            var process2 = InitialiseProcess2();

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

        private static Process InitialiseProcess2()
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
            return process2;
        }

        private static DTE2 GetCurrentVisualStudioInstance()
        {
            var dte2 = (DTE2)Marshal.GetActiveObject(VisualStudioVersion);

            // Note: this demo requires exactly one instance of Visual Studio to be open.
            // If you need to identify one amongst several instances, some options are described here:
            // https://stackoverflow.com/questions/13432057/how-to-use-marshal-getactiveobject-to-get-2-instance-of-of-a-running-process-t
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

        // This method has been copied from Microsoft's source code here: https://github.com/microsoft/referencesource/blob/5697c29004a34d80acdaf5742d7e699022c64ecd/mscorlib/system/runtime/interopservices/marshal.cs#L2404
        public static class Marshal2
        {
            internal const String OLEAUT32 = "oleaut32.dll";
            internal const String OLE32 = "ole32.dll";

            [System.Security.SecurityCritical]  // auto-generated_required
            public static Object GetActiveObject(String progID)
            {
                Object obj = null;
                Guid clsid;

                // Call CLSIDFromProgIDEx first then fall back on CLSIDFromProgID if
                // CLSIDFromProgIDEx doesn't exist.
                try
                {
                    CLSIDFromProgIDEx(progID, out clsid);
                }
                //            catch
                catch (Exception)
                {
                    CLSIDFromProgID(progID, out clsid);
                }

                GetActiveObject(ref clsid, IntPtr.Zero, out obj);
                return obj;
            }

            //[DllImport(Microsoft.Win32.Win32Native.OLE32, PreserveSig = false)]
            [DllImport(OLE32, PreserveSig = false)]
            [ResourceExposure(ResourceScope.None)]
            [SuppressUnmanagedCodeSecurity]
            [System.Security.SecurityCritical]  // auto-generated
            private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] String progId, out Guid clsid);

            //[DllImport(Microsoft.Win32.Win32Native.OLE32, PreserveSig = false)]
            [DllImport(OLE32, PreserveSig = false)]
            [ResourceExposure(ResourceScope.None)]
            [SuppressUnmanagedCodeSecurity]
            [System.Security.SecurityCritical]  // auto-generated
            private static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] String progId, out Guid clsid);

            //[DllImport(Microsoft.Win32.Win32Native.OLEAUT32, PreserveSig = false)]
            [DllImport(OLEAUT32, PreserveSig = false)]
            [ResourceExposure(ResourceScope.None)]
            [SuppressUnmanagedCodeSecurity]
            [System.Security.SecurityCritical]  // auto-generated
            private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out Object ppunk);
        }
    }
}
