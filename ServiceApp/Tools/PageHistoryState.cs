namespace ServiceApp.Tools
{
    public class PageHistoryState
    {
        private List<string> _previousPages;

        public PageHistoryState()
        {
            _previousPages = new List<string>();
        }
        public void AddPageToHistory(string pageName)
        {
            _previousPages.Add(pageName);
            RemoveOldestHisory();
        }

        public void RemoveOldestHisory()
        {
            if (_previousPages.Count > 15)
                _previousPages.RemoveRange(0, 10);
        }

        public string GetGoBackPage()
        {
            if (_previousPages.Count > 1)
            {
                return _previousPages.ElementAt(_previousPages.Count - 2);
            }
            return _previousPages.FirstOrDefault();
        }

        public bool CanGoBack()
        {
            return _previousPages.Count > 1;
        }
    }
}
