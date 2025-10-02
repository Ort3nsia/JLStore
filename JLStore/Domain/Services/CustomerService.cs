using AutoMapper;
using JLStore.Domain.Repositories;
using JLStore.Dto;

namespace JLStore.Domain.Services;

public class CustomerService(ICustomerRepository repo,
    IMapper mapper) : ICustomerService
{
    public async Task<CustomerDto?> GetAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id);
        return entity is null ?
            null :
            mapper.Map<CustomerDto>(entity);
    }

    public async Task<int> CreateAsync(CustomerCreateDto dto)
    {
        var entity = mapper.Map<Models.Customer>(dto);
        await repo.AddAsync(entity);
        await repo.SaveChangesAsync();
        return entity.ID;
    }

    public async Task<bool> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return false;

        mapper.Map(dto, entity);

        await repo.UpdateAsync(entity);
        return await repo.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id);
        if (entity is null) return false;

        await repo.DeleteAsync(entity);
        return await repo.SaveChangesAsync();
    }
}
