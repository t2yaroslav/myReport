using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyReport
{
    public enum Status
    {
        [EnumMember(Value = "0"), Display(Name = "in progress")] InProgress = 0,
        [EnumMember(Value = "1"), Display(Name = "fixed")] Fixed = 1,
        [EnumMember(Value = "2"), Display(Name = "completed")] Completed = 2,
    }
}