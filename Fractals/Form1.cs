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
        static double currentmaxr = 0;
        static double currentminr = 0;
        static double currentmaxi = 0;
        static double currentmini = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap img = GetMandelbrotSetImage(pictureBox, 2, -2, 2, -2);
            pictureBox.Image = img;
        }
        static Bitmap GetMandelbrotSetImage(PictureBox pictureBox1, double maxr, double minr, double maxi, double mini)
        {
            currentmaxr = maxr;
            currentmaxi = maxi;
            currentminr = minr;
            currentmini = mini;
            Bitmap img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            double zx = 0;
            double zy = 0;
            double cx = 0;
            double cy = 0;
            double xjump = ((maxr - minr) / Convert.ToDouble(img.Width));
            double yjump = ((maxi - mini) / Convert.ToDouble(img.Height));
            double tempzx = 0;
            int loopmax = 1000;
            int loopgo = 0;
            for (int x = 0; x < img.Width; x++)
            {
                cx = (xjump * x) - Math.Abs(minr);
                for (int y = 0; y < img.Height; y++)
                {
                    zx = 0;
                    zy = 0;
                    cy = (yjump * y) - Math.Abs(mini);
                    loopgo = 0;
                    while (zx * zx + zy * zy <= 4 && loopgo < loopmax)
                    {
                        loopgo++;
                        tempzx = zx;
                        zx = (zx * zx) - (zy * zy) + cx;
                        zy = (2 * tempzx * zy) + cy;
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
            double currentxjump = ((currentmaxr - currentminr) / Convert.ToDouble(pictureBox.Width));
            double currentyjump = ((currentmaxi - currentmini) / Convert.ToDouble(pictureBox.Height));

            int zoomx = pictureBox.Width / 5;
            int zoomy = pictureBox.Height / 5;
            Bitmap img = GetMandelbrotSetImage(pictureBox, ((ex + zoomx) * currentxjump) - Math.Abs(currentminr), ((ex - zoomx) * currentxjump) - Math.Abs(currentminr), ((ey + zoomy) * currentyjump) - Math.Abs(currentmini), ((ey - zoomy) * currentyjump) - Math.Abs(currentmini));
            pictureBox.Image.Dispose();
            pictureBox.Image = img;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Bitmap img = GetMandelbrotSetImage(pictureBox, 2, -2, 2, -2);
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


