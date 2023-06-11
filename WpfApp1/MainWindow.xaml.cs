using System;

using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Runtime.InteropServices;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwflags, uint dx, uint dy, uint dwData, uint dwExtraInf);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void SearchPixel(object sender, RoutedEventArgs e)
        {
            string inputHexColor = HexColor.Text;
            SearchPixel(inputHexColor);
        }

        private void DBClick(int posX, int posY)
        {
            SetCursorPos(posX, posY);
            Click();
            //take a break before clicking again
            System.Threading.Thread.Sleep(250);
            Click();
        }
        private bool SearchPixel(string hexcode)
        {
            //create empty bitmap of current screen
            //Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            //create empty bitmap of all connected screens
            Bitmap bitmap = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

            //creates a graphics object that captures the screen
            Graphics graphics = Graphics.FromImage(bitmap as Image);
           
            //screenshot moment -> screen content to graphics object
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

            //translates ex #FFFFFF to color object
            Color desiredPixelColor = ColorTranslator.FromHtml(hexcode);


            //this for loop runs through all the x-value collumns
            for (int x = 0; x < SystemInformation.VirtualScreen.Width; x++)
            {
                //this runs through all the rows, y-values
                for (int y = 0; y < SystemInformation.VirtualScreen.Height; y++)
                {
                    //get the current pixel color
                    Color currentPixelColor = bitmap.GetPixel(x, y);

                    //compare current hex color to desired color, if they match woohoo!
                    if (desiredPixelColor == currentPixelColor)
                    {
                        MessageBox.Show(String.Format("Found your pixel here {0},{1} setting mouse cursor!", x, y));
                        
                        DBClick(x, y);
                        return true;
                        //this exits out of the if statement
                    }
                }
            }
            return false;
        }
    }
}
