using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using IronWebScraper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LondonSpider
{
    internal class ImageScraper : WebScraper
    {
        public string ImageSearch;
        private static BlobService _blobService;
        public override void Init()
        {
            this.LoggingLevel = WebScraper.LogLevel.All;
            _blobService = new BlobService();

            string url =
                $"{Environment.GetEnvironmentVariable("ImageWebsiteUrl")}/{ImageSearch}/";
            
            this.Request(url, Parse);
            
        }

        public override void Parse(Response response)
        {
            List<string> imageUrls = new List<string>();

            foreach (var node in response.Css(".photo-item__img"))
            {
                var url = node.GetAttribute("srcset").Split('?')[0];
                Console.WriteLine(url);
                imageUrls.Add(url);
            }
            Console.WriteLine("done");
            DownloadImagesToBlob(imageUrls);
        }

        public async void DownloadImagesToBlob(List<string> imageList)
        {
            using (WebClient wc = new WebClient())
            {
                foreach (var imageUrl in imageList)
                {
                    using (Stream imageStream = wc.OpenRead(imageUrl))
                    {
                        using (Image<Rgba32> image = Image.Load(imageStream))
                        {
                            
                            image.Mutate(x => x.Resize(1080, 1080));
                            using (Stream mutatedStream = new MemoryStream())
                            {
                                var filePath = "img.jpeg";
                                image.Save(filePath);

                                await _blobService.UploadBlobAsync(filePath, imageUrl);
                            }
                        }
                    }
                }
            }
        } 
    }
}