using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticsearchMultiJoinMappingV7;

public static class StaticCollections
{
    public static List<BaseDocument> GetProducts()
    {
        List<BaseDocument> products = new List<BaseDocument>()
        {
            new Product()
            {
                Id = 1,
                Name = "IPhone 7",
                Price = 100,
                JoinField = "product",
            },

            new Product()
            {
                Id = 2,
                Name = "IPhone 8",
                Price = 100,
                JoinField = "product"
            },
            new Product()
            {
                Id = 3,
                Name = "Modern 14 C11M-061PL i5-1155G7 / 8 GB / 512 GB / W11",
                Price = 5000,
                JoinField = "product"
            },
            new Product()
            {
                Id = 4,
                Name = "MSI GF63 Thin 11UC-215XPL",
                Price = 3000,
                JoinField = "product"
            },
            new Product()
            {
                Id = 5,
                Name = "MSI GF63 Thin 11UC-215XPL / 16 GB RAM / 512 GB SSD PCIe",
                Price = 3400,
                JoinField = "product"
            }
        };

        return products;
    }

    public static  List<BaseDocument> GetStocks()
    {
        var stocks = new List<BaseDocument>()
        {
            new Stock()
            {
                Id = 300,
                Country="USA",
                JoinField = JoinField.Link("stock", 1),
                Parent = 1
            },

            new Stock()
            {
                Id = 301,
                Country="UK",
                JoinField = JoinField.Link("stock", 2),
                Parent = 2
            },

            new Stock()
            {
                Id = 302,
                Country="Germany",
                JoinField = JoinField.Link("stock", 2),
                Parent = 3
            },
            new Stock()
            {
                Id = 303,
                Country= "Poland",
                JoinField = JoinField.Link("stock", 3),
                Parent = 3

            },
            new Stock()
            {
                Id = 304,
                Country= "Poland",
                JoinField = JoinField.Link("stock", 4),
                Parent = 4
            },
            new Stock()
            {
                Id = 305,
                Country= "Poland",
                JoinField = JoinField.Link("stock", 5),
                Parent = 5
            },
        };

        return stocks;
    }

    public static List<BaseDocument> GetSuppliers()
    {
        var suppliers = new List<BaseDocument>()
        {
            new Supplier()
            {
                Id = 100,
                SupplierDescription="Apple",
                Parent = 1,
                JoinField = JoinField.Link("supplier", 1),
            },

            new Supplier()
            {
                Id = 101,
                SupplierDescription="A supplier",
                Parent = 1,
                JoinField = JoinField.Link("supplier", 1)
            },

            new Supplier()
            {
                Id = 102,
                SupplierDescription="Another supplier",
                Parent = 2,
                JoinField = JoinField.Link("supplier", 2)
            },
            new Supplier()
            {
                Id = 103,
                SupplierDescription="X-KOM",
                Parent = 3,
                JoinField = JoinField.Link("supplier", 3)
            },
            new Supplier()
            {
                Id = 104,
                SupplierDescription="Komputronik",
                Parent = 3,
                JoinField = JoinField.Link("supplier", 3)
            },
            new Supplier()
            {
                Id = 105,
                SupplierDescription="X-KOM",
                Parent = 4,
                JoinField = JoinField.Link("supplier", 4)
            },
            new Supplier()
            {
                Id = 106,
                SupplierDescription="X-KOM",
                Parent = 5,
                JoinField = JoinField.Link("supplier", 5)
            },
        };

        return suppliers;
    }


    public static List<BaseDocument> GetCategoriees()
    {

        var categoriees = new List<BaseDocument>()
        {
            new Category()
            {
                Id = 200,
                CategoryDescription= "Electronic",
                JoinField = JoinField.Link("category", 1),
                Parent = 1
            },

            new Category()
            {
                Id = 201,
                CategoryDescription = "Smart Phone",
                JoinField = JoinField.Link("category", 2),
                Parent = 2
            },

            new Category()
            {
                Id = 202,
                CategoryDescription = "Phone",
                JoinField = JoinField.Link("category", 2),
                Parent = 2
            },
            new Category()
            {
                Id = 203,
                CategoryDescription= "Laptops",
                JoinField = JoinField.Link("category", 3),
                Parent = 3
            },
            new Category()
            {
                Id = 204,
                CategoryDescription= "Laptops",
                JoinField = JoinField.Link("category", 4),
                Parent = 4
            },
            new Category()
            {
                Id = 205,
                CategoryDescription= "Laptops",
                JoinField = JoinField.Link("category", 5),
                Parent = 5
            },
        };

        return categoriees;
    }
}
