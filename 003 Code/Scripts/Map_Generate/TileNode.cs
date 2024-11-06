using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode
{
    public TileNode leftNode;
    public TileNode rightNode;
    public TileNode parNode;
    public int x,y,height,width;
    // public Vector2Int center
    // {
    //     get {
    //         return new Vector2Int(roomRect.x+roomRect.width/2, roomRect.y + roomRect.height/ 2);
    //     }
    //     //방의 가운데 점. 방과 방을 이을 때 사용
    // }
    public TileNode(int x,int y,int width=80,int height=80) // 변경
    {
        this.x = x;
        this.y = y;
        this.height = height;
        this.width = width;
    }
}
