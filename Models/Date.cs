using System.ComponentModel.DataAnnotations;

namespace TodayMantaRay.Models;

public class Date
{
    [Required]
    [Range(1, 12)]
    public int Month { get; set; }

    [Required]
    [Range(1, 31)]
    public int Day { get; set; }
}