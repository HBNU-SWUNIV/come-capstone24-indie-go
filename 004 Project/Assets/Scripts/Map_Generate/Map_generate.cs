using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;
using System.Diagnostics.Tracing;

public class Map_generate : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public static Map_generate instance;
    public const int max = 4;
    public Map_Node[,] map_list = new Map_Node[max, max];

    int way,
        next_num;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                map_list[i, j] = new Map_Node(new RectInt(j * 80, -i * 80, 80, 80)); //변경
            }
        }
        map_generate();

    }
    void map_generate()
    {
        int num = UnityEngine.Random.Range(0, max);
        map_list[0, num].way = 9;
        for (int i = 0; i < max; i++)
        {
            if (i != 0) map_list[i, num].way = 3;
            int count = 1;
            while (true)
            {
                bool test = find(i);
                if (num - count < 0 && num + count > max - 1)
                {
                    if (!test)
                    {
                        if (map_list[i, 0].way == 1)
                        {
                            next_num = 0;
                            map_list[i, 0].way = 2;
                        }
                        else
                        {
                            next_num = max - 1;
                            map_list[i, max - 1].way = 2;
                        }
                    }
                    break;
                }
                if (num == 0 || num == max - 1)
                {
                    if (count == 1)
                    {
                        way = UnityEngine.Random.Range(1, 3);
                    }
                    else
                    {
                        if (!test)
                        {
                            if (!test)
                            {
                                if (count == max - 1) way = 2;
                                else way = UnityEngine.Random.Range(1, 3);
                            }
                        }
                        else way = UnityEngine.Random.Range(0, 2);
                    }
                    if (num == 0 && map_list[i, num + count - 1].way != 0)
                    {
                        if (way == 2) next_num = count;
                        map_list[i, count].way = way;
                    }
                    else if (num == max - 1 && map_list[i, num - count + 1].way != 0)
                    {
                        if (way == 2) next_num = max - count - 1;
                        map_list[i, max - count - 1].way = way;
                    }
                }
                else
                {
                    if (num - count > -1)
                    {
                        if (test) way = UnityEngine.Random.Range(0, 2);
                        else way = UnityEngine.Random.Range(0, 3);
                        if (map_list[i, num - count + 1].way != 0)
                        {
                            if (way == 2) next_num = num - count;
                            map_list[i, num - count].way = way;
                        }
                    }
                    test = find(i);
                    if (num + count < max)
                    {
                        if (test) way = UnityEngine.Random.Range(0, 2);
                        else way = UnityEngine.Random.Range(1, 3);
                        if (map_list[i, num + count - 1].way != 0)
                        {
                            if (way == 2) next_num = num + count;
                            map_list[i, num + count].way = way;
                        }
                    }

                }
                count++;
            }
            num = next_num;
        }
        SetNode();
        Set_Type();
        Set_Node();
    }

    bool find(int i)
    {
        for (int j = 0; j < max; j++)
        {
            if (map_list[i, j].way == 2) return true;
        }
        return false;
    }
    void SetNode() // node를 할당해서 추후 맵생성에 있어서 원할하게할 목적
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (map_list[i, j].way == 0) continue;
                else
                {
                    if (map_list[i, j].way == 2)
                    {
                        if (i + 1 == 4) continue;
                        map_list[i, j].Down_node = map_list[i + 1, j];
                        map_list[i + 1, j].Up_node = map_list[i, j];
                    }

                    if (j + 1 == 4) continue;
                    if (map_list[i, j + 1].way != 0)
                    {
                        map_list[i, j].Right_node = map_list[i, j + 1];
                        map_list[i, j + 1].Left_node = map_list[i, j];
                    }
                }
            }
        }
    }
    void Set_Type()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                if (map_list[i, j].way == 9)
                {
                    map_list[i, j].map_type = Map_Node.Map_type.Enterance;


                    // player.transform.position= new Vector3(map_list[0, j].nodeRect.x,map_list[0, j].nodeRect.y,1);
                }

                else if (i == 3 && map_list[i, j].way == 2)
                {
                    map_list[i, j].map_type = Map_Node.Map_type.Exit;
                    map_list[i, j].way = 8;


                    // player.transform.position= new Vector3(map_list[0, j].nodeRect.x,map_list[0, j].nodeRect.y,1);
                }
                else if (i == 3 && map_list[i, j].way == 2)
                {
                    map_list[i, j].map_type = Map_Node.Map_type.Exit;
                    map_list[i, j].way = 8;


                }
                else
                {
                    int rand = UnityEngine.Random.Range(0, 10);

                    if (rand == 0) map_list[i, j].map_type = Map_Node.Map_type.Treasure;
                    else map_list[i, j].map_type = Map_Node.Map_type.Enemy;

                }
                /*Debug.Log(i + " " + j + " " + map_list[i, j].map_type);
                Debug.Log(map_list[i, j].map_type);*/
            }
        }
    }
    void Set_Node() // 문자열을 출력해서 debug를 위한 목적
    {
        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                Tile_Map_Create.instance.Tile_Node(map_list[i, j]);
            }
        }
        Set_Load();
    }
    void Set_Load()
    {
        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {

                Tile_Map_Create.instance.MakeRoad(map_list[i,j],map_list[i,j].node.leftNode,map_list[i,j].node.rightNode);
                if (map_list[i, j].Right_node != null) Tile_Map_Create.instance.Horiontal_add(map_list[i, j], map_list[i, j].Right_node, map_list[i, j].node.rightNode, map_list[i, j + 1].node.leftNode);
                if(map_list[i,j].Down_node != null) Tile_Map_Create.instance.Vertical_add(map_list[i,j],map_list[i,j].Down_node,map_list[i,j].node,map_list[i,j].Down_node.node);
            }
        }
        Tile_generate();
    }
    void Tile_generate()
    {
        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                Tile_Map_Create.instance.Make_Tile(map_list[i, j]);
            }
        }
    }
    
}
