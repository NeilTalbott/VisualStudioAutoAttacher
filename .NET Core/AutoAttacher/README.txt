INTRODUCTION
============

The solutions in this repository illustrate how to attach the Visual Studio debugger programmatically to a running process.

Examples are provided for .NET Framework (v4.7.2) and .NET Core (v5.0). Changes required for similar or subsequent .NET versions should be minimal.

This repository was inspired by the StackOverflow question at the following link:

https://stackoverflow.com/questions/72022151/can-i-programmatically-attach-a-running-net-6-process-to-a-running-instance-of/72075450?noredirect=1#comment127405081_72075450

If this code helps you, please consider upvoting both the question and answer so that it becomes easier to find for other developers with a similar problem.

USAGE INSTRUCTIONS
==================

1. Ensure you have exactly one instance of Visual Studio running. (See comments in code for further details.)

2. In Visual Studio, load the solution for your desired .NET flavour (Framework or Core).

3. Ensure that Process1 is set as the (single) startup project.

4. Open Process1\Program.cs. Modify the following class constants:

    - Visual Studio version. This is set for VS2019 (16.0) by default.
    - Output subfolder (.NET Core only). This is "net5.0" by default.

5. Place a breakpoint within the 'while' loop in Process2\Program.cs.

6. Run the solution and follow on-screen instructions. Pressing any key (when prompted) in the Process1 console window should cause the breakpoint in Process2 to be hit.

<end>
