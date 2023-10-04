using System;

namespace delegateEventsAsync
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            List<string> imageUrls = new List<string>
        {
            "https://example.com/image1.jpg",
            "https://example.com/image2.jpg",
            "https://example.com/image3.jpg",
            "https://example.com/image4.jpg",
            "https://example.com/image5.jpg",
            "https://example.com/image6.jpg",
            "https://example.com/image7.jpg",
            "https://example.com/image8.jpg",
            "https://example.com/image9.jpg",
            "https://example.com/image10.jpg",
        };
            var cts = new CancellationTokenSource();
            CancellationToken cancellationToken = cts.Token;
            var downloader = new ImageDownloader();
            downloader.ImageStarted += (sender, fileName) =>
            {
                Console.WriteLine($"Скачивание файла {fileName} началось.");
            };

            downloader.ImageCompleted += (sender, fileName) =>
            {
                Task.Delay(1000);
                Console.WriteLine($"Скачивание файла {fileName} завершено.");
            };

            var downloadTasks = new List<Task>();

            foreach (var url in imageUrls)
            {
                var fileName = Path.GetFileName(url);
                downloadTasks.Add(downloader.DownloadAsync(url, fileName, cancellationToken));
            }
            Console.WriteLine("Для отмены нажмите клавишу 'A'...");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey(intercept: true).Key == ConsoleKey.A)
                    {
                        Console.WriteLine("Отмена всех загрузок...");
                        foreach (var task in downloadTasks)
                        {
                           cts.Cancel();
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nСостояние загрузки:");
                        for (int i = 0; i < imageUrls.Count; i++)
                        {
                            Console.WriteLine($"{imageUrls[i]} - {downloadTasks[i].IsCompleted}");
                        }
                    }
                }
            }
        }
    }
}