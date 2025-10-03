using AutoMapper;
using JLStore.Domain.Models;
using JLStore.Dto;

namespace JLStore.Mapping;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {

        // CustomerCreateDto -> Customer (Da input utente a DB)
        CreateMap<CustomerCreateDto, Customer>()
            .ConstructUsing(s => new Customer(s.Name, s.Surname));

        // Mappa Customer -> CustomerDto (Da DB a DTO di lettura)
        CreateMap<Customer, CustomerDto>().ForMember(
            dest => dest.FiscalCode,
            opt => opt.MapFrom(src => src.FiscalCode)
        );

        // Mappa CustomerUpdateDto -> Customer (Aggiornamento dei dati utente)
        CreateMap<CustomerUpdateDto, Customer>()
            .AfterMap((s, d) =>
            {
                d.UpdateFullName(s.Name, s.Surname);
            });

    }
}
