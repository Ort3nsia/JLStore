using JLStore.Helpers;

namespace JLStore.Domain.Models;

public class Customer
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string FiscalCode => GenerateFiscalCode();

    private Customer() { }

    public Customer(string Name, string Surname)
    {
        UpdateFullName(Name, Surname);
    }

    public void UpdateFullName(string Name, string Surname)
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
            throw new ArgumentException("Name and Surname are required!");
        this.Name = Name;
        this.Surname = Surname;
    }

    private string GenerateFiscalCode()
    {
        var Name = StringHelpers.RemoveVowels(this.Name);
        var Surname = StringHelpers.RemoveVowels(this.Surname);
        Name = Name.Length > 3
            ? Name.Substring(0, 3).ToUpper()
            : Name.ToUpper();
        Surname = Surname.Length > 3
            ? Surname.Substring(0, 3).ToUpper()
            : Surname.ToUpper();

        return $"{Name}{Surname}";
    }
}