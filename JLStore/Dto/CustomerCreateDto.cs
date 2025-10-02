using System.ComponentModel.DataAnnotations;

namespace JLStore.Dto;

/*I DTO CustomerCreateDto e CustomerUpdateDto sono uguali ma solo per il momento
* saranno estesi in futuro, così rispettano la SRP
*/
public class CustomerCreateDto()
{
    [Required, StringLength(50, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(50, MinimumLength = 1)]
    public string Surname { get; init; } = string.Empty;
}
