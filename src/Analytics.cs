﻿using System;
using System.Collections.Generic;
using Umod.Libraries;
using Umod.Libraries.Covalence;
using Umod.Plugins;

namespace Umod
{
    public static class Analytics
    {
        private static readonly WebRequests Webrequests = Interface.Umod.GetLibrary<WebRequests>();
        private static readonly PluginManager PluginManager = Interface.Umod.RootPluginManager;
        private static readonly Covalence Covalence = Interface.Umod.GetLibrary<Covalence>();
        private static readonly Lang Lang = Interface.Umod.GetLibrary<Lang>();

        private const string trackingId = "UA-48448359-3";
        private const string url = "https://www.google-analytics.com/collect";

        /*public static string Filename = Utility.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);

        private static Plugin[] Plugins() => PluginManager.GetPlugins().Where(pl => !pl.IsCorePlugin).ToArray();

        private static IEnumerable<string> PluginNames() => new HashSet<string>(Plugins().Select(pl => pl.Name));

        private static readonly Dictionary<string, string> Tags = new Dictionary<string, string>
        {
            { "dimension1", IntPtr.Size == 8 ? "x64" : "x86" }, // CPU architecture
            { "dimension2", Environment.OSVersion.Platform.ToString().ToLower() }, // OS platform
            { "dimension3", Environment.OSVersion.Version.ToString().ToLower() }, // OS version
            { "dimension4", Filename.ToLower().Replace("dedicated", "").Replace("server", "").Replace("-", "").Replace("_", "")  }, // Game name
            { "dimension5", Plugins().Length.ToString() }, // Plugin count
            { "dimension6", string.Join(", ", PluginNames().ToArray()) } // Plugin names
        };*/

        private static readonly string Identifier = $"{Covalence.Server.Address}:{Covalence.Server.Port}";

        public static void Collect()
        {
            string payload = $"v=1&tid={trackingId}&cid={Identifier}&t=screenview&cd={Covalence.Game}+{Covalence.Server.Version}";
            payload += $"&an=Umod&av={Umod.Version}&ul={Lang.GetServerLanguage()}";
            //payload += string.Join("&", Tags.Select(kv => kv.Key + "=" + kv.Value).ToArray());
            SendPayload(payload);
        }

        public static void Event(string category, string action)
        {
            string payload = $"v=1&tid={trackingId}&cid={Identifier}&t=event&ec={category}&ea={action}";
            SendPayload(payload);
        }

        public static void SendPayload(string payload)
        {
            Dictionary<string, string> headers = new Dictionary<string, string> { { "User-Agent", $"Umod/{Umod.Version} ({Environment.OSVersion}; {Environment.OSVersion.Platform})" } };
            Webrequests.Enqueue(url, Uri.EscapeUriString(payload), (code, response) => { }, null, RequestMethod.POST, headers);
        }
    }
}
