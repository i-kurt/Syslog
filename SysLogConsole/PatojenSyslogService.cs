using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Syslog
{
    partial class PatojenSyslogService : ServiceBase
    {
        public static UdpClient syslogServer;
        public static object objLock = new object();
        public static bool blLogla = true;

        public object SystemIcons { get; private set; }

        public PatojenSyslogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            startService();
        }

        public void startService()
        {
            Task.Factory.StartNew(new Action(() =>
            {
                while (blLogla)
                {
                    try
                    {
                        var result = syslogServer.ReceiveAsync();
                        result.Wait();
                        Task.Factory.StartNew(new Action(() => ProcessLog(result.Result.Buffer)));
                    }
                    catch (Exception exlog)
                    {
                        Console.WriteLine(exlog.Message);
                    }
                }
            }));
        }

        private void ProcessLog(byte[] baLog)
        {
            try
            {
                if (Encoding.UTF8.GetString(baLog).ToLowerInvariant().Contains("sending discover..."))
                {
                    return;
                }

                lock (objLock)
                {
                    string strLogFile = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "log.txt");
                    File.AppendAllText(strLogFile, Encoding.UTF8.GetString(baLog) + Environment.NewLine);
                }
                if (Environment.UserInteractive)
                    Console.WriteLine(Encoding.UTF8.GetString(baLog));

                if (Encoding.UTF8.GetString(baLog).ToLowerInvariant().Contains("attack") || Encoding.UTF8.GetString(baLog).ToLowerInvariant().Contains("firewall") || Encoding.UTF8.GetString(baLog).ToLowerInvariant().Contains("XTM link down."))
                {
                    //saldırı anında modemi kapatıp 5 dakika sonra açmak IP adresini değiştirdiği için saldırıyı güvenlik duvarına bırakmamış oluyoruz.
                    //burada amaç son kullanıcıyı (uzaktan eğitim alan çocuklarınızı vb...) kısaca saldırıdan kurtarmak ve normal internet kullanımına geri dönmektir.
                    var notifyIcon = new NotifyIcon();
                    notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                    notifyIcon.BalloonTipTitle = "Modem";
                    notifyIcon.BalloonTipText = "Modemi kapat 5 dakika sonra aç.";
                    notifyIcon.Visible = true;
                    notifyIcon.Icon =System.Drawing.SystemIcons.Warning;
                    notifyIcon.ShowBalloonTip(5000);
                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                }

                if (Encoding.UTF8.GetString(baLog).ToLowerInvariant().Contains("xdsl: xtm link down."))
                {
                    var notifyIcon = new NotifyIcon();
                    notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                    notifyIcon.BalloonTipTitle = "Modem";
                    notifyIcon.BalloonTipText = "İnternet bağlantısı kesildi.";
                    notifyIcon.Visible = true;
                    notifyIcon.Icon = System.Drawing.SystemIcons.Warning;
                    notifyIcon.ShowBalloonTip(5000);
                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                }

                if (Encoding.UTF8.GetString(baLog).ToLowerInvariant().Contains("\"state\": \"up\""))
                {
                    var notifyIcon = new NotifyIcon();
                    notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
                    notifyIcon.BalloonTipTitle = "Modem";
                    notifyIcon.BalloonTipText = "İnternet bağlantısı geldi.";
                    notifyIcon.Visible = true;
                    notifyIcon.Icon = System.Drawing.SystemIcons.Warning;
                    notifyIcon.ShowBalloonTip(5000);
                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                blLogla = false;
                syslogServer.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
