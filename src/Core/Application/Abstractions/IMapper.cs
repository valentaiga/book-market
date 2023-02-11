namespace Application.Abstractions;

public interface IMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
}