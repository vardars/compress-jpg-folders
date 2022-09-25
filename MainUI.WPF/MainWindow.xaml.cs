using FreeImageAPI;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace MainUI.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txtFolder.IsEnabled = false;

            btnSelectFolder.Click += btnSelectFolder_MouseDown;

            btnProcessFolders.Click += BtnProcessFolders_Click;
        }

        private void btnSelectFolder_MouseDown(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txtFolder.Text = dialog.SelectedPath;
                }
            }
        }

        private void BtnProcessFolders_Click(object sender, RoutedEventArgs e)
        {
            var files = Directory.EnumerateFiles(txtFolder.Text, "*.*", SearchOption.AllDirectories)
                .Where(p => 
                    p.ToLowerInvariant().EndsWith(".jpg", StringComparison.InvariantCulture) || 
                    p.ToLowerInvariant().EndsWith(".jpeg", StringComparison.InvariantCulture))
                .ToList();

            files.ForEach(s => {
                ProcessImage(s);
                txtOutput.Text += s + " PROCESSED." + Environment.NewLine;
            });
        }

        private void ProcessImage(string filePath)
        {
            var image = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_JPEG, filePath, FREE_IMAGE_LOAD_FLAGS.JPEG_ACCURATE);
            var newPath = Path.Combine(Path.GetDirectoryName(filePath)!, Path.GetFileNameWithoutExtension(filePath) + "-converted" + Path.GetExtension(filePath));
            FreeImage.Save(FREE_IMAGE_FORMAT.FIF_JPEG, image, newPath, FREE_IMAGE_SAVE_FLAGS.JPEG_OPTIMIZE);
        }
    }
}
