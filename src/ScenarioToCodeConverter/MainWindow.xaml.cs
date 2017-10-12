using System.Windows;

namespace ScenarioToCodeConverter
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

        private void OnGenerateImplementationWitchCopyToClipboard(object sender, RoutedEventArgs e)
        {
            string sourceCode = SourceTextBox.Text;
            string result = SimpleBdd.ScenarioToCodeConverter.Convert(sourceCode);
            ResultTextBox.Text = result;
            Clipboard.SetText(result, TextDataFormat.UnicodeText);
        }

        private void OnGenerateMethodNameFromString(object sender, RoutedEventArgs e)
        {
            string sourceCode = SourceTextBox.Text;
            string result = SimpleBdd.ScenarioToCodeConverter.GetMethodNameFromString(sourceCode);
            ResultTextBox.Text = result;
            Clipboard.SetText(result, TextDataFormat.UnicodeText);
        }
    }
}
