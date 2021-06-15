using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileUnpadder
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap input = new Bitmap(args[0]);
            int tileSize = Convert.ToInt32(args[1]);
            int margin = Convert.ToInt32(args[2]);
            string outputPath = Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileNameWithoutExtension(args[0]) + "_unpadded" + Path.GetExtension(args[0]));
            Bitmap output = new Bitmap(input.Width, input.Height);

            int newWidth = 0;
            int newHeight = 0;

            for (int x = 0; ((x * tileSize) + (x * margin)) < input.Width; x++)
            {
                for (int y = 0; ((y * tileSize) + (y * margin)) < input.Height; y++)
                {
                    Rectangle crop = new Rectangle(new Point((x * margin) + (x * tileSize), (y * margin) + (y * tileSize)), new Size(tileSize, tileSize));
                    CopyRegionIntoImage(input, crop, ref output, new Rectangle(new Point(x * tileSize, y * tileSize), new Size(tileSize, tileSize)));
                    if (x == 0)
                        newHeight += tileSize;
                }
                newWidth += tileSize;
            }

            output.Clone(new Rectangle(new Point(0, 0), new Size(newWidth, newHeight)), output.PixelFormat).Save(outputPath);
        }

        // https://stackoverflow.com/a/9616789
        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }
    }
}
