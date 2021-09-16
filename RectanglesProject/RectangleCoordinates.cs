using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectanglesProject
{
    class RectangleCoordinates
    {
        public RectangleCoordinates(Point point1, Point point2)
        {
            int temp;

            if (point1.X > point2.X)
            {
                temp = point1.X;
                point1.X = point2.X;
                point2.X = temp;
            }

            if (point1.Y > point2.Y)
            {
                temp = point1.Y;
                point1.Y = point2.Y;
                point2.Y = temp;
            }

            Lowest = point1;
            Highest = point2;
        }

        public RectangleCoordinates(int x1, int y1, int x2, int y2)
        {
            Point point1 = new Point(x1, y1);
            Point point2 = new Point(x2, y2);

            int temp;

            if (point1.X > point2.X)
            {
                temp = point1.X;
                point1.X = point2.X;
                point2.X = temp;
            }

            if (point1.Y > point2.Y)
            {
                temp = point1.Y;
                point1.Y = point2.Y;
                point2.Y = temp;
            }

            Lowest = point1;
            Highest = point2;

        }
        virtual public void Draw(Graphics g)
        {
            // Код для заливки обычных прямоугольников (у списков выключен useFilling).
            // Можно раскомментировать, если это улучшит визуальное восприятие объектов
            //if (useFilling)
            //{
            //    var b = new SolidBrush(lineColor);
            //    g.FillRectangle(b, lowest.X + 1, lowest.Y + 1, Width - 1, Height - 1);
            //}
            //else
            {
                var p = new Pen(lineColor);
                p.Width = lineWidth;
                g.DrawRectangle(p, lowest.X, lowest.Y, Width, Height);
            }
        }

        protected Point lowest;
        protected Point highest;
        protected int lineWidth = 1;
        protected bool useFilling = true;
        protected Color lineColor = Color.Black;
        public Point Lowest { get => lowest; set => lowest = value; }
        public Point Highest { get => highest; set => highest = value; }

        public int LowestX { get => lowest.X; set => lowest.X = value; }
        public int HighestX { get => highest.X; set => highest.X = value; }
        public int LowestY { get => lowest.Y; set => lowest.Y = value; }
        public int HighestY { get => highest.Y; set => highest.Y = value; }


        public int Width { get => Highest.X - Lowest.X; }
        public int Height { get => Highest.Y - Lowest.Y; }

        public int CheckVerticalCross(Point point) //проверка пересечения при уходе от точки по вертикали
        {
            if (Highest.Y <= point.Y && point.X >= Lowest.X && point.X <= Highest.X)
            {
                return 1; //пересечение сверху
            }
            else if (Lowest.Y >= point.Y && point.X >= Lowest.X && point.X <= Highest.X)
            {
                return 2; //пересечение снизу
            }
            return -1;
        }

        public int CheckHorizontalCross(Point point) //проверка пересечения при уходе от точки по горизонтали
        {
            if (Highest.X <= point.X && point.Y >= Lowest.Y && point.Y <= Highest.Y)
            {
                return 1; //пересечение слева
            }
            else if (Lowest.X >= point.X && point.Y >= Lowest.Y && point.Y <= Highest.Y)
            {
                return 2; //пересечение справа
            }
            return -1;
        }

        public bool CheckVerticalLineCross(int lowestY, int highestY) //проверка есть ли пересечение если перемещать вертикалиную линию по горизонтали
        {
            // Если высшая точка прямоугольника ниже данной низшей точки или
            // низшая точка прямоугольника выше данной точки, то прямоугольник 
            // не прошёл проверку пересечения.
            return !(Highest.Y <= lowestY || Lowest.Y >= highestY);
        }
        public bool CheckHorizontalLineCross(int lowestX, int highestX) //проверка есть ли пересечение если перемещать горизонтальную линию по вертикали
        {
            return !(Highest.X <= lowestX || Lowest.X >= highestX);
        }

        public bool IsPointIn(Point point) //Проверка вхождения точки в прямоугольник
        {
            if (point.X <= this.HighestX &&
               point.X >= this.LowestX &&
               point.Y <= this.HighestY &&
               point.Y >= this.LowestY)
            {
                return true;
            }

            return false;
        }
    }

    class RectangleList : RectangleCoordinates
    {
        private List<RectangleCoordinates> list = new List<RectangleCoordinates>();
        public List<RectangleCoordinates> List { get => list; }
        public RectangleList() : base(0, 0, 0, 0)
        {
            lineWidth = 2;
            lineColor = Color.Blue;
            useFilling = false;
        }
        public RectangleList(Point point1, Point point2) : base(new Point(0, 0), new Point(0, 0))
        {
            lineWidth = 2;
            lineColor = Color.Blue;
            useFilling = false;
        }

        public RectangleList(int x1, int y1, int x2, int y2) : base(0, 0, 0, 0)
        {
            lineWidth = 2;
            lineColor = Color.Blue;
            useFilling = false;
        }

        private void RecalcSize()
        {
            this.highest.X = -100000;
            this.highest.Y = -100000;
            this.lowest.X = 100000;
            this.lowest.Y = 100000;
            foreach (RectangleCoordinates rect in this.list)
            {
                highest.X = Math.Max(highest.X, rect.HighestX);
                highest.Y = Math.Max(highest.Y, rect.HighestY);
                lowest.X = Math.Min(lowest.X, rect.LowestX);
                lowest.Y = Math.Min(lowest.Y, rect.LowestY);
            }
        }

        public void Add(RectangleCoordinates rect)
        {
            this.list.Add(rect);
            RecalcSize();
        }

        public int Count { get => list.Count; }

        override public void Draw(Graphics g)
        {
            base.Draw(g);
            foreach (RectangleCoordinates rect in this.list)
                rect.Draw(g);
        }
        
    }
}
