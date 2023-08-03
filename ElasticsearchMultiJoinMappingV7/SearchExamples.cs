using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElasticsearchMultiJoinMappingV7;

public static class SearchExamples
{
    public static async Task ProductsThatHaveMSINameAsync(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMSINameAsync)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .Match(m => m.Field(f => f.Name).Query("MSI")))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveMSIOrIPhoneNameAsync(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMSIOrIPhoneNameAsync)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .Match(m => m.Field(f => f.Name).Query("MSI IPhone")))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveIPhoneAnd8NameAsync(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveIPhoneAnd8NameAsync)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .Match(m => m.Field(f => f.Name).Query("IPhone 8")
            .Operator(Operator.And)))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }


    public static async Task ProductsThatHavePrinceLessThan3200(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHavePrinceLessThan3200)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .LongRange(m => m.Field(f => f.Price).LessThan(3200)))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }



    public static async Task ProductsThatHaveWildCardhoneAsync(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveWildCardhoneAsync)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .Wildcard(m => m.Field(f => f.Name).Value("*?ne")))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveMultiMatchNVIDIA(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMultiMatchNVIDIA)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .MultiMatch(m =>
                m.Fields(
                    f => f.Field(f => f.Specyfication,3).Field(f => f.Description,2).Field(f => f.Name, 4)
                ).Query("NVIDIA")
                )
         )
        .Size(20));

        PrintResultWithScore(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveMultiMatchNVIDIAWithCategory(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMultiMatchNVIDIAWithCategory)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
            .MultiMatch(m =>
                m.Fields(
                    f => 
                    f.Field(f => f.Specyfication)
                    .Field(f => f.Description)
                    .Field(f => f.Name)
                ).Query("NVIDIA")
             )
            ||
            q.HasChild<Category>
            (
                (s => s.Query(k => k.Wildcard(
                    a => a.Field(field => field.CategoryDescription)
                    .Value("*NVIDIA*"))).Boost(1.2))
            )
         )
        .Size(20));

        PrintResultWithScore(fullTextSearchResponse);
    }

    public static async Task ProductThatHaveCategoryThatContainsWordNVIDIA(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductThatHaveCategoryThatContainsWordNVIDIA)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => 
            q.HasChild<Category>
                (s => s.Query(k => k.Wildcard(a => a.Field(field => field.CategoryDescription).Value("*NVIDIA*")))
        ))
        .Size(10));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveIdOneAndFour(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveIdOneAndFour)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q
        .Ids(c => c
            .Values(1, 4)
        )));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveAStockInCountryThatBeginsWithP(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveAStockInCountryThatBeginsWithP)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.HasChild<Stock>(s => s.Query(k => k.Wildcard(a => a.Field(field => field.Country).Value("P*")))
        ))
        .Size(10));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductThatHaveCategoryLaptops(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductThatHaveCategoryLaptops)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.HasChild<Category>(s => s.Query(k => k.MatchPhrase(a =>
                a.Field(field => field.CategoryDescription).Query("Laptops")))
        ))
        .Size(10));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductThatHaveSupplierKomputronik(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductThatHaveSupplierKomputronik)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.HasChild<Supplier>(s => s.Query(k => k.MatchPhrase(a =>
                a.Field(field => field.SupplierDescription).Query("Komputronik")))
        ))
        .Size(10));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductThatInnerHits(ElasticClient _ElasticClient,
        string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductThatInnerHits)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.HasChild<Supplier>
        (
            s => s.Query
            (k => k.MatchPhrase
                (a => a.Field(field => field.SupplierDescription).Query("X-KOM"))
            ).InnerHits()
        )
        ||
        q.HasChild<Category>
        (
            s => s.Query
            (k => k.MatchPhrase
                (a => a.Field(field => field.CategoryDescription).Query("Laptops"))
            ).InnerHits()
        )
        ||
        q.HasChild<Stock>
        (
            s => s.Query
            (k => k.MatchPhrase
                (a => a.Field(field => field.Country).Query("USA"))
            ).InnerHits()
        )
        )
        .Size(10));

        PrintInnerHits(fullTextSearchResponse);
    }

    public static async Task ProductsAndAllThierStocksInOneQuery(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMSINameAsync)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => 
            q.HasChild<Stock>
            (
                s => s.Query
                (k => !k.MatchPhrase(a => a.Field(field => field.Country).Query("!!!"))
                ).InnerHits()
            )
            ||
            q.Match(m => m.Field(f => f.Name).Query("MSI"))
        )
        .Size(20));

        PrintInnerHits(fullTextSearchResponse);
    }


    public static void PrintResult(ISearchResponse<Product> fullTextSearchResponse)
    {
        Console.WriteLine(fullTextSearchResponse.DebugInformation);

        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var data in fullTextSearchResponse.Documents)
        {
            Console.WriteLine($"{data.Name} {data.Price}");
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintResultWithScore(ISearchResponse<Product> fullTextSearchResponse)
    {
        Console.WriteLine(fullTextSearchResponse.DebugInformation);

        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var hit in fullTextSearchResponse.Hits)
        {
            Console.WriteLine($"{hit.Score} {hit.Source.Name} {hit.Source.Price}");
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintInnerHits(ISearchResponse<Product>
        fullTextSearchResponse)
    {
        Console.WriteLine(fullTextSearchResponse.DebugInformation);

        for (var i = 0; i < fullTextSearchResponse.Documents.Count; i++)
        {
            var doc = fullTextSearchResponse.Documents.ElementAt(i);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{doc.Name} {doc.Price}");

            var hit = fullTextSearchResponse.Hits.ElementAt(i);

            Console.ForegroundColor = ConsoleColor.Yellow;
            if (hit.InnerHits.ContainsKey("stock"))
            {
                var inner = hit.InnerHits["stock"];

                var stocks = inner.Documents<Stock>();

                foreach (var stock in stocks)
                {
                    Console.WriteLine($"{stock.Country}");
                }
            }

            if (hit.InnerHits.ContainsKey("category"))
            {
                var inner = hit.InnerHits["category"];

                var categroies = inner.Documents<Category>();

                foreach (var categroie in categroies)
                {
                    Console.WriteLine($"{categroie.CategoryDescription}");
                }
            }

            if (hit.InnerHits.ContainsKey("supplier"))
            {
                var inner = hit.InnerHits["supplier"];

                var suppliers = inner.Documents<Supplier>();

                foreach (var supplier in suppliers)
                {
                    Console.WriteLine($"{supplier.SupplierDescription}");
                }
            }
        }

        Console.ResetColor();
        Console.WriteLine("");
    }
}
