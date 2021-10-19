using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Reflection;

namespace recursive_fractal
{
    public partial class MainWindow : Window
    {
        static float prevx = 0; // just for inital drawline
        static float prevy = 640; // read above
        static float pointx = 610;
        static float pointy = 0;
        static int amount = 20;
        Random r = new Random();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 20; i++)
            {
                zig(16);
            }
        }

        public void DrawLine(Canvas canvas, float sx, float sy, bool turn) // turn, true = left, false = right
        {
            Line ln = new Line();
            Thickness thickness = new Thickness(10, 10, 10, 10);
            ln.Margin = thickness;
            ln.Visibility = System.Windows.Visibility.Visible;
            ln.StrokeThickness = 1;
            ln.Stroke = RandBrush(r);
            ln.X1 = sx;
            ln.Y1 = sy;
            int rotation = getRotation(prevx, prevy, sx, sy);
            float[] exy = advance(sx, sy, rotation, turn);
            ln.X2 = exy[0];
            ln.Y2 = exy[1];
            prevx = sx;
            prevy = sy;
            pointx = exy[0];
            pointy = exy[1];
            canvas.Children.Add(ln);
            canvas.UpdateLayout();
        }

        public void zig(int n)
        {
            if (n == 1)
            {
                // turn left
                // advance 1
                // turn left
                // advance 1

                DrawLine(canvas, pointx, pointy, true);
                DrawLine(canvas, pointx, pointy, true);

            }
            else
            {
                zig(n / 2);
                zag(n / 2);
                zig(n / 2);
                zag(n / 2);
            }
        }

        public void zag(int n)
        {
            if (n == 1)
            {
                // turn right
                // advance 1
                // turn right
                // advance 1
                // turn left
                // advance 1

                DrawLine(canvas, pointx, pointy, false);
                DrawLine(canvas, pointx, pointy, false);
                DrawLine(canvas, pointx, pointy, true);
            }
            else
            {
                zag(n / 2);
                zag(n / 2);
                zig(n / 2);
                zag(n / 2);
            }
        }

        public int getRotation(float x1, float y1, float x2, float y2)
        {
            // get the last rotation
            // if went up, left = x - 100 and y = y
            // if went down, left = x + 100 and y = y
            // if went left, left = x = x and y - 100
            // if went right, left = x = x and y + 100

            // 1 = up 2 = down 3 = left 4 = right

            int turnpos = 0;

            if (x1 > x2) // gone left
            {
                turnpos = 1;
            }
            else if (x1 < x2) // gone right
            {
                turnpos = 2;
            }
            else if (y1 < y2) // gone up
            {
                turnpos = 3;
            }
            else if (y1 > y2) // gone down
            {
                turnpos = 4;
            }

            return turnpos;
        }

        public float[] advance(float initialx, float initialy, int direction, bool rotate) // this is truly a mess but I cant think of a better way (am stooopid)
        {
            float endx = 0;
            float endy = 0;

            switch (rotate) // true = left false = right
            {
                case true:
                    switch (direction) 
                    {
                        case 1: // left
                            endx = initialx;
                            endy = initialy - amount;
                            break;
                        case 2: // right
                            endx = initialx;
                            endy = initialy + amount;
                            break;
                        case 3: // up
                            endx = initialx - amount;
                            endy = initialy;
                            break;
                        case 4: // down
                            endx = initialx + amount;
                            endy = initialy;
                            break;
                    }
                    break;
                case false:
                    switch (direction) 
                    {
                        case 1:
                            endx = initialx;
                            endy = initialy + amount;
                            break;
                        case 2:
                            endx = initialx;
                            endy = initialy - amount;
                            break;
                        case 3:
                            endx = initialx + amount;
                            endy = initialy;
                            break;
                        case 4:
                            endx = initialx - amount;
                            endy = initialy;
                            break;
                    }
                    break;
            }
            float[] endpos = { endx, endy};
            return endpos;
        }

        public Brush RandBrush(Random rnd)
        {
            Brush result = Brushes.Transparent;
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();
            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            return result;
        }
    }
}