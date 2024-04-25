using System.Reflection.Metadata.Ecma335;

namespace SimplifiedPayApi.Models;

public class User
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? IdentificationNumebr { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserType UserType { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Account>? Account { get; set; }
}
