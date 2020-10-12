using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ngsoft.Media
{
    public class PictureResizer
    {
        private readonly byte[] _source;
        private readonly ImageFormat _outputFormat;
        private int _requiredWidth;
        private int _requiredHeight;

        public PictureResizer(byte[] sourcePicture, ImageFormat outputFormat)
        {
            if (sourcePicture == null)
            {
                throw new ArgumentNullException(nameof(sourcePicture));
            }
            if (sourcePicture.Length == 0)
            {
                throw new ArgumentException("Source picture bytes cannot be empty.", nameof(sourcePicture));
            }
            if (outputFormat == null)
            {
                throw new ArgumentNullException(nameof(outputFormat));
            }

            _source = sourcePicture;
            _outputFormat = outputFormat;
        }

        public static PictureResizer Create(byte[] sourcePicture, ImageFormat outputFormat)
        {
            return new PictureResizer(sourcePicture, outputFormat);
        }

        public PictureResizer SetRequiredWidth(int width)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Required width cannot be negative or zero.");
            }

            _requiredWidth = width;
            return this;
        }

        public PictureResizer SetRequiredHeight(int height)
        {
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Required height cannot be negative or zero.");
            }

            _requiredHeight = height;
            return this;
        }

        public byte[] ResizeByWidth()
        {
            if (_requiredWidth == 0)
            {
                throw new InvalidOperationException("Required width not set.");
            }

            using (var sourceMemoryStream = new MemoryStream(_source))
            {
                using (var sourceImage = Image.FromStream(sourceMemoryStream))
                {
                    float ratio = sourceImage.Width / (float)_requiredWidth;
                    float resultHeight = sourceImage.Height / ratio;
                    using (var bitmapImage = new Bitmap(original: sourceImage, width: _requiredWidth, height: (int)resultHeight))
                    {
                        using (var outputMemoryStream = new MemoryStream())
                        {
                            bitmapImage.Save(outputMemoryStream, _outputFormat);
                            return outputMemoryStream.ToArray();
                        }
                    }
                }
            }
        }

        public byte[] ResizeByHeight()
        {
            if (_requiredHeight == 0)
            {
                throw new InvalidOperationException("Required height not set.");
            }

            using (var sourceMemoryStream = new MemoryStream(_source))
            {
                using (var sourceImage = Image.FromStream(sourceMemoryStream))
                {
                    float ratio = sourceImage.Height / (float)_requiredHeight;
                    float resultWidth = sourceImage.Width / ratio;
                    using (var bitmapImage = new Bitmap(original: sourceImage, width: (int)resultWidth, height: _requiredHeight))
                    {
                        using (var outputMemoryStream = new MemoryStream())
                        {
                            bitmapImage.Save(outputMemoryStream, _outputFormat);
                            return outputMemoryStream.ToArray();
                        }
                    }
                }
            }
        }
    }
}
