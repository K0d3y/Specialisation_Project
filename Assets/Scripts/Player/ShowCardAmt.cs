using Photon.Pun;
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
            GameplayManager.Instance.player.gameObject.GetComponentInChildren<ShowCardAmt>() == this)
        {
            count = 0;
            if (hit.transform.parent.name.Contains("Deck"))
            {
                count = GetComponentInChildren<DeckContainer>().cardList.Count;
            }
            else if (hit.transform.parent.name == "Discard")
            {
                count = GetComponentInChildren<DiscardContainer>().cardList.Count;
            }
            if (hit.transform.parent.name == "Resilience")
            {
                count = GetComponentInChildren<ResilienceContainer>().cardList.Count;
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
