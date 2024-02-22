using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask
{
    public class AgentUI: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI agentNameText;

        [SerializeField] private Image portraitBackgroundImage;
        [SerializeField] private Image portraitImage;

        [SerializeField] private List<Transform> agentHearts;

        private void Start()
        {
            Agent.OnDeselected += Agent_OnDeselected;
            Agent.OnSelected += Agent_OnSelected;
            Agent.OnHealthChanged += Agent_OnHealthChanged;

            Hide();
        }

        private void Agent_OnHealthChanged(object sender, Agent.OnHealthChangedEventArgs e)
        {
            agentHearts[e.hp].gameObject.SetActive(false);
        }

        private void Agent_OnDeselected(object sender, EventArgs e)
        {
            Hide();
        }

        private void Agent_OnSelected(object sender, Agent.OnSelectedEventArgs e)
        {
            UpdateAgentName(e.name);
            UpdatePortrait(e.potraitBackground, e.potrait);
            UpdateAgentHearts(e.hp);

            Show();
        }

        private void UpdateAgentName(string agentName)
        {
            agentNameText.SetText(agentName);
        }

        private void UpdatePortrait(Sprite portraitBackground, Sprite portrait)
        {
            portraitImage.sprite = portrait;
            portraitBackgroundImage.sprite = portraitBackground;
        }



        private void UpdateAgentHearts(int agentHp)
        {
            for (int i = 0; i < agentHearts.Count; i++)
            {
                agentHearts[i].gameObject.SetActive(i + 1 <= agentHp);
            }
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Agent.OnDeselected -= Agent_OnDeselected;
            Agent.OnSelected -= Agent_OnSelected;
            Agent.OnHealthChanged-= Agent_OnHealthChanged;
        }

    }
}
