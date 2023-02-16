using Application.Abstractions.Cache;
using Application.Extensions;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Infrastructure.Services.Cache;

public class AuthorCacheDecorator : IAuthorRepository
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ICacheProvider _cacheProvider;

    public AuthorCacheDecorator(IAuthorRepository authorRepository, ICacheProvider cacheProvider)
    {
        _authorRepository = authorRepository;
        _cacheProvider = cacheProvider;
    }

    public Task<AuthorDto[]> GetAll(CancellationToken ct)
    {
        var cacheKey = $"author:{nameof(GetAll)}";
        return _cacheProvider.GetOrSet(
            cacheKey,
            () => _authorRepository.GetAll(ct), ct);
    }

    public Task<AuthorDto?> GetById(Guid authorId, CancellationToken ct)
    {
        var cacheKey = $"author:{nameof(GetById)}:{authorId}";
        return _cacheProvider.GetOrSet(
            cacheKey,
            () => _authorRepository.GetById(authorId, ct), ct);
    }

    public async Task<Guid> Insert(AuthorDto author)
    {
        var authorId = await _authorRepository.Insert(author);
        
        var byIdCacheKey = $"author:{nameof(GetById)}:{authorId}";
        await _cacheProvider.Delete(byIdCacheKey);
        var getAllCacheKey = $"author:{nameof(GetAll)}";
        await _cacheProvider.Delete(getAllCacheKey);

        return authorId;
    }

    public async Task<bool> Delete(Guid authorId)
    {
        var success = await _authorRepository.Delete(authorId);
        if (success)
        {
            var cacheKey = $"author:{nameof(GetById)}:{authorId}";
            await _cacheProvider.Delete(cacheKey);
            
            var getAllCacheKey = $"author:{nameof(GetAll)}";
            await _cacheProvider.Delete(getAllCacheKey);
        }
        
        return success;
    }
}