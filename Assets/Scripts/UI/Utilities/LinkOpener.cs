using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InfinityCraft.UI.Utilities
{
    public class LinkOpener : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            TMP_Text text = GetComponent<TMP_Text>();
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }
}