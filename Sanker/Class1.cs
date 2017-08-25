using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Scenes
    {
        public Random r = new Random();
        public char[,] maps = new char[Height, Width];
        static public int Height = 15;
        static public int Width = 30;
        public int Player_x = 5;
        public int Player_y = 5;
        public List<int[]> play = new List<int[]>();
        public List<int[]> monster = new List<int[]>();
        public int m = 5;
        public int n = 5;//m,n用于接取蛇头的上一次的坐标   
       

        public void Fill_Map()//关卡1的地图
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    maps[i, j] = ' ';
                }
            }

            for (int i = 0; i < Height; i++)
            {
                maps[i, 0] = '#';
            }
            for (int i = 0; i < Height; i++)
            {
                maps[i, Width - 1] = '#';
            }
            for (int j = 0; j < Width; j++)
            {
                maps[0, j] = '#';
            }
            for (int j = 0; j < Width; j++)
            {
                maps[Height - 1, j] = '#';
            }
        }

        public void Fill_Map2()//关卡2的地图
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    maps[i, j] = ' ';
                }
            }

            for (int i = 0; i < Height; i++)
            {
                maps[i, 0] = '#';
            }
            for (int i = 0; i < Height; i++)
            {
                maps[i, Width - 1] = '#';
            }
            for (int j = 0; j < Width; j++)
            {
                maps[0, j] = '#';
            }
            for (int j = 0; j < Width; j++)
            {
                maps[Height - 1, j] = '#';
            }

            for (int j = 3; j < 7; j++)
            {
                maps[10, j] = '#';
                maps[11, j] = '#';
            }

        }

        public void TarGet_Position()//初始蛇头以及苹果的位置
        {
            int[] play1 = new int[2] { Player_x, Player_y };
            play.Add(play1);
            int[] ms1 = new int[2] { 10, 15 };
            monster.Add(ms1);
        }

        public void AddMs()//添加苹果
        {
            int a, b;
            bool c = true;
            a = r.Next(1,14);
            b = r.Next(1, 28);
            while (c)
            {
                if (maps[a, b] != 'O')
                {

                    int[] Tmpms = new int[2];
                    Tmpms[0] = a;
                    Tmpms[1] = b;
                    monster.Add(Tmpms);
                    c = false;
                }
                a = r.Next(1, 15);
                b = r.Next(1, 29);
            }
        }

        public void Paint_Map()//打印地图
        {
            
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i==play[0][0]&&j==play[0][1])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                    }
                    Console.Write(maps[i, j]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (j == (Width - 1))
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        public void Transfer()//当蛇吃了苹果后，将苹果的坐标添加到列表的最后一位，使用一个临时变量获取这个坐标，同时将列表中的所有坐标元素全部向后移动一位，然后将临时变量的值赋给列表索引的第一位！
        {
            if (play.Count>1)
            {
                int[] TMP = new int[2];//声明临时变量

                TMP[0]= play[play.Count - 1][0];
                TMP[1] = play[play.Count - 1][1];//将苹果的坐标值赋给临时变量
                for (int i= play.Count - 1; i>0;i--)//列表数据移动，向后移动一位
                {
                    int[] TMP2 = new int[2];
                    TMP2[0] = play[i - 1][0];
                    TMP2[1] = play[i - 1][1];
                    play[i][0] = TMP2[0];
                    play[i][1] = TMP2[1];
                }
                play[0][0] = TMP[0];
                play[0][1] = TMP[1];//临时变量值赋给列表索引第一位
                
            }
        }

        public void MoveList()
        {
            if (play.Count>1)
            {
                for(int i = play.Count - 1; i > 0; i--)//列表数据移动，向后移动一位
                {
                    int[] TMP2 = new int[2];
                    TMP2[0] = play[i - 1][0];
                    TMP2[1] = play[i - 1][1];
                    play[i][0] = TMP2[0];
                    play[i][1] = TMP2[1];
                }
            }
        }

        public bool Apple(int x,int y)//吃到了苹果的增加的函数
        {
            
            if (maps[x,y]=='0')
            {
                for (int i=0;i<monster.Count;i++)
                {
                    if (monster[i][0] == x && monster[i][1] == y)
                    {
                        monster.RemoveAt(i);//将苹果从列表中删除
                    }
                }
                int[] play3 = new int[2] { x, y };
                play.Add(play3);//把刚才吃的苹果的值添加到蛇身体列表的最后一位
                Transfer();//重新排序列表
                return true;
            }
            return false;
        }


        public bool Move()//移动函数
        {
            Thread.Sleep(400);
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo Info = Console.ReadKey();

                switch (Info.Key)
                {
                    case ConsoleKey.UpArrow:
                        m = play[0][0];
                        n = play[0][1];//获取改变之前蛇头的位置
                        if (play[0][0] - 1 <= 0)//判断改变坐标是否超出地图
                        {

                            return false;
                        }
                        if (maps[play[0][0] - 1, play[0][1]] == '0')//判断改变坐标是否是苹果
                        {
                            Apple(play[0][0] - 1, play[0][1]);
                            return true;
                        }
                        if (maps[play[0][0] - 1, play[0][1]] == 'O')//判断改变坐标是否是自己的身体
                        {
                            return false;
                        }
                        if (maps[play[0][0] - 1, play[0][1]] == '#')//判断改变坐标是否撞到了地图障碍
                        {
                            return false;
                        }
                        MoveList();//重新排列列表数据
                        play[0][0] = play[0][0] - 1;//更改列表索引的第一位的值
                        return true;
                    case ConsoleKey.DownArrow:
                        m = play[0][0];
                        n = play[0][1];
                        if (play[0][0] + 1 >= Height - 1)
                        {
                            return false;
                        }
                        if (maps[play[0][0] + 1, play[0][1]] == '0')
                        {
                            Apple(play[0][0] + 1, play[0][1]);
                            return true;
                        }
                        if (maps[play[0][0] + 1, play[0][1]] == 'O')
                        {
                            return false;
                        }
                        if (maps[play[0][0] + 1, play[0][1]] == '#')
                        {
                            return false;
                        }
                        MoveList();
                        play[0][0] = play[0][0] + 1;
                        return true;
                    case ConsoleKey.LeftArrow:
                        m = play[0][0];
                        n = play[0][1];
                        if (play[0][1] - 1 <= 0)
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1] - 1] == '0')
                        {
                            Apple(play[0][0], play[0][1] - 1);
                            return true;
                        }
                        if (maps[play[0][0], play[0][1] - 1] == 'O')
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1] - 1] == '#')
                        {
                            return false;
                        }
                        MoveList();
                        play[0][1] = play[0][1] - 1;
                        return true;
                    case ConsoleKey.RightArrow:
                                                m = play[0][0];
                        n = play[0][1];
                        if (play[0][1] + 1 >= Width - 1)
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1] + 1] == '0')
                        {
                            Apple(play[0][0], play[0][1] + 1);
                            return true;
                        }
                        if (maps[play[0][0], play[0][1] + 1] == 'O')
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1] + 1] == '#')
                        {
                            return false;
                        }
                        MoveList();
                        play[0][1] = play[0][1] + 1;
                        return true;
                }
            }
            else
            {
                bool c;   

                c = Ai_Move();
                return c;
            }

            return true;
        }


        public bool Ai_Move()//自动移动函数
        {
            if (m == play[0][0] && n == play[0][1])//游戏开始不做更改，蛇头坐标向上移动
            {
                play[0][0] = play[0][0] - 1;
            }
            else
            {
                if (play[0][0] > m)//与上一次蛇头的位置进行比较，判断方向，进行自动移动
                {
                    m = play[0][0];//更改坐标，接取改变之前蛇头的位置
                    if (play[0][0] + 1 < 14)
                    {
                        if (maps[play[0][0] + 1, play[0][1]] == 'O')
                        {
                           
                            return false;
                        }
                        if (maps[play[0][0] + 1, play[0][1]] == '#')
                        {

                            return false;
                        }
                        if (maps[play[0][0] + 1, play[0][1]] == '0')
                        {
                            Apple(play[0][0] + 1, play[0][1]);
                            return true;
                        }
                        MoveList();
                        play[0][0] = play[0][0] + 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (play[0][0] < m)
                {
                    m = play[0][0];
                    
                    if (play[0][0] - 1 > 0)
                    {

                        if (maps[play[0][0] - 1, play[0][1]] == 'O')
                        {
                            return false;
                        }
                        if (maps[play[0][0] - 1, play[0][1]] == '#')
                        {
                            return false;
                        }
                        if (maps[play[0][0]-1, play[0][1]] == '0')
                        {
                            Apple(play[0][0]-1, play[0][1]);
                            return true;
                        }
                        MoveList();
                        play[0][0] = play[0][0] - 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (play[0][1] > n)
                {
                    n = play[0][1];
                    
                    if (play[0][1] + 1 < 29)
                    {
                        if (maps[play[0][0], play[0][1] + 1] == 'O')
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1] + 1] == '#')
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1]+1] == '0')
                        {
                            Apple(play[0][0], play[0][1]+1);
                            return true;
                        }
                        MoveList();
                        play[0][1] = play[0][1] + 1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if(play[0][1] < n)
                {
                    n = play[0][1];
                   
                    if (play[0][1] - 1 > 0)
                    {
                        if (maps[play[0][0], play[0][1] - 1] == 'O')
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1] - 1] == '#')
                        {
                            return false;
                        }
                        if (maps[play[0][0], play[0][1]-1] == '0')
                        {
                            Apple(play[0][0], play[0][1]-1);
                            return true;
                        }
                        MoveList();
                        play[0][1] = play[0][1] - 1;
                    }
                    else
                    {
                        return false;
                    }

                }
            }

            return true;
        }

       
    }
}
