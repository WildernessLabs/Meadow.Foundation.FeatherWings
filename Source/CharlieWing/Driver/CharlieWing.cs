using System;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents an Adafruit CharliePlex 15x7 feather wing
    /// </summary>
    public class CharlieWing : IGraphicsDisplay
    {
        /// <summary>
        /// Is31fl3731 object to manage the leds
        /// </summary>
        protected readonly Is31fl3731 iS31FL3731;

        /// <summary>
        /// Color mode of display
        /// </summary>
        public ColorMode ColorMode => ColorMode.Format8bppGray;

        /// <summary>
        /// Width of display in pixels
        /// </summary>
        public int Width => 15;

        /// <summary>
        /// Height of display in pixels
        /// </summary>
        public int Height => 7;

        /// <summary>
        /// The Is31fl3731 active frame 
        /// </summary>
        public byte Frame { get; set; }

        /// <summary>
        /// The pixel buffer that represents the off-screen buffer
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
            iS31FL3731 = new Is31fl3731(i2cBus, address);
            iS31FL3731.Initialize();

            for (byte i = 0; i <= 7; i++)
            {
                iS31FL3731.SetLedState(i, true);
                iS31FL3731.Clear(i);
            }
        }

        /// <summary>
        /// Clear the display buffer
        /// </summary>
        /// <param name="updateDisplay">Force a display update if true, false to clear the buffer</param>
        public void Clear(bool updateDisplay = false)
        {
            iS31FL3731.Clear(Frame);
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
            if (x > 7)
            {
                x = 15 - x;
                y += 8;
            }
            else
            {
                y = 7 - y;
            }

            //Swap
            (y, x) = (x, y);

            iS31FL3731.SetLedPwm(Frame, (byte)(x + y * 16), brightness);
        }

        /// <summary>
        /// Invert the color of the pixel at the given location
        /// </summary>
        /// <param name="x">The x position in pixels 0 indexed from the left</param>
        /// <param name="y">The y position in pixels 0 indexed from the top</param>
        public void InvertPixel(int x, int y)
        {
            throw new NotImplementedException();
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
                for (int j = 0;j < Height; j++)
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
        /// Update the display from the off-screen buffer
        /// </summary>
        public void Show()
        {
            iS31FL3731.DisplayFrame(Frame);
        }

        /// <summary>
        /// Update a region of the display from the off-screen buffer 
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
        /// Update the display from a specific iS31FL3731 frame
        /// </summary>
        /// <param name="frame">The frame to show (0-7)</param>
        public void Show(byte frame)
        {   
            iS31FL3731.DisplayFrame(frame);
        }
    }
}