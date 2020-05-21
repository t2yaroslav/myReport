using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MyReport
{
    static class Program
    {
        // git settings
        private const string Command = @"log --pretty=tformat:""%s"" --since=""{0}"" --committer=""{1}""";
        private const string Period = "1 day ago";
        private const string NameCommiter = @"Jacob Kirkwood";
        private const string WorkingDirectory = @"C:\Projects\WPS";
        private const string GitPath = "git.exe";
        private const string FormatDate = "dd/MM/yyyy";
        private const string ReportPath = @"C:\Users\Admin\Desktop\Reports.txt";

        // format settings
        private const string TitleReport = "Daily report ";
        private const string TitlePrefix = "    - ";
        private const string DescriptionPrefix = "        - ";
        private const string TaskSimbol = "n";
        private const string MergeStr = "Merge";
        private const string FixStr = "fix";

        static readonly List<Task> Tasks = new List<Task>();

        static void Main(string[] args)
        {
            var log = GetGitLog();
            var commits = log.Split("\n");

            ParseCommits(commits);

            var lines = new List<string> {"\n" + TitleReport + DateTime.Now.ToString(FormatDate)};
            foreach (var task in Tasks)
            {
                lines.Add(TitlePrefix + task.Url + " - " + task.Status.GetDisplayName());
                lines.AddRange(task.Items.Select(item => DescriptionPrefix + item));
            }
            WriteToFile(lines);
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

        private static void ParseCommits(string[] commits)
        {
            foreach (var commit in commits.Reverse())
            {
                if (commit == "") continue;
                var words = commit.Split(@" ");
                var numberTask = words[0];
                if (numberTask == MergeStr) continue;
                var decryption = commit.Substring(numberTask.Length + 1, commit.Length - numberTask.Length - 1);

                var status = numberTask.Contains(TaskSimbol) ?Status.InProgress : Status.Completed;
                numberTask = numberTask.Replace(TaskSimbol, "");

                AddItemToTask(numberTask, decryption, status);
            }
        }

        private static void AddItemToTask(string numberTask, string decryption, Status status)
        {
            var index = Tasks.FindIndex(t => t.Number == numberTask);
            if (index == -1)
            {
                Tasks.Add(new Task() {Number = numberTask});
                index = Tasks.FindIndex(t => t.Number == numberTask);
            }

            if (Tasks[index].Items.Count == 1 && Tasks[index].Items.First().StartsWith(FixStr)) status = Status.Fixed;

            Tasks[index].Status = status;
            Tasks[index].Items.Add(decryption);
        }

        private static string GetGitLog()
        {
            var output = RunProcess(string.Format(Command, Period, NameCommiter));
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