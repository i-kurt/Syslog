using System;
using System.IO;
using System.Net.Sockets;
using System.ServiceProcess;

namespace Syslog
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string strLogFile = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "log.txt"); 
                FileInfo fi = new FileInfo(strLogFile);
                if (fi.Length > 1024 * 1024 * 10)
                {
                    fi.MoveTo(Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "log_" + DateTime.Now.Ticks.ToString() + ".txt"));
                }


                PatojenSyslogService.syslogServer = new UdpClient(514, AddressFamily.InterNetwork);
                if (!Environment.UserInteractive)
                    using (var service = new PatojenSyslogService())
                        ServiceBase.Run(service);
                else
                {
                    PatojenSyslogService ptjn = new PatojenSyslogService();
                    ptjn.startService();
                    Console.WriteLine("Bitirmek için enter");
                    Console.ReadLine();
                    ptjn.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
