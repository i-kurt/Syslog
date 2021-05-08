using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SysLogServer
{
    public class csLogger : IDisposable
    {
        public UdpClient syslogServer = null;
        public object objLock = new object();
        public bool blLogla = true;

        public csLogger()
        {
            if (syslogServer != null)
            {
                syslogServer.Close();
                syslogServer.Dispose();
            }

            syslogServer = new UdpClient(514, AddressFamily.InterNetwork);
        }

        public Task startService()
        {
            return Task.Factory.StartNew(new Action(() =>
             {
                 while (blLogla)
                 {
                     try
                     {
                         var rTask = syslogServer.ReceiveAsync();
                         rTask.Wait();
                         Task.Factory.StartNew(new Action(() => ProcessLog(rTask.Result.Buffer)));
                     }
                     catch (Exception exlog)
                     {
                         if (!exlog.Message.Contains("Cannot access a disposed object"))
                             Console.WriteLine(exlog.Message);
                     }
                 }
             }));
        }

        private void ProcessLog(byte[] baLog)
        {
            try
            {
                string strMsg = Encoding.UTF8.GetString(baLog);

                if (strMsg.ToLowerInvariant().Contains("sending discover..."))
                {
                    return;
                }

                if (Environment.UserInteractive)
                {
                    Console.WriteLine(strMsg);
                }

                lock (objLock)
                {
                    SysLogServer.DBHelper db = new SysLogServer.DBHelper();
                    db.SysLogKaydet(strMsg + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void stopService()
        {
            try
            {
                blLogla = false;
                syslogServer.Close();
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("Servis durdurulurken hata oluştu! " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            if (syslogServer != null)
            {
                syslogServer.Close();
                syslogServer.Dispose();
            }
        }
    }
}
