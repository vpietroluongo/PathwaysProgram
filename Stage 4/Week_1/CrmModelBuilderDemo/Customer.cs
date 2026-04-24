using System.Formats.Asn1;

public partial class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    //public string Phone { get; set; } = string.Empty;
    public PhoneNumber Phone { get; set; } = new PhoneNumber();
    public DateTime DateOfBirth { get; set; }
    public CustomerStatus Status { get; set; }
    public decimal LifetimeValue { get; set; }

    // Navigation property
    public List<Interaction> Interactions { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
}