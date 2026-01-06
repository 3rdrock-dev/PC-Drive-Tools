// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Windows;

namespace SSDToolsWPF.UI;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}