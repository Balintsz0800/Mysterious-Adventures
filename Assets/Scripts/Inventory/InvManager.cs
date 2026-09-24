using UnityEngine;

public class InvManager : MonoBehaviour
{
    public InvSlot[] slots;
    private int selectedSlot = -1;
    public GameObject inventory;
    private bool isOpen;

    public int maxStackedItem = 30;
    public GameObject invItemPrefab;
    public Transform dropPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeSelectedSlot(0);
        inventory.SetActive(false);
    }

    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            int newSlot = selectedSlot;

            if (scroll < 0f)
            {
                newSlot++;
            }
            else if (scroll > 0f)
            {
                newSlot--;
            }

            if (newSlot >= 9)
            {
                newSlot = 0;
            }
            else if (newSlot < 0)
            {
                newSlot = 8;
            }

            if (newSlot != selectedSlot)
            {
                ChangeSelectedSlot(newSlot);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isOpen)
            {
                inventory.SetActive(true);
                isOpen = true;
            }
            else if (isOpen)
            {
                inventory.SetActive(false);
                isOpen = false;
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            slots[selectedSlot].Deselect();
        }

        slots[newValue].Select();
        selectedSlot = newValue;
    }

    private void DropSelectedItem()
    {
        if (selectedSlot < 0 || selectedSlot >= slots.Length)
        {
            return;
        }

        InvSlot invSlot = slots[selectedSlot];

        InvItem invItem = invSlot.GetComponentInChildren<InvItem>();

        if (invItem == null || invItem.item == null)
        {
            return;
        }

        if (invItem.item.handPrefab != null && dropPoint != null)
        {
            Instantiate(invItem.item.itemPrefab, dropPoint.position, Quaternion.identity);
        }

        invItem.count--;

        if (invItem.count <= 0)
        {
            Destroy(invItem.gameObject);
        }
        else
        {
            invItem.RefreshCount();
        }
    }

    private void SpawnNewItem(Item item, InvSlot slot, int amount)
    {
        GameObject newItem = Instantiate(invItemPrefab, slot.transform);
        
        InvItem invItem = newItem.GetComponent<InvItem>();
        
        invItem.initaliseItem(item);
        
        invItem.count = amount;
        
        invItem.RefreshCount();
    }

    public Item GetSelectedItem(bool use)
    {
        if (selectedSlot < 0 || selectedSlot >= slots.Length)
        {
            return null; 
        }

        InvSlot slot = slots[selectedSlot];
        
        InvItem itemInSlot = slot.GetComponentInChildren<InvItem>();

        if (itemInSlot == null)
        {
            return null;
        }

        Item item = itemInSlot.item;

        if (use)
        {
            itemInSlot.count--;

            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                itemInSlot.RefreshCount();
            }
        }
        return item;
    }

    public bool AddItem(Item item, int amount)
    {
        if (item.stackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                InvItem existing = slots[i].GetComponentInChildren<InvItem>();

                if (existing != null && existing.item == item && existing.count < maxStackedItem)
                {
                    existing.count += amount;
                    existing.RefreshCount();
                    
                    return true;
                }
            }
            
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].GetComponentInChildren<InvItem>() == null)
                {
                    SpawnNewItem(item, slots[i], amount);
                
                    return true;
                }
            }
            return false;
        }

        for (int x = 0; x < amount; x++)
        {
            bool placed = false;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].GetComponentInChildren<InvItem>() == null)
                {
                    SpawnNewItem(item, slots[i], 1);
                    placed = true;
                    
                    break;
                }
            }

            if (!placed)
            {
                return false;
            }
        }
        return true;
    }

}