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

namespace WPFCarta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int BitmapXSize = 152;
        const int BitmapYSize = 200;
        const int CroppedXSize = 141;
        const int CroppedYSize = 189;

        //BitmapSource[] bmps;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            int startx;
            int starty;
            System.Drawing.Graphics g;

            // contains the card number the corresponding bitmap belongs to
            int[] BitmapOrdering = new int[] {
                1, 2, 3, 4,
                5, 6, 8, 9, /* 7's image missing here */
                10, 11, 12, 13,
                14, 15, 16, 17,
                18, 19, 20, 21,
                22, 23, 24, 25,
                26, 27, 28, 29,
                30, 31, 32, 33,
                34, 35, 36, 37,
                38, 39, 40, 41, 
                42, 43, 44, 45,
                46, 47, 48, 49,
                50, 51, 52, 53,
                54, 55, 56, 57,
                58, 59, 60, 61,
                62, 63, 64, 65,
                66, 67, 68, 69,
                70, 71, 72, 73,
                74, 75, 76, 77,
                78, 79, 80, 81,
                82, 83, 84, 85,
                86, 87, 88, 89,
                90, 7 /* 7's image is here at the end */
            };

            //bmps = new BitmapSource[90];
            BitmapSource src = Resource1.ToG___Cards.GetBitmapSource();
            
            for (int i = 0; i < 90; i++)
            {
                // first, crop out the relevant image from the source
                starty = (i / 4) * BitmapYSize + 1;
                startx = (i % 4) * BitmapXSize + 1;

                System.Drawing.Bitmap dst = new System.Drawing.Bitmap(CroppedXSize, CroppedYSize);

                g = System.Drawing.Graphics.FromImage(dst);
                g.DrawImage(Resource1.ToG___Cards, new System.Drawing.RectangleF(0, 0, CroppedXSize, CroppedYSize),
                    new System.Drawing.RectangleF(startx, starty, CroppedXSize, CroppedYSize), System.Drawing.GraphicsUnit.Pixel);
                g.Dispose();

                // now convert to BitmapSource
                int cardno = BitmapOrdering[i];
                Cards.GetCardByNumber(cardno).Bitmap = dst.GetBitmapSource();
                dst.Dispose();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow wnd = new ConfigWindow();

            if (wnd.ShowDialog().Value)
            {
                // save and apply settings
            }
        }

        private Image draggedImage;
        private Point mousePosition;
        private Border selectedBorder = null;

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = e.Source as Image;           

            if (img != null && canvas1.CaptureMouse())
            {
                Border brd = img.Parent as Border;

                // move image to front
                canvas1.Children.Remove(brd);
                canvas1.Children.Add(brd);

                // if not in play mode, start dragging
                // start dragging
                draggedImage = img;
                mousePosition = e.GetPosition(canvas1);

                // select image
                if (selectedBorder != null)
                {
                    selectedBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                selectedBorder = brd;
                brd.BorderBrush = new SolidColorBrush(Colors.Red);
                CardSelected(brd.Tag as Card);
            }
            else if (img == null)
            {
                if (selectedBorder != null)
                {
                    selectedBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                selectedBorder = null;
                CardSelected(null);
            }
        }

        void CardClicked(Border brd)
        {
            //AudioPlayer.PlayResourceInBackground((brd.Tag as Card).GetQuote(1, 1), (brd.Tag as Card).GetQuote(2, 1));
            AudioPlayer.PlayResourceInBackground((brd.Tag as Card).GetQuote(1, ConfigWindow.Voice), (brd.Tag as Card).GetQuote(2, ConfigWindow.Voice));
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedImage != null)
            {
                canvas1.ReleaseMouseCapture();

                draggedImage = null;
            }
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedImage != null)
            {
                Border brd = draggedImage.Parent as Border;

                Point position = e.GetPosition(canvas1);
                Vector offset = position - mousePosition;
                mousePosition = position;
                Canvas.SetLeft(brd, Canvas.GetLeft(brd) + offset.X);
                Canvas.SetTop(brd, Canvas.GetTop(brd) + offset.Y);
            }
        }

        private void CardSelected(Card crd)
        {
            if (crd == null)
            {
                infobox1.Text = "";
                return;
            }
            infobox1.Text = String.Format("Card #{0} : {1} - \"{2}\" - \"{3}\"", crd.Number, crd.Name, crd.Quote1, crd.Quote2);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int numPoints = 20;
            double xSize = canvas1.ActualWidth - BitmapXSize;
            double ySize = canvas1.ActualHeight - BitmapYSize;
            double minDist;
            Random rnd = new Random();

            List<Point> points = new List<Point>();
            int[] cards = new int[numPoints];

            // clear canvas
            canvas1.Children.Clear();

            // arbitraty value
            minDist = 50;

            // generate points
            for (int i = 0; i < numPoints; i++)
            {
                bool retry;
                Point pt;

                // generate new point
                do
                {
                    pt = new Point(rnd.NextDouble() * xSize, rnd.NextDouble() * ySize);
                    retry = false;

                    // check for minimum distance
                    foreach (Point other in points)
                    {
                        double dx = other.X - pt.X;
                        double dy = other.Y - pt.Y;
                        if (Math.Sqrt(dx * dx + dy * dy) < minDist)
                        {
                            retry = true;
                            break;
                        }
                    }
                } while (retry);

                // add point to list
                points.Add(pt);
            }

            string enabledCards = ConfigWindow.EnabledCards;

            // Generate a list of all available cards
            List<Card> avail = new List<Card>();
            List<Card> chosen = new List<Card>();
            for (int i = 0; i < 90; i++)
            {
                if (enabledCards[i] == '1')
                    avail.Add(Cards.GetCardByNumber(i + 1));
            }

            // now find up to 20 random cards
            for (int i = 0; i < numPoints; i++)
            {
                if (avail.Count == 0)
                    break;

                // select a card
                int cardNum = rnd.Next(avail.Count);
                Card card = avail[cardNum];

                // move it from "avail" to "chosen"
                avail.RemoveAt(cardNum);
                chosen.Add(card);
            }

            // finally, draw the cards
            int pointIdx = 0;

            foreach (Card crd in chosen)
            {
                double alpha = 80;

                Image img = new Image();
                Border brd = new Border();

                brd.BorderThickness = new Thickness(2.0);
                brd.BorderBrush = new SolidColorBrush(Colors.Transparent);
                img.Stretch = Stretch.Fill;
                img.Source = crd.Bitmap;
                brd.Child = img;
                brd.Tag = crd;
                // rotate the image 
                brd.RenderTransform = new RotateTransform(rnd.NextDouble() * 2 * alpha - alpha, 0.5 * img.Source.Width, 0.5 * img.Source.Height);

                Canvas.SetLeft(brd, points[pointIdx].X);
                Canvas.SetTop(brd, points[pointIdx].Y);
                pointIdx++;
                canvas1.Children.Add(brd);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AudioPlayer.Cleanup();
        }

        private void canvas1_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = e.Source as Image;

            if (img != null)
            {
                Border brd = img.Parent as Border;

                // handle click if not already playing sound
                if (!AudioPlayer.IsBackgroundPlaying)
                {
                    CardClicked(brd);
                }
                return;
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Noch nicht..");
        }
    }
}
