/* $RCSfile$    
* $Author: egonw $    
* $Date: 2006-07-14 14:09:39 +0200 (Fri, 14 Jul 2006) $    
* $Revision: 6672 $
* 
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK
{
    /// <summary> A set of reactions, for example those taking part in a reaction.
    /// 
    /// To retrieve the reactions from the set, there are two options:
    /// 
    /// <pre>
    /// Reaction[] reactions = setOfReactions.getReactions();
    /// for (int i=0; i < reactions.length; i++) {
    /// Reaction reaction = reactions[i];
    /// }
    /// </pre>
    /// 
    /// and
    /// 
    /// <pre>
    /// for (int i=0; i < setOfReactions.getReactionCount(); i++) {
    /// Reaction reaction = setOfReactions.getReaction(i);
    /// }
    /// </pre>
    /// 
    /// </summary>
    /// <cdk.module>  data </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  reaction </cdk.keyword>
    /// <cdk.keyword>  reaction </cdk.keyword>
    [Serializable]
    public class SetOfReactions : ChemObject, ISetOfReactions, ICloneable
    {
        /// <summary>  Returns the array of Reactions of this container.
        /// 
        /// </summary>
        /// <returns>    The array of Reactions of this container 
        /// </returns>
        virtual public IReaction[] Reactions
        {
            get
            {
                Reaction[] result = new Reaction[reactionCount];
                for (int i = 0; i < reactionCount; i++)
                {
                    result[i] = (Reaction)reactions[i];
                }
                return result;
            }

        }
        /// <summary> Returns the number of Reactions in this Container.
        /// 
        /// </summary>
        /// <returns>     The number of Reactions in this Container
        /// </returns>
        virtual public int ReactionCount
        {
            get
            {
                return this.reactionCount;
            }

        }

        /// <summary> Determines if a de-serialized object is compatible with this class.
        /// 
        /// This value must only be changed if and only if the new version
        /// of this class is imcompatible with the old version. See Sun docs
        /// for <a href=http://java.sun.com/products/jdk/1.1/docs/guide
        /// /serialization/spec/version.doc.html>details</a>.
        /// </summary>
        private const long serialVersionUID = 1528749911904585204L;

        /// <summary>  Array of Reactions.</summary>
        private IReaction[] reactions;

        /// <summary>  Number of Reactions contained by this container.</summary>
        private int reactionCount;

        /// <summary>  Amount by which the Reactions array grows when elements are added and
        /// the array is not large enough for that. 
        /// </summary>
        private int growArraySize = 5;


        /// <summary>  Constructs an empty SetOfReactions.</summary>
        public SetOfReactions()
        {
            reactionCount = 0;
            reactions = new Reaction[growArraySize];
        }



        /// <summary>  Adds an reaction to this container.
        /// 
        /// </summary>
        /// <param name="reaction"> The reaction to be added to this container 
        /// </param>
        public virtual void addReaction(IReaction reaction)
        {
            if (reactionCount + 1 >= reactions.Length)
                growReactionArray();
            reactions[reactionCount] = reaction;
            reactionCount++;
            notifyChanged();
        }


        /// <summary>  
        /// Returns the Reaction at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the Reaction to be returned
        /// </param>
        /// <returns>         The Reaction at position <code>number</code>
        /// </returns>
        public virtual IReaction getReaction(int number)
        {
            return (Reaction)reactions[number];
        }

        /// <summary>  Grows the reaction array by a given size.
        /// 
        /// </summary>
        /// <seealso cref="growArraySize">
        /// </seealso>
        private void growReactionArray()
        {
            growArraySize = reactions.Length;
            Reaction[] newreactions = new Reaction[reactions.Length + growArraySize];
            Array.Copy(reactions, 0, newreactions, 0, reactions.Length);
            reactions = newreactions;
        }

        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(32);
            buffer.Append("SetOfReactions(");
            buffer.Append(this.GetHashCode());
            buffer.Append(", R=").Append(ReactionCount).Append(", ");
            IReaction[] reactions = Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                buffer.Append(reactions[i].ToString());
            }
            buffer.Append(')');
            return buffer.ToString();
        }

        /// <summary> Clones this <code>SetOfReactions</code> and the contained <code>Reaction</code>s
        /// too.
        /// 
        /// </summary>
        /// <returns>  The cloned SetOfReactions
        /// </returns>
        public override System.Object Clone()
        {
            SetOfReactions clone = (SetOfReactions)base.Clone();
            // clone the reactions
            clone.reactionCount = this.reactionCount;
            clone.reactions = new Reaction[clone.reactionCount];
            for (int f = 0; f < clone.reactionCount; f++)
            {
                clone.reactions[f] = (Reaction)((Reaction)reactions[f]).Clone();
            }
            return clone;
        }

        /// <summary> Removes all Reactions from this container.</summary>
        public virtual void removeAllReactions()
        {
            for (int pos = this.reactionCount - 1; pos >= 0; pos--)
            {
                this.reactions[pos] = null;
            }
            this.reactionCount = 0;
            notifyChanged();
        }
    }
}