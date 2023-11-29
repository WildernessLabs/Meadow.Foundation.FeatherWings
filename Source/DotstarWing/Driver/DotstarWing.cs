using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using static Meadow.Foundation.Leds.Apa102;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's Dotstar feather wing 12x6
    /// </summary>
    public class DotstarWing : IGraphicsDisplay
    {
        readonly Apa102 ledMatrix;

        /// <summary>
        /// Returns the color mode
        /// </summary>
        public ColorMode ColorMode => ColorMode.Format12bppRgb444;

        /// <summary>
        /// Color modes supported by the device
        /// </summary>
        public ColorMode SupportedColorModes => ColorMode.Format12bppRgb444;

        /// <summary>
        /// Returns the width of the RGB LED matrix
        /// </summary>
        public int Width => 12;

        /// <summary>
        /// Returns the height of the RGB LED matrix
        /// </summary>
        public int Height => 6;

        /// <summary>
        /// Get/Sets the RGB LED Matrix brightness
        /// </summary>
        public float Brightness
        {
            get => ledMatrix.Brightness;
            set => ledMatrix.Brightness = value;
        }

        /// <summary>
        /// Get the offscreen pixel buffer
        /// </summary>
        public IPixelBuffer PixelBuffer => ledMatrix.PixelBuffer;

        /// <summary>
        /// Creates a DotstarWing driver
        /// </summary>
        /// <param name="spiBus">The SPI bus used by the Dotstar Wing</param>
        public DotstarWing(ISpiBus spiBus)
        {
            ledMatrix = new Apa102(spiBus, 72, PixelOrder.BGR);
        }

        /// <summary>
        /// Clear the RGB LED Matrix buffer
        /// </summary>
        /// <param name="updateDisplay">If true, update the display, if false, only clear the buffer</param>
        public void Clear(bool updateDisplay = false)
        {
            ledMatrix.Clear(updateDisplay);
        }

        /// <summary>
        /// Turn on an RGB LED with the specified color on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="color">The color to draw normalized to black/off or white/on</param>
        public void DrawPixel(int x, int y, Color color)
        {
            int minor = x;
            int major = Height - 1 - y;

            int pixelOffset = (major * Width) + minor;

            if (pixelOffset >= 0 && pixelOffset < Height * Width)
            {
                ledMatrix.SetLed(pixelOffset, color);
            }
        }

        /// <summary>
        /// Turn on a LED on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="colored">Led is on if true, off if false</param>
        public void DrawPixel(int x, int y, bool colored)
            => DrawPixel(x, y, colored ? Color.White : Color.Black);

        /// <summary>
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        public void InvertPixel(int x, int y)
            => ledMatrix.InvertPixel(x, y);

        /// <summary>
        /// Draw a buffer to the display
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="displayBuffer">The display buffer to draw to the CharlieWing</param>
        public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
            => ledMatrix.WriteBuffer(x, y, displayBuffer);

        /// <summary>
        /// Fill the display buffer to a normalized color
        /// </summary>
        /// <param name="fillColor">The clear color which will be normalized to black/off or white/on</param>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Fill(Color fillColor, bool updateDisplay = false)
            => ledMatrix.Fill(fillColor, updateDisplay);

        /// <summary>
        /// Fill the display
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="width">The width to fill in pixels</param>
        /// <param name="height">The height to fill in pixels</param>
        /// <param name="fillColor">The fillColor color which will be normalized to black/off or white/on</param>
        public void Fill(int x, int y, int width, int height, Color fillColor)
            => ledMatrix.Fill(x, y, width, height, fillColor);

        /// <summary>
        /// Update the display from the offscreen buffer
        /// </summary>
        public void Show()
            => ledMatrix.Show();

        /// <summary>
        /// Update a region of the display from the offscreen buffer 
        /// </summary>
        /// <param name="left">The left bounding position in pixels</param>
        /// <param name="top">The top bounding position in pixels</param>
        /// <param name="right">The right bounding position in pixels</param>
        /// <param name="bottom">The bottom bounding position in pixels</param>
        public void Show(int left, int top, int right, int bottom)
            => ledMatrix.Show(left, top, right, bottom);
    }
}