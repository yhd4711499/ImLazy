using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImLazy.ControlPanel.Commands
{
    public class RelayAsyncCommand<T> : ICommand
    {
        public event EventHandler Started;

        public event EventHandler Ended;

        public bool IsExecuting { get; private set; }

        public RelayAsyncCommand(Action<T> execute, Func<T, Boolean> canExecute = null)
        {
            IsExecuting = false;
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            var result = _canExecute == null || _canExecute((T)parameter);
            return (result && (!IsExecuting));
        }

        public void Execute(object parameter)
        {
            try
            {
                IsExecuting = true;
                if (Started != null)
                {
                    Started(this, EventArgs.Empty);
                }

                Task task = Task.Factory.StartNew(() => _execute((T)parameter));
                task.ContinueWith(t => OnRunWorkerCompleted(EventArgs.Empty),
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        private void OnRunWorkerCompleted(EventArgs e)
        {
            IsExecuting = false;
            if (Ended != null)
            {
                Ended(this, e);
            }
        }

        private readonly Func<T,Boolean> _canExecute;

        private readonly Action<T> _execute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
    }
}
