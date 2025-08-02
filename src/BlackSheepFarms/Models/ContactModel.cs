using System.ComponentModel.DataAnnotations;

namespace BlackSheepFarms;

public class ContactModel
{
    public bool IsComplete { get; set; }

    [EmailAddress]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string EmailAddress { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string Name { get; set; } = default!;

    [Phone]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string Phone { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(2048)]
    public string ServiceDescription { get; set; } = default!;
}