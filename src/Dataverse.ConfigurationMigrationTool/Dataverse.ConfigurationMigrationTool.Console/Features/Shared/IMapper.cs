namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared
{
    public interface IMapper<in TSource, out TDestination>
    {
        TDestination Map(TSource source);
    }
}
