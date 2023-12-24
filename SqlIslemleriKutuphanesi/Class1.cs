using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SqlIslemleriKutuphanesi
{
    public class MSSQL
    {
        static SqlConnection baglanti;
        public MSSQL(string sunucu,string veritabaniAdi,OturumTipi oturumTipi,string kullaniciAdi=null,string sifre=null)
        {
            baglanti = new SqlConnection();
            string baglantiMetni=string.Format("Data Source={0};Initial Catalog={1};",sunucu,veritabaniAdi);
            if (oturumTipi == OturumTipi.WindowsAuthentication)
                baglantiMetni += "Integrated Security=true;";
            else
                baglantiMetni += string.Format("Integrated Security=false;User Id={0};Password={1};", kullaniciAdi, sifre);
            baglanti.ConnectionString = baglantiMetni;

            try
            {
                baglanti.Open();
                baglanti.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Baglanti Hatası\nInternet bağlantınız yok ya da sunucu bilgileriniz yanlış",ex);
            }
        }

        public Response SelectIslemi(string sorgu, params SqlParametresi[] parametreler)
        {
            Response response = new Response();
            try
            {
                SqlDataAdapter adaptor = new SqlDataAdapter();
                SqlCommand komut = new SqlCommand();
                komut.CommandText = sorgu;
                adaptor.SelectCommand = komut;
                adaptor.SelectCommand.Connection = baglanti;
                if(parametreler.Length>0)
                    for (int i = 0; i < parametreler.Length; i++)
                        komut.Parameters.AddWithValue(parametreler[i].parametreAdi, parametreler[i].datasi);                    
                adaptor.Fill(response.tablo);
            }
            catch (Exception ex)
            {
                response.HataliMi = true;
                response.Mesaj = ex.Message;
                response.tablo = null;
            }
            return response;
        }

        public Response FizikselKomut(string sorgu,params SqlParametresi[] parametreler)
        {
            SqlCommand komut = new SqlCommand();
            komut.CommandText = sorgu;
            for (int i = 0; i < parametreler.Length; i++)
                komut.Parameters.AddWithValue(parametreler[i].parametreAdi, parametreler[i].datasi);
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            Response response = new Response();
            try
            {
                baglanti.Open();
                int etkilenenSatir=komut.ExecuteNonQuery();
                baglanti.Close();
                response.Mesaj = string.Format("{0} satır etkilendi.", etkilenenSatir);
            }
            catch (Exception ex)
            {
                baglanti.Close();
                response.HataliMi = true;
                response.Mesaj = ex.Message;
            }
            return response;
        }

    }
}
