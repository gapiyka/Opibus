using System.Collections.Generic;
using System.Text;
using Opibus.Resource;
using TMPro;
using UnityEngine;


namespace Opibus.Controllers
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI description;

        private Dictionary<Building, string> buildingsDescription;

        public void ChangeBuildingDescription(Building building, string text) =>
        buildingsDescription[building] = text;

        private void Awake() => buildingsDescription = new();

        private void Update()
        {
            var sb = new StringBuilder();
            foreach (var building in buildingsDescription)
                sb.Append($"{building.Key.name}: {building.Value}\n");
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1); // Trim extra \n
            description.text = sb.ToString();
        }
    }
}