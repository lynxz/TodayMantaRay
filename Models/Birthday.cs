using System.ComponentModel.DataAnnotations;

namespace TodayMantaRay.Models;

public class Birthday : Event
{

    [Required]
    public Date? Date { get; set; }

    [Required]
    public int BirthYear { get; set; }

    public override string ToString(int year) =>
        $"{Name} has a birthday and is turning {year - BirthYear}";

}
