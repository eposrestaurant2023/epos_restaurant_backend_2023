using AspNetCore.Reporting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Text; 
using ReportModel;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using Reporting.Models;
using System.Text.Json;

namespace Reporting.Services
{
    public class PrintRequestAction : IPrintRequestAction
    {
        public Task<string> Invoice(DynamicModel receipt_data,  ReceiptSettingModel setting, string file_path)
        {
            try
            {


                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var _file = $"{file_path}\\{setting.invoice_file_name}.rdlc";
                LocalReport report = new LocalReport(_file); 


                //sale data
                DataTable sale_data = new DataTable();
                List<SaleReportModel> sales = new List<SaleReportModel>();
                sales = JsonSerializer.Deserialize<List<SaleReportModel>>(receipt_data.sale_data);
                sale_data = CreateDataTable(sales);
                //sale_product_data
                DataTable sale_product_data = new DataTable();
                List<SaleProductReportModel> sale_products = new List<SaleProductReportModel>();
                sale_products = JsonSerializer.Deserialize<List<SaleProductReportModel>>(receipt_data.sale_product_data);
                sale_product_data = CreateDataTable(sale_products);


                //Grand total data
                DataTable grand_total_data = new DataTable();
                List<GrandTotalReportModel> grand_totals = new List<GrandTotalReportModel>();
                grand_totals = JsonSerializer.Deserialize<List<GrandTotalReportModel>>(receipt_data.grand_total_data);
                grand_total_data = CreateDataTable(grand_totals);

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);

                //coupon voucher data
                DataTable coupon_voucher_data = new DataTable();
                List<CouponVoucherReportModel> coupon_voucher_list = new List<CouponVoucherReportModel>();
                if (receipt_data.coupon_voucher_data != null)
                {
                    coupon_voucher_list = JsonSerializer.Deserialize<List<CouponVoucherReportModel>>(receipt_data.coupon_voucher_data);
                }
                coupon_voucher_data = CreateDataTable(coupon_voucher_list);


                report.AddDataSource("Sale", sale_data);
                report.AddDataSource("SaleProduct", sale_product_data);
                report.AddDataSource("GrandTotal", grand_total_data);
                report.AddDataSource("Setting", setting_data);                                       
                report.AddDataSource("CouponVoucher", coupon_voucher_data);  
                var result = report.Execute(RenderType.Image);
                byte[] byts = result.MainStream;
                var img = byteArrayToImage(byts);
                Bitmap bmp = new Bitmap(width: img.Width, height: img.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.DrawImage(img, new Rectangle(new Point(), img.Size), new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                }

                return Task.FromResult(CropTopBottomImage(bmp, bmp.Width, setting.feed_papper));
            }
            catch (Exception ex)
            {
                return Task.FromResult("");
            }
        }
        public Task<string> WifiPassword(DynamicModel receipt_data, string file_path)
        {
            try
            {

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var _file = $"{file_path}\\rpt_wifi_password.rdlc";
                LocalReport report = new LocalReport(_file);                  

                //Setting data
                DataTable setting_data = new DataTable();
                List<SettingReportModel> settings = new List<SettingReportModel>();
                settings = JsonSerializer.Deserialize<List<SettingReportModel>>(receipt_data.setting_data);
                setting_data = CreateDataTable(settings);
                 
                report.AddDataSource("Setting", setting_data); 
                var result = report.Execute(RenderType.Image);
                byte[] byts = result.MainStream;
                var img = byteArrayToImage(byts);
                Bitmap bmp = new Bitmap(width: img.Width, height: img.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.DrawImage(img, new Rectangle(new Point(), img.Size), new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                }

                return Task.FromResult(CropTopBottomImage(bmp, bmp.Width, 0));
            }
            catch (Exception ex)
            {
                return Task.FromResult("");
            }
        }


        public  DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public string CropTopBottomImage(Bitmap bmp, int image_width,short feed_papper)
        {
            int h = bmp.Height;
            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < image_width; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }


            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedHeight = bottommost - topmost;



            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            } 
            croppedHeight = croppedHeight + (feed_papper * 10);

            try
            {
                var target = new Bitmap(image_width, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, image_width, croppedHeight),
                      new RectangleF(0, topmost, image_width, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                MemoryStream ms = new MemoryStream();
                target.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                var result = Convert.ToBase64String(byteImage); // Get Base64
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, 0, 0, image_width, croppedHeight),
                  ex);
            }
        }       
    }
}
