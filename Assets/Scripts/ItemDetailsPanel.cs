using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemQuantityText;

    public void Show(ItemDetails item, int quantity)
    {
        if (itemImage == null)
        {
            Clear();
            return;
        }
        itemImage.sprite = item.itemIcon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemQuantityText.text = "Quantity: " + quantity;
        itemImage.enabled = true;
    }

    public void Clear()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemQuantityText.text = "";

    }
    
}
