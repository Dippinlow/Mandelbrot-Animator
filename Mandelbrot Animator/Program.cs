using System.Drawing;

namespace Mandelbrot_Animator
{
    internal class Program
    {
        static void Main()
        {
            int frameRate = 25;
            float secondsPerZoomDouble = 1;


            int framesPerZoom = (int)Math.Round(secondsPerZoomDouble * frameRate);




            for (int i = 0; i < 0; i++) {

                float zoom = getZoom(i, framesPerZoom);
                int maxIt = getMaxIt(zoom);
                
                MandelbrotFrame m = new MandelbrotFrame(
                    default, default,
                    -0.7585453376717782, - 0.06508398694610079,
                    zoom,
                    maxIt, 
                    default);

                Bitmap img = m.processImage();
                

                img.Save($"C:\\ImageSpam\\{numberName(i)}.png");
            }   
        }


        private static float getZoom(int frameNumber, int framesPerZoomDouble)
        {
            return (float)frameNumber / framesPerZoomDouble * 2;

        }

        private static int getMaxIt(float zoom)
        {
            float maxIt = 10 + zoom / 2;
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
