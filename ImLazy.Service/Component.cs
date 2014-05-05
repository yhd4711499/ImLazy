namespace ImLazy.Service
{
    class Component
    {
        private static Component _installComponent;
        internal static Component InstallComponent
        {
            get
            {
                if (_installComponent == null)
                {
                    _installComponent = new Component("Install.bat");
                }
                return _installComponent;
            }
            set { _installComponent = value; }
        }

        private static Component _uninstallComponent;
        internal static Component UninstallComponent
        {
            get
            {
                if (_installComponent == null)
                {
                    _installComponent = new Component("Uninstall.bat");
                }
                return _installComponent;
            }
            set { _installComponent = value; }
        }

        public string FilePath { get; private set; }

        private Component(string filePath)
        {
            FilePath = filePath;
        }

        public void Run()
        {
            
        }
    }
}
