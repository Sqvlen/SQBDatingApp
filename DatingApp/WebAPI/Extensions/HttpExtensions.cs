using System.Text.Json;
using WebAPI.Helpers;

namespace WebAPI.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse httpResponse, PaginationHeader paginationHeader)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            httpResponse.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
