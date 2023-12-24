using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SqlIslemleriKutuphanesi
{
    public class Response
    {
        public DataTable tablo;
        public bool HataliMi;
        public string Mesaj;

        public Response()
        {
            tablo = new DataTable();
            HataliMi = false;
            Mesaj = "";
        }
    }

    public class SqlParametresi
    {
        public string parametreAdi;
        public object datasi;
        public SqlParametresi(string parametreAdi,object datasi)
        {
            this.parametreAdi = parametreAdi;
            this.datasi = datasi;
        }
    }
}
