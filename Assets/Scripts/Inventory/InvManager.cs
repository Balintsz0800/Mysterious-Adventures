using UnityEngine;

public class InvManager : MonoBehaviour
{
    public InvSlot[] slots;
    private int selectedSlot = -1;
    public GameObject inventory;
    private bool isOpen;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeSelectedSlot(0);
    }

    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number -1);
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

            if (newSlot >= slots.Length)
            {
                newSlot = 0;
            }
            else if (newSlot < 0)
            {
                newSlot = slots.Length - 1;
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
}
