using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MyReport
{
    class Program
    {
        static string command = @"log --pretty=tformat:""%s"" --since=""{0}"" --committer=""jak""";
        static string workingDirectory = @"C:\Projects\WPS";
        static string url = @"https://pts.bbconsult.co.uk/taskEditor?id=";
        static string descriptionPrefix = "\t\t - ";


        static void Main(string[] args)
        {
            var log = GetGitLog();
            var commits = log.Split("\n");
            var result = ParseCommits(commits);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        private static List<string> ParseCommits(string[] commits)
        {
            var lastNumber = "";
            var result = new List<string>();

            foreach (var commit in commits)
            {
                if (commit == "") continue;
                var words = commit.Split(" ");
                var decryption = descriptionPrefix +
                                 commit.Substring(words[0].Length + 1, commit.Length - words[0].Length - 1);

                if (lastNumber != words[0])
                {
                    result.Add(url + words[0]);
                    result.Add(decryption);
                    lastNumber = words[0];
                }
                else result.Add(decryption);
            }

            return result;
        }

        private static string GetGitLog()
        {
            var yesterday = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            var output = RunProcess(string.Format(command, yesterday));
            return output;
        }

        private static string RunProcess(string command)
        {
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "git.exe",
                    Arguments = command,
                    WorkingDirectory = workingDirectory
                }
            };
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}