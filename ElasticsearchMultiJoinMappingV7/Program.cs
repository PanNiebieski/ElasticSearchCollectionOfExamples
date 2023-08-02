
using ElasticsearchMultiJoinMappingV7;
using static ElasticsearchMultiJoinMappingV7.StaticCollections;
using static ElasticsearchMultiJoinMappingV7.SearchExamples;
using static ElasticsearchMultiJoinMappingV7.CreatorOfIndexes;
using Nest;

var eslasticClient = new ElasticClient(GetConnection());
CreatorOfIndexes._ElasticClient = eslasticClient;

CreateIndexAndMappings(numberOfReplicas: 0, numberOfShards: 5, refreshInterval: -1);

IndexDocuments(GetProducts());
IndexChildDocuments(GetStocks());
IndexChildDocuments(GetSuppliers());
IndexChildDocuments(GetCategoriees());

await ProductsThatHaveMSINameAsync(_ElasticClient, _IndexName);
await ProductsThatHavePrinceLessThan3200(_ElasticClient, _IndexName);
await ProductsThatHaveWildCardhoneAsync(_ElasticClient, _IndexName);
await ProductsThatHaveIdOneAndFour(_ElasticClient, _IndexName);
await ProductsThatHaveAStockInCountryThatBeginsWithP(_ElasticClient, _IndexName);
await ProductThatHaveCategoryLaptops(_ElasticClient, _IndexName);
await ProductThatHaveSupplierKomputronik(_ElasticClient, _IndexName);

await ProductThatInnerHits(_ElasticClient, _IndexName);

await ProductsAndAllThierStocksInOneQuery(_ElasticClient, _IndexName);
