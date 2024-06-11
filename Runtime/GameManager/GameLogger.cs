namespace Glitch9.Game
{
    public class GameLogger : ILogger
    {
        private const string FALLBACK_PROJECT_NAME = "Unknown Project";
        public string ProjectName
        {
            get
            {
                if (string.IsNullOrEmpty(_projectName))
                {
                    _projectName = FALLBACK_PROJECT_NAME;
                }
                return _projectName;
            }
        }
        private string _projectName;
        public GameLogger(string projectName)
        {
            _projectName = projectName;
        }

        public void Info(string message)
        {
            GNLog.Info(ProjectName, message);
        }

        public void Warning(string message)
        {
            GNLog.Warning(ProjectName, message);
        }

        public void Error(string message)
        {
            GNLog.Error(ProjectName, message);
        }

        public void Info(string tag, string message)
        {
            GNLog.Info(tag, message);
        }

        public void Warning(string tag, string message)
        {
            GNLog.Warning(tag, message);
        }

        public void Error(string tag, string message)
        {
            GNLog.Error(tag, message);
        }
    }
}