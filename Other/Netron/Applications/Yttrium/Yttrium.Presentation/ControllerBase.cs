using System;
using System.Collections.Generic;
using System.Text;

using MathNet.Symbolics.Core;
using MathNet.Symbolics.Workplace;
using MathNet.Symbolics.Backend;
using MathNet.Symbolics.Backend.Channels;
using MathNet.Symbolics.Backend.Channels.Commands;

namespace MathNet.Symbolics.Presentation
{
    public abstract class ControllerBase : ISystemObserver
    {
        private Project _project;

        protected ControllerBase()
        {
            _project = new Project();
        }
        protected ControllerBase(Context context)
        {
            _project = new Project(context);
        }
        protected ControllerBase(Project project)
        {
            _project = project;
        }

        protected void Init()
        {
            _project.AttachLocalObserver(this);
        }

        public Project Project
        {
            get { return _project; }
        }
        public Context Context
        {
            get { return _project.Context; }
        }
        public MathSystem CurrentSystem
        {
            get { return _project.CurrentSystem; }
        }
        public Mediator CurrentMediator
        {
            get { return _project.CurrentSystem.Mediator; }
        }
        public CommandChannel CurrentCommands
        {
            get { return _project.CurrentSystem.Mediator.Commands; }
        }

        #region Load/Unload System
        protected abstract void LoadSystem(MathSystem system);
        protected abstract void UnloadSystem(MathSystem system);
        public virtual bool AutoDetachOnSystemChanged
        {
            get { return false; }
        }
        public virtual bool AutoInitialize
        {
            get { return true; }
        }
        public void AttachedToSystem(MathSystem system)
        {
            LoadSystem(system);
        }
        public void DetachedFromSystem(MathSystem system)
        {
            UnloadSystem(system);
        }
        public virtual void BeginInitialize()
        {
        }
        public virtual void EndInitialize()
        {
        }
        #endregion

        #region MathSystem Observer
        public virtual void OnSignalAdded(Signal signal, int index)
        {
        }
        public virtual void OnSignalRemoved(Signal signal, int index)
        {
        }
        public virtual void OnSignalMoved(Signal signal, int indexBefore, int indexAfter)
        {
        }
        public virtual void OnBusAdded(Bus bus, int index)
        {
        }
        public virtual void OnBusRemoved(Bus bus, int index)
        {
        }
        public virtual void OnBusMoved(Bus bus, int indexBefore, int indexAfter)
        {
        }
        public virtual void OnPortAdded(Port port, int index)
        {
        }
        public virtual void OnPortRemoved(Port port, int index)
        {
        }
        public virtual void OnPortMoved(Port port, int indexBefore, int indexAfter)
        {
        }
        public virtual void OnInputAdded(Signal signal, int index)
        {
        }
        public virtual void OnInputRemoved(Signal signal, int index)
        {
        }
        public virtual void OnInputMoved(Signal signal, int indexBefore, int indexAfter)
        {
        }
        public virtual void OnOutputAdded(Signal signal, int index)
        {
        }
        public virtual void OnOutputRemoved(Signal signal, int index)
        {
        }
        public virtual void OnOutputMoved(Signal signal, int indexBefore, int indexAfter)
        {
        }
        public virtual void OnPortDrivesSignal(Signal signal, Port port, int outputIndex)
        {
        }
        public virtual void OnPortDrivesSignalNoLonger(Signal signal, Port port, int outputIndex)
        {
        }
        public virtual void OnSignalDrivesPort(Signal signal, Port port, int inputIndex)
        {
        }
        public virtual void OnSignalDrivesPortNoLonger(Signal signal, Port port, int inputIndex)
        {
        }
        public virtual void OnBusAttachedToPort(Bus bus, Port port, int busIndex)
        {
        }
        public virtual void OnBusAttachedToPortNoLonger(Bus bus, Port port, int busIndex)
        {
        }
        #endregion

        #region Command Helper
        public void PostCommand(ICommand command)
        {
            _project.CurrentSystem.Mediator.PostCommand(command);
        }
        public virtual void PostCommandNewSignal()
        {
            PostCommand(new NewSignalCommand());
        }
        public virtual void PostCommandNewBus()
        {
            PostCommand(new NewBusCommand());
        }
        public virtual void PostCommandNewPort(MathIdentifier entityId, int inputCount, int busCount)
        {
            NewPortCommand cmd = new NewPortCommand();
            cmd.EntityId = entityId;
            cmd.NumberOfInputs = inputCount;
            cmd.NumberOfBuses = busCount;
            PostCommand(cmd);
        }
        public virtual void PostCommandRemoveSignal(CommandReference reference, bool isolate)
        {
            RemoveSignalCommand cmd = new RemoveSignalCommand();
            cmd.SignalReference = reference;
            cmd.Isolate = isolate;
            PostCommand(cmd);
        }
        public virtual void PostCommandRemoveBus(CommandReference reference)
        {
            RemoveBusCommand cmd = new RemoveBusCommand();
            cmd.BusReference = reference;
            PostCommand(cmd);
        }
        public virtual void PostCommandRemovePort(CommandReference reference, bool isolate)
        {
            RemovePortCommand cmd = new RemovePortCommand();
            cmd.PortReference = reference;
            cmd.Isolate = isolate;
            PostCommand(cmd);
        }
        #endregion
    }
}
