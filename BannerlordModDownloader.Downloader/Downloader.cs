using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using MonoTorrent;
using MonoTorrent.Client;

namespace BannerlordModDownloader.Downloader {
    public class Downloader {
        public ClientEngine Engine {  get; init; }
        private CancellationTokenSource Cancellation {  get; set; }
        private DownloadConfig Config {  get; set; }
        private List<Uri> TrackerList { get; set; }
        public Downloader (DownloadConfig config) {
            Config = config;
            Cancellation = new CancellationTokenSource();
            TrackerList = new List<Uri> ();
            // Give an example of how settings can be modified for the engine.
            var settingBuilder = new EngineSettingsBuilder {
                UsePartialFiles = true,
                AllowLocalPeerDiscovery = true,
                MaximumConnections = 10000,
                MaximumHalfOpenConnections = 100,
                MaximumOpenFiles = 100,
                // Allow the engine to automatically forward ports using upnp/nat-pmp (if a compatible router is available)
                AllowPortForwarding = true,

                // Automatically save a cache of the DHT table when all torrents are stopped.
                AutoSaveLoadDhtCache = true,

                // Automatically save 'FastResume' data when TorrentManager.StopAsync is invoked, automatically load it
                // before hash checking the torrent. Fast Resume data will be loaded as part of 'engine.AddAsync' if
                // torrent metadata is available. Otherwise, if a magnetlink is used to download a torrent, fast resume
                // data will be loaded after the metadata has been downloaded. 
                AutoSaveLoadFastResume = true,

                // If a MagnetLink is used to download a torrent, the engine will try to load a copy of the metadata
                // it's cache directory. Otherwise the metadata will be downloaded and stored in the cache directory
                // so it can be reloaded later.
                AutoSaveLoadMagnetLinkMetadata = true,

                // Use a fixed port to accept incoming connections from other peers for testing purposes. Production usages should use a random port, 0, if possible.
                ListenEndPoints = new Dictionary<string, IPEndPoint> {
                    { "ipv4", new IPEndPoint (IPAddress.Any, config.ListenPort) },
                    { "ipv6", new IPEndPoint (IPAddress.IPv6Any, config.ListenPort) }
                },

                // Use a fixed port for DHT communications for testing purposes. Production usages should use a random port, 0, if possible.
                DhtEndPoint = new IPEndPoint(IPAddress.Any, config.ListenPort+1),

            };
            Engine = new ClientEngine(settingBuilder.ToSettings());
        }
        public async Task DownloadLink(string Magnetlink) {
            if (TrackerList.Count == 0) {
                var client = new HttpClient();
                var resp = await client.GetStringAsync("https://cf.trackerslist.com/all.txt");
                foreach (var trackerString in resp.Split("\n\n")) {
                    TrackerList.Add(new Uri(trackerString));
                }
            }
            if (!MagnetLink.TryParse(Magnetlink, out MagnetLink link)) {
                throw new MagnetException("Cannot parse Magnet link!");
            }
            //var manager = await Engine.AddStreamingAsync(link, Path.Combine(Config.SaveDirectory, link.InfoHashes.V1.ToHex()));
            var manager = await Engine.AddAsync(link, Path.Combine(Config.SaveDirectory, link.InfoHashes.V1.ToHex()));

            manager.PeerConnected += (o, e) => {
                Console.WriteLine($"First peer connected. Time since torrent started: {e.Peer.PeerID.Text}");
            };
            manager.PeersFound += (o, e) => {
                Console.WriteLine($"First peers found via {e.GetType().Name}. Time since torrent started: {manager.Peers.Available}");
            };
            manager.PieceHashed += (o, e) => {
                Console.WriteLine($"Piece {e.PieceIndex} hashed. Time since torrent started: ");
            };

            //await manager.HashCheckAsync(true);
            await manager.StartAsync();
            foreach(var tracker in TrackerList) {
                await manager.TrackerManager.AddTrackerAsync(tracker);
            }
            await manager.WaitForMetadataAsync(Cancellation.Token);
            await manager.DhtAnnounceAsync();
            await manager.LocalPeerAnnounceAsync();
            await manager.TrackerManager.ScrapeAsync(Cancellation.Token);
            await manager.TrackerManager.AnnounceAsync(Cancellation.Token);
            //var resume = await manager.SaveFastResumeAsync();
        }

        public IEnumerable<(string, double)> GetDownloadStatus() {
            return Engine.Torrents.Select(manager => (manager.MagnetLink.InfoHashes.V1OrV2.ToHex(), manager.PartialProgress));
        }

    }
}
