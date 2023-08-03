using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticsearchEnglishLanguageASPCore.Controllers;

public class SearchHController : Controller
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<SearchHController> _logger;

    public SearchHController(
        IElasticClient elasticClient,
        ILogger<SearchHController> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string q)
    {
        if (string.IsNullOrEmpty(q))
        {
            var noResultsVM = new SearchViewModel { Term = "[No Search]" };
            return View(noResultsVM);
        }

        // Good article if you want to read an overview of the types of
        // queries available: https://qbox.io/blog/elasticsearch-queries-match-phrase-match
        // Specific usage for MultiMatch: https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/multi-match-usage.html
        var response = await _elasticClient.SearchAsync<Book>(s =>
            s.Query(sq =>
                sq.MultiMatch(mm => mm
                    .Query(q)
                    .Fuzziness(Fuzziness.Auto)
                )
            )
            .Highlight
            (k => k.Fields
                (fs => fs.Field(f => f.Opening))
            .PreTags("<span class=\"red\">").PostTags("</span>")
            .NumberOfFragments(0)
            .Encoder(HighlighterEncoder.Html)
        ));

        var vm = new SearchViewModel
        {
            Term = q
        };

        if (response.IsValid)
        {
            List<Book> books = new List<Book>();

            for (int i = 0; i < response.Hits.Count(); i++)
            {
                var hit = response.Hits.ElementAt(i);
                var book = hit.Source;

                foreach (var item in hit.Highlight)
                {
                    if (item.Key == "opening")
                    {
                        book.Opening = string.Join("",item.Value);


                    }
                    //if (item.Key == "title")
                    //{
                    //    book.Title = string.Join("", item.Value);
                    //}
                }

                books.Add(book);
            }

            vm.Results = books;

        }
        else
            _logger.LogError(response.OriginalException, "Problem searching Elasticsearch for term {0}", q);

        return View(vm);
    }
}
