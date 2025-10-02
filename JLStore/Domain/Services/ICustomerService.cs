using JLStore.Dto;

namespace JLStore.Domain.Services;

public interface ICustomerService
{
    Task<CustomerDto?> GetAsync(int id);
    Task<int>  CreateAsync(CustomerCreateDto dto);
    Task<bool> UpdateAsync(int id, CustomerUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}