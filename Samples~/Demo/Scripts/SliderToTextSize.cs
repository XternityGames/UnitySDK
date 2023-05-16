using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Xternity.Samples
{
    public class SliderToTextSize : MonoBehaviour
    {
        public float minFontSize = 16;
        public float maxFontSize = 72;
        public Slider Slider;
        public TextMeshProUGUI Text;

        private void Awake()
        {
            Slider.onValueChanged.AddListener((val) => Text.fontSize = Mathf.Lerp(minFontSize, maxFontSize, val));
        }
    }
}
