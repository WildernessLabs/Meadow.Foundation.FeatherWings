using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents an Adafruit Led Matrix 8x16 feather wing (HT16K33)
    /// </summary>
    public partial class LedMatrix8x16Wing : IPixelDisplay
    {
        readonly Ht16k33 ht16k33;

        /// <summary>
        /// Returns the color mode
        /// </summary>
        public ColorMode ColorMode => ColorMode.Format1bpp;

        /// <summary>
        /// Color modes supported by the device
        /// </summary>
        public ColorMode SupportedColorModes => ColorMode.Format1bpp;

        /// <summary>
        /// Returns the width of the RGB LED matrix
        /// </summary>
        public int Width => 8;

        /// <summary>
        /// Returns the height of the RGB LED matrix
        /// </summary>
        public int Height => 16;

        /// <summary>
        /// Gets/Sets property to ignore boundaries when drawing outside of the LED matrix
        /// </summary>
        public bool IgnoreOutOfBoundsPixels { get; set; }

        /// <summary>
        /// The pixel buffer that represents the offscreen buffer
        /// Not implemented for this driver
        /// </summary>
        public IPixelBuffer PixelBuffer => this;

        /// <summary>
        /// Creates a LedMatrix8x16Wing driver
        /// </summary>
        /// <param name="i2cBus">The I2CBus used by the CharlieWing</param>
        /// <param name="address">The I2C address</param>
        public LedMatrix8x16Wing(II2cBus i2cBus, byte address = (byte)Ht16k33.Addresses.Default)
        {
            ht16k33 = new Ht16k33(i2cBus, address);
        }

        /// <summary>
        /// Clear the RGB LED Matrix offscreen buffer
        /// </summary>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Clear(bool updateDisplay = false)
        {
            ht16k33.ClearDisplay();
        }

        /// <summary>
        /// Clear the RGB LED Matrix offscreen buffer
        /// </summary>
        public void Clear()
        {
            ht16k33.ClearDisplay();
        }

        /// <summary>
        /// Turn on an RGB LED with the specified color on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="color">The color to draw normalized to black/off or white/on</param>
        public void DrawPixel(int x, int y, Color color)
        {
            DrawPixel(x, y, color.Color1bpp);
        }

        /// <summary>
        /// Turn on a LED on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="colored">Led is on if true, off if false</param>
        public void DrawPixel(int x, int y, bool colored)
        {
            if (IgnoreOutOfBoundsPixels)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                { return; }
            }

            if (y < 8)
            {
                y *= 2;
            }
            else
            {
                y = (y - 8) * 2 + 1;
            }
            ht16k33.SetLed((byte)(y * Width + x), colored);
        }

        /// <summary>
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        public void InvertPixel(int x, int y)
        {
            if (IgnoreOutOfBoundsPixels)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                { return; }
            }

            if (y < 8)
            {
                y *= 2;
            }
            else
            {
                y = (y - 8) * 2 + 1;
            }

            ht16k33.ToggleLed((byte)(y * Width + x));
        }

        /// <summary>
        /// Draw a buffer to the display
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="displayBuffer">The display buffer to draw to the display</param>
        public void WriteBuffer(int x, int y, IPixelBuffer displayBuffer)
        {
            for (int i = 0; i < displayBuffer.Width; i++)
            {
                for (int j = 0; j < displayBuffer.Height; j++)
                {
                    DrawPixel(x + i, y + j, displayBuffer.GetPixel(i, j));
                }
            }
        }

        /// <summary>
        /// Fill the display buffer to a normalized color
        /// </summary>
        /// <param name="fillColor">The clear color which will be normalized to black/off or white/on</param>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Fill(Color fillColor, bool updateDisplay = false)
        {
            Fill(0, 0, Width, Height, fillColor);

            if (updateDisplay) Show();
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
            bool isColored = fillColor.Color1bpp;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    DrawPixel(i, j, isColored);
                }
            }
        }

        /// <summary>
        /// Show changes on the display
        /// </summary>
        public void Show()
        {
            ht16k33.UpdateDisplay();
        }

        /// <summary>
        /// Update a region of the display from the offscreen buffer 
        /// Currently always redraws the entire display
        /// </summary>
        /// <param name="left">The left bounding position in pixels</param>
        /// <param name="top">The top bounding position in pixels</param>
        /// <param name="right">The right bounding position in pixels</param>
        /// <param name="bottom">The bottom bounding position in pixels</param>
        public void Show(int left, int top, int right, int bottom)
        {   //ToDo - should be possible - check UpdateDisplay and adjust starting address
            Show();
        }
    }
}