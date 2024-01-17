using System.Reflection;
using AutoMapper;

namespace LinkShortener.Application.Common.Mappings;

// TODO: Maybe delete?
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(this, Assembly.GetExecutingAssembly());
    }

    public static void ApplyMappingsFromAssembly(Profile profile, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t != typeof(IMapFrom<>) && t != typeof(IMapTo<>) && t.GetInterfaces().Contains(typeof(IMapped)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping")
                             ?? type.GetInterface("IMapped")?.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { profile });
        }
    }
}