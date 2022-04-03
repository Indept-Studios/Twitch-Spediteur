﻿using System;
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

namespace Twitch_Spediteur
{
    /// <summary>
    /// Interaktionslogik für UserInterface.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private Spieler sp;

        public UserWindow(Spieler spieler)
        {
            InitializeComponent();
            sp = spieler;

            tbkSpieler.Text = sp.Spielername;
            txtBargeld.Text = Convert.ToDecimal(sp.Bargeld) + " €";
            txtKontostand.Text = Convert.ToDecimal(sp.Konto) + " €";
        }        
    }
}
