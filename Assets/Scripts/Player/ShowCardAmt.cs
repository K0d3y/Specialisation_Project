using TMPro;
using UnityEngine;

public class ShowCardAmt : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private LayerMask cardLayerMask;

    private int count;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, 100, cardLayerMask) && 
            GetComponent<PlayerController>().view.IsMine)
        {
            count = 0;
            if (hit.transform.parent.name.Contains("Deck"))
            {
                count = hit.transform.gameObject.GetComponentInParent<DeckContainer>().cardList.Count;
            }
            else if (hit.transform.parent.name == "Discard")
            {
                count = hit.transform.gameObject.GetComponentInParent<DiscardContainer>().cardList.Count;
            }
            else if (hit.transform.parent.name == "Resilience")
            {
                count = hit.transform.gameObject.GetComponentInParent<ResilienceContainer>().cardList.Count;
            }
            text.gameObject.SetActive(true);
            text.text = count.ToString();
            text.transform.position = Input.mousePosition;
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
