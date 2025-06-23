Ecommerce Demo Project
This is a proof-of-concept e-commerce backend system built with:

.NET 9 with ASP.NET Core for the API

Azure Functions (Isolated) for product and category import

Entity Framework Core for database interaction

Redis cache (mocked) for product/category data

Swagger UI with API Key authentication for testing

How It Works
API
Exposes product data via two endpoints:

GET /products – Paginated product listing with optional search and category filters.

GET /products/{id} – Single product detail by ID.

GET /products/by-category – All products in a category.

Importer Functions
Reads local JSON files (/data/products.json, /data/categories.json) and imports them into SQL via Entity Framework.

Uses ETag hash comparison to avoid unnecessary updates.

(Mocked) Redis cache is updated or invalidated based on data changes to simulate cache sync.

How to Run Locally
Make sure you have:

.NET 9 SDK

SQL Server or SQL Edge (local or cloud)

Visual Studio 2022+ or VS Code

Update your local.settings.json and appsettings.Development.json with your SQL connection string.

Use API Key: 123test