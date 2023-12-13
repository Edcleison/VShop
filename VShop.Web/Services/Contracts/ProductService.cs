using System.Text;
using System.Text.Json;
using VShop.Web.Models;

namespace VShop.Web.Services.Contracts
{
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

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            var client = _httpClientFactory.CreateClient("ProductApi");

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
        public async Task<ProductViewModel> FindProductById(int id)
        {
            var client = _httpClientFactory.CreateClient("ProductApi");

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

        public async Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel)
        {
            var client = _httpClientFactory.CreateClient("ProductApi");

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

    }
    public Task<ProductViewModel> UpdateProduct(ProductViewModel productViewModel)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductById(int id)
    {
        throw new NotImplementedException();
    }

}
}
