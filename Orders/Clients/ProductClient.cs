using Orders.Entities;

namespace Orders.Clients
{
    public class ProductClient
    {
        private readonly HttpClient httpClient;

        public ProductClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<ProductDto>> GetCatalogItemsAsync()
        {
            var product = await httpClient.GetFromJsonAsync<IReadOnlyCollection<ProductDto>>("/items");
            if (product != null)
            {
                return product;
            }


            return new List<ProductDto>();

        }
    }
}