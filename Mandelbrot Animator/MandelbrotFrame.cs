using System.Diagnostics;
using System.Drawing;
using Wacton.Unicolour;

namespace Mandelbrot_Animator
{
    internal class MandelbrotFrame(int canvasWidth = 1920, int canvasHeight = 1080, double centreRe = 0, double centreIm = 0, float zoom = 1, int maxIterations = 100, int pixelResolution = 3)
    {
        int[, , ] iterationMap;

        public Bitmap processImage()
        {
            iterationMap = new int[canvasWidth, canvasHeight, pixelResolution * pixelResolution];

            float dx = (float)1 / pixelResolution;
            float halfDx = dx / 2;

            for (int x = 0; x < canvasWidth; x++)
            {
                for (int y = 0; y < canvasHeight; y++)
                {
                    for (int px = 0; px < pixelResolution; px++)
                    {
                        for (int py = 0; py < pixelResolution; py++)
                        {
                            float xPosition, yPosition;
                            double real, imaginary;
                            
                            xPosition = px * dx + halfDx + x;
                            yPosition = py * dx + halfDx + y;

                            (real, imaginary) = canvasToComplexPlane(xPosition, yPosition);
                            int iterations = getIterations(real, imaginary);
                            iterationMap[x, y, py + px * pixelResolution] = iterations;
                        }
                    }
                }
            }


            Bitmap img = new Bitmap(canvasWidth, canvasHeight);

            for (int x = 0; x < canvasWidth; x++)
            {
                for (int y = 0; y < canvasHeight; y++)
                {
                    Unicolour[] colours = new Unicolour[pixelResolution * pixelResolution];
                    for (int i = 0; i < pixelResolution * pixelResolution; i++)
                    {
                        colours[i] = getColour(iterationMap[x, y, i]);
                    }


                    Unicolour mixed = mixColours(colours);

                    int red = (int)(mixed.Rgb.R * 255);
                    int green = (int)(mixed.Rgb.G * 255);
                    int blue = (int)(mixed.Rgb.B * 255);



                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

            return img;
        }

        private Unicolour mixColours(Unicolour[] colours)
        {
            Unicolour mixedColour = colours[0];

            for (int i = 1; i < colours.Length; i++)
            {
                mixedColour = mixedColour.Mix(ColourSpace.Hsb, colours[i]);
            }
            return mixedColour;
        }


        private Unicolour getColour(int iterations)
        {
            double hue = (double)iterations / maxIterations * 360 + 240;
            double saturation = 1;
            double brightness;
            if(iterations == maxIterations)
            {
                return new Unicolour(ColourSpace.Hsb, 0, 0, 0);
            }
            else if (iterations < maxIterations /  3)
            {
                brightness = iterations / ((double)maxIterations / 3);
            }
            else
            {
                brightness = 1;
            }
            return new Unicolour(ColourSpace.Hsb, hue, saturation, brightness);

        }



        private int getIterations(double real, double imaginary)
        {
            double Za = 0;
            double Zb = 0;
            for (int i = 0; i < maxIterations; i++)
            {
                double newZa = Za * Za - Zb * Zb;
                double newZb = 2 * Za * Zb;

                Za = newZa + real;
                Zb = newZb + imaginary;

                if (Za * Za + Zb * Zb > 4)
                {
                    return i;
                }
            }
            return maxIterations;
        }

        private (double, double) canvasToComplexPlane(float x, float y)
        {
            double complexWidth, complexHeight;
            double shortSide = (double)4 / zoom;

            if (canvasWidth > canvasHeight)
            {
                complexWidth = shortSide * canvasWidth / canvasHeight;
                complexHeight = shortSide;
            }
            else
            {
                complexWidth = shortSide;
                complexHeight = shortSide * canvasHeight / canvasWidth;
            }

            double xRatio = x / canvasWidth;
            double yRatio = 1 - y / canvasHeight;

            double real = xRatio * complexWidth;
            double imaginary = yRatio * complexHeight;

            real += centreRe - complexWidth / 2;
            imaginary += centreIm - complexHeight / 2;

            return (real, imaginary);
        }

    }
}
