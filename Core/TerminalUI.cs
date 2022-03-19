#if DEVELOPMENT || UNITY_EDITOR||DEVELOPMENT_RELEASE

using System;
using GameWarriors.TerminalDomain.Abstraction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameWarriors.TerminalDomain.Core
{
    public class TerminalUI : MonoBehaviour
    {
        [SerializeField] private Button _terminalButton;
        [SerializeField] private TextMeshProUGUI _outputText;
        [SerializeField] private TMP_InputField _termialInput;
        [SerializeField] private FpsCounter _fpsCounter;

        private bool _isActive;
        private Func<string, string> _onInvoke;

        public void Initialization(ITerminal terminal, Func<string, string> onInvoke)
        {
            _onInvoke = onInvoke;
            _outputText.gameObject.SetActive(false);
            _termialInput.gameObject.SetActive(false);

            _terminalButton.gameObject.SetActive(true);

            _terminalButton.onClick.AddListener(TerminalButtonOnClicked);
            _termialInput.onEndEdit.AddListener(TerminalInputOnEndEdit);

            DontDestroyOnLoad(gameObject);
            _fpsCounter.Initialization(terminal);
            terminal.Register(() => gameObject.SetActive(false), "terminaloff");
            gameObject.SetActive(true);
        }

        //public string stringToEdit = "Hello World";
        //private void OnGUI()
        //{
        //    var tmp = GUI.TextField(new Rect(0, 0, Screen.width, Screen.height/18), stringToEdit, 30);
        //    if (tmp != stringToEdit)
        //    {
        //        Debug.Log("edit change : " + tmp);
        //    }
        //    stringToEdit = tmp;

        //    if (Event.current.isKey)
        //    {
        //        Debug.Log("terminal key: " + Event.current.keyCode);
        //    }
        //    if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) {
        //        Debug.Log("terminal event: " + stringToEdit);
        //    }

        //    GUI.Button(new Rect(0, Screen.height - (Screen.height/24), Screen.width/6, Screen.height/24), "Terminal");

        //}

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                ToggleTerminalInput();
        }

        private void TerminalInputOnEndEdit(string input)
        {
            if (!_isActive || string.IsNullOrWhiteSpace(input))
                return;

            string output = _onInvoke?.Invoke(input);

            ShowOutput(output);

            HideTerminalInput();
        }

        private void TerminalButtonOnClicked()
        {
            ToggleTerminalInput();
        }

        private void ToggleTerminalInput()
        {
            if (_isActive)
            {
                HideTerminalInput();
            }
            else
            {
                ShowTerminalInput();
            }
        }

        private void ShowTerminalInput()
        {
            _termialInput.gameObject.SetActive(true);
            _termialInput.text = string.Empty;

            _termialInput.ActivateInputField();
            _isActive = true;
        }

        private void HideTerminalInput()
        {
            _termialInput.gameObject.SetActive(false);

            _isActive = false;
        }

        private void ShowOutput(string output)
        {
            _outputText.text = output;

            _outputText.gameObject.SetActive(true);

            CancelInvoke("HideOutput");
            Invoke("HideOutput", 4);
        }

        private void HideOutput()
        {
            _outputText.gameObject.SetActive(false);
        }
    }
}
#endif