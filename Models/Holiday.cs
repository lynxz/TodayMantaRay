using System.ComponentModel.DataAnnotations;

namespace TodayMantaRay.Models;

public class Holiday : Event
{

    [Required]
    public Date? Start { get; set; }

    public Date? End { get; set; }

    public override string ToString(int year) =>
        $"It's {Name}";

}