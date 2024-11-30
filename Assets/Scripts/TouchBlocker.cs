using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Releases the player's pointer if entering this object's bounds
/// </summary>
public class RaycastBlocker : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // Trigger the end drag event if applicable
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.endDragHandler);

            // Trigger pointer up events to ensure interactions are released
            ExecuteEvents.Execute(eventData.pointerDrag, eventData, ExecuteEvents.pointerUpHandler);

            // Reset the drag state
            eventData.pointerDrag = null;

            Debug.Log("Pointer interaction stopped on: " + eventData.pointerDrag);
        }
    }
}