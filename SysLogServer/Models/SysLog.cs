using System;

#nullable disable

namespace SysLogServer.Models
{
    public partial class SysLog
    {
        public DateTime[] Tarih { get; set; }
        public string Aciklama { get; set; }
        public int Id { get; set; }
    }
}
