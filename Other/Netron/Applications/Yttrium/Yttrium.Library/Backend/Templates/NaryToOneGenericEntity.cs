#region Copyright 2001-2006 Christoph Daniel R�egg [GPL]
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
using System.Xml;

using MathNet.Symbolics.Core;

namespace MathNet.Symbolics.Backend.Templates
{
    [Serializable]
    public class NaryToOneGenericEntity : Entity
    {
        public NaryToOneGenericEntity(string symbol, string label, string domain)
            : base(symbol, label, domain, InfixNotation.None, -1, true) { }
        public NaryToOneGenericEntity(string symbol, string label, string domain, InfixNotation notation, int precedence)
            : base(symbol, label, domain, notation, precedence, true) { }

        //protected override Entity CompileGenericEntity(Signal[] inputSignals, Bus[] buses)
        public override Entity CompileGenericEntity(int inputSignalsCount, int busesCount)
        {
            string lbl = EntityId.Label;
            string prefixIn = lbl + "_in_";

            string[] newInputLabels = new string[inputSignalsCount];
            string[] newOutputLabels = new string[] { lbl + "_out" };

            for(int i = 0; i < inputSignalsCount; i++)
            {
                string nr = i.ToString(System.Globalization.NumberFormatInfo.InvariantInfo).PadLeft(3, '0');
                newInputLabels[i] = prefixIn + nr;
            }

            return new Entity(Symbol, EntityId, Notation, PrecedenceGroup, newInputLabels, newOutputLabels, Buses);
        }

        //public override bool Equals(Entity other)
        //{
        //    return base.Equals(other) && other is NaryToOneGenericEntity;
        //}
    }
}
