/* $RCSfile$
* $Author: kaihartmann $
* $Date: 2006-06-06 10:55:35 +0200 (Tue, 06 Jun 2006) $
* $Revision: 6346 $
*
* Copyright (C) 2002-2006  The Jmol Development Team
*
* Contact: cdk-devel@lists.sourceforge.net
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.IO.Setting;
using Org.OpenScience.CDK.IO.Formats;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.IO.Listener;

namespace Org.OpenScience.CDK.IO
{
    /// <summary> Abstract class that ChemObjectReader's can implement to have it
    /// take care of basic stuff, like managing the ReaderListeners.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    public abstract class DefaultChemObjectReader : IChemObjectReader
    {
        virtual public IOSetting[] IOSettings
        {
            get
            {
                return new IOSetting[0];
            }

        }
        public abstract IResourceFormat Format { get;}

        /// <summary> An event to be sent to listeners when a frame is read.</summary>
        private ReaderEvent frameReadEvent = null;

        /// <summary> Holder of reader event listeners.</summary>
        private System.Collections.ArrayList listenerList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

        public virtual void addChemObjectIOListener(IChemObjectIOListener listener)
        {
            listenerList.Add(listener);
        }

        public virtual void removeChemObjectIOListener(IChemObjectIOListener listener)
        {
            listenerList.Remove(listener);
        }

        /* Extra convenience methods */

        /// <summary> Sends a frame read event to the registered ReaderListeners.</summary>
        protected internal virtual void fireFrameRead()
        {
            for (int i = 0; i < listenerList.Count; ++i)
            {
                IChemObjectIOListener listener = (IChemObjectIOListener)listenerList[i];
                if (listener is IReaderListener)
                {
                    // Lazily create the event:
                    if (frameReadEvent == null)
                    {
                        frameReadEvent = new ReaderEvent(this);
                    }
                    ((IReaderListener)listener).frameRead(this, frameReadEvent);
                }
            }
        }

        protected internal virtual void fireIOSettingQuestion(IOSetting setting)
        {
            for (int i = 0; i < listenerList.Count; ++i)
            {
                IChemObjectIOListener listener = (IChemObjectIOListener)listenerList[i];
                listener.processIOSettingQuestion(setting);
            }
        }
        public abstract IChemObject read(IChemObject param1);
        public abstract void setReader(System.IO.Stream param1);
        public abstract void setReader(System.IO.StreamReader param1);
        public abstract bool accepts(System.Type param1);
        public abstract void close();
    }
}