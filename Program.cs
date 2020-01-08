using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MyReport
{
    static class Program
    {
        private const string Command = @"log --pretty=tformat:""%s"" --since=""{0}"" --committer=""{1}""";
        private const string NameCommiter = @"Jacob Kirkwood";
        private const string WorkingDirectory = @"C:\Projects\WPS";
        private const string Url = @"https://pts.bbconsult.co.uk/taskEditor?id=";
        private const string DescriptionTitlePrefix = "\t - ";
        private const string DescriptionPrefix = "\t\t - ";
        private const string GitPath = "git.exe";
        private const string FormatDate = "dd/MM/yyyy";
        private const string ReportPath = @"C:\Users\Admin\Desktop\Reports.txt";

        static void Main(string[] args)
        {
            var log = GetGitLog();
            var commits = log.Split("\n");
            var result = ParseCommits(commits);
            WriteToFile(result);
        }

        private static void WriteToFile(List<string> result)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(ReportPath, true);
            foreach (var item in result)
            {
                writer.WriteLine(item);
                Console.WriteLine(item);
            }

            writer.Close();
        }

        private static List<string> ParseCommits(string[] commits)
        {
            var lastNumber = "";
            var result = new List<string> {"\n" + DateTime.Now.ToString(FormatDate)};

            foreach (var commit in commits)
            {
                if (commit == "") continue;
                var words = commit.Split(@" ");
                var firstWord = words[0];
                if (firstWord == "Merge") continue;
                firstWord = firstWord.Replace("n","");

                var decryption = DescriptionPrefix +
                                 commit.Substring(firstWord.Length + 1, commit.Length - firstWord.Length - 1);
                if (lastNumber != firstWord)
                {
                    result.Add(DescriptionTitlePrefix + Url + firstWord);
                    result.Add(decryption);
                    lastNumber = firstWord;
                }
                else result.Add(decryption);
            }

            return result;
        }

        private static string GetGitLog()
        {
            var yesterday = DateTime.Now.AddDays(-1).ToString(FormatDate);
            var output = RunProcess(string.Format(Command, yesterday, NameCommiter));
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
                    FileName = GitPath,
                    Arguments = command,
                    WorkingDirectory = WorkingDirectory
                }
            };
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}