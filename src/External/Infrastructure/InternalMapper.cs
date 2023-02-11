using IInternalMapper = Application.Abstractions.IMapper;
using IMapster = MapsterMapper.IMapper;

namespace Infrastructure;

public class InternalMapper : IInternalMapper
{
    private readonly IMapster _mapper;

    public InternalMapper(IMapster mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
        => _mapper.Map<TSource, TDestination>(source);
}