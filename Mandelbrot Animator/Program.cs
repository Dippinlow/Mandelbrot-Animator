using System.Diagnostics;
using System.Drawing;

namespace Mandelbrot_Animator
{
    internal class Program
    {
        static void Main()
        {
            int frameRate = 25;
            float secondsPerZoomDouble = 1;
            float animationTime = 20;

            double centreRe = -0.7585453376717782;
            double centreIm = -0.06508398694610079;

            int framesPerZoomDouble = (int)Math.Round(secondsPerZoomDouble * frameRate);

            int totalFrames = (int)Math.Round(animationTime * frameRate);

            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < totalFrames; i++){

                float zoom = getZoom(i, framesPerZoomDouble);

                int maxIt = getMaxIt(zoom);
                
                MandelbrotFrame m = new MandelbrotFrame(
                    1920, 1080, 
                    centreRe, 
                    centreIm, 
                    zoom, 
                    maxIt, 
                    2
                    );

                stopwatch.Restart();
                Bitmap img = m.processImage();
                stopwatch.Stop();

                float processingTime = stopwatch.ElapsedMilliseconds / 1000;
                string frameName = numberName(i);
                string frameNameLong = frameName + " \\ " + numberName(totalFrames);


                img.Save($"C:\\AnimationFrames\\1\\{frameName}.png");

                img = addData(img, centreRe, centreIm, zoom, maxIt, processingTime, frameNameLong);
                img.Save($"C:\\AnimationFrames\\2\\{frameName}.png");

                Console.WriteLine(numberName(i) + " / " + numberName(totalFrames));


            }   
        }


        private static Bitmap addData(Bitmap img, double centreRe, double centreIm, float zoom, int maxIt, float processingTime, string name)
        {
            Graphics g = Graphics.FromImage(img);
            Font f = new Font("Comic sans", 24);
            Brush b1 = new SolidBrush(Color.Black);
            Brush b2 = new SolidBrush(Color.White);

            g.FillRectangle(b2, 0, 0, 520, 210);

            string text = $"Frame: {name}\n"
                        + $"CentreRe: {centreRe}\n"
                        + $"CentreIm: {centreIm}\n"
                        + $"Zoom: {zoom}\n"
                        + $"Max Iteration: {maxIt}\n"
                        + $"Time to process frame: {processingTime}s";

            g.DrawString(text, f, b1, 0, 0);
            return img;
        }

        private static float getZoom(int frameNumber, int framesPerZoomDouble)
        {
            float power = (float)frameNumber / framesPerZoomDouble;

            return MathF.Pow(2, power);

        }

        private static int getMaxIt(float zoom)
        {
            float maxIt = 100 + zoom / 2;
            return (int)Math.Round(maxIt);
        }

        private static string numberName(int number)
        {
            if (number < 10)
            {
                return "000" + number;
            }
            if (number < 100)
            {
                return "00" + number;
            }
            if (number < 1000)
            {
                return "0" + number;
            }
            return "" + number;
        }


    }
}
