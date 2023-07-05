using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;

public partial class Response
{
    [JsonProperty("products")]
    public List<Product> Products { get; set; }

    [JsonProperty("next_page_info")]
    public string NextPageUrl { get; set; }
}
public class Product
{
    [JsonProperty("id")]
    public long Id { get; set; }
    [JsonProperty("variants")]
    public List<Variant> Variants { get; set; }
    [JsonProperty("handle")]
    public string Handle { get; set; }
}

public class Variant
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string Sku { get; set; }
}

public class Program
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main()
    {
        string baseURL = "";
        string apiUrl = $"{baseURL}/admin/api/2023-01/products.json?published_status=any&limit=250";
        string accessToken = "";
        int timeoutSeconds = 10;

        string csvFilePath = "products.csv";
        string downloadPath = GetDownloadPath();
        string fullPath = Path.Combine(downloadPath, csvFilePath);

        List<Product> products = GetAllProductsFromShopify(apiUrl, accessToken, timeoutSeconds)?.Result?.ToList() ?? new List<Product>();
        if (products == null)
        {
            Console.WriteLine("Failed to retrieve products from Shopify.");
            return;
        }

        List<Product> filteredProducts = FilterProductsByHandle(products);
        DataTable dataTable = ConvertProductsToDataTable(filteredProducts);
        SaveDataTableToCsv(dataTable, fullPath);

        Console.WriteLine("Products imported and saved to CSV.");
    }

    public static string GetDownloadPath()
    {
        string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string downloadPath = Path.Combine(userProfilePath, "Downloads");
        return downloadPath;
    }

    public static async Task<List<Product>> GetAllProductsFromShopify(string apiUrl, string accessToken, int timeoutSeconds)
    {
        List<Product> allProducts = new List<Product>();
        string nextPageUrl = apiUrl;

        do
        {
            Response response = await GetProductsFromShopify(nextPageUrl, accessToken, timeoutSeconds);
            if (response == null)
            {
                Console.WriteLine("Failed to retrieve products from Shopify.");
                return null;
            }

            allProducts.AddRange(response.Products);
            nextPageUrl = response.NextPageUrl;
        } while (!string.IsNullOrEmpty(nextPageUrl));

        return allProducts;
    }

    public static async Task<Response> GetProductsFromShopify(string apiUrl, string accessToken, int timeoutSeconds)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", accessToken);
            httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseString = response.Content.ReadAsStringAsync().Result;
                var responseObject = JsonConvert.DeserializeObject<Response>(responseString);

                // Extract the "link" header from the response headers
                if (response.Headers.TryGetValues("link", out var linkHeaders))
                {
                    var linkHeader = linkHeaders.FirstOrDefault();
                    if (linkHeader != null)
                    {
                        // Extract the URL for the next page from the "link" header
                        var regex = new Regex("<(.*?)>;\\s*rel=\"next\"");
                        var match = regex.Match(linkHeader);
                        if (match.Success)
                        {
                            var nextPageUrl = match.Groups[1].Value;
                            // URL-decode the next page URL
                            nextPageUrl = Uri.UnescapeDataString(nextPageUrl);
                            responseObject.NextPageUrl = nextPageUrl;
                        }
                    }
                }

                return responseObject;
            }
            catch { return null; }
        }
    }


    public static List<Product> FilterProductsByHandle(List<Product> products)
    {
        if (products == null)
        {
            return new List<Product>();
        }

        List<Product> filteredProducts = new List<Product>();

        foreach (Product product in products)
        {
            if (!string.IsNullOrEmpty(product.Handle) && values.Any(value => string.Equals(value, product.Handle, StringComparison.OrdinalIgnoreCase)))
            {
                filteredProducts.Add(new Product { Id = product.Id });
            }
        }

        return filteredProducts;
    }


    public static DataTable ConvertProductsToDataTable(List<Product> products)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Id");

        if (products != null)
        {
            foreach (Product product in products)
            {
                dataTable.Rows.Add(product.Id);
            }
        }

        return dataTable;
    }

    public static void SaveDataTableToCsv(DataTable dataTable, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                writer.Write(column.ColumnName);
                writer.Write(",");
            }
            writer.WriteLine();

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    writer.Write(item.ToString());
                    writer.Write(",");
                }
                writer.WriteLine();
            }
        }
    }

    public static List<string> values = new List<string>()
{
    "W928101066",
    "W928101065",
    "W928101005",
    "TH9001LSJ-8",
    "K-4012-NS",
    "K-4012-MB",
    "K-4012-BG",
    "TH4003MB",
    "TH4003NS",
    "TH2806W",
    "TH94027MB02",
    "TH94026NS02",
    "TH94026MB02",
    "TH9013LSJ",
    "TH9013MB",
    "TH9013NS",
    "TH9001LSJ",
    "TH9001MB",
    "TH9001NS",
    "TH8020NS",
    "TH8020MB",
    "Item Sku",
    "W138164227",
    "W92864164",
    "W92864163",
    "W127263094",
    "W122562738",
    "W122562735",
    "W122552781",
    "W122552138",
    "W92852027",
    "W92851685",
    "W92850585",
    "W92850260",
    "W92850257",
    "W92850252",
    "W92850238",
    "W92850236",
    "W92850227",
    "W92850211",
    "W92850157",
    "W108347967",
    "W123243770",
    "W122543477",
    "TH94027MB02-8",
    "TH94026NS02-8",
    "TH94026MB02-8",
    "JYBB41202BG",
    "TH-8806D-MB",
    "TH-8806D-BG",
    "TH9013NS-8",
    "TH9001NS-8",
    "TH9001MB-8",
    "W928100995",
    "W928100989",
    "W928100986",
    "W928100985",
    "W928100960",
    "W928100959",
    "W122581051",
    "W92877189",
    "W92869107",
    "W105668269",
    "W92867780",
    "W108366782",
    "W138164919",
    "W138164917",
    "W138164916",
    "W138164228",
    "TH-4001NS9",
    "TH-4001NS8",
    "TH4006--BG",
    "TH4006-NS",
    "TH4006-MB",
    "TH-4006WH",
    "TH94027NS02",
    "20S05101MBL",
    "TH8041CH",
    "TH8041NS",
    "TH8041MB",
    "THQ7006TMB",
    "THSP002NS",
    "THSP002CH",
    "THSP002MB",
    "THSP002BG",
    "TH-4014MB02",
    "TH8046NS",
    "THQ7006TCP",
    "TH4026NS01",
    "TH-4026MB01",
    "TH4003MB02",
    "TH-4003LSJ",
    "TH-8045MB",
    "TH-8045LSJ",
    "TH15016NS",
    "TH4001MB",
    "THQ7006NS",
   "TH8046MB",
    "TH8046BG",
    "W54331545",
    "W928101067"
};

}
