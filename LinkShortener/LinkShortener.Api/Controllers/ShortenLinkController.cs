﻿using System.Security.Claims;
using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateDto = LinkShortener.Application.Models.ShortenLinks.Dtos.CreateShortenLinkDto;
using UpdateDto = LinkShortener.Application.Models.ShortenLinks.Dtos.UpdateShortenLinkDto;
using GetVm = LinkShortener.Application.Models.ShortenLinks.ViewModels.ShortenLinkVm;

namespace LinkShortener.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ShortenLinkController : Controller
{
    private readonly IShortenLinksCommands commands;
    private readonly IShortenLinksQueries queries;
    private readonly IConfiguration configuration;

    public ShortenLinkController(IShortenLinksCommands commands, IShortenLinksQueries queries, IConfiguration configuration)
    {
        this.commands = commands;
        this.queries = queries;
        this.configuration = configuration;
    }

    [HttpPost]
    public async Task<string> Create(CreateDto dto)
    {
        var userIdClaim = GetUserIdClaim();
        return await commands.CreateAsync(dto, userIdClaim?.Value ?? "");
    }

    [HttpPut]
    [Authorize]
    public async Task Update(UpdateDto dto)
    {
        var userIdClaim = GetUserIdClaim();
        if (userIdClaim is null)
        {
            throw new Exception("401. Unauthorized");
        }

        await commands.UpdateAsync(dto, userIdClaim.Value);
    }

    [HttpDelete]
    [Authorize]
    public async Task Delete(long id)
    {
        var userIdClaim = GetUserIdClaim();
        if (userIdClaim is null)
        {
            throw new Exception("401. Unauthorized");
        }
        await commands.DeleteAsync(id, userIdClaim.Value);
    }

    [HttpGet]
    [Authorize]
    public async Task<GetVm> Get(long id)
    {
        var userIdClaim = GetUserIdClaim();
        if (userIdClaim is null)
        {
            throw new Exception("401. Unauthorized");
        }

        return await queries.GetAsync(id, userIdClaim.Value);
    }

    [HttpGet]
    [Authorize]
    public async Task<List<GetVm>> GetList()
    {
        var userIdClaim = GetUserIdClaim();
        if (userIdClaim is null)
        {
            return Enumerable.Empty<GetVm>().ToList();
        }

        return await queries.GetList(userIdClaim.Value);
    }

    [HttpGet]
    public async Task<string> GetFullLink(string token)
    {
        return await queries.GetFullLink(token);
    }

    private Claim? GetUserIdClaim()
    {
        return User.Claims.FirstOrDefault(x => x.Type == configuration["Jwt:UserIdPropName"]);
    }
}