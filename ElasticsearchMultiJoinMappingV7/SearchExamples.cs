﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticsearchMultiJoinMappingV7;

public static class SearchExamples
{
    public static async Task ProductsThatHaveMSINameAsync(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMSINameAsync)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Query(q => q
            .Match(m => m.Field(f => f.Name).Query("MSI")))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHavePrinceLessThan3200(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHavePrinceLessThan3200)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
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
        .Query(q => q
            .Wildcard(m => m.Field(f => f.Name).Value("*hone*")))
        .Size(20));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task ProductsThatHaveIdOneAndFour(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveIdOneAndFour)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
        .Index(_IndexName)
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
        .Query(q => q.HasChild<Supplier>(s => s.Query(k => k.MatchPhrase(a =>
                a.Field(field => field.SupplierDescription).Query("Komputronik")))
        ))
        .Size(10));

        PrintResult(fullTextSearchResponse);
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
}
