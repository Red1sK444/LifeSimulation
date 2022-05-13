using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    public partial class Form1 : Form
    {
        private Drawer _drawer;
        private Game _game;
        public Form1()
        {
            InitializeComponent();
            GameInit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameInit();
            _game.StartSimulation(Timer);
        }

        private void DrawableMap_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            _game.SelectInfoItem(coordinates);
        }

        public void GameInit()
        {
            _drawer = new Drawer(DrawableMap);
            _game = new Game(_drawer, label1);
            
            Timer.Enabled = false;
            Timer.Interval = 300;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _game.TimeTick();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            _drawer.ScalePlus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _drawer.ScaleMinus();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}