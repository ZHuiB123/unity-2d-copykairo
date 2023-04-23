using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavSystem
{
    public float accuracy = 0.04f;

    private Vector3 NowTransform;
    private Vector3 EndPoint;
    private int[] EndPointPosT = new int[2];
    private int[,] map;
    private int[,] distance = new int[14, 11];//值
    private int[,] distance_start = new int[14, 11];//起点值
    private int[,] distance_end = new int[14, 11];//终点值
    private int[,] direction = new int[14, 11];//0-NULL 上下左右
    public List<Person> path = new List<Person>();
    List<Person> persons = new List<Person>();

    private void ReStart()
    {
        path.Clear();
        persons.Clear();
    }

    public void NavStart()
    {
        ReStart();
        EndPointPosT = TransformCoordinate._instance.TransformToCoordinate(EndPoint);
        DelayOpen();
    }

    void DelayOpen()
    {
        //print("End=(" + EndPointPosT[0] + "," + EndPointPosT[1]+ ")");
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                distance[i, j] = 999;
            }
        }
        map = MapMessage._instance.MapCategory;
        int[] pos_C = TransformCoordinate._instance.TransformToCoordinate(NowTransform);
        int x = pos_C[0];
        int y = pos_C[1];
        distance[x, y] = 999;
        distance_start[x, y] = 0;
        distance_end[x, y] = distance[x, y];
        Person start_p = new Person(x, y, 999, 0, 999, -1);
        Near_distance(x, y, start_p);
        //print(x+","+ y);
        Person end_p = new Person(-1, -1, 999, 999, 999, 0);
        while (true)
        {
            int min_dis = 999;
            Person next = new Person(-1, -1, 999, 999, 999, 0);
            foreach (Person p in persons)
            {
                if (p.Getdis() < min_dis)
                {
                    min_dis = p.Getdis();
                    next = p;
                }
                else if (p.Getdis() == min_dis && p.Getend() < next.Getend())
                {
                    next = p;
                }
            }
            if (next.Getend() < 2)
            {
                end_p = next;
                break;
            }
            Near_distance(next.Getx(), next.Gety(), next);
            persons.Remove(next);
        }
        //path.Add(end_p);
        path.Add(Near_end(end_p));
        int path_id = 0;
        while (true)
        {
            Person p = path[path_id];
            path.Add(p.before);
            path_id++;
            if (p.before == start_p) break;
        }
        path.Reverse();
        //for (int i = 0; i < path.Count; i++)
        //{
        //    Debug.Log("(" + path[i].Getx() + "," + path[i].Gety() + "):" + path[i].Getdir());
        //}
    }

    void Near_distance(int x, int y, Person before)
    {
        bool[] find = { true, true, true, true };//上下左右
        if (x < 0.5f || (map[x - 1, y] != 1))
        {
            find[2] = false;
        }
        if (x > 12.5f || (map[x + 1, y] != 1))
        {
            find[3] = false;
        }
        if (y < 0.5f || (map[x, y - 1] != 1))
        {
            find[1] = false;
        }
        if (y > 9.5f || (map[x, y + 1] != 1))
        {
            find[0] = false;
        }
        if (find[0])//上
        {
            if (distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x) + Mathf.Abs(EndPointPosT[1] - y - 1) < distance[x, y + 1])
            {
                distance[x, y + 1] = distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x) + Mathf.Abs(EndPointPosT[1] - y - 1);
                distance_start[x, y + 1] = distance_start[x, y] + 1;
                distance_end[x, y + 1] = Mathf.Abs(EndPointPosT[0] - x) + Mathf.Abs(EndPointPosT[1] - y - 1);
                direction[x, y + 1] = 2;//下
                //print("("+x+","+y+"):"+"上" + distance[x, y + 1] + " " + distance_start[x, y + 1] + " " + distance_end[x, y + 1]);
                Person per = new Person(x, y + 1, distance[x, y + 1], distance_start[x, y + 1], distance_end[x, y + 1], direction[x, y + 1]);
                per.before = before;
                persons.Add(per);
            }
        }
        if (find[1])//下
        {
            if (distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x) + Mathf.Abs(EndPointPosT[1] - y + 1) < distance[x, y - 1])
            {
                distance[x, y - 1] = distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x) + Mathf.Abs(EndPointPosT[1] - y + 1);
                distance_start[x, y - 1] = distance_start[x, y] + 1;
                distance_end[x, y - 1] = Mathf.Abs(EndPointPosT[0] - x) + Mathf.Abs(EndPointPosT[1] - y + 1);
                direction[x, y - 1] = 1;//上
                //print("(" + x + "," + y + "):" + "下" + distance[x, y - 1] + " " + distance_start[x, y - 1] + " " + distance_end[x, y - 1]);
                Person per = new Person(x, y - 1, distance[x, y - 1], distance_start[x, y - 1], distance_end[x, y - 1], direction[x, y - 1]);
                per.before = before;
                persons.Add(per);
            }
        }
        if (find[2])//左
        {
            if (distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x + 1) + Mathf.Abs(EndPointPosT[1] - y) < distance[x - 1, y])
            {
                distance[x - 1, y] = distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x + 1) + Mathf.Abs(EndPointPosT[1] - y);
                distance_start[x - 1, y] = distance_start[x, y] + 1;
                distance_end[x - 1, y] = Mathf.Abs(EndPointPosT[0] - x + 1) + Mathf.Abs(EndPointPosT[1] - y);
                direction[x - 1, y] = 4;//右
                //print("(" + x + "," + y + "):" + "左" + distance[x - 1, y] + " " + distance_start[x - 1, y] + " " + distance_end[x - 1, y]);
                Person per = new Person(x - 1, y, distance[x - 1, y], distance_start[x - 1, y], distance_end[x - 1, y], direction[x - 1, y]);
                per.before = before;
                persons.Add(per);
            }
        }
        if (find[3])//右
        {
            if (distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x - 1) + Mathf.Abs(EndPointPosT[1] - y) < distance[x + 1, y])
            {
                distance[x + 1, y] = distance_start[x, y] + 1 + Mathf.Abs(EndPointPosT[0] - x - 1) + Mathf.Abs(EndPointPosT[1] - y);
                distance_start[x + 1, y] = distance_start[x, y] + 1;
                distance_end[x + 1, y] = Mathf.Abs(EndPointPosT[0] - x - 1) + Mathf.Abs(EndPointPosT[1] - y);
                direction[x + 1, y] = 3;//左
                //print("(" + x + "," + y + "):" + "右" + distance[x + 1, y] + " " + distance_start[x + 1, y] + " " + distance_end[x + 1, y]);
                Person per = new Person(x + 1, y, distance[x + 1, y], distance_start[x + 1, y], distance_end[x + 1, y], direction[x + 1, y]);
                per.before = before;
                persons.Add(per);
            }
        }
    }

    Person Near_end(Person before)
    {
        //print("(" + before.Getx() + "," + before.Gety() + ")");
        //print("(" + EndPointPosT[0] + "," + EndPointPosT[1] + ")");
        if (before.Getx() - 1 == EndPointPosT[0] && before.Gety() == EndPointPosT[1])//终点在左方
        {
            Person TheEnd = new Person(before.Getx() - 1, before.Gety(), before.Getstart() + 1, before.Getstart() + 1, 0, 4);
            TheEnd.before = before;
            return TheEnd;
        }
        if (before.Getx() + 1 == EndPointPosT[0] && before.Gety() == EndPointPosT[1])//右
        {
            Person TheEnd = new Person(before.Getx() + 1, before.Gety(), before.Getstart() + 1, before.Getstart() + 1, 0, 3);
            TheEnd.before = before;
            return TheEnd;
        }
        if (before.Getx() == EndPointPosT[0] && before.Gety() + 1 == EndPointPosT[1])//上
        {
            Person TheEnd = new Person(before.Getx(), before.Gety() + 1, before.Getstart() + 1, before.Getstart() + 1, 0, 2);
            TheEnd.before = before;
            return TheEnd;
        }
        if (before.Getx() == EndPointPosT[0] && before.Gety() - 1 == EndPointPosT[1])//下
        {
            Person TheEnd = new Person(before.Getx(), before.Gety() - 1, before.Getstart() + 1, before.Getstart() + 1, 0, 1);
            TheEnd.before = before;
            return TheEnd;
        }
        return new Person(-1, -1, 999, 999, 999, 0);
    }

    public NavSystem(Vector3 EndPoint, Vector3 NowTransform)
    {
        this.EndPoint = EndPoint;
        this.NowTransform = NowTransform;
    }
}

public class Person
{
    public Person before;
    private int x;
    private int y;
    private int direction;//方向
    private int distance;//值
    private int distance_start;//起点值
    private int distance_end;//终点值
    public Person(int x, int y, int distance, int distance_start, int distance_end, int direction)
    {
        this.x = x;
        this.y = y;
        this.distance = distance;
        this.distance_start = distance_start;
        this.distance_end = distance_end;
        this.direction = direction;
    }
    public int Getdir()
    {
        return direction;
    }
    public int Getdis()
    {
        return distance;
    }
    public int Getstart()
    {
        return distance_start;
    }
    public int Getend()
    {
        return distance_end;
    }
    public int Getx()
    {
        return x;
    }
    public int Gety()
    {
        return y;
    }
}
