using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.MarkoPolo
{
    public class MarkoPoloUI: MonoBehaviour
    {
        private const int StartNumber = 0;
        private const int EndNumber = 100;

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI markoPoloText;
        [SerializeField] private Transform background;

        private void Awake()
        {
            MarkoPoloAlgorithm markoPoloAlgorithm = new MarkoPoloAlgorithm(StartNumber, EndNumber);
            button.onClick.AddListener(() =>
            {
                background.gameObject.SetActive(true);
                markoPoloText.SetText(markoPoloAlgorithm.Execute());
            });
        }

    }
}
