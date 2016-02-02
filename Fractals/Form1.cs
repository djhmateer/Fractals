using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Fractals
{
    // First run through to try and understand
    // 0  - got into source control :-)
    //   - read through notes http://23programs.blogspot.co.uk/2012/03/c-mandelbrot-set-fractal.html
    //   - clicked link to wikipedia: https://en.wikipedia.org/wiki/Mandelbrot_set
    //   -  hmm.. complex numbers, and maths.. unknown at the moment.. need good explanation in the code
    public partial class Form1 : Form
    {
        static double currentMaxR;
        static double currentMinR;
        static double currentMaxI;
        static double currentMinI;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Form1.Width = 708;
            //ClientSize.Height = 509;

            // This aspect ratio is good
            pictureBox.Width = 704;
            pictureBox.Height = 469;
            Bitmap img = GetMandelbrotImage(pictureBox, 2, -2, 2, -2);
            pictureBox.Image = img;
        }

        static Bitmap GetMandelbrotImage(PictureBox pictureBox, double maxr, double minr, double maxi, double mini)
        {
            currentMaxR = maxr;
            currentMaxI = maxi;
            currentMinR = minr;
            currentMinI = mini;
            var img = new Bitmap(pictureBox.Width, pictureBox.Height);
            double xjump = (maxr - minr)/Convert.ToDouble(img.Width);
            double yjump = (maxi - mini)/Convert.ToDouble(img.Height);
            int loopmax = 1000; // the 'resolution'.. lower means faster..
            for (int x = 0; x < img.Width; x++)
            {
                double cx = (xjump*x) - Math.Abs(minr);
                for (int y = 0; y < img.Height; y++)
                {
                    double zx = 0;
                    double zy = 0;
                    var cy = (yjump*y) - Math.Abs(mini);
                    var loopgo = 0;
                    while (zx*zx + zy*zy <= 4 && loopgo < loopmax)
                    {
                        loopgo++;
                        var tempzx = zx;
                        zx = (zx*zx) - (zy*zy) + cx;
                        zy = (2*tempzx*zy) + cy;
                    }
                    if (loopgo != loopmax)
                        img.SetPixel(x, y, Color.FromArgb(loopgo % 128 * 2, loopgo % 32 * 7, loopgo % 16 * 14));
                    else
                        img.SetPixel(x, y, Color.Black);
                }
            }
            return img;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int ex = e.X;
            int ey = e.Y;
            double currentxjump = ((currentMaxR - currentMinR)/Convert.ToDouble(pictureBox.Width));
            double currentyjump = ((currentMaxI - currentMinI)/Convert.ToDouble(pictureBox.Height));

            int zoomx = pictureBox.Width/5;
            int zoomy = pictureBox.Height/5;
            Bitmap img = GetMandelbrotImage(pictureBox, ((ex + zoomx)*currentxjump) - Math.Abs(currentMinR),
                ((ex - zoomx)*currentxjump) - Math.Abs(currentMinR), ((ey + zoomy)*currentyjump) - Math.Abs(currentMinI),
                ((ey - zoomy)*currentyjump) - Math.Abs(currentMinI));
            pictureBox.Image.Dispose();
            pictureBox.Image = img;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Bitmap img = GetMandelbrotImage(pictureBox, 2, -2, 2, -2);
            pictureBox.Image.Dispose();
            pictureBox.Image = img;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filename = @"\Fractals";
            var filetype = @".jpg";
            int i = 0;
            while (File.Exists(path + filename + i + filetype)) i++;
            pictureBox.Image.Save(path + filename + i + filetype);
            MessageBox.Show("Saved To Desktop");
        }
    }
}