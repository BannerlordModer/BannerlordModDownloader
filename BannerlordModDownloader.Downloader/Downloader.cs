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
        private ClientEngine Engine {  get; set; }
        private CancellationTokenSource Cancellation {  get; set; }
        private DownloadConfig Config {  get; set; }
        public Downloader (DownloadConfig config) {
            Config = config;
            Cancellation = new CancellationTokenSource();
            // Give an example of how settings can be modified for the engine.
            var settingBuilder = new EngineSettingsBuilder {
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
                DhtEndPoint = new IPEndPoint(IPAddress.Any, config.ListenPort),

            };
            Engine = new ClientEngine(settingBuilder.ToSettings());
        }
        public async Task DownloadLink(string Magnetlink) {
            if (!MagnetLink.TryParse(Magnetlink, out MagnetLink link)) {
                throw new MagnetException("Cannot parse Magnet link!");
            }
            var manager = await Engine.AddStreamingAsync(link, Config.SaveDirectory);
            await manager.StartAsync();
            await manager.WaitForMetadataAsync(Cancellation.Token);
        }

        public IEnumerable<(string, double)> GetDownloadStatus() {
            return Engine.Torrents.Select(manager => (manager.InfoHashes.V1.ToHex(), manager.PartialProgress));
        }

    }
}
