using System.Diagnostics;
using System.Drawing;

namespace Mandelbrot_Animator
{
    internal class Program
    {
        static void Main()
        {


            


            int frameRate = 24;
            //float secondsPerZoomDouble = 1;
            float animationTime = 4;

            double centreRe = 0;
            double centreIm = 0;

            //int framesPerZoomDouble = (int)Math.Round(secondsPerZoomDouble * frameRate);
            float zoom = 1;
            int maxIt = 50;
            int totalFrames = (int)Math.Round(animationTime * frameRate);


            double[] randomColours = new double[maxIt + 1];
            Random rand = new Random();

            for (int i = 0; i < maxIt; i++)
            {
                randomColours[i] = rand.NextDouble() * 360;
            }


            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < totalFrames; i++){

                //float zoom = getZoom(i, framesPerZoomDouble);

                //int maxIt = getMaxIt(i);
                float check = (float)i / totalFrames * 4;
                MandelbrotFrame m = new MandelbrotFrame(
                    randomColours,
                    1920, 1080, 
                    centreRe, 
                    centreIm, 
                    zoom, 
                    maxIt, 
                    3,
                    check
                    );

                stopwatch.Restart();
                Bitmap img = m.processImage();
                stopwatch.Stop();

                float processingTime = stopwatch.ElapsedMilliseconds / 1000;
                string frameName = numberName(i);
                string frameNameLong = frameName + " / " + numberName(totalFrames);


                img.Save($"C:\\AnimationFrames2\\5\\{frameName}.png");

                img = addData(img, centreRe, centreIm, check, maxIt, processingTime, frameNameLong);
                img.Save($"C:\\AnimationFrames2\\6\\{frameName}.png");

                Console.WriteLine(numberName(i) + " / " + numberName(totalFrames));


            }   
        }


        private static Bitmap addData(Bitmap img, double centreRe, double centreIm, float check, int maxIt, float processingTime, string name)
        {
            Graphics g = Graphics.FromImage(img);
            Font f = new Font("Comic sans", 24);
            Brush b1 = new SolidBrush(Color.Black);
            Brush b2 = new SolidBrush(Color.White);

            g.FillRectangle(b2, 0, 0, 510, 215);

            string text = $"Frame: {name}\n"
                        + $"CentreRe: {centreRe}\n"
                        + $"CentreIm: {centreIm}\n"
                        + $"Check: {check}\n"
                        + $"Max Iterations: {maxIt}\n"
                        + $"Time to process frame: {processingTime}s";

            g.DrawString(text, f, b1, 0, 0);
            return img;
        }

        private static float getZoom(int frameNumber, int framesPerZoomDouble)
        {
            float power = (float)frameNumber / framesPerZoomDouble;

            return MathF.Pow(2, power);

        }

        private static int getMaxIt(int frameNumber)
        {
            //float maxIt = 100 + zoom / 4;
            //return (int)Math.Round(maxIt);

            float maxIt = (float)9.8 * frameNumber + 100;
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
