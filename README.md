# Shopify Product Import

This project is a simple console application that allows you to export products from a Shopify store and save them to a CSV file. The exported products are filtered based on their handles, and only the desired products will be included in the CSV file.

## Prerequisites

Before running the application, make sure you have the following prerequisites:

- .NET Framework 4.7.2 or later
- Newtonsoft.Json package (can be installed via NuGet)

## Usage

1. Open the `Program.cs` file in your preferred IDE.
2. Replace the placeholders with your actual values:

   - `baseURL`: The base URL of your Shopify store.
   - `accessToken`: Your Shopify access token.
   - `timeoutSeconds`: The timeout value in seconds for the HTTP requests.
   - `csvFilePath`: The name of the CSV file to be generated.
   - `values`: The list of product handles to be filtered.

3. Build and run the application.

The application will retrieve products from your Shopify store, filter them based on the provided handles, convert them to a DataTable, and save them to a CSV file.

## Notes

- The application uses the Newtonsoft.Json package to deserialize the JSON response from Shopify.
- The HTTP requests to Shopify are made using the HttpClient class.
- The filtering of products is done based on the product handles provided in the `values` list.
- The generated CSV file will be saved in the user's Downloads folder.

Please ensure that you have a stable internet connection and valid credentials for accessing your Shopify store before running the application.

For further information or assistance, please contact the project maintainer.
