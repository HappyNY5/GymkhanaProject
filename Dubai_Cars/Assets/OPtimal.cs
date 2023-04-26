using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OPtimal : MonoBehaviour
{
 [SerializeField] Text carPrice;
    private float carPriceInt = 0;
    [SerializeField] TMP_Text vbitVTamosnu;
   [SerializeField] Text tamosnPrice;
   private float tamPriceInt = 0;

   [SerializeField] TMP_Text itogoPrice;
   private float itogoPriceInt = 0;
   

 
   public void FirstStep()
   {
        carPriceInt = int.Parse(carPrice.text);
        carPriceInt = (1.1f * carPriceInt) * 0.27f; // v $

     itogoPrice.text = $"Стоимость авто {carPriceInt} \n Стоимость таможни {tamPriceInt} \n Итого: {itogoPriceInt}";


        tamPriceInt = (carPriceInt * 0.94f )/2;
        vbitVTamosnu.text = $"Вбить в таможню {tamPriceInt}";

        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
   }

   public void SecondStep()
   {
        tamPriceInt = int.Parse(tamosnPrice.text);

        itogoPriceInt = (carPriceInt + tamPriceInt/ 0.94f + 4000f);
        itogoPrice.text = $"Стоимость авто {carPriceInt} \n Стоимость таможни {tamPriceInt/ 0.94f} \n\n Итого $: {itogoPriceInt}\n Итого Р: {itogoPriceInt * 78.5f}";
   }
}
