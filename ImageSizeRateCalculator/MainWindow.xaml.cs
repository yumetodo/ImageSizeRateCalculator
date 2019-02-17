using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageSizeRateCalculator
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ResultPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
        private async void ResultDropFiles(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if(2 == files?.Length)
            {
                try
                {
                    var sizes = await Task.WhenAll(files.Select(async fileName => await Task.Run(() => System.Drawing.Image.FromFile(fileName).Size)));
                    result.Text
                        = $"縦: x{Math.Round(sizes[0].Height * 1.0 / sizes[1].Height, 4)} x{Math.Round(sizes[1].Height * 1.0 / sizes[0].Height, 4)}\n"
                        + $"横: x{Math.Round(sizes[0].Width * 1.0 / sizes[1].Width, 4)} x{Math.Round(sizes[1].Width * 1.0 / sizes[0].Width, 4)}";
                }
                catch (OutOfMemoryException)
                {
                    result.Text = "入力は本当に画像ですか？";
                }
                catch (Exception er)
                {
                    result.Text = "error\n" + er.ToString();
                }
            }
            else
            {
                result.Text = "";
            }
        }
        private void OnCtrlC(object sender, RoutedEventArgs e)
        {
            if (string.Empty != result.Text)
            {
                Clipboard.SetData(DataFormats.Text, result.Text);
            }
        }
    }
}
