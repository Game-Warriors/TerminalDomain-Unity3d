#if DEVELOPMENT || UNITY_EDITOR||DEVELOPMENT_RELEASE

using GameWarriors.TerminalDomain.Abstraction;
using TMPro;
using UnityEngine;

namespace GameWarriors.TerminalDomain.Core
{
    public class FpsCounter : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _fpsText;

        [SerializeField] private float _fpsMeasurePeriod = 0.5f;

        private int _fpsAccumulator = 0;
        private float _fpsNextPeriod = 0;
        private int _currentFps;

        private ITerminal _terminal;

        #endregion

        #region Methods

        public void Initialization(ITerminal terminal)
        {
            _terminal = terminal;
            gameObject.SetActive(false);
            _fpsNextPeriod = Time.realtimeSinceStartup + _fpsMeasurePeriod;

            _terminal.Register(ToggleFPS, "FPS");
        }

        #endregion

        #region Helper

        private void Update()
        {
            _fpsAccumulator++;
            if (Time.realtimeSinceStartup > _fpsNextPeriod)
            {
                _currentFps = (int) (_fpsAccumulator / _fpsMeasurePeriod);
                _fpsAccumulator = 0;
                _fpsNextPeriod += _fpsMeasurePeriod;
            }

            if (gameObject.activeInHierarchy)
            {
                SetFPS();
            }
        }

        private void SetFPS()
        {
            if (_currentFps > 50)
                _fpsText.text = $"<color=green>FPS {_currentFps}</color>";
            else
                if (_currentFps > 40)
                    _fpsText.text = $"<color=yellow>FPS {_currentFps}</color>";
                else
                    if (_currentFps > 30)
                        _fpsText.text = $"<color=orange>FPS {_currentFps}</color>";
                    else
                        _fpsText.text = $"<color=red>FPS {_currentFps}</color>";
        }

        private void ToggleFPS()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        #endregion
    }
}
#endif