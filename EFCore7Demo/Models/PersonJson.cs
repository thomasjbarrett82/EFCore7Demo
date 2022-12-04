namespace EFCore7Demo.Models;

public class PersonJson {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<AddressJson> Addresses { get; set; }
}
