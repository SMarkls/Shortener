using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateDto = LinkShortener.Application.Models.ShortenLinks.Dtos.CreateShortenLinkDto;
using UpdateDto = LinkShortener.Application.Models.ShortenLinks.Dtos.UpdateShortenLinkDto;
using GetVm = LinkShortener.Application.Models.ShortenLinks.ViewModels.ShortenLinkVm;

namespace LinkShortener.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ShortenLinkController
{
    private readonly IShortenLinksCommands commands;
    private readonly IShortenLinksQueries queries;

    public ShortenLinkController(IShortenLinksCommands commands, IShortenLinksQueries queries)
    {
        this.commands = commands;
        this.queries = queries;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<long> Create(CreateDto dto)
    {
        return await commands.CreateAsync(dto);
    }

    [HttpPut]
    [Route("[action]")]
    public async Task Update(UpdateDto dto)
    {
        await commands.UpdateAsync(dto);
    }

    [HttpDelete]
    [Route("[action]")]
    public async Task Delete(long id)
    {
        await commands.DeleteAsync(id);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<GetVm> Get(long id)
    {
        return await queries.GetAsync(id);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<List<GetVm>> GetList()
    {
        return await queries.GetList();
    }
}