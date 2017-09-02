using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public enum ForwardDirection
{
    None,
    Up,
    Down,
    Left,
    Right,

}


namespace RougeLike
{


    public class Pos:IEquatable<Pos>
    {
        public int x;
        public int y;
        public List<ForwardDirection> dirt = new List<ForwardDirection>();

        public Pos(int _x,int _y)
        {
            x = _x;
            y = _y;
        }

        public bool Equals(Pos other)
        {
            if (other.x == this.x && other.y == this.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Room
    {
        public List<Pos> roomPos = new List<Pos>();//装入房间的点，装入房间边缘的点
        public List<Pos> roomEdgePos = new List<Pos>();

        public void AddRoomPos(Pos roomSpot)
        {
            roomPos.Add(roomSpot);
        }

        public void AddRoomEdgePos(Pos roomEdgeSpot)
        {
            roomEdgePos.Add(roomEdgeSpot);
        }
    }

    class Program
    {
        public static Random r = new Random();
        public static char[,] maps = new char[49, 49];//地图
        public static List<Pos> emptyPosList = new List<Pos>();//装入空余的点的坐标
        public static List<Pos> dirPos = new List<Pos>();//装入转向的点
        public static List<Pos> roadPos = new List<Pos>();//装入所有道路的点
        public static List<Room> roomS = new List<Room>();//装入所有房间的点，以及其边缘的点
        public static List<Pos> deletPos = new List<Pos>();//装入所有死路的点
        public static int keyNumber = 0;

        public static ForwardDirection nowDirct;

        public static void Fill()
        {
            for (int i=0;i<49;i++)
            {
                for (int j=0;j<49;j++)
                {
                    maps[i, j] =' ';
                }
            }


        }

        public static void RandomRoom()
        {
            int roomlength = r.Next(3, 8);
            int roomWidth = r.Next(3,8);//随机房间的大小
            int randomStratX = r.Next(1,48);
            int randomStratY = r.Next(1, 48);//随机房间的开始生成位置
            if (randomStratX%2==0|| randomStratY%2==0)
            {
                return;
            }
            if (roomlength%2==0 || roomWidth%2==0)
            {
                return;
            }


            int roomEdgeMinX = randomStratX - 1;
            int roomEdgeMinY = randomStratY - 1;//计算房间边缘最小的点的坐标

            int roomMaxX = randomStratX + roomlength-1;
            int roomMaxY = randomStratY + roomWidth-1;//计算房间的最大XY
            int roomEdgeMaxX = roomEdgeMinX + roomlength + 1;
            int roomEdgeMaxY = roomEdgeMinY + roomWidth + 1;//计算房间边缘的最大的XY


            if (roomEdgeMaxX < 49&& roomEdgeMaxY < 49)
            {
                for (int i = roomEdgeMinX; i<= roomEdgeMaxX;i++)//判断房间是否重叠
                {
                    for (int j = roomEdgeMinY;j<= roomEdgeMaxY;j++)
                    {
                        if (maps[i,j]=='1')
                        {
                            return;
                        }
                    }
                }

                Room tmp_Room = new Room();
                #region//房间边填充石块
                for (int i = roomEdgeMinX; i <= roomEdgeMaxX; i++)
                {
                    if (i != roomEdgeMinX && i!= roomEdgeMaxX)
                    {
                        Pos tmp_EdgePosLeft = new Pos(i, roomEdgeMinY);
                        Pos tmp_EdgePosRight = new Pos(i, roomEdgeMaxY);
                        tmp_Room.AddRoomEdgePos(tmp_EdgePosLeft);
                        tmp_Room.AddRoomEdgePos(tmp_EdgePosRight);//将房间边缘的点添加进房间类的列表中进行保存
                    }
                    

                    maps[i, roomEdgeMinY] = '#';
                    maps[i, roomEdgeMaxY] = '#';
                }

                for (int j = roomEdgeMinY; j <= roomEdgeMaxY; j++)
                {
                    if (j!= roomEdgeMinY && j != roomEdgeMaxY)
                    {
                        Pos tmp_EdgePosUp = new Pos(roomEdgeMinX, j);
                        Pos tmp_EdgePosDown = new Pos(roomEdgeMaxX, j);
                        tmp_Room.AddRoomEdgePos(tmp_EdgePosUp);
                        tmp_Room.AddRoomEdgePos(tmp_EdgePosDown);
                        maps[roomEdgeMinX, j] = '#';
                        maps[roomEdgeMaxX, j] = '#';
                    }
                   
                }
                #endregion
                for (int i= randomStratX;i<= roomMaxX; i++)//房间填充
                {
                    for (int j= randomStratY;j<= roomMaxY;j++)
                    {
                        Pos roomSpot = new Pos(i,j);
                        tmp_Room.AddRoomPos(roomSpot);
                        maps[i, j] = '1';
                    }
                }
                roomS.Add(tmp_Room);


            }

        }

        public static void FillEmpty()
        {
            for (int i = 1; i < 48; i++)
            {
                for (int j = 1; j < 48; j++)
                {
                    if (maps[i, j] == ' ')
                    {
                        Pos emptyPos = new Pos(i, j);
                        emptyPosList.Add(emptyPos);
                    }
                }
            }

            for (int i = 0; i < 49; i++)
            {
                for (int j = 0; j < 49; j++)
                {
                    if (maps[i, j] == '#')
                    {
                        maps[i, j] = ' ';
                    }
                }
            }
            //for (int i = 0; i < 50; i++)
            //{
            //    maps[i, 0] = '#';
            //    maps[i, 49] = '#';
            //    maps[0, i] = '#';
            //    maps[49, i] = '#';
            //}
        }

        public static Pos Carving()//选择初始点
        {

            Pos tmp_emptyPos = emptyPosList[0];
            foreach (var pair in emptyPosList)//选择空节点中坐标最小的
            {
                if (pair.x < tmp_emptyPos.x && pair.y < tmp_emptyPos.y)
                {
                    tmp_emptyPos = pair;
                }
                else if (pair.x < tmp_emptyPos.x)
                {
                    tmp_emptyPos = pair;
                }
                else if (pair.x <= tmp_emptyPos.x && pair.y < tmp_emptyPos.y)
                {
                    tmp_emptyPos = pair;
                }
            }
            nowDirct = ForwardDirection.None;//重置
            return tmp_emptyPos;
        }

        public static void Connect(List<Pos> roomEdgePos)//选出房间的连接点
        {
            for (int i =0;i< roomEdgePos.Count;i++)
            {
                int stone = 0;//表示这个点周围的石头数量
                int tmp_X = roomEdgePos[i].x;
                int tmp_Y = roomEdgePos[i].y;


                if (tmp_X-1>=0 && (maps[tmp_X - 1, tmp_Y] == '='|| maps[tmp_X - 1, tmp_Y] == '1'))
                {
                    stone++;
                }
                if (tmp_X + 1 < 49 && (maps[tmp_X + 1, tmp_Y]=='=' || maps[tmp_X + 1, tmp_Y] == '1'))
                {
                    stone++;
                }
                if (tmp_Y-1>=0 && (maps[tmp_X,tmp_Y-1]=='=' || maps[tmp_X, tmp_Y - 1] == '1'))
                {
                    stone++;
                }
                if (tmp_Y+1<49 && (maps[tmp_X,tmp_Y+1]=='=' || maps[tmp_X, tmp_Y + 1] == '1'))
                {
                    stone++;
                }

                if (stone<2)
                {
                    roomEdgePos.RemoveAt(i);
                    i--;
                }


            }


        }

        public static Pos Painting(Pos startPos)//画点
        {
            ChoiceDir(startPos);
            ForwardDirection dirt2=ForwardDirection.None;
            if (startPos.dirt.Count > 0)
            {
                if (nowDirct==ForwardDirection.None)
                {
                    nowDirct = startPos.dirt[0];
                }

                //bool isCanDir = IsDir(startPos);
                if (startPos.dirt.Count==1)
                {
                    dirt2 = startPos.dirt[0];
                }

                if (startPos.dirt.Count>1)
                {

                    bool isDir = r.Next(1, 4) == 2 ? true : false;//是否转向

                    if (isDir)
                    {
                        dirt2 = startPos.dirt[r.Next(0, startPos.dirt.Count)];
                    }
                    else
                    {
                        if (startPos.dirt.Contains(nowDirct))
                        {
                            dirt2 = nowDirct;
                        }
                        else
                        {
                            dirt2 = startPos.dirt[0];
                        }
                                    
                    } 

                }
                FillRoad(startPos);//画点
                roadPos.Add(startPos);//添加到道路列表
                DeltEmptyPos(startPos);
                startPos.dirt.Clear();
            }
            else
            {
                FillRoad(startPos);//画点
                return null;
            }
            
            nowDirct = dirt2;
            Pos tmp_ForwardPos;//声明临时POS用于接取下一个点的引用
            switch (dirt2)
            {
                case ForwardDirection.Up:
                    tmp_ForwardPos = new Pos(startPos.x - 1, startPos.y);//从空列表获取这个点表示的POS引用
                    return tmp_ForwardPos;//传出这个点的引用
                case ForwardDirection.Down:
                    tmp_ForwardPos = new Pos(startPos.x + 1, startPos.y);
                    return tmp_ForwardPos;
                case ForwardDirection.Left:
                    tmp_ForwardPos = new Pos(startPos.x, startPos.y-1);
                    return tmp_ForwardPos;
                case ForwardDirection.Right:
                    tmp_ForwardPos = new Pos(startPos.x, startPos.y + 1);
                    return tmp_ForwardPos;
            }
            return null;

        }

        public static void FillRoad()//填充道路
        {
            roadPos.Clear();

            for (int i=0;i<49;i++)
            {
                for (int j=0;j<49;j++)
                {
                    if (maps[i,j]=='=')
                    {
                        Pos roadSpot = new Pos(i, j);
                        roadPos.Add(roadSpot);
                    }
                }
            }

        }

        public static void DeletRoad()//删除死路
        {

            for (int i =0;i<roadPos.Count;i++)
            {
                int stone = 0;//表示这个点周围的石头数量
                int tmp_X = roadPos[i].x;
                int tmp_Y = roadPos[i].y;
                if (tmp_X - 1 >= 0 && maps[tmp_X - 1, tmp_Y] == ' ')
                {
                    stone++;
                }
                if (tmp_X - 1 < 49 && maps[tmp_X + 1, tmp_Y] == ' ')
                {
                    stone++;
                }
                if (tmp_Y - 1 >= 0 && maps[tmp_X, tmp_Y - 1] == ' ')
                {
                    stone++;
                }
                if (tmp_Y + 1 < 49 && maps[tmp_X, tmp_Y + 1] == ' ')
                {
                    stone++;
                }

                if (stone >=3)
                {
                    deletPos.Add(roadPos[i]);
                }

            }
        }

        public static void ChoiceDir(Pos tmp_emptyPos)//计算这个点能去的其余方向
        {
            ForwardDirection dir;
            if (tmp_emptyPos.x - 2 >= 0 && maps[tmp_emptyPos.x - 1, tmp_emptyPos.y] == ' ' && maps[tmp_emptyPos.x - 2, tmp_emptyPos.y] == ' ')
            {

                DeltEmptyPos(tmp_emptyPos.x - 1, tmp_emptyPos.y);
               // DeltEmptyPos(tmp_emptyPos.x - 2, tmp_emptyPos.y);
                int tmp_dir = ISJs(tmp_emptyPos);
                if (tmp_dir==3 || tmp_dir == 2)
                {
                    dir = ForwardDirection.Up;

                    tmp_emptyPos.dirt.Add(dir);
                }
                

            }

            if (tmp_emptyPos.x + 2 < 49 && maps[tmp_emptyPos.x + 1, tmp_emptyPos.y] == ' ' && maps[tmp_emptyPos.x + 2, tmp_emptyPos.y] == ' ')
            {

                DeltEmptyPos(tmp_emptyPos.x + 1, tmp_emptyPos.y);
               // DeltEmptyPos(tmp_emptyPos.x + 2, tmp_emptyPos.y);
                int tmp_dir = ISJs(tmp_emptyPos);
                if (tmp_dir == 3 || tmp_dir == 2)
                {
                    dir = ForwardDirection.Down;
                    tmp_emptyPos.dirt.Add(dir);
                }
            }

            if (tmp_emptyPos.y - 2 >= 0 && maps[tmp_emptyPos.x, tmp_emptyPos.y - 1] == ' ' && maps[tmp_emptyPos.x, tmp_emptyPos.y - 2] == ' ')
            {

                DeltEmptyPos(tmp_emptyPos.x, tmp_emptyPos.y-1);
                //DeltEmptyPos(tmp_emptyPos.x, tmp_emptyPos.y-2);
                int tmp_dir = ISJs(tmp_emptyPos);
                if (tmp_dir == 3 || tmp_dir == 1)
                {
                    dir = ForwardDirection.Left;
                    tmp_emptyPos.dirt.Add(dir);
                }
            }

            if (tmp_emptyPos.y + 2 < 49 && maps[tmp_emptyPos.x, tmp_emptyPos.y + 1] == ' ' && maps[tmp_emptyPos.x, tmp_emptyPos.y + 2] == ' ')
            {
                DeltEmptyPos(tmp_emptyPos.x, tmp_emptyPos.y + 1);
               // DeltEmptyPos(tmp_emptyPos.x, tmp_emptyPos.y + 2);

                int tmp_dir = ISJs(tmp_emptyPos);
                if (tmp_dir == 3 || tmp_dir == 1)
                {
                    dir = ForwardDirection.Right;
                    tmp_emptyPos.dirt.Add(dir);
                }
            }
        }

        public static bool IsDir(Pos startPos)//计算这个点是否能够转向
        {
            bool a, b, c, d;
            a = startPos.x - 1>0?true:false;
            b = startPos.x + 1<49?true:false;
            c = startPos.y - 1>0?true:false;
            d = startPos.y + 1<49?true:false;


            if (a && c &&maps[startPos.x-1,startPos.y-1] !=' ')
            {
                return false;
            }

            if (a && d && maps[startPos.x - 1, startPos.y + 1] != ' ')
            {
                return false;
            }

            if (b && c &&maps[startPos.x + 1, startPos.y - 1] != ' ')
            {
                return false;
            }

            if (b && d && maps[startPos.x + 1, startPos.y + 1] != ' ')
            {
                return false;
            }

            return true;
        }

        public static void DeltEmptyPos(Pos emptyPos)//从空列表中进行删除操作
        {
            

            for (int i=0;i< emptyPosList.Count;i++)
            {
                if (emptyPosList[i].x== emptyPos.x && emptyPosList[i].y== emptyPos.y)
                {
                    emptyPosList.RemoveAt(i);
                    break;
                }
            }

        }

        public static int ISJs(Pos startPos)
        {
            int dir = 0;

            if (startPos.x%2 ==1)
            {
                dir += 1;
            }

            if (startPos.y%2 ==1)
            {
                dir += 2;
            }
            return dir;
        }

        public static void DeltEmptyPos(int x, int y)
        {
            for (int i = 0; i < emptyPosList.Count; i++)
            {
                if (emptyPosList[i].x == x && emptyPosList[i].y == y)
                {
                    emptyPosList.RemoveAt(i);
                    break;
                }
            }
        }

        public static void FillRoad(Pos emptyPos)
        {
            maps[emptyPos.x, emptyPos.y] = '=';
        }

        public static void Print()
        {
            for (int i=0;i<49;i++)
            {
                for (int j = 0; j<49;j++)
                {
                    if (j == 48)
                    {
                        //if (maps[i, j] < 256)
                        //{
                        //    Console.Write(" ");
                        //    Console.WriteLine(maps[i, j]);
                        //}
                        //else
                        //{
                            Console.WriteLine(maps[i, j]);
                        //}
                        
                    }
                    else
                    {
                        //if (maps[i, j] < 256)
                        //{
                        //    Console.Write(" ");
                        //    Console.Write(maps[i, j]);
                        //}
                        //else
                        //{
                            Console.Write(maps[i, j]);
                       // }
                        
                    }
                }
            }
        }

        public static void RandomProp(List<Pos> roomSpot)//房间随机生成道具
        {
            if (roomSpot.Count >= 25)
            {
                int propNumber = r.Next(2, 6);

                while (propNumber > 0)
                {
                    Pos propSpot = roomSpot[r.Next(0, roomSpot.Count)];
                    int prop = Chance();

                    if (prop == 1)
                    {
                        maps[propSpot.x, propSpot.y] = 't';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 2)
                    {
                        maps[propSpot.x, propSpot.y] = 'y';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 3)
                    {
                        maps[propSpot.x, propSpot.y] = 'z';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 4)
                    {
                        maps[propSpot.x, propSpot.y] = 'b';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 5)
                    {
                        maps[propSpot.x, propSpot.y] = 'j';
                        roomSpot.Remove(propSpot);
                    }
                    propNumber--;
                }
            }
            else
            {
                int propNumber = r.Next(1, 3);

                while (propNumber > 0)
                {
                    Pos propSpot = roomSpot[r.Next(0, roomSpot.Count)];
                    int prop = Chance();

                    if (prop == 1)
                    {
                        maps[propSpot.x, propSpot.y] = 't';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 2)
                    {
                        maps[propSpot.x, propSpot.y] = 'y';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 3)
                    {
                        maps[propSpot.x, propSpot.y] = 'z';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 4)
                    {
                        maps[propSpot.x, propSpot.y] = 'b';
                        roomSpot.Remove(propSpot);
                    }
                    else if (prop == 5)
                    {
                        maps[propSpot.x, propSpot.y] = 'j';
                        roomSpot.Remove(propSpot);
                    }
                    propNumber--;
                }
            }
        }

        public static void RandomMonster(List<Pos> roomSpot)//房间随机生成怪物
        {
            int isCreateMonster = Chance();
            if (isCreateMonster == 2)
            {
                return;
            }
            else
            {
                if (roomSpot.Count >= 25)
                {
                    int monsterNumber = r.Next(3, 6);
                    while (monsterNumber > 0)
                    {
                        Pos propSpot = roomSpot[r.Next(0, roomSpot.Count)];
                        maps[propSpot.x, propSpot.y] = 'm';
                        monsterNumber--;
                        roomSpot.Remove(propSpot);
                    }
                }
                else
                {
                    int monsterNumber = r.Next(1, 3);
                    while (monsterNumber > 0)
                    {
                        Pos propSpot = roomSpot[r.Next(0, roomSpot.Count)];
                        maps[propSpot.x, propSpot.y] = 'm';
                        monsterNumber--;
                        roomSpot.Remove(propSpot);
                    }
                }
            }

        }

        public static int Chance()//概率计算
        {
            int timeSeed = r.Next(0, 100);
            if (timeSeed == 0)//  1/100
            {
                return 1;
            }
            else if (timeSeed >= 1 && timeSeed <= 10)//  1/10
            {
                return 2;
            }
            else if (timeSeed >= 50 && timeSeed <= 51)//   1/50
            {
                return 3;
            }
            else if (timeSeed >= 30 && timeSeed <= 40)//  1/10
            {
                return 4;
            }
            else
            {
                return 5;
            }

        }

        public static void RandomStairs()//随机楼梯
        {
            Room stairsRoomUp = roomS[r.Next(0, roomS.Count)];//随机房间
            maps[stairsRoomUp.roomPos[0].x, stairsRoomUp.roomPos[0].y] = 'p';//改变标识
            Pos tmp_PointUp = InitPoint(stairsRoomUp.roomPos[0],"Down");//生成起始点
            DeletListPos(stairsRoomUp.roomPos, tmp_PointUp);
            stairsRoomUp.roomPos.RemoveAt(0);
            maps[stairsRoomUp.roomPos[0].x, stairsRoomUp.roomPos[0].y] = 'k';
            stairsRoomUp.roomPos.RemoveAt(0);


            Room stairsRoomDown = roomS[r.Next(0, roomS.Count)];
            maps[stairsRoomDown.roomPos[0].x, stairsRoomDown.roomPos[0].y] = 'o';
            Pos tmp_PointDown = InitPoint(stairsRoomDown.roomPos[0], "Up");
            DeletListPos(stairsRoomDown.roomPos, tmp_PointDown);
            stairsRoomDown.roomPos.RemoveAt(0);
        }

        public static void DeletListPos(List<Pos> posList,Pos deletPos)//从pos列表中删除指定元素
        {

            for (int i=0;i< posList.Count;i++)
            {
                if (posList[i].x==deletPos.x && posList[i].y==deletPos.y)
                {
                    posList.RemoveAt(i);
                    return;
                }
            }

        }

        public static Pos InitPoint(Pos stairs,string dir)//设置上下楼层的初始点
        {
            int sign = 0;
            if (dir == "Down")
            {
                if (maps[stairs.x - 1, stairs.y] == '1')
                {
                    maps[stairs.x - 1, stairs.y] = '9';
                    sign = 1;
                }
                else if (maps[stairs.x + 1, stairs.y] == '1')
                {
                    maps[stairs.x + 1, stairs.y] = '9';
                    sign = 2;
                }
                else if (maps[stairs.x, stairs.y-1] == '1')
                {
                    maps[stairs.x, stairs.y-1] = '9';
                    sign = 3;
                }
                else if (maps[stairs.x, stairs.y + 1] == '1')
                {
                    maps[stairs.x, stairs.y + 1] = '9';
                    sign = 4;
                }
            }
            else
            {
                if (maps[stairs.x - 1, stairs.y] == '1')
                {
                    maps[stairs.x - 1, stairs.y] = '6';
                    sign = 1;
                }
                else if (maps[stairs.x + 1, stairs.y] == '1')
                {
                    maps[stairs.x + 1, stairs.y] = '6';
                    sign = 2;
                }
                else if (maps[stairs.x, stairs.y - 1] == '1')
                {
                    maps[stairs.x, stairs.y - 1] = '6';
                    sign = 3;
                }
                else if (maps[stairs.x, stairs.y + 1] == '1')
                {
                    maps[stairs.x, stairs.y + 1] = '6';
                    sign = 4;
                }
            }

            if (sign == 1)
            {
                Pos tmp_Pos = new Pos(stairs.x - 1, stairs.y);
                return tmp_Pos;
            }
            else if (sign == 2)
            {
                Pos tmp_Pos = new Pos(stairs.x + 1, stairs.y);
                return tmp_Pos;
            }
            else if (sign == 3)
            {
                Pos tmp_Pos = new Pos(stairs.x, stairs.y-1);
                return tmp_Pos;
            }
            else
            {
                Pos tmp_Pos = new Pos(stairs.x, stairs.y + 1);
                return tmp_Pos;
            }
        }

        public static void RandomRoadKey()//道路上随机钥匙
        {
            keyNumber = roomS.Count-1;
            int roadKey = r.Next(3, 7);
            keyNumber = keyNumber - roadKey;
            while (roadKey>0)
            {
                Pos keyPoint = roadPos[r.Next(0, roadPos.Count)];
                maps[keyPoint.x, keyPoint.y] = 'k';
                roadKey--;
            }
        }

        public static void RandomRoomKey()//房间随机钥匙
        {
            int i = 0;
            while (keyNumber>0)
            {
                int j = 0;
                int tmp_KeyNumber = r.Next(1,3);                
                if (tmp_KeyNumber>= keyNumber)
                {
                    tmp_KeyNumber = keyNumber;
                }
                keyNumber = keyNumber - tmp_KeyNumber;
                while (tmp_KeyNumber>0)
                {
                    maps[roomS[i].roomPos[j].x, roomS[i].roomPos[j].y] = 'k';
                    j++;
                    tmp_KeyNumber--;
                }
                
                i++;
            }
        }

        static void Main(string[] args)
        {
            Fill();
            int randomRoomFrequency = 0;
            while (randomRoomFrequency < 800)
            {
                RandomRoom();
                randomRoomFrequency++;
            }
            FillEmpty();
            Pos startPos = Carving();
            while (emptyPosList.Count > 5)
            {
                startPos = Painting(startPos);
                if (startPos == null)
                {
                    if (roadPos.Count > 0)
                    {
                        for (int i = roadPos.Count - 1; i >= 0; i--)
                        {
                            ChoiceDir(roadPos[i]);
                            if (roadPos[i].dirt.Count > 0)//IsDir(roadPos[i]) &&
                            {
                                startPos = roadPos[i];
                                break;
                            }
                        }
                    }
                    if (startPos == null)
                    {
                        startPos = Carving();
                    }
                }
            }

            for (int i = 0; i < roomS.Count; i++)//选出合适的连接点
            {
                Connect(roomS[i].roomEdgePos);
                int road = r.Next(0, roomS[i].roomEdgePos.Count);
                maps[roomS[i].roomEdgePos[road].x, roomS[i].roomEdgePos[road].y] = 'g';//选出连接点

            }
            FillRoad();
            DeletRoad();
            while (deletPos.Count > 0)//去死路
            {
                foreach (var deletSpot in deletPos)
                {
                    maps[deletSpot.x, deletSpot.y] = ' ';
                }
                deletPos.Clear();
                FillRoad();
                DeletRoad();

            }
            for (int i = 0; i < roomS.Count; i++)
            {
                RandomMonster(roomS[i].roomPos);
                RandomProp(roomS[i].roomPos);
            }

            FillRoad();//重新填充道路列表
            RandomStairs();//生成楼梯
            RandomRoadKey();
            RandomRoomKey();


           

            Print();
            Console.ReadKey();
        }
    }
}
