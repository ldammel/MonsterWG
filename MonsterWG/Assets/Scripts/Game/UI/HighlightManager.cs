using Game.Interactions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HighlightManager : MonoBehaviour
    {
        [BoxGroup("Transform")]
        [SerializeField]
        private Vector3 positionOffset;

        [BoxGroup("Transform")]
        [SerializeField]
        private Quaternion rotation;

        [BoxGroup("Transform")]
        [SerializeField]
        private Vector3 scale;

        [SerializeField]
        private RectTransform Prefab;

        [BoxGroup("Interaction")]
        [SerializeField]
        private Sprite interactSpriteBase;
        [BoxGroup("Interaction")]
        [SerializeField]
        private Sprite consumedInteractionSpriteP1;
        [BoxGroup("Interaction")]
        [SerializeField]
        private Sprite consumedInteractionSpriteP2;
        [BoxGroup("Interaction")]
        [SerializeField]
        private Sprite interactionSpriteP1;
        [BoxGroup("Interaction")]
        [SerializeField]
        private Sprite interactionSpriteP2;

        [BoxGroup("PickUp")]
        [SerializeField]
        private Sprite pickUpSprite;

        [BoxGroup("Cheat")]
        [SerializeField]
        private Sprite cheatSpriteP1;

        [BoxGroup("Cheat")]
        [SerializeField]
        private Sprite cheatSpriteP2;

        [SerializeField]
        private List<SignChildren> _transforms = new List<SignChildren>();

        private ShowMode _p1HighlightMode = ShowMode.None;
        private ShowMode _p2HighlightMode = ShowMode.None;

        private PlayerInteractionController _player1;
        private PlayerInteractionController _player2;

        private float _timer1;
        private float _timer2;

        void Start()
        {
            foreach (var player in FindObjectsOfType<PlayerInteractionController>())
            {
                if (player.isPlayerOne)
                {
                    _player1 = player;
                }
                else
                {
                    _player2 = player;
                }
            }

            InitType<Interaction>(interactSpriteBase, UIType.Interaction);
            InitType<Pickup>(pickUpSprite, UIType.Pickup);
            InitType<StoreInteraction>(interactSpriteBase, UIType.Storage);
        }
    
        private void Update()
        {
            _timer1 -= Time.deltaTime;
            _timer2 -= Time.deltaTime;

            _p1HighlightMode = DetermineModeForPlayer(_player1);
            _p2HighlightMode = DetermineModeForPlayer(_player2);

            foreach (var entry in _transforms)
            {
                if(entry.Original == null)
                {
                    entry.Parent.gameObject.SetActive(false);
                    continue;
                }

                if (entry.HasOverrideOffset)
                {
                    HighlightOffsetOverride overrideOffset = entry.GetOverrideOffset;
                    entry.Parent.position = entry.Original.position + overrideOffset.positionOffset;
                    entry.Content.rotation = overrideOffset.rotation;
                    entry.Content.localScale = overrideOffset.scale;
                }
                else
                {
                    entry.Parent.position = entry.Original.position + positionOffset;
                    entry.Content.rotation = rotation; 
                    entry.Content.localScale = scale;
                }

                bool p1Visible = IsSignVisible(entry, _player1, _p1HighlightMode, _timer1);
                bool p2Visible = IsSignVisible(entry, _player2, _p2HighlightMode, _timer2);

                if((_player2.CurrentItem && _player2.CurrentItem.transform == entry.Original) 
                    || (_player1.CurrentItem && _player1.CurrentItem.transform == entry.Original))
                {
                    p1Visible = false;
                    p2Visible = false;
                }

                if (p1Visible)
                {
                    UpdateSignSprite(entry, _player1, _p1HighlightMode);
                }
                if (p2Visible)
                {
                    UpdateSignSprite(entry, _player2, _p2HighlightMode);
                }

                entry.Parent.gameObject.SetActive(p1Visible || p2Visible);
            }
        }

        private void InitType<T>(Sprite sprite, UIType type) where T : Component
        {
            foreach (T obj in FindObjectsOfType<T>())
            {
                Transform parent = new GameObject(obj.gameObject.name).transform;
                parent.SetParent(transform);
                RectTransform inst = Instantiate(Prefab, parent);

                // Hack because we have NO TIME

                UIType typeCoyp = type;
                if(typeCoyp == UIType.Storage)
                {
                    StoreInteraction interaction = obj.GetComponent<StoreInteraction>();

                    if (interaction.cheatStorage)
                    {
                        typeCoyp = UIType.Cheat;
                    }
                }

                SignChildren uiObject = new SignChildren(obj.transform, parent, inst, typeCoyp);
                uiObject.SetSprite(sprite);

                _transforms.Add(uiObject);
            }
        }
        private ShowMode DetermineModeForPlayer(PlayerInteractionController player)
        {
            if (player.CurrentItem == null) return ShowMode.Free;

            return ShowMode.Current;
        }
        private bool IsSignVisible(SignChildren sign, PlayerInteractionController player, ShowMode showMode, float timer)
        {
            if (timer <= 0) return false;

            if (!sign.Original.gameObject.activeSelf) return false;

            switch (showMode)
            {
                case ShowMode.None: return false;
                case ShowMode.Free:
                    if(sign.Type == UIType.Interaction)
                    {
                        Interaction interaction = sign.Original.GetComponent<Interaction>();
                        if (interaction.IsDone()) return false;

                        if (interaction.useCleaningCondition)
                        {
                            return !interaction.cleaningCondition.NeedsItem;
                        }
                    }
                    else if(sign.Type == UIType.Cheat)
                    {
                        return false;
                    }
                    else if (sign.Type == UIType.Storage)
                    {
                        return false;
                    }
                    return true;
                case ShowMode.Current:
                    if (sign.Type == UIType.Interaction)
                    {
                        if (player.CurrentItem.canBeStored) return false;

                        Interaction interaction = sign.Original.GetComponent<Interaction>();
                        if (interaction.IsDone()) return false;

                        if(player.CurrentItem.NeedsWater && player.CurrentItem.CurrentWaterAmount == 0)
                        {
                            return interaction.isWaterSource;
                        }
                        else if(player.CurrentItem.NeedsWater && player.CurrentItem.CurrentWaterAmount > 0)
                        {
                            return interaction.useCleaningCondition && interaction.cleaningCondition.NeedsWater;
                        }

                        if (interaction.useCleaningCondition)
                        {
                            return interaction.cleaningCondition.WouldMetCondition(player);
                        }

                        return false;
                    }
                    else if(sign.Type == UIType.Storage || sign.Type == UIType.Cheat)
                    {
                        return player.CurrentItem.canBeStored && !sign.Original.GetComponent<StoreInteraction>().isFull;
                    }
                    return false;
            }

            return false;
        }
        private void UpdateSignSprite(SignChildren sign, PlayerInteractionController player, ShowMode showMode)
        {
            switch (showMode)
            {
                case ShowMode.None: return;
                case ShowMode.Free: return;
                case ShowMode.Current:
                    switch (sign.Type)
                    {
                        case UIType.None: return;
                        case UIType.Pickup: return;
                        case UIType.Interaction:
                            Interaction interaction = sign.Original.GetComponent<Interaction>();
                            if (interaction.consumesItem)
                            {
                                if (interaction.useCleaningCondition)
                                {
                                    if (interaction.cleaningCondition.WouldMetCondition(player))
                                    {
                                        sign.SetSprite(player.isPlayerOne ? consumedInteractionSpriteP1 : consumedInteractionSpriteP2);
                                    }
                                }
                                else
                                {
                                        sign.SetSprite(player.isPlayerOne ? consumedInteractionSpriteP1 : consumedInteractionSpriteP2);
                                }
                            }
                            else
                            {
                                sign.SetSprite(player.isPlayerOne ? interactionSpriteP1 : interactionSpriteP2);
                            }
                            return;
                        case UIType.Storage:
                            sign.SetSprite(player.isPlayerOne ? consumedInteractionSpriteP1 : consumedInteractionSpriteP2);
                            return;
                        case UIType.Cheat:
                            sign.SetSprite(player.isPlayerOne ? cheatSpriteP1 : cheatSpriteP2);
                            return;

                    }
                    break;
            }
        }

        public void HighlightPlayerObjects(bool playerOne, float time)
        {
            if (playerOne)
            {
                _timer1 = time;
            }
            else
            {
                _timer2 = time;
            }
        }

        [System.Serializable]
        public struct SignChildren
        {
            public SignChildren(Transform original, Transform parent, RectTransform content, UIType type)
            {
                Original = original;
                Parent = parent;
                Content = content;
                Type = type;

                Image = content.GetComponentInChildren<Image>();
                OverrideOffset = Original.GetComponent<HighlightOffsetOverride>();
            }

            public Transform Original;
            public Transform Parent;
            public RectTransform Content;

            public bool HasOverrideOffset => OverrideOffset != null;
            public HighlightOffsetOverride GetOverrideOffset => OverrideOffset;

            public UIType Type;

            private Image Image;
            private HighlightOffsetOverride OverrideOffset;

            public void SetSprite(Sprite sprite)
            {
                Image.sprite = sprite;
            }
        }

        public enum UIType
        {
            None = 0,
            Pickup = 1,
            Interaction = 2,
            Storage = 3,
            Cheat = 4
        }

        public enum ShowMode
        {
            None = 0,
            Free = 1,
            Current = 2
        }

    }
}
