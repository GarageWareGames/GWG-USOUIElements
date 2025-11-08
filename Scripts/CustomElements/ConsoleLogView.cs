using UnityEngine;
using UnityEngine.UIElements;
using GWG.WindowFramework;
//using GWWE.Core;

namespace GWG.NGOCore
{
    public class ConsoleLogView : WindowFrame
    {
        private readonly ScrollView _scrollView = new ScrollView();
        private readonly Label _logLabel = new Label();
        private ConsoleLogReader _consoleLogReader;

        public ConsoleLogView()
        {
            name = "ConsoleLogViewer";
            Title = "Console Log Viewer";

            Content.Add(_scrollView);
            var container = new VisualElement();

            _scrollView.Add(container);
            _logLabel.style.whiteSpace = WhiteSpace.Normal;
            container.Add(_logLabel);
            Debug.Log("Logging Ready.");
        }

        public ConsoleLogView(ConsoleLogReader consoleLogReader)
        {
            name = "ConsoleLogViewer";
            Title = "Console Log Viewer";

            Content.Add(_scrollView);
            var container = new VisualElement();

            _scrollView.Add(container);
            _logLabel.style.whiteSpace = WhiteSpace.Normal;
            container.Add(_logLabel);
            Initialize(consoleLogReader);
            Debug.Log("Logging Ready.");
        }

        public void Initialize(ConsoleLogReader consoleLogReader)
        {
            _consoleLogReader = consoleLogReader;
            _consoleLogReader.LogUpdated += UpdateLogText;
        }

        private void OnDestroy()
        {
            if (_consoleLogReader != null)
            {
                _consoleLogReader.LogUpdated -= UpdateLogText;
            }
        }

        private void UpdateLogText(string logText)
        {
            _logLabel.text = logText;
        }
    }
}