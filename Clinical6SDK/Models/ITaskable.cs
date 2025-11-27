using System;
namespace Clinical6SDK.Models
{
    public interface ITaskable
    {
        int? Id { get; set; }
        string Type { get; set; }
    }
}
