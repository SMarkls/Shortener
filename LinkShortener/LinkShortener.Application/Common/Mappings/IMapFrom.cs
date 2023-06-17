using AutoMapper;

namespace LinkShortener.Application.Common.Mappings;

public interface IMapFrom<T> : IMapped
{
    void IMapped.Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}