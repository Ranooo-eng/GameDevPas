using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGrabtableItem
{
    InputAction inventoryAction;

    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    [SerializeField] private ItemDetailPanel detailsPanel;

    private bool InventoryActive;
    public InventoryManager inventoryManager;
    public ItemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();

        if (InventoryCanvas == null)
        {
            Debug.LogError("InventoryCanvas belum diisi di Inspector!", this);
            return;
        }

        CreateSlots();

        InventoryCanvas.SetActive(false);

        inventoryAction = InputSystem.actions.FindAction("Inventory");

        InventoryActive = false;

        if (inventoryAction == null)
        {
            Debug.LogError("Action Inventory tidak ditemukan!", this);
        }

        if (detailsPanel == null)
        {
            Debug.LogWarning("ItemDetailPanel belum diisi. Detail item tidak akan ditampilkan.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryAction == null || InventoryCanvas == null)
        {
            return;
        }

        if (inventoryAction.triggered && InventoryActive == false)
        {
            InventoryActive = true;
            Time.timeScale = 0f;

            Debug.Log("Inventory dibuka");

            InventoryCanvas.SetActive(true);

            ShowFirstItem();
        }
        else if (inventoryAction.triggered && InventoryActive == true)
        {
            InventoryActive = false;
            Time.timeScale = 1f;

            Debug.Log("Inventory ditutup");

            InventoryCanvas.SetActive(false);
        }
    }

    // Build the slot objects at runtime so no manual wiring is needed in the scene
    private void CreateSlots()
    {
        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlot");

        if (slotPrefab == null)
        {
            Debug.LogError("Prefab ItemSlot tidak ditemukan di folder Resources!", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        // Put the slots inside the GridLayoutGroup container (InventorySlots), if there is one
        Transform parent = InventoryCanvas.transform;

        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);

        if (grid != null)
        {
            parent = grid.transform;
        }

        itemSlot = new ItemSlot[slotCount];

        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(slotPrefab, parent);
        }
    }

    public void AddItem(ItemDetails item)
    {
        if (item == null)
        {
            return;
        }

        Debug.Log(
            "Item ditambahkan ke inventory: " +
            item.itemName +
            " Quantity: " +
            item.quantity
        );

        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogWarning("Inventory tidak memiliki slot. Item tidak dapat ditambahkan.");
            return;
        }

        // Logic to check if item already exists in the inventory and can be stacked
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i] != null &&
                itemSlot[i].CanStackWith(item.itemName))
            {
                itemSlot[i].AddQuantity(item.quantity);
                return;
            }
        }

        // Logic to add the item to an empty slot if it cannot be stacked
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i] != null && itemSlot[i].IsEmpty)
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        Debug.LogWarning(
            "Inventory penuh, '" + item.itemName + "' tidak dapat ditambahkan."
        );
    }

    // Called by a slot when the player clicks on it
    public void SelectSlot(ItemSlot slot)
    {
        if (slot == null)
        {
            return;
        }

        DeselectAllItems();

        // Show the selection highlight on the clicked slot
        slot.SelectedPanel.SetActive(true);
        slot.IsItemSelected = true;

        // Show the item details on the right-side panel
        if (detailsPanel != null)
        {
            detailsPanel.Show(slot.ItemDetails, slot.ItemQuantity);
        }
    }

    // When the inventory opens, show the first item on the details panel
    private void ShowFirstItem()
    {
        if (itemSlot != null)
        {
            foreach (ItemSlot slot in itemSlot)
            {
                if (slot != null && !slot.IsEmpty)
                {
                    SelectSlot(slot);
                    return;
                }
            }
        }

        // No items yet: leave the panel empty
        if (detailsPanel != null)
        {
            detailsPanel.Clear();
        }
    }

    public void DeselectAllItems()
    {
        if (itemSlot == null)
        {
            return;
        }

        foreach (ItemSlot slot in itemSlot)
        {
            if (slot != null)
            {
                slot.IsItemSelected = false;

                if (slot.SelectedPanel != null)
                {
                    slot.SelectedPanel.SetActive(false);
                }
            }
        }
    }

    void OnDisable()
    {
        if (InventoryActive)
        {
            InventoryActive = false;

            if (InventoryCanvas != null)
            {
                InventoryCanvas.SetActive(false);
            }

            Time.timeScale = 1f;
        }
    }
}

