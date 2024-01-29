using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string apiEndpoint = "/api/products/";
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private ProductViewModel productViewModel;
    private IEnumerable<ProductViewModel> productsVM;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts(string token)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                productsVM = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        return productsVM;
    }



    public async Task<ProductViewModel> FindProductById(int id, string token)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productViewModel = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        return productViewModel;
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel, string token)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthotization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(productViewModel), Encoding.UTF8, "application/json");
        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productViewModel = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        return productViewModel;
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productViewModel, string token)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");
        ProductViewModel productUpdated = new ProductViewModel();
        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.PutAsJsonAsync(apiEndpoint, productViewModel))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productUpdated = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null;
            }
        }
        return productUpdated;

    }

    public async Task<bool> DeleteProductById(int id, string token)
    {
        var client = _httpClientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthotization(token, client);

        using (var response = await client.DeleteAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }
    private static void PutTokenInHeaderAuthotization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

}
