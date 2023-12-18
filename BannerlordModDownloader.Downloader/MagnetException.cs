using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BannerlordModDownloader.Downloader {
    [Serializable]
    public class MagnetException : Exception {
        public MagnetException() {
        }

        public MagnetException(string? message) : base(message) {
        }

        public MagnetException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
