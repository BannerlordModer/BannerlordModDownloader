using System.Linq;

namespace BannerlordModDownloader.Downloader.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var downloader = new Downloader(new DownloadConfig() { ListenPort = 63124, SaveDirectory = Environment.CurrentDirectory });
            var task = downloader.DownloadLink("magnet:?xt=urn:btih:PZBJTWVKSSY74WJLTPT7EG2TESBRQNEK&dn=&tr=http%3A%2F%2F104.143.10.186%3A8000%2Fannounce&tr=udp%3A%2F%2F104.143.10.186%3A8000%2Fannounce&tr=http%3A%2F%2Ftracker.openbittorrent.com%3A80%2Fannounce&tr=http%3A%2F%2Ftracker3.itzmx.com%3A6961%2Fannounce&tr=http%3A%2F%2Ftracker4.itzmx.com%3A2710%2Fannounce&tr=http%3A%2F%2Ftracker.publicbt.com%3A80%2Fannounce&tr=http%3A%2F%2Ftracker.prq.to%2Fannounce&tr=http%3A%2F%2Fopen.acgtracker.com%3A1096%2Fannounce&tr=https%3A%2F%2Ft-115.rhcloud.com%2Fonly_for_ylbud&tr=http%3A%2F%2Ftracker1.itzmx.com%3A8080%2Fannounce&tr=http%3A%2F%2Ftracker2.itzmx.com%3A6961%2Fannounce&tr=udp%3A%2F%2Ftracker1.itzmx.com%3A8080%2Fannounce&tr=udp%3A%2F%2Ftracker2.itzmx.com%3A6961%2Fannounce&tr=udp%3A%2F%2Ftracker3.itzmx.com%3A6961%2Fannounce&tr=udp%3A%2F%2Ftracker4.itzmx.com%3A2710%2Fannounce&tr=http%3A%2F%2Ftr.bangumi.moe%3A6969%2Fannounce&tr=http%3A%2F%2Ft.nyaatracker.com%2Fannounce&tr=http%3A%2F%2Fopen.nyaatorrents.info%3A6544%2Fannounce&tr=http%3A%2F%2Ft2.popgo.org%3A7456%2Fannonce&tr=http%3A%2F%2Fshare.camoe.cn%3A8080%2Fannounce&tr=http%3A%2F%2Fopentracker.acgnx.se%2Fannounce&tr=http%3A%2F%2Ftracker.acgnx.se%2Fannounce&tr=http%3A%2F%2Fnyaa.tracker.wf%3A7777%2Fannounce&tr=https%3A%2F%2Ftracker.lilithraws.org%2Fannounce&tr=http%3A%2F%2Fopentracker.acgnx.com%3A6869%2Fannounce&tr=http%3A%2F%2Ft.acg.rip%3A6699%2Fannounce");
            for (int i = 0; i < 100; i++) {
                downloader.GetDownloadStatus().Select(data => {
                    Console.WriteLine($"{data.Item1}: {data.Item2}");
                    return data;
                });
                Task.Delay(1000).Wait();
            }
            task.Wait();
        }
    }
}
