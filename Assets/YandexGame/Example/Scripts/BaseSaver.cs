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
    /// Наследуй этот класс для монобихейвер если хочешь
    /// сохранять переменные этого класса в облако гугл диска
    /// Когда отнаследуешь используй аттрибут Saveable
    /// для сохранения. Сохраняются все примитивные типы данных
    /// которые конвертируются через Convert в стринг
    /// На реактивке UniRx R3 может не работать, не тестил
    /// Обязательно вызови метод Init базового класса и передай ему this, 
    /// ссылку на дочерний класс а то ничё сохраняться не будет
    /// </summary>
    public  abstract class BaseSaver : MonoBehaviour
    {
        private Type childType;
        private object childObject;

        /// <summary>
        /// Вызывай обезательно для работы сохранений
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
    /// Атрибут который после наследования BaseSaver
    /// ставится полю которое хочешь сохранять и загружать в бд
    /// Сохраняются все примитивные типы данных
    /// которые конвертируются через Convert в стринг
    /// На реактивке UniRx R3 может не работать, не тестил
    /// </summary>
    public class SaveableAttribute : Attribute { }
    
}