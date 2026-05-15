using System.ComponentModel.DataAnnotations;

namespace TodoMvcApp.Models;

public class TodoItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    [Display(Name = "Due date")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [Display(Name = "Completed")]
    public bool IsDone { get; set; }
}
