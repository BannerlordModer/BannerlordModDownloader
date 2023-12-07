# Bannerlord Mod Downloader
使用Blazor Hybrid技术编写界面，并使用Monotorrent来实现文件传输。

## 实现功能
1. 自动获取游戏目录
1. 自动识别游戏版本
1. 自动下载对应版本的Mod
1. 提示Mod版本是否兼容
1. 可以设置的自动分享
1. Mod发布功能

## 分库设计
1. `BannerlordModDownloader` UI部分，包含应用内的UI设计相关
1. `BannerlordModDownloader.Core` 存放核心部分，包括完整的应用逻辑。
1. `BannerlordModDownloader.Downloader` 下载器部分，实现完善的下载功能
1. `BannerlordModDownloader.Cli` 命令行版本，实现一些服务端和命令行模式需要的功能