using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace delegateEventsAsync
{
    internal class ImageDownloader
    {
        public event EventHandler<string>? ImageStarted;
        public event EventHandler<string>? ImageCompleted;
        public async Task<bool> DownloadAsync(string remoteUri, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                var myWebClient = new WebClient();
                ImageStarted?.Invoke(this, fileName);
                var downloadTask = myWebClient.DownloadFileTaskAsync(new Uri(remoteUri), fileName);
                await Task.WhenAny(downloadTask, Task.Delay(Timeout.Infinite, cancellationToken));
                if (cancellationToken.IsCancellationRequested)
                    return false;
                await Task.Delay(1000);
                ImageCompleted?.Invoke(this, fileName);

                return downloadTask.IsCompleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при загрузке изображения: " + ex.Message);
                return false;
            }
        }

    }
}
