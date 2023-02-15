using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Books.Commands.CreateBook;

namespace BookMarket.Tests;

public static class ApiRequestBuilder
{
    public static class Book
    {
        private const string Prefix = "api/books";
        public static HttpRequestMessage Get(Guid bookId) => new (HttpMethod.Get, $"{Prefix}/{bookId}");
        public static HttpRequestMessage GetAll() => new (HttpMethod.Get, $"{Prefix}");
        public static HttpRequestMessage Create(CreateBookRequest req) 
            => new HttpRequestMessage(HttpMethod.Post, $"{Prefix}").With(req);
        public static HttpRequestMessage Delete(Guid bookId) 
            => new HttpRequestMessage(HttpMethod.Delete, $"{Prefix}/{bookId}");
    }

    private static HttpRequestMessage With<TContent>(this HttpRequestMessage req, TContent data)
    {
        req.Content = JsonContent.Create(data, new MediaTypeHeaderValue("application/json"));
        return req;
    }
}