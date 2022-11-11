using System;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;

namespace DD
{
    public static class Programm
    {
        public static void Main()
        {
            GetCenvertedImage(GetGrayscaleImage(new Bitmap(@"Kapibara2.jpg")), 100);

            Console.WriteLine("Done!");

            Console.ReadKey();
        }
        public static char[,] GetCenvertedImage(Bitmap imageToConvert, int convertedImageHeight)
        {
            float aspectRatioOfImage = (float)imageToConvert.Width / imageToConvert.Height;

            int oneHeightUnit = (int)Math.Ceiling((double)imageToConvert.Height / convertedImageHeight);
            int oneWidthUnit = (int)(oneHeightUnit * aspectRatioOfImage);

            int[,] grayscalesOfImage = new int[(int)(convertedImageHeight * aspectRatioOfImage), convertedImageHeight];
            int[,] countsOfInfluencingCells = new int[(int)(convertedImageHeight * aspectRatioOfImage), convertedImageHeight];

            for (int widthIndex = 0; widthIndex < imageToConvert.Width; widthIndex++) 
            {
                for(int heightIndex = 0; heightIndex < imageToConvert.Height; heightIndex++)
                {
                    grayscalesOfImage[widthIndex / oneHeightUnit, heightIndex / oneHeightUnit] += imageToConvert.GetPixel(widthIndex, heightIndex).R;
                    countsOfInfluencingCells[widthIndex / oneHeightUnit, heightIndex / oneHeightUnit]++;
                }
            }

            for (int i = 0; i < grayscalesOfImage.GetLength(0); i++)
            {
                for (int j = 0; j < grayscalesOfImage.GetLength(1); j++)
                {
                    if (countsOfInfluencingCells[i, j] == 0)
                        continue;

                    grayscalesOfImage[i, j] = grayscalesOfImage[i, j] / countsOfInfluencingCells[i, j];
                }
            }

            for (int widthIndex = 0; widthIndex < imageToConvert.Width; widthIndex++)
            {
                for (int heightIndex = 0; heightIndex < imageToConvert.Height; heightIndex++)
                {
                    int color = grayscalesOfImage[widthIndex / oneHeightUnit, heightIndex / oneHeightUnit];

                    imageToConvert.SetPixel(widthIndex, heightIndex, Color.FromArgb(color, color, color));
                }
            }

            imageToConvert.Save("newImage2.png");

            char[,] ASCIIImage = ConvertGrayscaleToASCII(grayscalesOfImage, true);

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < grayscalesOfImage.GetLength(1); i++)
            {
                for (int j = 0; j < grayscalesOfImage.GetLength(0); j++)
                {
                    stringBuilder.Append(ASCIIImage[j, i]);
                }
                stringBuilder.Append("\n");
            }

            File.AppendAllText("image.txt", stringBuilder.ToString());

            Console.WriteLine(stringBuilder.ToString());

            return null;
        }
        private static char[,] ConvertGrayscaleToASCII(int[,] grayscales, bool revers)
        {
            char[] symbols = new char[] { '@', '&', '#', 'B', 'G', 'P', '5', 'Y', 'J', '?', '7', '!', '~', '^'};

            int coff = 255/symbols.Length;

            char[,] ASCIIImage = new char[grayscales.GetLength(0), grayscales.GetLength(1)];

            for(int i = 0; i < grayscales.GetLength(0); i++)
            {
                for (int j = 0; j < grayscales.GetLength(1); j++)
                {
                    if(revers)
                        ASCIIImage[i, j] = symbols[symbols.Length - 1 - (int)(grayscales[i, j] / coff)];
                    else
                        ASCIIImage[i, j] = symbols[(int)(grayscales[i, j] / coff)];
                }
            }

            return ASCIIImage;
        }
        private static Bitmap GetGrayscaleImage(Bitmap originalImage)
        {
            Bitmap newbmp = originalImage;

            for (int row = 0; row < originalImage.Width; row++) 
            {
                for (int column = 0; column < originalImage.Height; column++)
                {
                    var colorValue = originalImage.GetPixel(row, column); 
                    var averageValue = ((int)colorValue.R + (int)colorValue.B + (int)colorValue.G) / 3;
                    newbmp.SetPixel(row, column, Color.FromArgb(averageValue, averageValue, averageValue));
                }
            }

            return newbmp;
        }
    }
}