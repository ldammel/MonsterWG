using Game.Interactions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CharacterUI : SerializedMonoBehaviour
    {

        [OdinSerialize]
        private Dictionary<Interaction.InteractionResult, Sprite> sprites = new Dictionary<Interaction.InteractionResult, Sprite>();

        [OdinSerialize]
        private Dictionary<Interaction.InteractionResult, string> texts = new Dictionary<Interaction.InteractionResult, string>();

        private TextMeshProUGUI _text;
        private Image _image;

        private float _timer;

        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _image = GetComponentInChildren<Image>();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if(_timer <= 0)
            {
                _image.enabled = false;
                _text.enabled = false;
            }
        }


        public void UpdateInteractionFailedUI(Interaction.InteractionResult result)
        {
            if (sprites.ContainsKey(result))
            {
                _image.enabled = true;
                _text.enabled = false;
                _image.sprite = sprites[result];
            }
            else
            {
                _image.enabled = false;
                _text.enabled = true;
                _text.text = texts[result];
            }

            _timer = 1.0f;
        }
    }
}
