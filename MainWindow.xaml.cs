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
using Microsoft.Win32;
using System.Drawing;
using System.IO;

//-------------------------------------------------------------------------------------

namespace Mass_Image_Scaler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private List<Bitmap> images = new List<Bitmap>();
        private List<string> fileNames = new List<string>();

        //-------------------------------------------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------

        private void OpenImages(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogBox = new OpenFileDialog();
            dialogBox.Multiselect = true;
            dialogBox.Title = "Open images";
            dialogBox.Filter = "All|*.png;*.jpeg;*.jpg;*.bmp|*.png|*.png|*.jpeg, *.jpg|*.jpeg;*.jpg|*.bmp|*.bmp";
            if (dialogBox.ShowDialog() == true)
            {
                images.Clear();
                fileNames.Clear();
                for (int a = 0; a < dialogBox.FileNames.Length; a++)
                {
                    images.Add(new Bitmap(System.Drawing.Image.FromFile(dialogBox.FileNames[a])));
                    fileNames.Add(dialogBox.FileNames[a]);
                }
            }

        }
        private void ExportImages(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MyMessageBox.Show("Are you sure you want to\nreplace " + (fileNames.Count == 1 ? "this image?" : "these images?"), "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            for (int a = 0; a < images.Count; a++)
            {
                float scale = float.Parse(textBox_scale.Text);
                Bitmap scaledImage;
                if (scale != 1)
                {
                    int newWidth = (int)Math.Round((float)images[a].Width * scale);
                    int newHeight = (int)Math.Round((float)images[a].Height * scale);
                    scaledImage = new Bitmap(newWidth, newHeight);
                }
                else
                {
                    scaledImage = new Bitmap(images[a].Width, images[a].Height);
                }
                Graphics graphics = Graphics.FromImage(scaledImage);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(images[a], new System.Drawing.Rectangle(0, 0, scaledImage.Width, scaledImage.Height), new System.Drawing.Rectangle(0, 0, images[a].Width, images[a].Height), GraphicsUnit.Pixel);

                System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, scaledImage.Width, scaledImage.Height);
                if ((bool)checkBox_trimEdges.IsChecked)
                {
                    void FindX()
                    {
                        for (int x = 0; x < scaledImage.Width; x++)
                        {
                            for (int y = 0; y < scaledImage.Height; y++)
                            {
                                if (scaledImage.GetPixel(x, y).A != 0)
                                {
                                    imageRectangle.X = x;
                                    return;
                                }
                            }
                        }
                    }
                    FindX();

                    void FindWidth()
                    {
                        for (int x = scaledImage.Width - 1; x >= 0; x--)
                        {
                            for (int y = 0; y < scaledImage.Height; y++)
                            {
                                if (scaledImage.GetPixel(x, y).A != 0)
                                {
                                    imageRectangle.Width = x + 1 - imageRectangle.X;
                                    return;
                                }
                            }
                        }
                        imageRectangle.Width = 0;
                    }
                    FindWidth();

                    void FindY()
                    {
                        for (int y = 0; y < scaledImage.Height; y++)
                        {
                            for (int x = 0; x < scaledImage.Width; x++)
                            {
                                if (scaledImage.GetPixel(x, y).A != 0)
                                {
                                    imageRectangle.Y = y;
                                    return;
                                }
                            }
                        }
                    }
                    FindY();

                    void FindHeight()
                    {
                        for (int y = scaledImage.Height - 1; y >= 0; y--)
                        {
                            for (int x = 0; x < scaledImage.Width; x++)
                            {
                                if (scaledImage.GetPixel(x, y).A != 0)
                                {
                                    imageRectangle.Height = y + 1 - imageRectangle.Y;
                                    return;
                                }
                            }
                        }
                        imageRectangle.Height = 0;
                    }
                    FindHeight();
                }

                float redFactor = float.Parse(textBox_red.Text);
                float greenFactor = float.Parse(textBox_green.Text);
                float blueFactor = float.Parse(textBox_blue.Text);
                for (int x = 0; x < scaledImage.Width; x++)
                {
                    for (int y = 0; y < scaledImage.Height; y++)
                    {
                        System.Drawing.Color inColor = scaledImage.GetPixel(x, y);
                        scaledImage.SetPixel(x, y, System.Drawing.Color.FromArgb(inColor.A, (int)Math.Round((float)inColor.R * redFactor), (int)Math.Round((float)inColor.G * greenFactor), (int)Math.Round((float)inColor.B * blueFactor)));
                    }
                }

                Bitmap outImage = scaledImage.Clone(imageRectangle, System.Drawing.Imaging.PixelFormat.DontCare);

                try
                {
                    if (File.Exists(fileNames[a])) File.Delete(fileNames[a]);
                    outImage.Save(fileNames[a]);
                }
                catch
                {
                    string name = fileNames[a];
                    for (int b = name.Length - 1; b >= 0; b--)
                    {
                        if (name[a] == '/' || name[a] == '\\')
                        {
                            name = name.Remove(0, b + 1);
                            break;
                        }
                    }
                    MyMessageBox.Show(name + " cannot be saved, it is used by another program.", "Error", MessageBoxButton.OK);
                }
            }
        }
    }
}
