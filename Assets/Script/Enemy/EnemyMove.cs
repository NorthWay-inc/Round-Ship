using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private Transform transformObj;

    #region Spawn 
    /// <summary>
    /// ��� ���������� ��� ������
    /// </summary>
    /// <param name="s">�������� ��������</param>
    /// <param name="t">Transform ������ � �������� ����� ���� ��������</param>
    public void Init(float s, Transform t)
    {
        speed = s;
        transformObj = t;
    }
    #endregion

    private void Update() { transform.position = Vector3.MoveTowards(transform.position, transformObj.position, speed); }
}
