using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private Transform transformObj;

    #region Spawn 
    /// <summary>
    /// Это вызывается при спавне
    /// </summary>
    /// <param name="s">Скорость движения</param>
    /// <param name="t">Transform обекта к которому будет идти движение</param>
    public void Init(float s, Transform t)
    {
        speed = s;
        transformObj = t;
    }
    #endregion

    private void Update() { transform.position = Vector3.MoveTowards(transform.position, transformObj.position, speed); }
}
