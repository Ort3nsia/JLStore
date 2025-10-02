namespace JLStore.Domain.Models;

public class Customer
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
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
}