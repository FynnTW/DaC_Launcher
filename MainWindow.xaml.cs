using PEFile;
using System;
using System.IO;
using System.Windows;

namespace DaC_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                checkInstall();
                setLAA();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Start not possible or Exception occurs {e}  ", "DaC Launcher Error");
            }

        }

        string exeMed = "";
        string exeKingdoms = "";

        private void checkInstall()
        {
            string gameDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(cwd, @"..\..\"));
            exeMed = gameDir + "/medieval2.exe";
            exeKingdoms = gameDir + "/kingdoms.exe";
            if (!File.Exists(exeMed) && (!File.Exists(exeKingdoms)))
            {
                string messageBoxText = "You have installed Divide & Conquer into the wrong location, no game executables were found.";
                string caption = "Wrong installation";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                System.Windows.Application.Current.Shutdown();
            }
            string casFiles = gameDir + "/data/models_missile/trollmen_javelin.cas";
            if (!File.Exists(casFiles))
            {
                if (!Directory.Exists(gameDir + "data/models_missile/textures"))
                {
                    Directory.CreateDirectory(gameDir + "data/models_missile/textures");
                }
                if (Directory.Exists(cwd + "/data/models_missile"))
                {
                    copyFiles(cwd + "/data/models_missile", gameDir + "/data/models_missile");
                }
            }
            if (!File.Exists(casFiles))
            {
                string messageBoxText = "You have not installed the missile cas models into your Medieval 2 directory. Install the latest hotfix correctly";
                string caption = "Missing game files";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult newresult;

                newresult = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private void setLAA()
        {
            if (File.Exists(exeMed))
            {
                LargeAddressAware.SetLargeAddressAware(exeMed);
            }
            if (File.Exists(exeKingdoms))
            {
                LargeAddressAware.SetLargeAddressAware(exeMed);
            }
        }

        private void runGame()
        {
            saveSettings();
            var program = new System.Diagnostics.Process();
            string argument = "@" + cwd + "\\TATW.cfg";
            program.StartInfo.Arguments = '"' + argument + '"';
            program.StartInfo.UseShellExecute = false;
            program.StartInfo.RedirectStandardOutput = true;
            program.StartInfo.CreateNoWindow = true;
            if (File.Exists(exeKingdoms))
            {
                program.StartInfo.FileName = exeMed;
            }
            else if (File.Exists(exeMed))
            {
                program.StartInfo.FileName = exeMed;
            } else
            {
                System.Windows.Application.Current.Shutdown();
            }
            program.Start();
            System.Windows.Application.Current.Shutdown();
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
            saveSettings();
        }

        private void saveSettings()
        {
            string sourceDir;
            string destinationDir = cwd + "/data";

            if (mapTextures)
            {
                sourceDir = cwd + "/extra/mapTextures";
            }
            else
            {
                sourceDir = cwd + "/extra/mapTexturesVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            if (javelinAnims)
            {
                sourceDir = cwd + "/extra/javelinAnims";
            }
            else
            {
                sourceDir = cwd + "/extra/javelinAnimsVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            if (permArrow)
            {
                sourceDir = cwd + "/extra/permArrow";
            }
            else
            {
                sourceDir = cwd + "/extra/permArrowVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            if (khazadStart)
            {
                sourceDir = cwd + "/extra/khazadStart";
            }
            else
            {
                sourceDir = cwd + "/extra/khazadStartVanilla";
            }
            copyFiles(sourceDir, destinationDir);
            saved.Text = "Settings saved.";
        }


        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            runGame();
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
