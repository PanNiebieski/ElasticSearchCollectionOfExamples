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
        var pathS = Directory.GetCurrentDirectory() + @"/Data/Specyfications/";
        var pathD = Directory.GetCurrentDirectory() + @"/Data/Descriptions/";

        List<string> files = new() { "IPhone7.txt", "IPhone8.txt", "Modern.txt", "MSI1.txt", "MSI2.txt" , "Nvidia.txt", "GeForce.txt"};
        List<string> specs = new List<string>();
        List<string> descs = new List<string>();

        foreach (var file in files)
        {
            var text = File.ReadAllText(pathS + file);
            specs.Add(text);
            text = File.ReadAllText(pathD + file);
            descs.Add(text);
        }

        List<BaseDocument> products = new List<BaseDocument>()
        {
            new Product()
            {
                Id = 1,
                Name = "IPhone 7",
                Price = 100,
                Specyfication = specs[0],
                Description = descs[0],
                JoinField = "product",
            },

            new Product()
            {
                Id = 2,
                Name = "IPhone 8",
                Price = 100,
                Specyfication = specs[1],
                Description = descs[1],
                JoinField = "product"
            },
            new Product()
            {
                Id = 3,
                Name = "Modern 14 C11M-061PL i5-1155G7 / 8 GB / 512 GB / W11",
                Price = 5000,
                Specyfication = specs[2],
                Description = descs[2],
                JoinField = "product"
            },
            new Product()
            {
                Id = 4,
                Name = "MSI GF63 i5-11400H/8GB/512 RTX3050 144Hz",
                Price = 3000,
                Specyfication = specs[3],
                Description = descs[3],
                JoinField = "product"
            },
            new Product()
            {
                Id = 5,
                Name = "MSI GF63 Thin 11UC-215XPL / 16 GB RAM / 512 GB SSD PCIe",
                Price = 3400,
                Specyfication = specs[4],
                Description = descs[4],
                JoinField = "product"
            },
            new Product()
            {
                Id = 6,
                Name = "NVIDIA SHIELD TV PRO 2019",
                Price = 999,
                Specyfication = specs[5],
                Description = descs[5],
                JoinField = "product"
            },
            new Product()
            {
                Id = 7,
                Name = "ASUS GeForce GT 1030 SL 2GB GDDR5",
                Price = 549,
                Specyfication = specs[6],
                Description = descs[6],
                JoinField = "product"
            },

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
            new Stock()
            {
                Id = 306,
                Country= "Poland",
                JoinField = JoinField.Link("stock", 6),
                Parent = 6
            },
            new Stock()
            {
                Id = 307,
                Country= "Poland",
                JoinField = JoinField.Link("stock", 7),
                Parent = 7
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
            new Supplier()
            {
                Id = 107,
                SupplierDescription="X-KOM",
                Parent = 6,
                JoinField = JoinField.Link("supplier", 6)
            },
            new Supplier()
            {
                Id = 108,
                SupplierDescription="X-KOM",
                Parent = 7,
                JoinField = JoinField.Link("supplier", 7)
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
            new Category()
            {
                Id = 206,
                CategoryDescription= "Consoles",
                JoinField = JoinField.Link("category", 6),
                Parent = 6
            },
            new Category()
            {
                Id = 207,
                CategoryDescription= "Graphic Cards NVIDIA",
                JoinField = JoinField.Link("category", 7),
                Parent = 7
            },
        };

        return categoriees;
    }
}
