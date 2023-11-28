using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceWatcher.Implementations.Models;

public class ItemWatcher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? LastCheckAt { get; set; }

    [InverseProperty(nameof(ItemWatcherLink.ItemWatcher))]
    public virtual ICollection<ItemWatcherLink> Links { get; set; } = new HashSet<ItemWatcherLink>();
}
