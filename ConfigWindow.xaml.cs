using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WPFCarta
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private static string defaultConfig = new String('1', 90);

        class ListItem
        {
            public bool Checked { get; set; }

            public string Description1 { get; set; }

            public string Description2 { get; set; }

            public string Name { get; set; }

            public BitmapSource Picture { get; set; }

            public ListItem(bool chk, BitmapSource pic, string nam, string desc1, string desc2)
            {
                Checked = chk;
                Name = nam;
                Description1 = desc1;
                Description2 = desc2;
                Picture = pic;
            }

        }

        public static string EnabledCards
        {
            get
            {
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                        .CreateSubKey("Software\\Darkstar\\TalesCarta", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    return key.GetValue("EnabledCards", defaultConfig) as string;
                }
                catch (Exception)
                {
                    return defaultConfig;
                }
            }
            set
            {
                if (value.Length == 90)
                {
                    try
                    {
                        RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                            .CreateSubKey("Software\\Darkstar\\TalesCarta", RegistryKeyPermissionCheck.ReadWriteSubTree);
                        key.SetValue("EnabledCards", value);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public static int Voice
        {
            get
            {
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                        .CreateSubKey("Software\\Darkstar\\TalesCarta", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    return Convert.ToInt32(key.GetValue("Voice", defaultConfig));
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            set
            {
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                        .CreateSubKey("Software\\Darkstar\\TalesCarta", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    key.SetValue("Voice", value);
                }
                catch (Exception)
                { }
            }
        }

        public static int Volume
        {
            get
            {
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                        .CreateSubKey("Software\\Darkstar\\TalesCarta", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    return Convert.ToInt32(key.GetValue("Volume", defaultConfig));
                }
                catch (Exception)
                {
                    return 75;
                }
            }
            set
            {
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                        .CreateSubKey("Software\\Darkstar\\TalesCarta", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    key.SetValue("Volume", value);
                }
                catch (Exception)
                { }
            }
        }

        public ConfigWindow()
        {
            InitializeComponent();

            // load data
            string cfg = EnabledCards;
            List<ListItem> items = new List<ListItem>();

            for (int i = 0; i < 90; i++)
            {
                bool enabled = cfg[i] == '1';
                Card crd = Cards.GetCardByNumber(i + 1);
                items.Add(new ListItem(enabled, crd.Bitmap, String.Format("{0} - {1}", crd.Number, crd.Name),
                    String.Format("\"{0}\"", crd.Quote1), String.Format(" -- \"{0}\"", crd.Quote2)));
            }
            lstEnabled.ItemsSource = items;

            switch (Voice)
            {
                case 2: radVoice2.IsChecked = true; break;
                case 3: radVoice3.IsChecked = true; break;
                default: radVoice1.IsChecked = true; break;
            }

            sldVolume.Value = Volume;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            // save checked cards
            for (int i = 0; i < lstEnabled.Items.Count; i++)
            {
                sb.Append((lstEnabled.Items[i] as ListItem).Checked ? '1' : '0');
            }
            EnabledCards = sb.ToString();

            // save selected voice
            if (radVoice2.IsChecked.Value)
                Voice = 2;
            else if (radVoice3.IsChecked.Value)
                Voice = 3;
            else
                Voice = 1;

            // save volume
            Volume = Convert.ToInt32(sldVolume.Value);
            this.Close();
        }
    }
}
