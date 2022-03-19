using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameWarriors.TerminalDomain.Abstraction;
using UnityEngine;

namespace GameWarriors.TerminalDomain.Core
{
    public class TerminalSystem : ITerminal
    {
        private readonly Dictionary<string, Action<string>> _oneArgMethodsTable;
        private readonly Dictionary<string, Action> _zeroArgMethodsTable;
        private readonly ITerminalEventHandler _event;
        private TerminalUI _terminalUI;

        [UnityEngine.Scripting.Preserve]
        public TerminalSystem(ITerminalEventHandler terminalEvent)
        {
            _event = terminalEvent;
            _oneArgMethodsTable = new Dictionary<string, Action<string>>(20);
            _zeroArgMethodsTable = new Dictionary<string, Action>(20);
            ResourceRequest terminalRequest = Resources.LoadAsync<GameObject>("Terminal");
            terminalRequest.completed += OnTerminalLoaded;
        }

        [UnityEngine.Scripting.Preserve]
        public async Task WaitForLoading()
        {
            while (_terminalUI == null)
            {
                await Task.Delay(100);
            }
        }

        private void OnTerminalLoaded(AsyncOperation operation)
        {
            if (operation is ResourceRequest terminalRequest)
            {
                GameObject terminalUI = terminalRequest.asset as GameObject;
                if (terminalUI != null)
                {
                    terminalUI = GameObject.Instantiate(terminalUI);
                    terminalUI.name = "Terminal";
                    _terminalUI = terminalUI.GetComponent<TerminalUI>();
                    _terminalUI.Initialization(this, InvokeMethod);
                }
                else
                {
                    Debug.LogWarning("Terminal System Initialize failed!");
                }
            }
        }

        //[System.Diagnostics.Conditional("DEVELOPMENT")]
        public void Register(Action<string> method, string methodName)
        {
            methodName = methodName.ToLower();
            if (!_oneArgMethodsTable.ContainsKey(methodName))
            {
                if (method == null)
                    Debug.LogWarning($"Method is null!");
                else if (string.IsNullOrEmpty(methodName))
                    Debug.LogWarning($"MethodName is null!");
                else
                    _oneArgMethodsTable.Add(methodName, method);
            }
            else
            {
                Debug.LogWarning($"Method {methodName} has been registered before!");
            }
        }

        //[System.Diagnostics.Conditional("DEVELOPMENT")]
        public void Register(Action method, string methodName)
        {
            methodName = methodName.ToLower();
            if (!_zeroArgMethodsTable.ContainsKey(methodName))
            {
                if (method == null)
                    Debug.LogWarning($"Method is null!");
                else if (string.IsNullOrEmpty(methodName))
                    Debug.LogWarning($"MethodName is null!");
                else
                    _zeroArgMethodsTable.Add(methodName, method);
            }
            else
            {
                Debug.LogWarning($"Method {methodName} has been registered before!");
            }
        }

        private string InvokeMethod(string terminalInput)
        {
            terminalInput = terminalInput.ToLower();
            string[] inputs = terminalInput.Split(' ');

            if (inputs.Length == 0)
            {
                return "<color=red>Input is Invalid!</color>";
            }
            else if (inputs.Length == 1)
            {
                string methodName = inputs[0];

                if (!_zeroArgMethodsTable.ContainsKey(methodName))
                {
                    return $"<color=red>Method {methodName} has not been registered yet!</color>";
                }

                _event.OnZeroArgInvoke(methodName);
                _zeroArgMethodsTable[methodName].Invoke();

                return $"<color=green>Method {methodName} has been invoked!</color>";
            }
            else
            {
                string methodName = inputs[0];
                string methodValue = inputs[1];

                if (!_oneArgMethodsTable.ContainsKey(methodName))
                {
                    return $"<color=red>Method {methodName} has not been registered yet!</color>";
                }

                _event.OnOneArgInvoke(methodName, inputs[1]);
                _oneArgMethodsTable[methodName].Invoke(methodValue);
                return $"<color=green>Method {methodName} : {methodValue} has been invoked!</color>";
            }
        }
    }
}