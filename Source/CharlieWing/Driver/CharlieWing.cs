using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents an Adafruit CharliePlex 15x7 feather wing
    /// </summary>
    public class CharlieWing : IPixelDisplay
    {
        private const int WidthInPixels = 15;
        private const int HeightInPixels = 7;
        private const int MaxFrames = 7;

        /// <summary>
        /// Is31fl3731 object to manage the LEDs
        /// </summary>
        protected readonly Is31fl3731 is31Fl3731;

        /// <summary>
        /// Color mode of display
        /// </summary>
        public ColorMode ColorMode => ColorMode.Format8bppGray;

        /// <summary>
        /// Width of display in pixels
        /// </summary>
        public int Width => WidthInPixels;

        /// <summary>
        /// Height of display in pixels
        /// </summary>
        public int Height => HeightInPixels;

        /// <summary>
        /// The Is31fl3731 active frame 
        /// </summary>
        public byte Frame { get; set; }

        /// <summary>
        /// The pixel buffer that represents the offscreen buffer
        /// Not implemented for this driver
        /// </summary>
        public IPixelBuffer PixelBuffer => throw new NotImplementedException();

        /// <summary>
        /// Color modes supported by the device
        /// </summary>
        public ColorMode SupportedColorModes => ColorMode.Format8bppGray;

        /// <summary>
        /// Creates a CharlieWing driver
        /// </summary>
        /// <param name="i2cBus">The I2CBus used by the CharlieWing</param>
        /// <param name="address">The I2C address</param>
        public CharlieWing(II2cBus i2cBus, byte address = (byte)Is31fl3731.Addresses.Default)
        {
            is31Fl3731 = new Is31fl3731(i2cBus, address);
            is31Fl3731.Initialize();

            for (byte i = 0; i <= MaxFrames; i++)
            {
                is31Fl3731.SetLedState(i, true);
                is31Fl3731.Clear(i);
            }
        }

        /// <summary>
        /// Clear the display buffer
        /// </summary>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Clear(bool updateDisplay = false)
        {
            is31Fl3731.Clear(Frame);
        }

        /// <summary>
        /// Turn on an RGB LED with the specified color on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="color">The color to draw normalized to black/off or white/on</param>
        public void DrawPixel(int x, int y, Color color)
        {
            DrawPixel(x, y, color.Color8bppGray);
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
        /// Turn on LED with the specified brightness on (x,y) coordinates
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="brightness">The led brightness from 0-255</param>
        public void DrawPixel(int x, int y, byte brightness)
        {
            if (x < 0 || x >= WidthInPixels || y < 0 || y >= HeightInPixels)
            {
                throw new ArgumentOutOfRangeException($"Pixel coordinates ({x}, {y}) are out of bounds.");
            }

            if (x > 7)
            {
                x = 15 - x;
                y += 8;
            }
            else
            {
                y = 7 - y;
            }

            // Swap
            (y, x) = (x, y);

            is31Fl3731.SetLedPwm(Frame, (byte)(x + y * 16), brightness);
        }

        /// <summary>
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        public void InvertPixel(int x, int y)
        {
            byte currentBrightness = is31Fl3731.GetLedPwm(Frame, (byte)(x + y * 16));
            byte invertedBrightness = (byte)(255 - currentBrightness);
            DrawPixel(x, y, invertedBrightness);
        }

        /// <summary>
        /// Draw a buffer to the display
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        /// <param name="displayBuffer">The display buffer to draw to the CharlieWing</param>
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
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    DrawPixel(i, j, fillColor);
                }
            }
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
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    DrawPixel(x + i, y + j, fillColor);
                }
            }
        }

        /// <summary>
        /// Update the display from the offscreen buffer
        /// </summary>
        public void Show()
        {
            is31Fl3731.DisplayFrame(Frame);
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

        /// <summary>
        /// Update the display from a specific Is31fl3731 frame
        /// </summary>
        /// <param name="frame">The frame to show (0-7)</param>
        public void Show(byte frame)
        {
            is31Fl3731.DisplayFrame(frame);
        }
    }
}
