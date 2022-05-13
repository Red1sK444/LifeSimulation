using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    public class Drawer
    {
        private readonly PictureBox _drawableMap;
        public Graphics Graphics;
        public static int CellSize = 25;
        private int _scale;

        public Drawer(PictureBox pictureBox)
        {
            _drawableMap = pictureBox;
        }

        public void ScalePlus()
        {
            _scale++;
        }

        public void ScaleMinus()
        {
            _scale--;
        }

        private void InitDrawableMap(int mapSize)
        {
            _drawableMap.Image = new Bitmap(mapSize * CellSize, mapSize * CellSize);
            Graphics = Graphics.FromImage(_drawableMap.Image);
        }

        Bitmap EagleFlag = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\donkey.png"));

        public void Draw(Cell cell, Color color)
        {
            Brush brush = new SolidBrush(color);
            Graphics.FillRectangle(brush,
                new Rectangle(cell.Position.X * CellSize, cell.Position.Y * CellSize, CellSize, CellSize));
        }

        public void Draw(Cell cell, Bitmap sprite)
        {
            sprite.SetResolution(sprite.HorizontalResolution, sprite.VerticalResolution);
            var warpMode = new ImageAttributes();
            warpMode.SetWrapMode(WrapMode.TileFlipXY);
            Graphics.DrawImage(sprite,
                new Rectangle(cell.Position.X * CellSize, cell.Position.Y * CellSize, CellSize, CellSize), 0, 0,
                sprite.Width, sprite.Height,
                GraphicsUnit.Pixel, warpMode);
        }

        private void StrokeCell(Cell cell)
        {
            var pen = new Pen(Color.SlateGray);
            Graphics.DrawRectangle(pen,
                new Rectangle(cell.Position.X * CellSize, cell.Position.Y * CellSize, CellSize, CellSize));
        }

        public void DrawCycle(List<Cell> changedCells)
        {
            for (var i = 0; i < changedCells.Count; i++)
            {
                changedCells[i].Draw(this);
                StrokeCell(changedCells[i]);
            }

            changedCells.Clear();
            _drawableMap.Refresh();
        }

        public void ScaleCheck(Cell[,] field, int mapSize)
        {
            if (_scale != 0)
            {
                CellSize += _scale;
                _scale = 0;
                if (CellSize < 5) CellSize = 5;
                //if (CellSize > 40) CellSize = 40;
                _drawableMap.Image.Dispose();
                _drawableMap.Image = new Bitmap(mapSize * CellSize, mapSize * CellSize);
                Graphics.Dispose();
                Graphics = Graphics.FromImage(_drawableMap.Image);
                for (var i = 0; i < mapSize; i++)
                {
                    for (var j = 0; j < mapSize; j++)
                    {
                        field[i, j].Draw(this);
                        StrokeCell(field[i, j]);
                    }
                }
            }

            _drawableMap.Refresh();
        }

        public void DrawMap(Cell[,] field, int mapSize)
        {
            InitDrawableMap(mapSize);
            for (var i = 0; i < mapSize; i++)
            {
                for (var j = 0; j < mapSize; j++)
                {
                    field[i, j].Draw(this);
                    StrokeCell(field[i, j]);
                }
            }

            _drawableMap.Refresh();
        }

        public void DrawSeasonChanging(Cell[,] field, int mapSize)
        {
            for (var y = 0; y < mapSize; y++)
            {
                for (var x = 0; x < mapSize; x++)
                {
                    field[y, x].Draw(this);
                    StrokeCell(field[y, x]);
                }
            }
            _drawableMap.Refresh();
            // for (var i = 0; i < mapSize; i++)
            // {
            //     var j = 0;
            //     var k = i;
            //     for (; j < mapSize && k >= 0; j++, k--)
            //     {
            //         field[k, j].Draw(this);
            //         StrokeCell(field[k, j]);
            //     }
            //
            //     _drawableMap.Refresh();
            // }
            //
            // for (var j = 0; j < mapSize; j++)
            // {
            //     var i = mapSize - 1;
            //     var k = j;
            //     for (; k < mapSize && i >= 0; k++, i--)
            //     {
            //         field[i, k].Draw(this);
            //         StrokeCell(field[i, k]);
            //     }
            //
            //     _drawableMap.Refresh();
            // }
        }
    }
}