using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MirrorTherapy.DAConverter;

namespace MirrorTherapy.Settings
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        private Rs232c rs232c;
        public SaveTimeToJson saveTimeToJson;
        [SerializeField, Tooltip("Button disable duration in seconds")] 
        private float buttonDisableDuration = 15f;
        private bool isButtonEnabled = true;
        public float ButtonDisableDuration
        {
            get => buttonDisableDuration;
            set => buttonDisableDuration = Mathf.Max(0, value); // 負の値を防ぐ
        }
        void Start()
        {
            ValidateReferences();
            startButton.onClick.AddListener(StartButtonClicked);
        }

        void Update()
        {
            CheckSpaceKey();
        }

        private void ValidateReferences()
        {
            if (startButton == null)
                Debug.LogError("Start Button is not set");
            if (rs232c == null)
                Debug.LogError("Rs232c is not set");
            if (saveTimeToJson == null)
                Debug.LogError("SaveTimeToJson is not set");
        }

        private void CheckSpaceKey()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isButtonEnabled)
            {
                StartButtonClicked();
            }
        }

        public void StartButtonClicked()
        {
            if (!isButtonEnabled) return;

            try
            {
                HandleStartAction();
                StartCoroutine(DisableButtonTemporarily());
                Debug.Log("Start Button Clicked at " + Time.time);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in StartButtonClicked: {ex.Message}\n{ex.StackTrace}");
                SetButtonState(true);
            }
        }

        private void HandleStartAction()
        {
            saveTimeToJson.SaveCurrentTime(SaveTimeToJson.TimeType.Start);
            rs232c.SendToStart();
        }

        private IEnumerator DisableButtonTemporarily()
        {
            SetButtonState(false);
            yield return new WaitForSeconds(buttonDisableDuration);
            rs232c.SendToStart();
            SetButtonState(true);
        }

        private void SetButtonState(bool state)
        {
            isButtonEnabled = state;
            startButton.interactable = state;
            UpdateButtonColor(state);
        }

        private void UpdateButtonColor(bool state)
        {
            ColorBlock colors = startButton.colors;
            colors.normalColor = state ? Color.white : Color.gray;
            startButton.colors = colors;
        }

        private void OnDestroy()
        {
            if (startButton != null)
            {
                startButton.onClick.RemoveListener(StartButtonClicked);
            }
        }
    }
}

