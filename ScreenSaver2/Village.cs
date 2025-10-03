using System.Drawing;

namespace ScreenSaver2
{
    public partial class Village : Form
    {
        Point imagePoint;
        List<Point> snowflakePoints = new List<Point>();
        System.Windows.Forms.Timer timer;
        Image snowflake;
        Bitmap resizeSnowflake;

        List<bool> snowflakeActive;
        List<int> snowflakeDelays;
        int snowflakeCounter = 0;
        Random random = new Random();
        int pointX = -1, pointY = -1;

        public Village()
        {
            InitializeComponent();

            using (var msShrek = new MemoryStream(Properties.Resources.Shrek))
            {
                this.BackgroundImage = Image.FromStream(msShrek);
            }
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;

            int newWidth = 45;
            int newHeight = 25;

            using (var msSnowflake = new MemoryStream(Properties.Resources.Snowflake))
            {
                snowflake = Image.FromStream(msSnowflake);
            }
            resizeSnowflake = new Bitmap(snowflake, new Size(newWidth, newHeight));

            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 85;

            snowflakeActive = new List<bool>();
            snowflakeDelays = new List<int>();

            this.KeyPreview = true;
            this.KeyDown += Village_KeyDown;

            this.Paint += Village_Paint_1;
            this.Load += Village_Load;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            snowflakeCounter++;
            for (int i = 0; i < snowflakePoints.Count; i++)
            {
                if (!snowflakeActive[i] && snowflakeCounter >= snowflakeDelays[i])
                {
                    snowflakeActive[i] = true;
                }
                if (snowflakeActive[i])
                {
                    Point point = snowflakePoints[i];
                    point.Y += 2;
                    snowflakePoints[i] = point;
                    if (point.Y > ClientRectangle.Height)
                    {
                        point.Y = random.Next(-500, -50);
                        point.X = random.Next(0, ClientRectangle.Width);
                        snowflakePoints[i] = point;
                    }
                }
            }
            Invalidate();
        }

        private void Village_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void Village_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void Village_Paint_1(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < snowflakePoints.Count; i++)
            {
                if (snowflakeActive[i])
                {
                    e.Graphics.DrawImage(resizeSnowflake, snowflakePoints[i]);
                }
            }

            var infoText = "Press ESC to exit";
            var font = new Font("Arial", 12, FontStyle.Bold);
            var brush = new SolidBrush(Color.White);
            var textSize = e.Graphics.MeasureString(infoText, font);
            var textLocation = new PointF(10, ClientRectangle.Height - textSize.Height - 10);

            e.Graphics.DrawString(infoText, font, brush, textLocation);
        }

        private void Village_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 160; i++)
            {
                pointX = random.Next(0, ClientRectangle.Width);
                pointY = random.Next(-500, -50);
                imagePoint = new Point(pointX, pointY);

                snowflakeActive.Add(false);
                snowflakeDelays.Add(random.Next(0, 300));
                snowflakePoints.Add(imagePoint);
            }
            timer.Start();
        }

        private void Village_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}