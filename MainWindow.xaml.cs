using Newtonsoft.Json;
using PEFile;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace DaC_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                LoadSettings();
                CheckInstall();
                if (_settings is { StartInstantly: true })
                {
                    RunGame();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Start not possible or Exception occurs {e}  ", "DaC Launcher Error");
            }
        }

        private string _mapTexturesChosen = "vanilla";

        private void LoadSettings()
        {
            if (File.Exists(Cwd + "/DaC_Config.json"))
            {
                _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Cwd + "/DaC_Config.json"));
                if (_settings is { PermanentArrows: true })
                {
                    permArrowCheck.IsChecked = true;
                }
                if (_settings is { KhazadJourneySkip: true })
                {
                    khazadStartCheck.IsChecked = true;
                }
                if (_settings is { JavelinRunAnims: true })
                {
                    javelinAnimsCheck.IsChecked = true;
                }
                if (_settings is { AgoTextures: true })
                {
                    mapTexturesCheck.IsChecked = true;
                    _mapTexturesChosen = "ago";
                }
                saved.Text = "";
            }
            else
            {
                _settings = new Settings();
                var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
                File.WriteAllText(Cwd + "/DaC_Config.json", json);
            }
        }

        private string _exeMed = "";
        private string _exeKingdoms = "";
        private Settings? _settings = new();

        private void CheckInstall()
        {
            var gameDir = Path.GetFullPath(Path.Combine(Cwd, @"..\..\"));
            _exeMed = gameDir + "/medieval2.exe";
            _exeKingdoms = gameDir + "/kingdoms.exe";
            if (!File.Exists(_exeMed) && (!File.Exists(_exeKingdoms)))
            {
                const string messageBoxText = "You have installed Divide & Conquer into the wrong location, no game executables were found. Check your path to the mod folder.";
                const string caption = "Wrong installation!";
                const MessageBoxButton button = MessageBoxButton.OK;
                const MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                Application.Current.Shutdown();
            }
            if (File.Exists(_exeKingdoms))
            {
                laaapplied.Text = LargeAddressAware.IsLargeAddressAware(_exeKingdoms) ? "LAA applied" : "LAA not applied";
            }
            else
            {
                laaapplied.Text = LargeAddressAware.IsLargeAddressAware(_exeMed) ? "LAA applied" : "LAA not applied";
            }
            var casFiles = gameDir + "/data/models_missile/trollmen_javelin.cas";
            if (!File.Exists(casFiles))
            {
                if (!Directory.Exists(gameDir + "data/models_missile/textures"))
                {
                    Directory.CreateDirectory(gameDir + "data/models_missile/textures");
                }
                if (Directory.Exists(Cwd + "/data/models_missile"))
                {
                    CopyFiles(Cwd + "/data/models_missile", gameDir + "/data/models_missile");
                }
            }

            if (File.Exists(casFiles)) return;
            {
                const string messageBoxText = "You have not installed the missile cas models into your Medieval 2 directory. Install the latest hotfix correctly";
                const string caption = "Missing game files";
                const MessageBoxButton button = MessageBoxButton.OK;
                const MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private bool LaaWarning()
        {
            const string messageBoxText = "You have not applied LAA, you will experience many crashes. Do you want to apply it now?";
            const string caption = "LAA has not been applied";
            const MessageBoxButton button = MessageBoxButton.YesNoCancel;
            const MessageBoxImage icon = MessageBoxImage.Warning;

            var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SetLaa();
                    return true;
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.None:
                    return false;
                case MessageBoxResult.OK:
                    return false;
                default:
                    return false;
            }
        }

        private void SetLaa()
        {
            if (File.Exists(_exeMed))
            {
                LargeAddressAware.SetLargeAddressAware(_exeMed);
            }
            if (File.Exists(_exeKingdoms))
            {
                LargeAddressAware.SetLargeAddressAware(_exeKingdoms);
            }
        }

        private void RunGame()
        {
            SaveSettings();
            var program = new System.Diagnostics.Process();
            var argument = "@" + Cwd + "\\TATW.cfg";
            program.StartInfo.Arguments = '"' + argument + '"';
            program.StartInfo.UseShellExecute = false;
            program.StartInfo.RedirectStandardOutput = true;
            program.StartInfo.CreateNoWindow = true;
            var startGame = true;
            if (File.Exists(_exeKingdoms))
            {
                if (!LargeAddressAware.IsLargeAddressAware(_exeKingdoms))
                {
                    startGame = LaaWarning();
                }
                program.StartInfo.FileName = _exeKingdoms;
            }
            else if (File.Exists(_exeMed))
            {
                if (!LargeAddressAware.IsLargeAddressAware(_exeMed))
                {
                    startGame = LaaWarning();
                }
                program.StartInfo.FileName = _exeMed;
            }
            else
            {
                Application.Current.Shutdown();
            }
            if (!startGame) return;
            program.Start();
            Application.Current.Shutdown();
        }

        private bool _mapTextures;
        private bool _javelinAnims;
        private bool _permArrow;
        private bool _khazadStart;
        private bool _bypassLauncher;

        private static readonly string Cwd = Directory.GetCurrentDirectory();


        private void mapTexturesCheck_Checked(object sender, RoutedEventArgs e)
        {
            _mapTextures = true;
            saved.Text = "Unsaved settings!";
        }

        private void javelinAnimsCheck_Checked(object sender, RoutedEventArgs e)
        {
            _javelinAnims = true;
            saved.Text = "Unsaved settings!";

        }

        private void permArrowCheck_Checked(object sender, RoutedEventArgs e)
        {
            _permArrow = true;
            saved.Text = "Unsaved settings!";
        }

        private void khazadStartCheck_Checked(object sender, RoutedEventArgs e)
        {
            _khazadStart = true;
            saved.Text = "Unsaved settings!";
        }

        private void mapTexturesCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _mapTextures = false;
            saved.Text = "Unsaved settings!";
        }

        private void javelinAnimsCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _javelinAnims = false;
            saved.Text = "Unsaved settings!";
        }

        private void bypassLauncherCheck_Checked(object sender, RoutedEventArgs e)
        {
            _bypassLauncher = true;
            saved.Text = "Unsaved settings!";
        }

        private void bypassLauncherCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _bypassLauncher = false;
            saved.Text = "Unsaved settings!";
        }

        private void permArrowCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _permArrow = false;
            saved.Text = "Unsaved settings!";

        }
        private void khazadStartCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _khazadStart = false;
            saved.Text = "Unsaved settings!";

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            string sourceDir;
            var destinationDir = Cwd + "/data";

            if (_mapTextures)
            {
                sourceDir = Cwd + "/extra/agoCampaignTextures";
                if (_mapTexturesChosen == "vanilla")
                {
                    if (File.Exists(destinationDir + "/world/maps/base/map.rwm"))
                    {
                        File.Delete(destinationDir + "/world/maps/base/map.rwm");
                    }
                }
            }
            else
            {
                sourceDir = Cwd + "/extra/agoCampaignTexturesVanilla";
                if (_mapTexturesChosen == "ago")
                {
                    if (File.Exists(destinationDir + "/world/maps/base/map.rwm"))
                    {
                        File.Delete(destinationDir + "/world/maps/base/map.rwm");
                    }
                }
            }
            CopyFiles(sourceDir, destinationDir);
            if (_settings != null) _settings.AgoTextures = _mapTextures;

            if (_javelinAnims)
            {
                sourceDir = Cwd + "/extra/ebiiJavelins";
            }
            else
            {
                sourceDir = Cwd + "/extra/ebiiJavelinsVanilla";
            }
            CopyFiles(sourceDir, destinationDir);
            if (_settings != null) _settings.JavelinRunAnims = _javelinAnims;

            if (_permArrow)
            {
                sourceDir = Cwd + "/extra/permArrow";
            }
            else
            {
                sourceDir = Cwd + "/extra/permArrowVanilla";
            }
            CopyFiles(sourceDir, destinationDir);
            if (_settings != null) _settings.PermanentArrows = _permArrow;

            if (_settings != null) _settings.StartInstantly = _bypassLauncher;

            if (_khazadStart)
            {
                sourceDir = Cwd + "/extra/kdSkip";
            }
            else
            {
                sourceDir = Cwd + "/extra/kdSkipVanilla";
            }
            CopyFiles(sourceDir, destinationDir);
            if (_settings != null) _settings.KhazadJourneySkip = _khazadStart;

            var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(Cwd + "/DaC_Config.json", json);
            saved.Text = "Settings saved.";
        }


        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            RunGame();
        }

        private static void CopyFiles(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destinationDir)) return;
            var allFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            foreach (var newPath in allFiles)
            {
                const bool overwriteFiles = true;
                File.Copy(newPath, newPath.Replace(sourceDir, destinationDir), overwriteFiles);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
