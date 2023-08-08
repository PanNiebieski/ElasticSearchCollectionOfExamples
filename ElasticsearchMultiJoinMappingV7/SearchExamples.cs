using ElasticSearch.DebugInformation;
using Nest;

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

    public static async Task AllProductsSortAscByPrice(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(AllProductsSortAscByPrice)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.MatchAll(k => k))
        .Sort(s => s.Ascending(f => f.Price)));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task AllProductsSortAscByPriceSize2Page2(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(AllProductsSortAscByPriceSize2Page2)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.MatchAll(k => k))
        .Size(2).From(2)
        .Sort(s => s.Ascending(f => f.Price)));

        PrintResult(fullTextSearchResponse);
    }

    public static async Task AllProductsSortAscByPriceTrack(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(AllProductsSortAscByPriceTrack)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Excludes(k => k.Field(f => f.Description).Field(f => f.Specyfication)))
        .Query(q => q.Match(k => k.Query("")))
        .Sort(s => s.Ascending(f => f.Price))
        .TrackScores(true));

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
                    f => f.Field(f => f.Specyfication, 3).Field(f => f.Description, 2).Field(f => f.Name, 4)
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

    public static async Task ProductsThatHaveMultiMatchNVIDIAWithCategoryWithExplain(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(ProductsThatHaveMultiMatchNVIDIAWithCategoryWithExplain)}\n");

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
        .Explain(true));

        PrintResultWithScoreAndExplation(fullTextSearchResponse);
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
        .Query(q => q.HasChild<Category>(s => s.Query(k => k.Terms(a =>
                a.Field(field => field.CategoryDescription).Terms("Laptops")))
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
        .Query(q => q.HasChild<Supplier>(s => s.Query(k => k.Terms(a =>
                a.Field(field => field.SupplierDescription).Terms("Komputronik")))
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

    public static async Task CategoryHasParent(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(CategoryHasParent)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Category>(s => s
        .Index(_IndexName)
        .Query
        (q => q.HasParent<Product>(s => s.Query(k => k.Ids(k => k.Values(1, 2, 3, 4, 5, 6, 7))))
        &&
        q.Exists(k => k.Field(f => f.CategoryDescription)
        )));

        PrintCategoryResult(fullTextSearchResponse);
    }

    public static async Task Category2HasParent(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(Category2HasParent)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<Category>(s => s
        .Index(_IndexName)
        .Query
        (q => q.Term(f => f.Field(f => f.Parent).Value(1))
        &&
        q.Exists(k => k.Field(f => f.CategoryDescription)
        )));

        PrintCategoryResult(fullTextSearchResponse);
    }

    public static async Task SupplierCategoryStockHasParent(ElasticClient _ElasticClient, string _IndexName)
    {
        Console.WriteLine($"\n{nameof(SupplierCategoryStockHasParent)}\n");

        var fullTextSearchResponse = await _ElasticClient.SearchAsync<dynamic>(s => s
        .Index(_IndexName)
        .Query(q => q.HasParent<Product>(s => s.Query(k => k.Ids(k => k.Values(1, 2, 3, 4, 5, 6, 7))))
        ));

        PrintCategoryStocSupplierResult(fullTextSearchResponse);
    }

    public static async Task ProductsAndAllThierStocksInOneQuery(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsAndAllThierStocksInOneQuery)}\n");

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

    public static async Task ProductsWithSpecBezdotykowegoAndSystemAndroid(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsWithSpecBezdotykowegoAndSystemAndroid)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Includes(k => k.Field(f => f.Name).Field(f => f.Specyfication)))
        .Query(q => q
            .MatchPhrase(m => m.Field(f => f.Specyfication)
            .Query("Bez dotykowego").Slop(1)
        )
        ||
         q.MatchPhrase(m => m.Field(f => f.Specyfication)
            .Query("System Android").Slop(1)
        )));

        PrintSpecResult(fullTextSearchResponse, "Bez dotykowego", "System Android");
    }

    public static async Task ProductsWithMinimumShouldMatch(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsWithMinimumShouldMatch)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Includes(k => k.Field(f => f.Name).Field(f => f.Specyfication)))
        .Query(q => q
            .Match(m => m.Field(f => f.Specyfication)
            .Query("NVIDIA GeForce iPhone").MinimumShouldMatch(2)
        )
        ));

        PrintSpecResult(fullTextSearchResponse, "NVIDIA GeForce", "NVIDIA Tegra");
    }

    public static async Task ProductsWithFuzzinessNVIDA(ElasticClient elasticClient, string indexName)
    {
        Console.WriteLine($"\n{nameof(ProductsWithFuzzinessNVIDA)}\n");

        var fullTextSearchResponse = await elasticClient.SearchAsync<Product>(s => s
        .Index(indexName)
        .Source(s => s.Includes(k => k.Field(f => f.Name).Field(f => f.Price)))
        .Query(q => q
            .Match(m => m.Field(f => f.Specyfication)
            .Query("Obsługa")
            .Fuzziness(Fuzziness.EditDistance(1))
        )
        ));

        PrintResult(fullTextSearchResponse);
    }

    public static void PrintResult(ISearchResponse<Product> fullTextSearchResponse)
    {
        ESDebug.ConsoleWriteWithoutResponse
            (fullTextSearchResponse.DebugInformation);

        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var data in fullTextSearchResponse.Documents)
        {
            if (data.Name == null)
                continue;

            Console.WriteLine($"{data.Name} {data.Price}");
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    //public static void PrintCategoryStocSupplierResult(ISearchResponse<dynamic> fullTextSearchResponse)
    //{
    //    Console.WriteLine(fullTextSearchResponse.DebugInformation);

    //    for (var i = 0; i < fullTextSearchResponse.Hits.Count; i++)
    //    {
    //        var child = fullTextSearchResponse.Hits.ElementAt(i);

    //        if (child is Category cat)
    //        {
    //            Console.WriteLine($"{cat.CategoryDescription}");
    //        }
    //        else if (child is Stock stoc)
    //        {
    //            Console.WriteLine($"{stoc.Country} {stoc.Quantity}");
    //        }
    //        else if (child is Supplier supp)
    //        {
    //            Console.WriteLine($"{supp.SupplierDescription}");
    //        }
    //    }

    //    Console.ResetColor();
    //    Console.WriteLine("");
    //}

    public static void PrintCategoryStocSupplierResult(ISearchResponse<dynamic> fullTextSearchResponse)
    {
        ESDebug.ConsoleWriteAll
            (fullTextSearchResponse.DebugInformation);

        foreach (var child in fullTextSearchResponse.Documents)
        {
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintCategoryResult(ISearchResponse<Category> fullTextSearchResponse)
    {
        ESDebug.ConsoleWriteWithoutResponse
            (fullTextSearchResponse.DebugInformation);

        foreach (var cat in fullTextSearchResponse.Documents)
        {
            Console.WriteLine(cat.CategoryDescription);
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintSpecResult(ISearchResponse<Product> fullTextSearchResponse, params string[] pa)
    {
        ESDebug.ConsoleWriteWithoutResponse
            (fullTextSearchResponse.DebugInformation);

        foreach (var data in fullTextSearchResponse.Documents)
        {
            if (data.Name == null)
                continue;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{data.Name}");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var item in pa)
            {
                var subitem = item.Split(" ").Last();

                int index = data.Specyfication.LastIndexOf(subitem);

                if (index != -1)
                {
                    var s = data.Specyfication.Substring(index - Convert.ToInt32(index * 0.1));

                    var s2 = s.Substring(0, s.Length / 4);

                    Console.WriteLine($"{s2}");
                }
            }
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintResultWithScore(ISearchResponse<Product> fullTextSearchResponse)
    {
        ESDebug.ConsoleWriteWithoutResponse
            (fullTextSearchResponse.DebugInformation);

        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var hit in fullTextSearchResponse.Hits)
        {
            Console.WriteLine($"{hit.Score} {hit.Source.Name} {hit.Source.Price}");
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintResultWithScoreAndExplation(ISearchResponse<Product> fullTextSearchResponse)
    {
        ESDebug.ConsoleWriteWithoutResponse
            (fullTextSearchResponse.DebugInformation);

        foreach (var hit in fullTextSearchResponse.Hits)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{hit.Score} {hit.Source.Name} {hit.Source.Price}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{hit.Explanation.Description} {hit.Explanation.Value}");

            foreach (var item in hit.Explanation.Details)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{item.Description} {item.Value}");

                foreach (var aa in item.Details)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{aa.Description} {aa.Value}");

                    foreach (var bb in aa.Details)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{bb.Description} {bb.Value}");

                        foreach (var cc in bb.Details)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine($"{cc.Description} {cc.Value}");

                            foreach (var dd in cc.Details)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"{dd.Description} {dd.Value}");
                            }
                        }
                    }
                }
            }
        }

        Console.ResetColor();
        Console.WriteLine("");
    }

    public static void PrintInnerHits(ISearchResponse<Product>
        fullTextSearchResponse)
    {
        ESDebug.ConsoleWriteWithoutResponse
            (fullTextSearchResponse.DebugInformation);

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