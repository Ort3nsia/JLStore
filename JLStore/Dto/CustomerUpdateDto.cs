using System.ComponentModel.DataAnnotations;

namespace JLStore.Dto;

public class CustomerUpdateDto
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 1)]
    public string Surname { get; init; } = string.Empty;
}