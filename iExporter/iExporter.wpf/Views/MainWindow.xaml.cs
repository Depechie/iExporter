using System.Windows;

namespace iExporter.wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnITunesLibrarySelectButtonClick(object sender, RoutedEventArgs e)
        {
            iTunesLibraryFileLocationTextBox.Text = "Clicked the button";
        }
    }
}
