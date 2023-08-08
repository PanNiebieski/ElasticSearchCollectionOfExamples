using ElasticSearch.DebugInformation;
using ElasticsearchMultiJoinMappingV7;
using Nest;
using static ElasticsearchMultiJoinMappingV7.CreatorOfIndexes;
using static ElasticsearchMultiJoinMappingV7.SearchExamples;
using static ElasticsearchMultiJoinMappingV7.StaticCollections;

var eslasticClient = new ElasticClient(GetConnection());
CreatorOfIndexes._ElasticClient = eslasticClient;

CreateIndexAndMappings(numberOfReplicas: 0, numberOfShards: 5, refreshInterval: -1);

IndexDocuments(GetProducts());
IndexChildDocuments(GetStocks());
IndexChildDocuments(GetSuppliers());
IndexChildDocuments(GetCategoriees());

await AllProductsSortAscByPrice(_ElasticClient, _IndexName);
await AllProductsSortAscByPriceTrack(_ElasticClient, _IndexName);
await AllProductsSortAscByPriceSize2Page2(_ElasticClient, _IndexName);

await ProductsThatHaveMSINameAsync(_ElasticClient, _IndexName);
await ProductsThatHaveMSIOrIPhoneNameAsync(_ElasticClient, _IndexName);
await ProductsThatHaveIPhoneAnd8NameAsync(_ElasticClient, _IndexName);
await ProductsThatHavePrinceLessThan3200(_ElasticClient, _IndexName);
await ProductsThatHaveWildCardhoneAsync(_ElasticClient, _IndexName);
await ProductsThatHaveIdOneAndFour(_ElasticClient, _IndexName);

await ProductsThatHaveMultiMatchNVIDIA(_ElasticClient, _IndexName);
await ProductThatHaveCategoryThatContainsWordNVIDIA(_ElasticClient, _IndexName);
await ProductsThatHaveMultiMatchNVIDIAWithCategory(_ElasticClient, _IndexName);
await ProductsThatHaveMultiMatchNVIDIAWithCategoryWithExplain(_ElasticClient, _IndexName);

await ProductsThatHaveAStockInCountryThatBeginsWithP(_ElasticClient, _IndexName);
await ProductThatHaveCategoryLaptops(_ElasticClient, _IndexName);
await ProductThatHaveSupplierKomputronik(_ElasticClient, _IndexName);
await ProductThatInnerHits(_ElasticClient, _IndexName);
await ProductsAndAllThierStocksInOneQuery(_ElasticClient, _IndexName);

await CategoryHasParent(_ElasticClient, _IndexName);
await Category2HasParent(_ElasticClient, _IndexName);

await SupplierCategoryStockHasParent(_ElasticClient, _IndexName);

await ProductsWithSpecBezdotykowegoAndSystemAndroid(_ElasticClient, _IndexName);

await ProductsWithMinimumShouldMatch(_ElasticClient, _IndexName);

await ProductsWithFuzzinessNVIDA(_ElasticClient, _IndexName);

var t = ESDebug.Structurize("EST #Resposne #Reqeust");
var t2 = ESDebug.Structurize("EST # Request:\n{} # Response:\n{}");

Console.WriteLine();