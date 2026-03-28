using System.Windows;

namespace PR4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Page1());
        }

        private void BtnPage1_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Page1());
        private void BtnPage2_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Page2());
        private void BtnPage3_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Page3());

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}