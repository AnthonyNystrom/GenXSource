/* $RCSfile$    
* $Author: egonw $    
* $Date: 2006-05-02 09:46:48 +0200 (Tue, 02 May 2006) $    
* $Revision: 6119 $
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

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A set of reactions, for example those taking part in a reaction.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  reaction </cdk.keyword>
    public interface ISetOfReactions : IChemObject
    {
        /// <summary> Returns the array of Reactions of this container.
        /// 
        /// </summary>
        /// <returns>    The array of Reactions of this container 
        /// </returns>
        IReaction[] Reactions
        {
            get;
        }

        /// <summary> Returns the number of Reactions in this Container.
        /// 
        /// </summary>
        /// <returns>     The number of Reactions in this Container
        /// </returns>
        int ReactionCount
        {
            get;
        }

        /// <summary> Adds an reaction to this container.
        /// 
        /// </summary>
        /// <param name="reaction"> The reaction to be added to this container 
        /// </param>
        void addReaction(IReaction reaction);

        /// <summary> Returns the Reaction at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the Reaction to be returned
        /// </param>
        /// <returns>         The Reaction at position <code>number</code>
        /// </returns>
        IReaction getReaction(int number);

        /// <summary> Removes all reactions from this set.</summary>
        void removeAllReactions();
    }
}