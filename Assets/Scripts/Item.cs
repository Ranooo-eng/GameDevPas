using UnityEngine;

public interface IGrabtableItem
{
    void AddItem(ItemDetails itemDetails);
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemDetails itemDetails;
    private IGrabtableItem itemCollector;

    void Awake()
    {
        foreach (MonoBehaviour component in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (component is IGrabtableItem collector)
            {
                itemCollector = collector;
                break;
            }
        }

        if (itemCollector == null)
        {
            Debug.LogError("No active component implementing GrabableItem was found in the scene.", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && itemDetails != null && itemCollector != null)
        {
            //Debug.Log("Player collided with item");
            GrabItem();
        }
    }

    private void GrabItem()
    {
        //Debug.Log("item grabbed");
        itemCollector.AddItem(itemDetails);
        Destroy(gameObject);
    }
}