using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace DaC_Launcher
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

        private bool mapTextures = false;
        private bool javelinAnims = false;
        private bool permArrow = false;
        private bool khazadStart = false;

        static public string cwd = Directory.GetCurrentDirectory();


        private void mapTexturesCheck_Checked(object sender, RoutedEventArgs e)
        {
            mapTextures = true;
            saved.Text = "Unsaved settings!";
        }

        private void javelinAnimsCheck_Checked(object sender, RoutedEventArgs e)
        {
            javelinAnims = true;
            saved.Text = "Unsaved settings!";

        }

        private void permArrowCheck_Checked(object sender, RoutedEventArgs e)
        {
            permArrow = true;
            saved.Text = "Unsaved settings!";

        }

        private void khazadStartCheck_Checked(object sender, RoutedEventArgs e)
        {
            khazadStart = true;
            saved.Text = "Unsaved settings!";

        }

        private void mapTexturesCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            mapTextures = false;
            saved.Text = "Unsaved settings!";

        }

        private void javelinAnimsCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            javelinAnims = false;
            saved.Text = "Unsaved settings!";

        }

        private void permArrowCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            permArrow = false;
            saved.Text = "Unsaved settings!";

        }
        private void khazadStartCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            khazadStart = false;
            saved.Text = "Unsaved settings!";

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string sourceDir;
            string destinationDir = cwd + "/data";

            if (mapTextures) {
                sourceDir = cwd + "/extra/mapTextures";
            } 
            else
            {
                sourceDir = cwd + "/extra/mapTexturesVanilla";
            } 
            copyFiles(sourceDir, destinationDir);
            if (javelinAnims) {
                sourceDir = cwd + "/extra/javelinAnims";
            } 
            else
            {
                sourceDir = cwd + "/extra/javelinAnimsVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            if (permArrow) {
                sourceDir = cwd + "/extra/permArrow";
            } 
            else
            {
                sourceDir = cwd + "/extra/permArrowVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            if (khazadStart) {
                sourceDir = cwd + "/extra/khazadStart";
            } 
            else
            {
                sourceDir = cwd + "/extra/khazadStartVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            saved.Text = "Settings saved.";
        }

        private void copyFiles(string sourceDir, string destinationDir)
        {
            var allFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            foreach (string newPath in allFiles)
            {
                bool overwriteFiles = true;
                File.Copy(newPath, newPath.Replace(sourceDir, destinationDir), overwriteFiles);
            }
        }
    }
}
