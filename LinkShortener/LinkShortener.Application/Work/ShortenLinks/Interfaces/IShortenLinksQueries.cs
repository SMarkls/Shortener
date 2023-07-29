using LinkShortener.Application.Models.ShortenLinks.ViewModels;

namespace LinkShortener.Application.Work.ShortenLinks.Interfaces;

public interface IShortenLinksQueries
{
    Task<ShortenLinkVm> GetAsync(long id);
    Task<List<ShortenLinkVm>> GetList();

    Task<string> GetFullLink(string token);
}