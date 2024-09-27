namespace Api.Models;

public class PartyInfo
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // add prop to store signature byte[]
}