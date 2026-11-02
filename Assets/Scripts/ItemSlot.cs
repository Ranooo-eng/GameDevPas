using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    private string itemName;
    private int itemQuantity;
    private Sprite itemIcon;
    private ItemDetails itemDetails;
    public ItemDetails ItemDetails => itemDetails;
    public int ItemQuantity => itemQuantity;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private int maxStackSize = 99;
    [SerializeField] public GameObject SelectedPanel;

    public bool IsItemSelected;

    private InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public bool isFull
    {
        get
        {
            return itemQuantity >= maxStackSize;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return itemQuantity <= 0 || string.IsNullOrEmpty(itemName);
        }
    }

    public bool CanStackWith(string otherItemName)
    {
        return !IsEmpty &&
               itemName == otherItemName &&
               !isFull;
    }

    public void AddItem(ItemDetails item)
    {
        if (item == null || item.quantity <= 0)
        {
            return;
        }

        if (!IsEmpty)
        {
            return;
        }

        itemDetails = item;
        itemName = item.itemName;
        itemIcon = item.itemIcon;
        itemQuantity = 0;

        AddQuantity(item.quantity);
    }

    public void AddQuantity(int amount)
    {
        if (amount <= 0 || (IsEmpty && string.IsNullOrEmpty(itemName)))
        {
            return;
        }

        itemQuantity = Mathf.Min(
            itemQuantity + amount,
            maxStackSize
        );

        UpdateSlot();
    }

    private void UpdateSlot()
    {
        if (quantityText != null)
        {
            quantityText.text = itemQuantity.ToString();
            quantityText.enabled = !IsEmpty;
        }

        if (itemImage != null)
        {
            itemImage.sprite = itemIcon;
            itemImage.enabled = !IsEmpty;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }

    private void LeftClick()
    {
        if (IsEmpty || inventoryManager == null)
        {
            return;
        }

        inventoryManager.SelectSlot(this);
    }

    private void RightClick()
    {
        Debug.Log("Right clicked on " + itemName);
        inventoryManager.DeselectAllItems();
    }
}
