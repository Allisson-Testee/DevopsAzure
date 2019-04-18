﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
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
using OSVersionHelper;
using Windows.ApplicationModel;

namespace WpfCoreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            versionText.Text = typeof(MainWindow).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

#if CI
            versionText.Text += " - CI";
#elif RELEASE
            versionText.Text += " - Release";
#endif
            inPackage.Text = WindowsVersionHelper.HasPackageIdentity.ToString();
            deploymentType.Text = GetDotNetInfo();
            packageVersion.Text = GetPackageVersion();
        }

        private string GetPackageVersion()
        {
            if (OSVersionHelper.WindowsVersionHelper.HasPackageIdentity)
            {
                return $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";
            }
            return "Not Packaged";
        }

        public static string GetDotNetInfo()
        {
            var runTimeDir = new FileInfo(typeof(string).Assembly.Location);
            var entryDir = new FileInfo(Assembly.GetEntryAssembly().Location);
            var IsSelfContaied = runTimeDir.DirectoryName == entryDir.DirectoryName;

            var result = ".NET Core - ";
            if (IsSelfContaied)
            {
                result += "Self Contained Deployment";
            }
            else
            {
                result += "Framework Dependent Deployment";
            }
            
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fs = File.OpenText(System.IO.Path.Combine(System.Environment.CurrentDirectory,
               Assembly.GetExecutingAssembly().GetName().Name + ".runtimeConfig.json"));

            var j = System.Text.Json.JsonDocument.Parse(fs.ReadToEnd());

            var v = j.RootElement.GetProperty("runtimeOptions").GetProperty("framework").GetProperty("version");
            RuntimeVersionInfo.Text = v.GetString();

        }
    }
}
