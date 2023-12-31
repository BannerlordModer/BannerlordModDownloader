using BannerlordModDownloader.Downloader;

namespace BannerlordModDownloader.Cli {
    internal class Program {
        static async Task Main(string[] args) {
            var downloader = new Downloader.Downloader(new DownloadConfig() { ListenPort = 63124, SaveDirectory = Environment.CurrentDirectory });
            Console.WriteLine("Before");
            downloader.DownloadLink("");
            Console.WriteLine("After");
            await Task.Delay(3000);
            while (downloader.Engine.IsRunning) {
                //Console.WriteLine($"downloader.Engine.IsRunning:{downloader.Engine.IsRunning}");
                //Console.WriteLine(downloader.GetDownloadStatus().ToList().Count);
                var tmp = downloader.GetDownloadStatus().ToList().FirstOrDefault();
                Console.WriteLine($"tmp is {tmp.Item1}:{tmp.Item2}");
                downloader.GetDownloadStatus().Select(data => {
                    Console.WriteLine($"{data.Item1}: {data.Item2}");
                    return data;
                });
                await Task.Delay(1000);
            }
        }
    }
}
