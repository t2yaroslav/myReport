using System;
using System.Diagnostics;
using System.IO;

namespace MyReport
{
    class Program
    {
        static string command = @"log --pretty=tformat:""%s"" --since=""{0}"" --committer=""jak""";
        static string workingDirectory = @"C:\Projects\WPS";


        static void Main(string[] args)
        {
            var result = GetGitLog();
            var commits = result.Split("\n");

            var lastNumber = "";
            foreach (var commit in commits)
            {
                if (commit == "") continue;
                var words = commit.Split(" ");
                if (lastNumber != words[0])
                {
                    Console.WriteLine("https://pts.bbconsult.co.uk/taskEditor?id=" + words[0]);
                    Console.WriteLine(commit);
                    lastNumber = words[0];
                }
                else Console.WriteLine(commit);
            }
        }

        public static string GetGitLog()
        {
            var yestoday = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            var output = RunProcess(string.Format(command, yestoday));
            return output;
        }

        private static string RunProcess(string command)
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "git.exe";
            p.StartInfo.Arguments = command;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.Start();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}