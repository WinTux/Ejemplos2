using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System;
using System.Drawing.Imaging;

namespace _20240320.Tags
{
    [HtmlTargetElement("codigoqr")]
    public class CodQRTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string contenido = context.AllAttributes["contenido"].Value.ToString();
            int ancho = int.Parse(context.AllAttributes["ancho"].Value.ToString());
            int alto = int.Parse(context.AllAttributes["alto"].Value.ToString());
            var barCodePixelData = new BarcodeWriterPixelData { 
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { 
                    Height = alto,
                    Width = ancho,
                    Margin = 0
                }
            };
            var datosPixel = barCodePixelData.Write(contenido);
            using (Bitmap bitmap = new Bitmap(datosPixel.Width, datosPixel.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb)) {
                using (var memoryStream = new MemoryStream()) {
                    var datosBitmap = bitmap.LockBits(new Rectangle(0, 0, datosPixel.Width, datosPixel.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(datosPixel.Pixels,0, datosBitmap.Scan0,datosPixel.Pixels.Length);
                    }
                    finally { 
                        bitmap.UnlockBits(datosBitmap);
                    }
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    output.TagName = "img";
                    output.Attributes.Clear();
                    output.Attributes.Add("width", ancho);
                    output.Attributes.Add("height", alto);
                    output.Attributes.Add("src", String.Format("data:image/png;base64,{0}",Convert.ToBase64String(memoryStream.ToArray())));
                }
            }

        }
    }
}
