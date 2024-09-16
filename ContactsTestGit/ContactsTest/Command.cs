using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactsTest
{
    public class OCommand : OCommandBase
    {
        public OCommand(Action execute, Func<bool> canExecute)
            : base(o => execute.Invoke(), o => canExecute.Invoke())
        {
        }

        public OCommand(Action execute)
            : this(execute, () => true)
        {
        }

        #region Члены ICommand

        public virtual bool CanExecute()
        {
            return base.CanExecute(null);
        }


        public virtual void Execute()
        {
            base.Execute(null);
        }

        #endregion
    }
    public abstract class OCommandBase : ICommand
    {
        // Fields
        private readonly Func<object, bool> _canExecuteMethod;
        private readonly Action<object> _executeMethod;

        // Methods
        protected OCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if ((executeMethod == null) || (canExecuteMethod == null))
            {
                throw new ArgumentNullException("canExecuteMethod is null");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        protected bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod.Invoke(parameter);
        }

        protected void Execute(object parameter)
        {
            _executeMethod(parameter);
        }

        protected virtual void OnCanExecuteChanged()
        {
            var handler = _canExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnIsActiveChanged()
        {
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        // Properties
        public bool IsActive { get; set; }


        private event EventHandler _canExecuteChanged;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                EventHandler handler2;
                var canExecuteChanged = this._canExecuteChanged;
                do
                {
                    handler2 = canExecuteChanged;
                    var handler3 = (EventHandler)Delegate.Combine(handler2, value);
                    canExecuteChanged = Interlocked.CompareExchange(ref this._canExecuteChanged, handler3, handler2);
                }
                while (canExecuteChanged != handler2);
            }
            remove
            {
                EventHandler handler2;
                var canExecuteChanged = this._canExecuteChanged;
                do
                {
                    handler2 = canExecuteChanged;
                    var handler3 = (EventHandler)Delegate.Remove(handler2, value);
                    canExecuteChanged = Interlocked.CompareExchange(ref this._canExecuteChanged, handler3, handler2);
                }
                while (canExecuteChanged != handler2);
            }
        }

    }
}
