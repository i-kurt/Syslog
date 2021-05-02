using System;
using System.Threading.Tasks;

namespace SysLogServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                csLogger cs = new csLogger();
                Task tLogla = cs.startService();
                Console.WriteLine("Servisi durdurmak için Enter basınız.");
                Console.ReadLine();
                cs.stopService();
                tLogla.Wait();
                Console.WriteLine("Servisi durduruldu. Çıkış için Enter basınız.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
