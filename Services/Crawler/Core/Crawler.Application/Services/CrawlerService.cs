
using System;
using System.Text.Json;
using System.Xml;
using Crawler.Application.Constants;
using Crawler.Application.Events;
using Crawler.Application.IServices;
using Crawler.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.Extensions.Logging;


namespace Crawler.Application.Services
{
    public class CrawlerService : ICrawlerService
    {
       
        private readonly CrawlerSettings _settings;
        private readonly INewsPublisher<NewFetchedEventDto> _publisher;
        private readonly ILogger<CrawlerService> _logger;
        public CrawlerService(IOptions<CrawlerSettings> settings, INewsPublisher<NewFetchedEventDto> publisher)
        {
            _settings = settings.Value;
            _publisher = publisher;
        }

        public async Task FetchNewAsync()
        {
            try
            {

                var list = new List<NewFetchedEventDto>();
             using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GotoAsync($"{_settings.BaseUrl}", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.Load
            });

            // Dinamik sayfa sonuna scroll yaparak içerik yükle
            await ScrollToBottomAsync(page, maxScrolls: 20, delayMs: 1000);

            var sectionsLocator = page.Locator("ht-section[data-section]");
            int sectionCount = await sectionsLocator.CountAsync();




           for (int i = 0; i < sectionCount; i++)
            {
                var section = sectionsLocator.Nth(i);
                var category = await section.GetAttributeAsync("data-section");

                var linksLocator = section.Locator("a");


                int linksCount = await linksLocator.CountAsync();

                for (int j = 0; j < linksCount; j++)
                {
                    var link = linksLocator.Nth(j);
                    var href = await link.GetAttributeAsync("href");
                    var title = await link.GetAttributeAsync("title");
                    var newsId = await link.GetAttributeAsync("data-newsid");

                    // Görsel URL'sini çek
                    var imgElement = link.Locator("img");
                    string? imageUrl = null;
                    if (await imgElement.CountAsync() > 0)
                    {
                        imageUrl = await imgElement.First.GetAttributeAsync("src");
                        if (string.IsNullOrWhiteSpace(imageUrl))
                            imageUrl = await imgElement.First.GetAttributeAsync("data-big-src");
                    }

                         // Yayın tarihini çek
                         var dateElement = link.Locator("span.date");
                         string? publishDate = null;
                         if (await dateElement.CountAsync() > 0)
                             publishDate = await dateElement.First.InnerTextAsync();



                   if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(href) && !string.IsNullOrWhiteSpace(imageUrl))
                    {
                        var dto = new NewFetchedEventDto
                        {
                            Id= newsId,
                            Title = title,
                            PublishDate = publishDate,
                            ImageUrl = imageUrl,
                            Link = href,
                            CategoryName = category ?? "Bilinmeyen"
                        };
                            list.Add(dto);
                        await _publisher.PublishMessageAsync(dto, MessageQueueNames.NewsQueue);
                    }
                }
            }

            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, $"Sayfa zaman aşımına uğradı: {_settings.BaseUrl}");
                return; // ya da hatayı swallow etme, yine fırlatabilirsin
            }

        }
        async Task ScrollToBottomAsync(IPage page, int maxScrolls = 20, int delayMs = 1000)
        {
            int previousHeight = 0;
            for (int i = 0; i < maxScrolls; i++)
            {
                // Sayfanın şu anki yüksekliğini al
                int currentHeight = await page.EvaluateAsync<int>("() => document.body.scrollHeight");

                if (currentHeight == previousHeight)
                {
                    // Artış yok, yani sayfa sonuna ulaştık, döngüden çık
                    break;
                }

                // Scroll yap
                await page.EvaluateAsync("() => window.scrollTo(0, document.body.scrollHeight)");

                // Yüklenme için bekle
                await Task.Delay(delayMs);

                previousHeight = currentHeight;
            }
        }



    }
}
