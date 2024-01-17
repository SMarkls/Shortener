using LinkShortener.Application.Models.ShortenLinks.ViewModels;

namespace LinkShortener.Application.Work.ShortenLinks.Interfaces;

public interface IShortenLinksQueries
{
    Task<ShortenLinkVm> GetAsync(long id, string userId);
    Task<List<ShortenLinkVm>> GetList(string userId);
    Task<string> GetFullLink(string token);
}