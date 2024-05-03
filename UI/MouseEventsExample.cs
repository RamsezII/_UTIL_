using UnityEngine;
using UnityEngine.EventSystems; // N'oubliez pas d'inclure cet espace de noms

public class MouseEventsExample : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Cette fonction est appel�e lorsqu'on survole l'�l�ment avec la souris
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("La souris est entr�e sur " + gameObject.name);
    }

    // Cette fonction est appel�e lorsque la souris quitte l'�l�ment
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("La souris a quitt� " + gameObject.name);
    }

    // Cette fonction est appel�e lorsque l'�l�ment est cliqu�
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("L'�l�ment " + gameObject.name + " a �t� cliqu�");
    }

    // Ajoutez d'autres m�thodes pour d'autres �v�nements de souris au besoin, comme OnPointerDown, OnPointerUp, etc.
}
