using System;
using System.Drawing;
using System.Security.Cryptography;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class Fruitable : ITickObject
    {
        private int _fruitCountToRipe;
        private Fruit _fruit;
        private Map _map;
        private Cell _cell;
        private int _fruitType;
        private Color _plantColor;

        private Fruit NewFruit()
        {
            if (_fruitType == 1)
            {
                return _fruit = new NonToxicFruit(_map, _cell, _plantColor);    
            }
            return _fruit = new ToxicFruit(_map, _cell, _plantColor);  
        }
        public Fruitable(Map map, Cell cell)
        {
            _map = map;
            _cell = cell;
            _plantColor = _cell.GetPlant().Color;
            _fruitType = Utils.GetRandomInt(2);
            _fruit = NewFruit();
            _fruitCountToRipe = Utils.GetRandomInt(15);
        }

        public bool HasOneMoreFruit()
        {
            return _fruitCountToRipe > 0;
        }

        public void DrawFruit(Drawer drawer)
        {
            _fruit.Draw(drawer);
        }

        private void FruitFellCheck()
        {
            if (_fruit.Fallen)
            {
                _fruitCountToRipe--;
                if (_fruitCountToRipe > 0)
                {
                    _fruit = NewFruit();
                }
                else
                {
                    _fruit = null;
                }
            }
        }

        public void TimeTick(int? timeTickCount)
        {
            _fruit.TimeTick(timeTickCount);
            FruitFellCheck();
        }
    }
}