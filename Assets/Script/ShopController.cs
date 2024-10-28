using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using YG.Example;


public class ShopController : BaseSaver
{
    private Management playerData;

    [SerializeField] private GameObject _template;
    [SerializeField] private RectTransform _container;
    [SerializeField] private CompositeDisposable _containerDisposable;
    [SerializeField] private TMP_Text _money;

    private void Start()
    {
        //ѕолучаем класс игрока дл€ изучени€ его полей
        playerData = (Management)FindFirstObjectByType(typeof(Management));

        CreateShopMenuByTagCanBuy();
    }

    private void CreateShopMenuByTagCanBuy()
    {
        FieldInfo[] fieldInfos = typeof(Management).GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (var item in fieldInfos)
        {
            CanBuyAttribute canBuyAttribute = item.GetCustomAttribute<CanBuyAttribute>();
            //≈сли у его есть переменные которые можно прокачивать в магазине то
            if (canBuyAttribute != null)
            {
                //—оздаем копию шаблонного пол€
                GameObject tempObject = Instantiate(_template);
                tempObject.transform.parent = _container;
                tempObject.SetActive(true);
                List<GameObject> Childs = new List<GameObject>();
                //ѕолучаем его деток
                for (int i = 0; i < tempObject.transform.childCount; i++)
                {
                    Childs.Add(tempObject.transform.GetChild(i).gameObject);
                }
                //Childs.Where((x)=>x.name == "ICON").FirstOrDefault().GetComponent<Image>().sprite = ;
                //ќбновл€ем состо€ние отображени€
                UpdateState(Childs, item);
                ///ѕри нажатии на покупку

                tempObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    //ѕолучаем текущий уровень прокачки определенной способности и инкрементируем его
                    int currentLevel = (int)item.GetValue(playerData);
                    //////////////////////////////////////////
                    ///Ћогика списани€ средств тут
                    if (playerData.Money >= canBuyAttribute.Cost)
                    {
                        if (canBuyAttribute.MaxLevel == -1 || canBuyAttribute.MaxLevel > currentLevel)
                        {
                            playerData.Money -= canBuyAttribute.Cost;
                            _money.text = playerData.Money.ToString();
                            item.SetValue(playerData, currentLevel + 1);
                            UpdateState(Childs, item);
                        }
                    }
                    /////////////////////////////////////
                    //”станавливаем нашему повышаем уровень нашему персонажу

                });
                ///сложность фибоначчи от n. можно сделать сложность n
                //»спользуетс€ дл€ обновлени€ показателей состо€ний
                void UpdateState(List<GameObject> objects, FieldInfo field)
                {
                    objects.Where((x) => x.name == "Icon").FirstOrDefault().GetComponent<Image>().sprite = Resources.Load<Sprite>("icons/" + canBuyAttribute.PathToIconInResources);
                    objects.Where((x) => x.name == "Level").FirstOrDefault().GetComponent<TMP_Text>().text = ((int)item.GetValue(playerData)).ToString();
                    objects.Where((x) => x.name == "Name").FirstOrDefault().GetComponent<TMP_Text>().text = canBuyAttribute.Name;
                    objects.Where((x) => x.name == "Description").FirstOrDefault().GetComponent<TMP_Text>().text = canBuyAttribute.Description;
                    objects.Where((x) => x.name == "Cost").FirstOrDefault().GetComponent<TMP_Text>().text = canBuyAttribute.Cost.ToString();
                }
            }
        }
    }
}
/// <summary>
/// јттрибут добавл€ющий переменную строго типа инт в магазин и который позвол€ет прокачивать его
/// –екомендаци€: »спользуйте дл€ повышени€ уровней способностей или другой хуйни тип int который просто будет увеличивать уровень
/// </summary>
public class CanBuyAttribute : Attribute
{
    public string PathToIconInResources;
    public string Name;
    public string Description;
    public int Cost;
    public int MaxLevel;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PathIcon"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="cost"></param>
    public CanBuyAttribute(string PathIcon, string name, string description, int cost, int maxLevel = -1)
    {
        this.PathToIconInResources = PathIcon;
        Name = name;
        Description = description;
        Cost = cost;
        MaxLevel = maxLevel;
    }
}

//public readonly struct ShopMenuItemForm
//{
//    public readonly string Title;
//    public readonly string Description;
//    public readonly int Cost;
//    public readonly int MaxLevel;
//}

//public readonly struct ShopMenuForm
//{
//    public readonly string Title;
//    public readonly ShopMenuItemForm[] ItemSource;
//}