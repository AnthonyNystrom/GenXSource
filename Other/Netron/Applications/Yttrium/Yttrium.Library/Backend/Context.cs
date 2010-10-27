#region Copyright 2001-2006 Christoph Daniel Rüegg [GPL]
//Math.NET Symbolics: Yttrium, part of Math.NET
//Copyright (c) 2001-2006, Christoph Daniel Rueegg, http://cdrnet.net/.
//All rights reserved.
//This Math.NET package is available under the terms of the GPL.

//This program is free software; you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation; either version 2 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion

using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

using MathNet.Symbolics.Core;
using MathNet.Symbolics.Workplace;
using MathNet.Symbolics.Backend.Simulation;
using MathNet.Symbolics.Backend.Events;
using MathNet.Symbolics.Backend.Channels;

namespace MathNet.Symbolics.Backend
{
    public class Context
    {
        private Builder builder;
        private Library library;
        private Containers.SignalTable singletonSignals;
        private Scheduler scheduler;
        private Random random;
        private HintChannel hintChannel;

        internal Context()
        {
            this.library = new Library(this);
            this.builder = new Builder(this);
            this.scheduler = new Scheduler(this);
            this.singletonSignals = new Containers.SignalTable();
            this.random = new Random();
            this.hintChannel = new HintChannel();
        }

        public Builder Builder
        {
            get { return builder; }
        }

        public Library Library
        {
            get { return library; }
        }

        public Containers.SignalTable SingletonSignals
        {
            get { return singletonSignals; }
        }

        public Scheduler Scheduler
        {
            get { return scheduler; }
        }

        public Random Random
        {
            get { return random; }
        }

        internal int GenerateTag()
        {
            return random.Next();
        }

        public Guid GenerateInstanceId()
        {
            return Guid.NewGuid();
        }

        public HintChannel Hints
        {
            get { return hintChannel; }
        }

        internal static string YttriumNamespace
        {
            get { return @"http://www.cdrnet.net/projects/nmath/symbolics/yttrium/system/0.50/"; }
        }

        #region Globalization
        public static CultureInfo Culture
        {
            get { return CultureInfo.InvariantCulture; }
        }

        public static StringComparer IdentifierComparer
        {
            get { return StringComparer.InvariantCulture; }
        }

        public static NumberFormatInfo NumberFormat
        {
            get { return NumberFormatInfo.InvariantInfo; }
        }

        public static Encoding DefaultEncoding
        {
            get { return Encoding.Unicode; }
        }
        #endregion

        #region Format Settings
        private char separatorCharacter = ',';
        public char SeparatorCharacter
        {
            get { return separatorCharacter; }
            set { separatorCharacter = value; }
        }

        private char executorCharacter = ';';
        public char ExecutorCharacter
        {
            get { return executorCharacter; }
            set { executorCharacter = value; }
        }

        private EncapsulationFormat listEncapsulation = new EncapsulationFormat('(', ')');
        public EncapsulationFormat ListEncapsulation
        {
            get {return listEncapsulation;}
            set {listEncapsulation = value;}
        }

        private EncapsulationFormat vectorEncapsulation = new EncapsulationFormat('[', ']');
        public EncapsulationFormat VectorEncapsulation
        {
            get { return vectorEncapsulation; }
            set { vectorEncapsulation = value; }
        }

        private EncapsulationFormat setEncapsulation = new EncapsulationFormat('{', '}');
        public EncapsulationFormat SetEncapsulation
        {
            get { return setEncapsulation; }
            set { setEncapsulation = value; }
        }

        private EncapsulationFormat scalarEncapsulation = new EncapsulationFormat('<', '>');
        public EncapsulationFormat ScalarEncapsulation
        {
            get { return scalarEncapsulation; }
            set { scalarEncapsulation = value; }
        }

        private EncapsulationFormat literalEncapsulation = new EncapsulationFormat('"', '"');
        public EncapsulationFormat LiteralEncapsulation
        {
            get { return literalEncapsulation; }
            set { literalEncapsulation = value; }
        }
        #endregion

        #region Notification (Trivial Mediator)
        #region Units Constructed & Deconstructed
        public event EventHandler<SignalEventArgs> OnNewSignalConstructed;
        internal void NotifyNewSignalConstructed(Signal signal)
        {
            EventHandler<SignalEventArgs> handler = OnNewSignalConstructed;
            if(handler != null)
                handler(this, new SignalEventArgs(signal));
        }

        public event EventHandler<PortEventArgs> OnNewPortConstructed;
        internal void NotifyNewPortConstructed(Port port)
        {
            EventHandler<PortEventArgs> handler = OnNewPortConstructed;
            if(handler != null)
                handler(this, new PortEventArgs(port));
        }

        public event EventHandler<BusEventArgs> OnNewBusConstructed;
        internal void NotifyNewBusConstructed(Bus bus)
        {
            EventHandler<BusEventArgs> handler = OnNewBusConstructed;
            if(handler != null)
                handler(this, new BusEventArgs(bus));
        }
        #endregion

        #region Port <-> Signal|Bus Connections
        public event EventHandler<SignalPortIndexEventArgs> OnSignalDrivenByPort;
        internal void NotifySignalDrivenByPort(Signal signal, Port port, int outputIndex)
        {
            EventHandler<SignalPortIndexEventArgs> handler = OnSignalDrivenByPort;
            if(handler != null)
                handler(this, new SignalPortIndexEventArgs(signal,port,outputIndex));
        }

        public event EventHandler<SignalPortIndexEventArgs> OnSignalNoLongerDrivenByPort;
        internal void NotifySignalNoLongerDrivenByPort(Signal signal, Port port, int outputIndex)
        {
            EventHandler<SignalPortIndexEventArgs> handler = OnSignalNoLongerDrivenByPort;
            if(handler != null)
                handler(this, new SignalPortIndexEventArgs(signal, port, outputIndex));
        }

        public event EventHandler<SignalPortIndexEventArgs> OnSignalDrivesPort;
        internal void NotifySignalDrivesPort(Signal signal, Port port, int inputIndex)
        {
            EventHandler<SignalPortIndexEventArgs> handler = OnSignalDrivesPort;
            if(handler != null)
                handler(this, new SignalPortIndexEventArgs(signal, port, inputIndex));
        }

        public event EventHandler<SignalPortIndexEventArgs> OnSignalNoLongerDrivesPort;
        internal void NotifySignalNoLongerDrivesPort(Signal signal, Port port, int inputIndex)
        {
            EventHandler<SignalPortIndexEventArgs> handler = OnSignalNoLongerDrivesPort;
            if(handler != null)
                handler(this, new SignalPortIndexEventArgs(signal, port, inputIndex));
        }

        public event EventHandler<BusPortIndexEventArgs> OnBusAttachedToPort;
        internal void NotifyBusAttachedToPort(Bus bus, Port port, int busIndex)
        {
            EventHandler<BusPortIndexEventArgs> handler = OnBusAttachedToPort;
            if(handler != null)
                handler(this, new BusPortIndexEventArgs(bus, port, busIndex));
        }

        public event EventHandler<BusPortIndexEventArgs> OnBusNoLongerAttachedToPort;
        internal void NotifyBusNoLongerAttachedToPort(Bus bus, Port port, int busIndex)
        {
            EventHandler<BusPortIndexEventArgs> handler = OnBusNoLongerAttachedToPort;
            if(handler != null)
                handler(this, new BusPortIndexEventArgs(bus, port, busIndex));
        }
        #endregion

        #region Signal State
        public event EventHandler<SignalEventArgs> OnSignalValueChanged;
        internal void NotifySignalValueChanged(Signal signal)
        {
            EventHandler<SignalEventArgs> handler = OnSignalValueChanged;
            if(handler != null)
                handler(this, new SignalEventArgs(signal));
        }
        #endregion
        #endregion
    }
}
