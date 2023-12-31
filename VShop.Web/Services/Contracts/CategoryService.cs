﻿using System.Net.Http;
using System.Text.Json;
using VShop.Web.Models;

namespace VShop.Web.Services.Contracts
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _options;
        private string apiEndpoint = "/api/categories/";

        public CategoryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            var client = _clientFactory.CreateClient("ProductApi");

            IEnumerable<CategoryViewModel> categories;

            var response = await client.GetAsync(apiEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return categories;
        }
    }
}
