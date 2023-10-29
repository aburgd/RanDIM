using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RanDIM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    using Weapons = Dictionary<string, string>;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Weapons dctPrimary = new();
        private Weapons dctSpecial = new();
        private Weapons dctAllWeapons = new();
        private Random random = new();
        private string[] arrPrimary = { "Auto Rifle", "Bow", "Hand Cannon", "Pulse Rifle", "Scout Rifle", "Sidearm", "SMG" };
        private string[] arrSpecial = { "Fusion Rifle", "Glaive", "Shotgun", "Sniper Rifle", "Trace Rifle" };
        private string[] arrMaps = { "Altar of Flame", "The Anomaly", "Bannerfall", "The Burnout", "Cauldron", "Cathedral of Dusk", "Convergence", "The Dead Cliffs", "Disjunction", "Distant Shore", "Endless Vale", "Eternity", "Exodus Blue", "The Fortress", "Fragment", "Javelin-4", "Midtown", "Pacifica", "Radiant Cliffs", "Rusted Lands", "Twilight Gap", "Vostok", "Widow's Court", "Wormhaven", "Meltdown", "Multiplex" };

        private void BtnRoll_Click(object sender, EventArgs e)
        {
            // clear all the fields
            txtDIMQuery.Clear();
            txtEnergy.Clear();
            txtKinetic.Clear();
            txtMap.Clear();

            // set up weapon types
            Prepare();

            // roll 'em.
            Weapons weapons = Roll();
            txtKinetic.Text = weapons["kinetic"];
            txtEnergy.Text = weapons["energy"];

            // run the weapon checks
            RerollInvalids();

            // build the DIM search string
            txtDIMQuery.Text = BuildDIMString();

            // roll a map (only if it's asked for)
            if (chkMaps.IsChecked == true)
            {
                txtMap.Text = arrMaps[random.Next(arrMaps.Length)];
            }
        }

        private void Prepare()
        {
            foreach (string key in arrPrimary)
            {
                dctPrimary[key] = key.ToLower().Replace(" ", "");
            }
            foreach (string key in arrSpecial)
            {
                dctSpecial[key] = key.ToLower().Replace(" ", "");
            }
            string[] arrAllWeapons = arrPrimary.Concat(arrSpecial).ToArray();
            foreach (string key in arrAllWeapons)
            {
                dctAllWeapons[key] = key.ToLower().Replace(" ", "");
            }
        }

        private Weapons Roll()
        {
            Weapons rolled = new()
            {
                { "kinetic", dctAllWeapons.Keys.ElementAt(random.Next(dctAllWeapons.Keys.Count)) },
                { "energy", dctAllWeapons.Keys.ElementAt(random.Next(dctAllWeapons.Keys.Count)) }
            };
            return rolled;
        }

        private void RerollInvalids()
        {
            bool boolBothSpecial = dctSpecial.ContainsKey(txtKinetic.Text) && dctSpecial.ContainsKey(txtEnergy.Text);
            bool boolBothPrimary = dctPrimary.ContainsKey(txtKinetic.Text) && dctPrimary.ContainsKey(txtEnergy.Text);
            bool boolKineticTraceRifle = dctAllWeapons[txtKinetic.Text] == "Trace Rifle";

            while (boolBothSpecial || (boolBothPrimary && chkDoublePrimary.IsChecked == false) || boolKineticTraceRifle)
            {
                txtKinetic.Text = dctAllWeapons.Keys.ElementAt(random.Next(dctAllWeapons.Keys.Count));
                txtEnergy.Text = dctAllWeapons.Keys.ElementAt(random.Next(dctAllWeapons.Keys.Count));
                boolBothSpecial = dctSpecial.ContainsKey(txtKinetic.Text) && dctSpecial.ContainsKey(txtEnergy.Text);
                boolBothPrimary = dctPrimary.ContainsKey(txtKinetic.Text) && dctPrimary.ContainsKey(txtEnergy.Text);
                boolKineticTraceRifle = dctAllWeapons[txtKinetic.Text] == "Trace Rifle";
            }
        }

        private string BuildDIMString()
        {
            string query = $"(is:kineticslot AND is:{dctAllWeapons[txtKinetic.Text]}) OR (is:energy AND is:{dctAllWeapons[txtEnergy.Text]})";
            if (chkExotic.IsChecked == false)
            {
                query = $"not:exotic ({query})";
            }
            return query;
        }



    }


}
