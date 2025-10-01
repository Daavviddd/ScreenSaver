using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class Village : Form
    {
        Point imagePoint;
        List<Point> snowflakePoints = new List<Point>();
        System.Windows.Forms.Timer timer;
        Image snowflake;
        Bitmap ResizeSnowflake;
        int deltaY = 1;
        Image scene;

        List<bool> snowflakeActive;
        List<int> snowflakeDelays;
        int snowflakeCounter = 0;
        Random random = new Random();
        int pointX = -1, pointY = -1;
        public Village()
        {
            InitializeComponent();

            this.BackgroundImage = Properties.Resources.Shrek;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;

            int newWindth = 45;
            int newHight = 25;

            

            snowflake = Properties.Resources.Snowflake;

            ResizeSnowflake = new Bitmap(snowflake, new Size(newWindth, newHight));

            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 85;

            snowflakeActive = new List<bool>();
            snowflakeDelays = new List<int>();
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
            scene = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void Village_Paint_1(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < snowflakePoints.Count; i++)
            {
                if (snowflakeActive[i])
                {
                    e.Graphics.DrawImage(ResizeSnowflake, snowflakePoints[i]);
                }
            }
        }

        private void Village_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 150; i++)
            {
                pointX = random.Next(0, ClientRectangle.Width);
                pointY = random.Next(-500, -50);
                imagePoint = new Point(pointX, pointY);

                snowflakeActive.Add(false);
                snowflakeDelays.Add(random.Next(0, 300));
                snowflakePoints.Add(imagePoint);
            }
            this.Paint += Village_Paint_1;
            timer.Start();
        }
    }
}
