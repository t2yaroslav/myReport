using System.Collections.Generic;

namespace MyReport
{
    public class Task
    {
        private const string BaseUrl = @"https://pts.bbconsult.co.uk/taskEditor?id=";

        public string Url
        {
            get { return BaseUrl + Number; }
        }

        public string Number { get; set; }
        public List<string> Items = new List<string>();
        public Status Status { get; set; }
    }
}