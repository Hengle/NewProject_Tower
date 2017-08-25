using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Scenes scene = new Scenes();
            scene.TarGet_Position();//目标位置就是初始蛇头以及苹果的位置
            bool c = true;
            int Checkpoint = 1;
            while (c)
            {
                if (Checkpoint == 1 && scene.play.Count < 10)//添加关卡
                {
                    Console.WriteLine("你现在体长{0}", scene.play.Count);
                    scene.Fill_Map();
                }
                else if(Checkpoint == 1 && scene.play.Count == 10)
                {
                    scene.play.Clear();//清除蛇的列表
                    Checkpoint++;
                    Console.WriteLine("进入下一关");
                    Console.ReadKey();
                    int[] Tmp_Play = new int[2] { 5, 5 };//重新添加蛇到列表去
                    scene.play.Add(Tmp_Play);
                }

                if (Checkpoint==2)
                {
                   
                    scene.Fill_Map2();//关卡地图
                }

                
                if (scene.play.Count>0)//蛇的身体大于0时，给蛇的身体的坐标全部附上‘O’
                {
                    foreach (var pair in scene.play)
                    {
                        scene.maps[pair[0], pair[1]] = 'O';
                    }
                }
                if (scene.monster.Count == 0)//苹果数量等于0的时候，重新随机添加一个苹果进入苹果列表
                {
                    scene.AddMs();
                }
                if (scene.monster.Count > 0)//苹果的数量大于0时，给苹果的坐标全部附上‘0’；
                {
                    foreach (var pair in scene.monster)
                    {
                        scene.maps[pair[0], pair[1]] = '0';
                    }
                }
                scene.Paint_Map();//打印地图
                c = scene.Move();//判断移动
                Console.Clear();//清除屏幕

            }

            Console.WriteLine("游戏结束！");

            Console.ReadKey();
        }
    }
}
