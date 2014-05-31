using ImLazy.Annotations;

namespace ImLazy.Service
{
    class Component
    {
        private static Component _installComponent;
        internal static Component InstallComponent
        {
            get { return _installComponent ?? (_installComponent = new Component("Install.bat")); }
            set { _installComponent = value; }
        }

        private static Component _uninstallComponent;
        internal static Component UninstallComponent
        {
            get { return _uninstallComponent ?? (_uninstallComponent = new Component("Uninstall.bat")); }
            set { _uninstallComponent = value; }
        }

        public string FilePath { [UsedImplicitly] get; private set; }

        private Component(string filePath)
        {
            FilePath = filePath;
        }

        public void Run()
        {
            
        }
    }
}
