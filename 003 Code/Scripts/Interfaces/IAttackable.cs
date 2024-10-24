using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void CheckAttack(Collider2D collision);
    //skill 뿐만 아니라 다른쪽에도 적용
}