INTRODUCTION
============

The solutions in this repository illustrate how to attach the Visual Studio debugger programmatically to a running process.

Examples are provided for .NET Framework (v4.7.2) and .NET Core (v5.0). Changes required for similar or subsequent .NET versions should be minimal.

USAGE INSTRUCTIONS
==================

1. Open Visual Studio and load the solution for your desired .NET flavour (Framework or Core).

2. Ensure that Process1 is set as the (single) startup project.

3. Open Process1\Program.cs. Modify the following class constants:

- Visual Studio version. This is set for VS2019 (16.0) by default.
- Output subfolder (.NET Core only). This is "net5.0" by default.

4. Place a breakpoint within the 'while' loop in Process2\Program.cs.

5. Run the solution and follow on-screen instructions. Pressing any key (when prompted) in the Process1 console window should cause the breakpoint in Process2 to be hit.

<end>
