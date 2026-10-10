using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;

    //Displays current ammo ammount
    public void UpdateAmmo(int ammoAmount)
    {
        _ammoText.text = $"{ammoAmount}";
        if (ammoAmount == 0)
        {
            _ammoText.color = Color.red; 
        }
        else
        {
            _ammoText.color = Color.white;
        }
    }
}
