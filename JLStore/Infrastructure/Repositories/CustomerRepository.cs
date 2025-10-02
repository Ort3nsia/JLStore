using JLStore.Domain.Models;
using JLStore.Domain.Repositories;
using JLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JLStore.Infrastructure.Repositories;

public class CustomerRepository(DataContext context) : ICustomerRepository
{

    public async Task AddAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);
    }

    public Task DeleteAsync(Customer customer)
    {
        context.Customers.Remove(customer);
        return Task.CompletedTask;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await context.Customers.FirstOrDefaultAsync(c => c.ID == id);
    }

    public Task UpdateAsync(Customer customer)
    {
        context.Customers.Update(customer);
        return Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync()
    {
        // Restituisce true se almeno una riga è stata interessata, altrimenti false.
        return await context.SaveChangesAsync() > 0; 
    }
}