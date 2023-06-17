using AutoMapper;

namespace LinkShortener.Application.Common.Mappings;

public interface IMapTo<T> : IMapped
{
    void IMapped.Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
}