using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nini.Config;
using System.Data.SQLite;
using D3Sharp.Core.Storage;

namespace D3Sharp
{
    public partial class ToonItems : Form
    {
        public ToonItems()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Write config file settings for [ToonItems]
            IConfigSource source = new IniConfigSource("config.ini");
            IConfig config = source.Configs["ToonItems"];
            config.Set("Head", lblHead.Text);
            config.Set("Chest", lblChest.Text);
            config.Set("Feet", lblFeet.Text);
            config.Set("Hands", lblHands.Text);
            config.Set("Weapon1", lblWeapon1.Text);
            config.Set("Weapon2", lblWeapon2.Text);
            config.Set("Shoulders", lblShoulders.Text);
            config.Set("Legs", lblLegs.Text);
            source.Save();

            var main = new Program(); // startup.
            main.StartupServers();
        }

        private void ToonItems_Load(object sender, EventArgs e)
        {
            GetHelmList();
            GetChestList();
            GetFeetList();
            GetHandsList();
            GetWeapon1List();
            GetWeapon2List();
            GetShouldersList();
            GetLegsList();

            SelectItems();
        }

        private void SelectItems()
        {
            IConfigSource source = new IniConfigSource("config.ini"); // get configuration file
            string _head = source.Configs["ToonItems"].GetString("Head", "");
            var query = "SELECT name from head WHERE id = '" + _head + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                cboHead.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboHead.Text = _head;
            }

            string _chest = source.Configs["ToonItems"].GetString("Chest", "");
            query = "SELECT name from chest WHERE id = '" + _chest + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboChest.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboChest.Text = _chest;
            }

            string _feet = source.Configs["ToonItems"].GetString("Feet", "");
            query = "SELECT name from feet WHERE id = '" + _feet + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboFeet.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboFeet.Text = _feet;
            }

            string _hands = source.Configs["ToonItems"].GetString("Hands", "");
            query = "SELECT name from hands WHERE id = '" + _hands + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboHands.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboHands.Text = _hands;
            }

            string _weapon1 = source.Configs["ToonItems"].GetString("Weapon1", "");
            query = "SELECT name from weapon WHERE id = '" + _weapon1 + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboWeapon1.Text = reader.GetString(0).ToString();
                set = true;
            }
            if (set == false)
            {
                cboWeapon1.Text = _weapon1;
            }

            string _weapon2 = source.Configs["ToonItems"].GetString("Weapon2", "");
            query = "SELECT name from weapon WHERE id = '" + _weapon2 + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboWeapon2.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboWeapon2.Text = _weapon2;
            }

            string _shoulders = source.Configs["ToonItems"].GetString("Shoulders", "");
            query = "SELECT name from shoulders WHERE id = '" + _shoulders + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboShoulders.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboShoulders.Text = _shoulders;
            }

            string _legs = source.Configs["ToonItems"].GetString("Legs", "");
            query = "SELECT name from legs WHERE id = '" + _legs + "'";
            cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            reader = cmd.ExecuteReader();
            set = false;
            while (reader.Read())
            {
                cboLegs.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                cboLegs.Text = _legs;
            }
        }

        public void GetHelmList()
        {
            var query = "SELECT name from head";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboHead.Items.Add(reader.GetString(0));
            }
        }

        private void cboHead_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from head WHERE name='" + cboHead.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblHead.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblHead.Text = cboHead.Text;
            }
        }

        public void GetChestList()
        {
            var query = "SELECT name from chest";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboChest.Items.Add(reader.GetString(0));
            }
        }

        private void cboChest_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from chest WHERE name='" + cboChest.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblChest.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblChest.Text = cboChest.Text;
            }
        }

        public void GetFeetList()
        {
            var query = "SELECT name from feet";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboFeet.Items.Add(reader.GetString(0));
            }
        }

        private void cboFeet_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from feet WHERE name='" + cboFeet.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblFeet.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblFeet.Text = cboFeet.Text;
            }
        }

        public void GetHandsList()
        {
            var query = "SELECT name from hands";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboHands.Items.Add(reader.GetString(0));
            }
        }

        private void cboHands_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from hands WHERE name='" + cboHands.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblHands.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblHands.Text = cboHands.Text;
            }
        }

        public void GetWeapon1List()
        {
            var query = "SELECT name from weapon";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboWeapon1.Items.Add(reader.GetString(0));
            }
        }

        private void cboWeapon1_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from weapon WHERE name='" + cboWeapon1.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblWeapon1.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblWeapon1.Text = cboWeapon1.Text;
            }
        }

        public void GetWeapon2List()
        {
            var query = "SELECT name from weapon";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboWeapon2.Items.Add(reader.GetString(0));
            }
        }

        private void cboWeapon2_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from weapon WHERE name='" + cboWeapon2.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblWeapon2.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblWeapon2.Text = cboWeapon2.Text;
            }
        }

        public void GetShouldersList()
        {
            var query = "SELECT name from shoulders";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboShoulders.Items.Add(reader.GetString(0));
            }
        }

        private void cboShoulders_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from shoulders WHERE name='" + cboShoulders.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblShoulders.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblShoulders.Text = cboShoulders.Text;
            }
        }

        public void GetLegsList()
        {
            var query = "SELECT name from legs";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cboLegs.Items.Add(reader.GetString(0));
            }
        }

        private void cboLegs_TextChanged(object sender, EventArgs e)
        {
            var query = "SELECT id from legs WHERE name='" + cboLegs.Text.Replace("'", "''") + "'";
            var cmd = new SQLiteCommand(query, DBManager.ItemsConnection);
            var reader = cmd.ExecuteReader();
            bool set = false;
            while (reader.Read())
            {
                lblLegs.Text = reader.GetString(0);
                set = true;
            }
            if (set == false)
            {
                lblLegs.Text = cboLegs.Text;
            }
        }
    }
}
