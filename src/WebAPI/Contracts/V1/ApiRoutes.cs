namespace WebAPI.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";


        public static class Products
        {
            public const string GetAll = Base + "/products";
            public const string GetById = Base + "/products/{id}";
            public const string Create = Base + "/products";
            public const string Update = Base + "/products";
            public const string Delete = Base + "/products/{id}";
        }
    }
}
