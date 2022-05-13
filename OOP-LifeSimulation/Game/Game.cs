using System.Drawing;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace OOP_LifeSimulation
{
    public class Game : ITickObject
    {
        private int _timeTickCounter = 1;
        private readonly Map _map = new Map(100);
        private readonly Spawner _spawner;
        private readonly Drawer _drawer;
        private Label _infoPanel;
        private IShowingInfo informator;

        public Game(Drawer drawer, Label infoPanel)
        {
            _drawer = drawer;
            _spawner = new Spawner(_map);
            _map.Spawner = _spawner;
            _infoPanel = infoPanel;
        }


        public void StartSimulation(Timer timer)
        {
            _map.GenerateLandscape(_drawer);
            _spawner.InitialUnitSpawn();
            _drawer.DrawCycle(_map.ChangedCells);

            timer.Enabled = true;
        }

        public void TimeTick(int? timeTickCount = null)
        {
            _map.TimeTick(_timeTickCounter);
            _drawer.DrawCycle(_map.ChangedCells);
            if (_timeTickCounter % _map.SeasonDuration == 0)
            {
                _map.ChangeSeason(_drawer);
            }
            _drawer.ScaleCheck(_map.Field, _map.MapSize); // timetick
            _timeTickCounter++;
            ShowInfo();
        }

        public void SelectInfoItem(Point coordinates)
        {
            while (coordinates.X % Drawer.CellSize != 0)
            {
                coordinates.X--;
            }
            while (coordinates.Y % Drawer.CellSize != 0)
            {
                coordinates.Y--;
            }

            var realCoordinates = new Coords(coordinates.X / Drawer.CellSize, coordinates.Y / Drawer.CellSize);
            informator = _map.Field[realCoordinates.Y, realCoordinates.X].GetIShowingInfo();
        }
        private void ShowInfo()
        {
            var info = $"{(informator != null ? informator.SendInfo() : "")}";
            _infoPanel.Text = "Info: " + info;
        }
    }
}