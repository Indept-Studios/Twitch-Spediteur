﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Twitch_Spediteur.Fenster;

namespace Twitch_Spediteur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Spieler> spielerList = new List<Spieler>();
        SQLite sql = new SQLite();

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpielerliste();
        }

        private void InitializeSpielerliste()
        {
            spielerList = sql.HoleSpieler();

            tbkRegistrierteSpieler.Text = spielerList.Count.ToString() + " Spieler bereits registriert";

            SpielerListeAktualisieren();
        }

        private void SpielerListeAktualisieren()
        {
            lstSpieler.ItemsSource = null;
            lstSpieler.Items.Clear();
            lstSpieler.ItemsSource = spielerList;
        }

        private void cmdRegistrieren_Click(object sender, RoutedEventArgs e)
        {
            tbkMessage.Text = "";

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            // Prüfe, ob alle Felder ausgefüllt wurden
            if (String.IsNullOrEmpty(txtName.Text) ||
                String.IsNullOrEmpty(txtMail.Text) ||
                String.IsNullOrEmpty(pwdPasswort.Password))
            {
                tbkMessage.Foreground = Brushes.Red;
                tbkMessage.Text = "Keine Daten angebeben.";
            }
            // Prüfe, ob die angegebene Mail-Adresse gültig ist
            else if (regex.Match(txtMail.Text).Success)
            {
                tbkMessage.Foreground = Brushes.Black;

                // Lege einen neuen Spieler an
                Spieler spieler = new Spieler(txtName.Text, txtMail.Text, pwdPasswort.Password);
                if (spieler.Registrieren())
                {
                    spielerList.Add(spieler);
                }

                tbkMessage.Text = spielerList.Count.ToString() + " Spieler sind angelegt.";

                LeereFormular();
            }
            else
            {
                // Bringe eine Fehlermeldung, dass Mail-Adresse ungültig ist
                tbkMessage.Foreground = Brushes.Red;
                tbkMessage.Text = "Mail ist ungültig!";
            }

            SpielerListeAktualisieren();
        }

        private void cmdEinloggen_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtName.Text) &&
                !String.IsNullOrEmpty(pwdPasswort.Password))
            {
                PruefeEinloggen(txtName.Text, pwdPasswort.Password);
                LeereFormular();
            }
            else if (!String.IsNullOrEmpty(txtMail.Text) &&
                !String.IsNullOrEmpty(pwdPasswort.Password))
            {
                PruefeEinloggen(txtMail.Text, pwdPasswort.Password);
                LeereFormular();
            }
            else
            {
                // Bringe eine Fehlermeldung, dass Mail-Adresse oder Spielername fehlt
                tbkMessage.Foreground = Brushes.Red;
                tbkMessage.Text = "Spielername oder Mailadresse fehlen!";
            }
        }

        private void LeereFormular()
        {
            txtMail.Text = "";
            txtName.Text = "";
            pwdPasswort.Password = "";
        }

        private void PruefeEinloggen(string name_mail, string passwort)
        {
            foreach (Spieler sp in spielerList)
            {
                if (sp.Spielername == name_mail || sp.Mail == name_mail)
                {
                    if (sp.Einloggen(name_mail, passwort))
                    {
                        SpielerFenster user = new SpielerFenster(sp, this);
                        user.Show();
                        this.Hide();
                    }

                    continue;
                }
            }
        }

        private void menuWarenKatalog_Click(object sender, RoutedEventArgs e)
        {
            WareFenster ware = new WareFenster();
            ware.Show();
        }

        private void menuRouten_Click(object sender, RoutedEventArgs e)
        {
            EntfernungFenster entfernungen = new EntfernungFenster();
            entfernungen.Show();
        }

        private void menuBeenden_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuRegistrieren_Click(object sender, RoutedEventArgs e)
        {
            stackRegisterLogin.Visibility = Visibility.Visible;
            cmdEinloggen.Visibility = Visibility.Collapsed;
            cmdRegistrieren.Visibility = Visibility.Visible;
            tbkEinlogAnforderung.Visibility = Visibility.Hidden;
        }

        private void menuEinloggen_Click(object sender, RoutedEventArgs e)
        {
            stackRegisterLogin.Visibility = Visibility.Visible;
            cmdRegistrieren.Visibility = Visibility.Collapsed;
            cmdEinloggen.Visibility = Visibility.Visible;
            tbkEinlogAnforderung.Text = "Spielername oder Mail angeben";
        }
    }
}
