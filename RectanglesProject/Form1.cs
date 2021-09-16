using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RectanglesProject
{
    public partial class Form1 : Form
    {
        Graphics g;

        RectangleList Rectangles = new RectangleList();
        public Form1()
        {
            InitializeComponent();

            //var rect = new RectangleList();
            //rect.Add(new RectangleCoordinates(20, 2, 50, 68));
            //rect.Add(new RectangleCoordinates(2, 99, 23, 75));
            //rect.Add(new RectangleCoordinates(140, 50, 230, 100));
            //Rectangles.Add(rect);
            //rect = new RectangleList();
            //rect.Add(new RectangleCoordinates(285, 40, 350, 90));
            //rect.Add(new RectangleCoordinates(498, 30, 600, 400));
            //Rectangles.Add(rect);
            //Rectangles.Add(new RectangleCoordinates(52, 176, 109, 230));
            //Rectangles.Add(new RectangleCoordinates(100, 320, 178, 400));

            //var rect1 = new RectangleList();
            //rect1.Add(new RectangleCoordinates(20, 2, 50, 68));
            //rect1.Add(new RectangleCoordinates(2, 99, 23, 75));
            //rect1.Add(new RectangleCoordinates(140, 50, 230, 100));
            //var rect2 = new RectangleList();
            //rect2.Add(new RectangleCoordinates(285, 40, 350, 90));
            //rect2.Add(new RectangleCoordinates(498, 30, 600, 400));
            //var rect3 = new RectangleList();
            //rect3.Add(rect1);
            //rect3.Add(rect2);
            //Rectangles.Add(rect3);
            //Rectangles.Add(new RectangleCoordinates(52, 176, 109, 230));
            //Rectangles.Add(new RectangleCoordinates(100, 320, 178, 400));

            // Список верхнего уровня
            var wholeRect = new RectangleList();
            

            // Список прямоугольников "границ". Внутри границ могут быть прямоугльники,
            // не входящие в этот список.
            var borderRect = new RectangleList();
            borderRect.Add(new RectangleCoordinates(50, 50, 450, 70));
            borderRect.Add(new RectangleCoordinates(50, 330, 450, 350));
            borderRect.Add(new RectangleCoordinates(50, 70, 70, 330));
            borderRect.Add(new RectangleCoordinates(430, 70, 450, 330));
            wholeRect.Add(borderRect);

            // Список прямоугольников, визуально расположенный внутри границ, но не принадлежащий им.
            var insideRect = new RectangleList();
            insideRect.Add(new RectangleCoordinates(70, 70, 100, 150));
            insideRect.Add(new RectangleCoordinates(70, 70, 100, 150));
            insideRect.Add(new RectangleCoordinates(100, 120, 150, 150));
            wholeRect.Add(insideRect);

            // Объект в списке верхнего уровня
            wholeRect.Add(new RectangleCoordinates(20, 400, 40, 450));
            
            Rectangles.Add(wholeRect);
            Rectangles.Lowest = new Point(0, 0);
            Rectangles.Highest = new Point(799, 499);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = CreateGraphics();
            g.Clear(Color.White);
            foreach (RectangleCoordinates currentRect in Rectangles.List)
            {
                currentRect.Draw(g);
            }
            g.Dispose();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = e.Location;

            Point lowest = new Point(0, 0);
            Point highest = new Point(799, 499);

            RectangleCoordinates First = new RectangleCoordinates(0, 0, 799, 499);
            RectangleCoordinates Second = new RectangleCoordinates(0, 0, 799, 499);

            Pen pen = new Pen(Color.Red);
            pen.Width = 2;

            RectangleCoordinates currentRectOrList = ListCheck(Rectangles, point);
            if(currentRectOrList != null)
            {
                lowest = currentRectOrList.Lowest;
                highest = currentRectOrList.Highest;
                First.Lowest = currentRectOrList.Lowest;
                First.Highest = currentRectOrList.Highest;
                Second.Lowest = currentRectOrList.Lowest;
                Second.Highest = currentRectOrList.Highest;
            }
            
            
            if(currentRectOrList is RectangleList)
            {
                RectangleList currentList = (RectangleList)currentRectOrList;
                CoreProcess(currentList, point, ref First, ref Second, ref lowest, ref highest);
            }

            RectangleList wholeList = ReturnOneLevelRectangles(Rectangles); //список, где все прямоугольники в одном уровне (без иерархии)
            CoreProcess(wholeList, point, ref First, ref Second, ref lowest, ref highest);

            g = CreateGraphics();
            RectangleCoordinates Result = new RectangleCoordinates(lowest, highest);
            g.DrawRectangle(pen, Result.Lowest.X, Result.Lowest.Y, Result.Width, Result.Height);
            g.Dispose();
        }


        RectangleCoordinates ListCheck(RectangleCoordinates Rect, Point point) //Функция возвращает прямоугольник/прямоугольник список внутри которого заданная точка
        {
            if (Rect.IsPointIn(point))
            {
                if (Rect is RectangleList)
                {
                    RectangleList tempList = (RectangleList)Rect;
                    foreach (RectangleCoordinates currentRect in tempList.List)
                    {
                        RectangleCoordinates temp = ListCheck(currentRect, point);
                        if(temp != null)
                        {
                            Rect = temp;
                        }
                    }
                }
                return Rect;
            }
            return null;
           
        }

        RectangleList ReturnOneLevelRectangles(RectangleList List)
        {
            RectangleList newList = new RectangleList();
            foreach(RectangleCoordinates currentRect in List.List)
            {
                if(currentRect is RectangleList)
                {
                    RectangleList addList = (RectangleList)currentRect;
                    newList.List.AddRange(ReturnOneLevelRectangles(addList).List);
                }
                newList.List.Add(currentRect);
            }
            
            return newList;
        }

        void CoreProcess(RectangleList rectangleList, Point clickPoint, ref RectangleCoordinates First, ref RectangleCoordinates Second, ref Point lowest, ref Point highest) //Основной процесс
        {
            foreach (RectangleCoordinates currentRect in rectangleList.List)
            {
                switch (currentRect.CheckVerticalCross(clickPoint))
                {
                    case 1:
                        if (currentRect.Highest.Y >= First.LowestY)
                            First.LowestY = currentRect.Highest.Y;
                        break;
                    case 2:
                        if (currentRect.Lowest.Y <= First.HighestY)
                            First.HighestY = currentRect.Lowest.Y;
                        break;
                }

                switch (currentRect.CheckHorizontalCross(clickPoint))
                {
                    case 1:
                        if (currentRect.Highest.X >= Second.LowestX)
                            Second.LowestX = currentRect.Highest.X;
                        break;
                    case 2:
                        if (currentRect.CheckHorizontalCross(clickPoint) == 2 && currentRect.Lowest.X <= Second.HighestX)
                            Second.HighestX = currentRect.Lowest.X;
                        break;
                }
            }

            foreach (RectangleCoordinates currentRect in rectangleList.List)
            {
                if (currentRect.CheckVerticalLineCross(First.LowestY, First.HighestY))
                {

                    if (currentRect.Lowest.X <= First.HighestX &&
                        currentRect.Lowest.X >= clickPoint.X)
                    {
                        First.HighestX = currentRect.Lowest.X;
                    }

                    else if (currentRect.Highest.X >= First.LowestX &&
                             currentRect.Highest.X <= clickPoint.X)
                    {
                        First.LowestX = currentRect.Highest.X;
                    }
                }

                if (currentRect.CheckHorizontalLineCross(Second.LowestX, Second.HighestX))
                {
                    if (currentRect.Lowest.Y <= Second.HighestY &&
                        currentRect.Lowest.Y >= clickPoint.Y)
                    {
                        Second.HighestY = currentRect.Lowest.Y;
                    }
                    else if (currentRect.Highest.Y >= Second.LowestY &&
                             currentRect.Highest.Y <= clickPoint.Y)
                    {
                        Second.LowestY = currentRect.Highest.Y;
                    }
                }

            }

            lowest.X = First.LowestX;
            lowest.Y = Second.LowestY;
            highest.X = First.HighestX;
            highest.Y = Second.HighestY;
        }
    }
}
