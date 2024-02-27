using Meadow.Foundation.Graphics.Buffers;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruits NeoPixel FeatherWing
    /// </summary>
    public class NeoPixelWing : IPixelDisplay
    {
        /// <summary>
        /// Color mode of display
        /// </summary>
        public ColorMode ColorMode => ColorMode.Format24bppGrb888;

        /// <summary>
        /// Width of display in pixels
        /// </summary>
        public int Width => 8;

        /// <summary>
        /// Height of display in pixels
        /// </summary>
        public int Height => 4;

        /// <summary>
        /// The pixel buffer that represents the offscreen buffer
        /// Not implemented for this driver
        /// </summary>
        public IPixelBuffer PixelBuffer { get; protected set; }

        /// <summary>
        /// Color modes supported by the device
        /// </summary>
        public ColorMode SupportedColorModes => ColorMode.Format24bppRgb888;

        /// <summary>
        /// Returns Ws2812 instance
        /// </summary>
        public Ws2812 Leds { get; protected set; }

        /// <summary>
        /// Creates a NeoPixelWing driver instance
        /// </summary>
        /// <param name="spiBus">The SPI bus connected to the wing</param>
        public NeoPixelWing(ISpiBus spiBus)
        {
            Leds = new Ws2812(spiBus, 32);
            PixelBuffer = new BufferRgb888(8, 4);
        }

        /// <summary>
        /// Clear the display buffer
        /// </summary>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Clear(bool updateDisplay = false)
        {
            PixelBuffer.Clear();

            if (updateDisplay)
            {
                Show();
            }
        }

        /// <summary>
        /// Turn on an RGB LED with the specified color on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="color">The color to draw normalized to black/off or white/on</param>
        public void DrawPixel(int x, int y, Color color)
        {
            PixelBuffer.SetPixel(x, y, color);
        }

        /// <summary>
        /// Turn on a LED on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="colored">Led is on if true, off if false</param>
        public void DrawPixel(int x, int y, bool colored)
        {
            DrawPixel(x, y, colored ? Color.White : Color.Black);
        }

        /// <summary>
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        public void InvertPixel(int x, int y)
        {
            PixelBuffer?.InvertPixel(x, y);
        }

        /// <summary>
        /// Draw a buffer to the display
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="displayBuffer">The display buffer to draw to the CharlieWing</param>
        public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
        {
            PixelBuffer.WriteBuffer(x, y, displayBuffer);
        }

        /// <summary>
        /// Fill the display buffer to a normalized color
        /// </summary>
        /// <param name="fillColor">The clear color which will be normalized to black/off or white/on</param>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Fill(Color fillColor, bool updateDisplay = false)
        {
            PixelBuffer.Fill(fillColor);

            if (updateDisplay)
            { Show(); }
        }

        /// <summary>
        /// Fill the display
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="width">The width to fill in pixels</param>
        /// <param name="height">The height to fill in pixels</param>
        /// <param name="fillColor">The fillColor color which will be normalized to black/off or white/on</param>
        public void Fill(int x, int y, int width, int height, Color fillColor)
        {
            PixelBuffer.Fill(height, x, y, width, fillColor);
        }

        /// <summary>
        /// Update the display from the offscreen buffer
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < PixelBuffer.Width; i++)
            {
                for (int j = 0; j < PixelBuffer.Height; j++)
                {
                    Leds.SetLed(i + j * Width, PixelBuffer.GetPixel(i, j));
                }
            }

            Leds.Show();
        }

        /// <summary>
        /// Update a region of the display from the offscreen buffer 
        /// </summary>
        /// <param name="left">The left bounding position in pixels</param>
        /// <param name="top">The top bounding position in pixels</param>
        /// <param name="right">The right bounding position in pixels</param>
        /// <param name="bottom">The bottom bounding position in pixels</param>
        public void Show(int left, int top, int right, int bottom)
        {
            Show();
        }
    }
}