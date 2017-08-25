using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{
    class NewMyList<T>
    {

        static int lenght = 8;//数组容量
        int count =0;//元素数量
        T[] eum = new T[lenght];
        



        public void Add(T data)//添加
        {
            if (count>=lenght)
            {
                lenght = lenght * 2;

                T[] tmp_eum = new T[lenght];
                eum.CopyTo(tmp_eum, 0);
                eum = tmp_eum;
            }

            eum[count] = data;
            count++;
        }

        public void ReMoveAt(int Indexes)//删除
        {

            if (Indexes>count)
            {
                Console.WriteLine("删除失败，超出数组索引长度");
                return;
            }

            for (int i = Indexes-1; i < count-1;i++)//循环移位
            {
                eum[i] = eum[i + 1];
            }
            count--;
        }


        public void Print()//打印
        {
            for (int i=0;i<count;i++)
            {
                Console.WriteLine(eum[i]);
            }
        }

        public void Set(int Indexes,T data)//更改
        {
            if (Indexes >count)
            {
                Console.WriteLine("更改失败，超出数组索引长度");
                return;
            }
            eum[Indexes-1] = data;
        }

        public void Get(int Indexes)//读取
        {
            if (Indexes > count)
            {
                Console.WriteLine("读取失败，超出数组索引长度");
                return;
            }
            Console.Write("读取的值：");
            Console.WriteLine(eum[Indexes-1]);
            
        }

        public void Insert(int Indexes, T data)//在指定下标添加元素
        {

            if (Indexes > count)
            {
                Console.WriteLine("添加失败，超出数组索引长度");
                return;
            }
            if (count >= lenght)
            {
                lenght = lenght * 2;

                T[] tmp_eum = new T[lenght];
                eum.CopyTo(tmp_eum, 0);
                eum = tmp_eum;
            }

            for (int i = count-1;i>=Indexes;i--)
            {
                eum[i+1] = eum[i];
            }

            eum[Indexes] = data;
            count++;

        }

    }



    class Program
    {
        static void Main(string[] args)
        {
            NewMyList<int> myList = new NewMyList<int>();
            //List<int> xx = new List<int>();

            myList.Add(9);
            myList.Add(6);
            myList.Add(3);
            myList.Add(4);
            myList.Add(54);
            myList.Add(35);
            myList.Add(15);
            myList.Add(98);


            //myList.ReMoveAt(6);
            myList.Insert(0, 187);

            //myList.Set(3,7);
            //myList.Get(9);
            myList.Print();

            Console.ReadKey();
        }
    }
}
