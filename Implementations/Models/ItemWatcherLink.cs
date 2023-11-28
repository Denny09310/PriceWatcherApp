using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceWatcher.Implementations.Models;

public class ItemWatcherLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = default!;

    [Required]
    public string Link { get; set; } = null!;

    public T ItemWatcherId { get; set; } = default!;

    [ForeignKey(nameof(ItemWatcherId))]
    public virtual ItemWatcher ItemWatcher { get; set; } = default!;
}
