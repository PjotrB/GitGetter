using GitGetter.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GitGetter.GitExe.Helpers
{
    internal static class OS
    {
        internal static string[] RunProgram(string ExeName, string Params, string WorkingDir, IErrorReporter Reporter)
        {
            Reporter.ClearError();
            var startInfo = new ProcessStartInfo
            {
                FileName = ExeName,
                Arguments = Params,
                WorkingDirectory = WorkingDir,
                UseShellExecute = false,
                RedirectStandardError = true,   // required for reading from StandardError below
                RedirectStandardOutput = true,  // required for reading from StandardOutput below 
                CreateNoWindow = true
            };
            using (var proc = new Process { StartInfo = startInfo })
            {
                try
                {
                    proc.Start();

                    var stdOutput = new List<string>();
                    while (proc.StandardOutput.Peek() > -1)
                        stdOutput.Add(proc.StandardOutput.ReadLine());

                    var stdError = new List<string>();
                    while (proc.StandardError.Peek() > -1)
                        stdError.Add(proc.StandardError.ReadLine());

                    // If we still get a hang in above code, try this: https://stackoverflow.com/a/7608823/1220550

                    proc.WaitForExit();

                    if (proc.StandardOutput.Peek() > -1) stdError.Add("StandardOutput has content AFTER we thought we read it all");
                    if (proc.StandardError.Peek() > -1) stdError.Add("StandardError has content AFTER we thought we read it all");

                    proc.Close();

                    if (stdError.Count > 0)
                        Reporter.ShowError("Error output from '" + ExeName + "':\r\n" + string.Join("\r\n", stdError));

                    return stdOutput.ToArray();
                }
                catch (Exception ex)
                {
                    Reporter.ShowError("Exception from running '" + ExeName + "':\r\n" + ex.Message);
                    return Array.Empty<string>();
                }
            }
        }
    }
}