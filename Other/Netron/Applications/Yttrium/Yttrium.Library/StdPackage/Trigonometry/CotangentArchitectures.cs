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
using System.Collections.Generic;

using MathNet.Symbolics.Core;
using MathNet.Symbolics.Backend;
using MathNet.Symbolics.Backend.Containers;
using MathNet.Symbolics.Backend.Templates;
using MathNet.Symbolics.Backend.Theorems;
using MathNet.Symbolics.Backend.Traversing;
using MathNet.Symbolics.StdPackage.Structures;
using MathNet.Symbolics.Workplace;

namespace MathNet.Symbolics.StdPackage.Trigonometry
{
    public class CotangentArchitectures : GenericArchitectureFactory
    {
        private static readonly MathIdentifier _entityId = new MathIdentifier("Cotangent", "Std");
        public static MathIdentifier EntityIdentifier
        {
            get { return _entityId; }
        }

        public CotangentArchitectures()
            : base(_entityId)
        {
            AddArchitecture(EntityId.DerivePrefix("Real"),
                RealValueCategory.IsRealValueMember,
                delegate(Port port) { return new Process[] { new GenericStdParallelProcess<RealValue, RealValue>(delegate(RealValue value) { return value.Cotangent(); }, RealValue.ConvertFrom, port.InputSignalCount) }; });

            AddArchitecture(EntityId.DerivePrefix("Complex"),
                ComplexValueCategory.IsComplexValueMember,
                delegate(Port port) { return new Process[] { new GenericStdParallelProcess<ComplexValue, ComplexValue>(delegate(ComplexValue value) { return value.Cotangent(); }, ComplexValue.ConvertFrom, port.InputSignalCount) }; });
        }

        public static ITheorem[] BuildTheorems(Context context)
        {
            ITheorem[] theorems = new ITheorem[2];

            theorems[0] = new Analysis.DerivativeTransformation(context.Library.LookupEntity(_entityId),
                delegate(Port port, SignalSet manipulatedInputs, Signal variable, bool hasManipulatedInputs)
                {
                    Builder b = context.Builder;
                    Signal[] outputs = new Signal[manipulatedInputs.Count];
                    ReadOnlySignalSet squares = b.Square(port.OutputSignals);
                    Signal one = IntegerValue.ConstantOne(context);
                    for(int i = 0; i < outputs.Length; i++)
                        outputs[i] = (one + squares[i]) * manipulatedInputs[i];
                    return b.Negate(outputs);
                });

            theorems[1] = new BasicTransformation("CotangentTrigonometricSusbtitute", "Std", "TrigonometricSubstitute", "Std",
                delegate(Port port) { return port.Entity.EntityId.Equals("Cotangent", "Std"); },
                delegate(Port port) { return ManipulationPlan.DoAlter; },
                delegate(Port port, SignalSet transformedInputs, bool hasTransformedInputs)
                {
                    Signal[] ret = new Signal[transformedInputs.Count];
                    for(int i = 0; i < ret.Length; i++)
                        ret[i] = Std.Cosine(port.Context, transformedInputs[i]) / Std.Sine(port.Context, transformedInputs[i]);
                    return ret;
                });

            return theorems;
        }
    }
}
