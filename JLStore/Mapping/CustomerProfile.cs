using AutoMapper;
using JLStore.Domain.Models;
using JLStore.Dto;

namespace JLStore.Mapping;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        // Customer -> CustomerDto (Da DB a stampa)
        CreateMap<Customer, CustomerDto>();

        // CustomerCreateDto -> Customer (Da input utente a DB)
        CreateMap<CustomerCreateDto, Customer>()
            .ConstructUsing(s => new Customer(s.Name, s.Surname));

        CreateMap<CustomerUpdateDto, Customer>()
            .AfterMap((s, d) =>
            {
                d.UpdateFullName(s.Name, s.Surname);
            });
    }
}
