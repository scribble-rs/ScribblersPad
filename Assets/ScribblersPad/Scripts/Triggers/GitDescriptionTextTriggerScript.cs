using TMPro;
using UnityEngine;

namespace ScribblersPad.Triggers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GitDescriptionTextTriggerScript : MonoBehaviour
    {
        private void Start()
        {
            TextAsset text_asset = Resources.Load<TextAsset>("GitDescription");
            if ((text_asset != null) && TryGetComponent(out TextMeshProUGUI git_description_text))
            {
                git_description_text.text = text_asset.text;
            }
            Destroy(this);
        }
    }
}
