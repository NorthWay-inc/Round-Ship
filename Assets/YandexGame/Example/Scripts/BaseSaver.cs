using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    /// <summary>
    /// �������� ���� ����� ��� ������������ ���� ������
    /// ��������� ���������� ����� ������ � ������ ���� �����
    /// ����� ������������ ��������� �������� Saveable
    /// ��� ����������. ����������� ��� ����������� ���� ������
    /// ������� �������������� ����� Convert � ������
    /// �� ��������� UniRx R3 ����� �� ��������, �� ������
    /// ����������� ������ ����� Init �������� ������ � ������� ��� this, 
    /// ������ �� �������� ����� � �� ���� ����������� �� �����
    /// </summary>
    public  abstract class BaseSaver : MonoBehaviour
    {
        private Type childType;
        private object childObject;

        /// <summary>
        /// ������� ����������� ��� ������ ����������
        /// </summary>
        /// <param name="childObject"></param>
        protected void Init(object childObject) {
            this.childObject = childObject;
            this.childType = childObject.GetType();
        }

        [SerializeField] public Dictionary<string,string> Saves;
        private void OnEnable() => YandexGame.GetDataEvent += GetLoad;
        private void OnDisable() => YandexGame.GetDataEvent -= GetLoad;

        public void Save()
        {
            Saves = YandexGame.savesData.Saves;
            var fields = childType.GetFields().Where((x) => x.GetCustomAttribute(typeof(SaveableAttribute)) != null);
            foreach (var item in fields)
            {
                if (Saves.ContainsKey(item.Name))
                {
                    Saves[item.Name]=item.GetValueOptimized(childObject).ToString();
                }
                else
                {
                    Saves.Add(item.Name, "");
                }
            }
            YandexGame.savesData.Saves = Saves;

            YandexGame.SaveProgress();
        }

        public void Load() => YandexGame.LoadProgress();

        public void GetLoad()
        {
            Saves = YandexGame.savesData.Saves;
            var fields = childType.GetFields().Where((x) => x.GetCustomAttribute(typeof(SaveableAttribute)) != null);
            foreach (var item in fields)
            {
                if (Saves.ContainsKey(item.Name))
                {
                    item.SetValueOptimized(childObject, Convert.ChangeType(Saves[item.Name], item.FieldType));
                }
                else
                {
                    Saves.Add(item.Name, "");
                }
            }
        }
    }
    /// <summary>
    /// ������� ������� ����� ������������ BaseSaver
    /// �������� ���� ������� ������ ��������� � ��������� � ��
    /// ����������� ��� ����������� ���� ������
    /// ������� �������������� ����� Convert � ������
    /// �� ��������� UniRx R3 ����� �� ��������, �� ������
    /// </summary>
    public class SaveableAttribute : Attribute { }
    
}