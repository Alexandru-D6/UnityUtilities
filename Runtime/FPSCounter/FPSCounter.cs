using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runtime.FPSCounter {
    public class FPSCounter : MonoBehaviour {
        private const int CacheNumbersAmount = 1000;
        private const int AverageFromAmount = 30;

        private Dictionary<int, string> _cachedNumberStrings = new();

        private int[] _frameRateSamples;
        private int _averageCounter;
        private int _currentAveraged;

        [SerializeField, InspectorName("Text")] private TMP_Text _text;

        void Awake() {
            for (int i = 0; i < CacheNumbersAmount; i++) {
                _cachedNumberStrings[i] = i.ToString();
            }

            _frameRateSamples = new int[AverageFromAmount];
        }

        void Update() {
            // Sample
            {
                var currentFrame = (int)Math.Round(1f / Time.smoothDeltaTime); // Use unscaledDeltaTime for more accurate, or if your game modifies Time.timeScale.
                _frameRateSamples[_averageCounter] = currentFrame;
            }

            // Average
            {
                var average = 0f;

                foreach (var frameRate in _frameRateSamples) {
                    average += frameRate;
                }

                _currentAveraged = (int)Math.Round(average / AverageFromAmount);
                _averageCounter = (_averageCounter + 1) % AverageFromAmount;
            }

            // Assign to UI
            {
                _text.text = _currentAveraged switch {
                    var x when x >= 0 && x < CacheNumbersAmount => $"fps: {_cachedNumberStrings[x]}",
                    var x when x >= CacheNumbersAmount => $"fps: > {CacheNumbersAmount}",
                    var x when x < 0 => "fps: < 0",
                    _ => "fps: ???"
                };
            }
        }
    }
}
