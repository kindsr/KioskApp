using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace iBeautyNail.Utility
{
    public class ImageUtility
    {
        #region SINGLETON
        private static ImageUtility _instance;
        private static object lockObj = new object();
        public static ImageUtility Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new ImageUtility();
                }
                return _instance;
            }
        }
        #endregion SINGLETON

        public ImageUtility()
        {

        }

        public Bitmap CropWithMask(Image oriImage, int imsiWidth, int imsiHeight, Image maskImage) // 이미지갤러리 마스크 모양으로 잘라냄.
        {
            Bitmap maskImg = new Bitmap(maskImage);
            Bitmap orgImg = new Bitmap(oriImage, (int)((float)(maskImg.Height * imsiWidth) / (float)imsiHeight) + 1, maskImg.Height);
            int leftMargin = (orgImg.Width - maskImg.Width) / 2;
            Bitmap newImg = new Bitmap(maskImg.Width, maskImg.Height);

            using (var g = Graphics.FromImage(newImg))
            {
                g.DrawImage(orgImg, new Rectangle(0, 0, newImg.Width, newImg.Height), 0, 0, newImg.Width, newImg.Height, GraphicsUnit.Pixel);
            }

            for (int y = 0; y <= orgImg.Height - 1; y++)
            {
                for (int x = 0; x <= orgImg.Width - 1; x++)
                {
                    if (x <= (leftMargin) - 1)
                    {

                    }
                    else if (x >= leftMargin + maskImg.Width)
                    {

                    }
                    else
                    {

                        if (maskImg.GetPixel(x - leftMargin, y).A != 255)
                        {
                            // 투명한 곳 처리
                            newImg.SetPixel(x - leftMargin, y, Color.FromArgb(0, 0, 0, 0));
                        }
                        else
                        {
                            newImg.SetPixel(x - leftMargin, y, Color.FromArgb(orgImg.GetPixel(x, y).A, orgImg.GetPixel(x, y).R, orgImg.GetPixel(x, y).G, orgImg.GetPixel(x, y).B));
                        }
                    }
                }
            }
            maskImg.Dispose();
            orgImg.Dispose();
            return newImg;
        }

        public Image BitmapImage2Image(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Image image = Image.FromStream(outStream);

                return image;
            }
        }

        public Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        public BitmapImage Image2BitmapImage(Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public void TrimImage(string path)
        {
            int threshhold = 250;


            int topOffset = 0;
            int bottomOffset = 0;
            int leftOffset = 0;
            int rightOffset = 0;
            Bitmap img = new Bitmap(path);


            bool foundColor = false;
            // Get left bounds to crop
            for (int x = 1; x < img.Width && foundColor == false; x++)
            {
                for (int y = 1; y < img.Height && foundColor == false; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                leftOffset += 1;
            }


            foundColor = false;
            // Get top bounds to crop
            for (int y = 1; y < img.Height && foundColor == false; y++)
            {
                for (int x = 1; x < img.Width && foundColor == false; x++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                topOffset += 1;
            }


            foundColor = false;
            // Get right bounds to crop
            for (int x = img.Width - 1; x >= 1 && foundColor == false; x--)
            {
                for (int y = 1; y < img.Height && foundColor == false; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                rightOffset += 1;
            }


            foundColor = false;
            // Get bottom bounds to crop
            for (int y = img.Height - 1; y >= 1 && foundColor == false; y--)
            {
                for (int x = 1; x < img.Width && foundColor == false; x++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.R < threshhold || color.G < threshhold || color.B < threshhold)
                        foundColor = true;
                }
                bottomOffset += 1;
            }


            // Create a new image set to the size of the original minus the white space
            //Bitmap newImg = new Bitmap(img.Width - leftOffset - rightOffset, img.Height - topOffset - bottomOffset);

            Bitmap croppedBitmap = new Bitmap(img);
            croppedBitmap = croppedBitmap.Clone(
            new Rectangle(leftOffset - 3, topOffset - 3, img.Width - leftOffset - rightOffset + 6, img.Height - topOffset - bottomOffset + 6),
            System.Drawing.Imaging.PixelFormat.DontCare);


            // Get a graphics object for the new bitmap, and draw the original bitmap onto it, offsetting it do remove the whitespace
            //Graphics g = Graphics.FromImage(croppedBitmap);
            //g.DrawImage(img, 1 - leftOffset, 1 - rightOffset);
            croppedBitmap.Save(path + "_crop.png", ImageFormat.Png);
        }

        public string Base64EncodeImage(BitmapSource img)
        {
            BitmapFrame frame = img as BitmapFrame;
            if (frame == null)
            {
                frame = BitmapFrame.Create(img);
            }
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);

            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);

            byte[] bytes = stream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        public BitmapSource DecodeBase64Image(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);

            MemoryStream stream = new MemoryStream(bytes);
            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.None);

            BitmapFrame frame = decoder.Frames[0];
            return frame;
        }

        public void SavePNGFile(BitmapSource bitmapSource, string filePath, PngInterlaceOption pngInterlaceOption)

        {
            PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();

            pngBitmapEncoder.Interlace = pngInterlaceOption;
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                pngBitmapEncoder.Save(fileStream);
            }
        }

        public void SaveTIFFFile(BitmapSource bitmapSource, string filePath)
        {
            TiffBitmapEncoder tiffBitmapEncoder = new TiffBitmapEncoder();

            tiffBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                tiffBitmapEncoder.Save(fileStream);
            }
        }
    }

}
