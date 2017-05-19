//taken from James7132 https://github.com/HouraiTeahouse/FantasyCrescendo

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    public sealed class FPSCounter : MonoBehaviour {

        [SerializeField]
        NetworkManager _networkManager;

        Text Counter;
        float deltaTime;
        float fps;
        string outputText;

        void Awake() {
            Counter = GetComponent<Text>();
            StartCoroutine(UpdateDisplay());
        }

        void Start() {
            if (_networkManager == null)
                _networkManager = NetworkManager.singleton;
        }

        void Update() {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            fps = 1.0f / deltaTime;
        }

        IEnumerator UpdateDisplay() {
            while (true) {
                yield return new WaitForSeconds(0.5f);
                var text = string.Format("{0:0.}FPS", fps);
                if (_networkManager != null && _networkManager.client != null)
                    text += string.Format("/{0:0.} RTT", _networkManager.client.GetRTT());
                Counter.text = text;
            }
        }

    }