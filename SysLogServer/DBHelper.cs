using SysLogServer.Models;
using System;

namespace SysLogServer
{
    public class DBHelper
    {
        public void SysLogKaydet(string strAciklama)
        {
            //TODO: Exception handle...
            using (postgresContext db = new postgresContext())
            {
                SysLog s = new SysLog();
                s.Aciklama = strAciklama;
                s.Tarih = new DateTime[] { DateTime.Now };
                db.SysLogs.Add(s);
                db.SaveChanges();
            }
        }
    }
}
