using System.ComponentModel.DataAnnotations;

namespace TodayMantaRay.Models;

public abstract class Event
{

    [Required]
    public string? Name { get; set; }

    public abstract string ToString(int year);
}
