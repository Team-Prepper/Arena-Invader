using UnityEngine;
using UnityEngine.UI;
using EasyH.Tool.LangKit;

public class GUIUnitItemDesc : GUIUnitItem
{
    [SerializeField] private Text _selectItemName;
    [SerializeField] private Text _selectItemDescription;

    public override void SetItemCode(ItemData itemData)
    {

        base.SetItemCode(itemData);

        _selectItemName.text =
            LangManager.Instance.GetStringByKey(itemData.Name);
        _selectItemDescription.text = itemData.Desc;

    }

}
