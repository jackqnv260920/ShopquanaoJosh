using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MarkShop.Models;
namespace MarkShop.Controllers
{
    public class ThongKeController : ApiController
    {
        QLBanQuanAoDataContext db = new QLBanQuanAoDataContext();
      
        public IHttpActionResult GetThongkesList()
        {
            #region query
            var sql = @"
                        select top(5) DATEPART(MONTH,HoaDon.NgayGiao) as MONTH, SUM(SanPham.GiaBan*ChiTietHoaDon.SoLuong)  as 'DoanhThuThang' 
                        from SanPham
                        inner join ChiTietHoaDon
                        on SanPham.MaSP = ChiTietHoaDon.MaSP
                        inner join HoaDon
                        on HoaDon.MaHD = ChiTietHoaDon.MaHD
                        where HoaDon.TinhTrang = 1
                        group by (HoaDon.NgayGiao)
";
            SqlConnection cnn;
            cnn = new SqlConnection(db.Connection.ConnectionString);
            SqlDataReader reader;
            SqlCommand cmd;
            List<Thongke> listTK = new List<Thongke>();
            try
            {
                cnn.Open();
                cmd = new SqlCommand(sql, cnn);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        listTK.Add(new Thongke() { Thang = (int)reader["MONTH"], GruopByTongtien = (decimal)reader["DoanhThuThang"] });

                    }

                }

                cnn.Close();
            }
            catch (Exception)
            {

            };
            #endregion
            return Json(listTK);
          
        }
    }
}
