using JLStore.Domain.Services;
using JLStore.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JLStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(ICustomerService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> Get(int id)
        => (await service.GetAsync(id))
            is { } dto ?
            Ok(dto) :
            NotFound();

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Create([FromBody] CustomerCreateDto dto)
    {
        var id = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] CustomerUpdateDto dto)
        => await service.UpdateAsync(id, dto)
        ? NoContent() 
        : NotFound();

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
        => await service.DeleteAsync(id)
        ? NoContent() 
        : NotFound();
}
