//
// In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
// creates "support classes" that duplicate the original functionality.  
//
// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;
using System.Collections;

namespace CSGraphT
{
    /// <summary>
    /// Contains conversion support elements such as classes, interfaces and static methods.
    /// </summary>
    public class SupportClass
    {
        /// <summary>
        /// SupportClass for the HashSet class.
        /// </summary>
        [Serializable]
        public class HashSetSupport : Hashtable, SetSupport
        {
            public HashSetSupport()
            {
            }

            public HashSetSupport(System.Collections.ICollection c)
            {
                this.AddAll(c);
            }

            public HashSetSupport(int capacity)
                : base(capacity)
            {
            }

            /// <summary>
            /// Adds a new element to the ArrayList if it is not already present.
            /// </summary>		
            /// <param name="obj">Element to insert to the ArrayList.</param>
            /// <returns>Returns true if the new element was inserted, false otherwise.</returns>

            /// <summary>
            /// Adds all the elements of the specified collection that are not present to the list.
            /// </summary>
            /// <param name="c">Collection where the new elements will be added</param>
            /// <returns>Returns true if at least one element was added, false otherwise.</returns>
            public bool AddAll(System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
                bool added = false;

                while (e.MoveNext() == true)
                {
                    if (this.AddItem(e.Current) == true)
                        added = true;
                }

                return added;
            }

            /// <summary>
            /// Returns a copy of the HashSet instance.
            /// </summary>		
            /// <returns>Returns a shallow copy of the current HashSet.</returns>
            public override System.Object Clone()
            {
                return base.MemberwiseClone();
            }

            #region IList Members

            public int IndexOf(object value)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void Insert(int index, object value)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void RemoveAt(int index)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public object this[int index]
            {
                get
                {
                    IEnumerator iter = Values.GetEnumerator();
                    iter.MoveNext();
                    return iter.Current;
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public int Add(object obj)
            {
                bool inserted;

                if ((inserted = this.Contains(obj)) == false)
                {
                    base.Add(obj, obj);
                }

                return !inserted ? 1 : 0;
            }

            #endregion

            #region SetSupport Members

            public bool AddItem(object obj)
            {
                Add(obj);
                return true;
            }

            #endregion
        }


        /*******************************/
        /// <summary>
        /// Represents a collection ob objects that contains no duplicate elements.
        /// </summary>	
        public interface SetSupport : System.Collections.ICollection, System.Collections.IList
        {
            /// <summary>
            /// Adds a new element to the Collection if it is not already present.
            /// </summary>
            /// <param name="obj">The object to add to the collection.</param>
            /// <returns>Returns true if the object was added to the collection, otherwise false.</returns>
            bool AddItem(System.Object obj);

            /// <summary>
            /// Adds all the elements of the specified collection to the Set.
            /// </summary>
            /// <param name="c">Collection of objects to add.</param>
            /// <returns>true</returns>
            bool AddAll(System.Collections.ICollection c);
        }


        /*******************************/
        /// <summary>
        /// SupportClass for the Stack class.
        /// </summary>
        public class StackSupport
        {
            /// <summary>
            /// Removes the element at the top of the stack and returns it.
            /// </summary>
            /// <param name="stack">The stack where the element at the top will be returned and removed.</param>
            /// <returns>The element at the top of the stack.</returns>
            public static System.Object Pop(System.Collections.ArrayList stack)
            {
                System.Object obj = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                return obj;
            }
        }


        /*******************************/
        /// <summary>
        /// This class has static methods to manage collections.
        /// </summary>
        public class CollectionsSupport
        {
            /// <summary>
            /// Copies the IList to other IList.
            /// </summary>
            /// <param name="SourceList">IList source.</param>
            /// <param name="TargetList">IList target.</param>
            public static void Copy(System.Collections.IList SourceList, System.Collections.IList TargetList)
            {
                for (int i = 0; i < SourceList.Count; i++)
                    TargetList[i] = SourceList[i];
            }

            /// <summary>
            /// Replaces the elements of the specified list with the specified element.
            /// </summary>
            /// <param name="List">The list to be filled with the specified element.</param>
            /// <param name="Element">The element with which to fill the specified list.</param>
            public static void Fill(System.Collections.IList List, System.Object Element)
            {
                for (int i = 0; i < List.Count; i++)
                    List[i] = Element;
            }

            /// <summary>
            /// This class implements System.Collections.IComparer and is used for Comparing two String objects by evaluating 
            /// the numeric values of the corresponding Char objects in each string.
            /// </summary>
            class CompareCharValues : System.Collections.IComparer
            {
                public int Compare(System.Object x, System.Object y)
                {
                    return System.String.CompareOrdinal((System.String)x, (System.String)y);
                }
            }

            /// <summary>
            /// Obtain the maximum element of the given collection with the specified comparator.
            /// </summary>
            /// <param name="Collection">Collection from which the maximum value will be obtained.</param>
            /// <param name="Comparator">The comparator with which to determine the maximum element.</param>
            /// <returns></returns>
            public static System.Object Max(System.Collections.ICollection Collection, System.Collections.IComparer Comparator)
            {
                System.Collections.ArrayList tempArrayList;

                if (((System.Collections.ArrayList)Collection).IsReadOnly)
                    throw new System.NotSupportedException();

                if ((Comparator == null) || (Comparator is System.Collections.Comparer))
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort();
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[Collection.Count - 1];
                }
                else
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort(Comparator);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[Collection.Count - 1];
                }
            }

            /// <summary>
            /// Obtain the minimum element of the given collection with the specified comparator.
            /// </summary>
            /// <param name="Collection">Collection from which the minimum value will be obtained.</param>
            /// <param name="Comparator">The comparator with which to determine the minimum element.</param>
            /// <returns></returns>
            public static System.Object Min(System.Collections.ICollection Collection, System.Collections.IComparer Comparator)
            {
                System.Collections.ArrayList tempArrayList;

                if (((System.Collections.ArrayList)Collection).IsReadOnly)
                    throw new System.NotSupportedException();

                if ((Comparator == null) || (Comparator is System.Collections.Comparer))
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort();
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[0];
                }
                else
                {
                    try
                    {
                        tempArrayList = new System.Collections.ArrayList(Collection);
                        tempArrayList.Sort(Comparator);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                    return (System.Object)tempArrayList[0];
                }
            }


            /// <summary>
            /// Sorts an IList collections
            /// </summary>
            /// <param name="list">The System.Collections.IList instance that will be sorted</param>
            /// <param name="Comparator">The Comparator criteria, null to use natural comparator.</param>
            public static void Sort(System.Collections.IList list, System.Collections.IComparer Comparator)
            {
                if (((System.Collections.ArrayList)list).IsReadOnly)
                    throw new System.NotSupportedException();

                if ((Comparator == null) || (Comparator is System.Collections.Comparer))
                {
                    try
                    {
                        ((System.Collections.ArrayList)list).Sort();
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                }
                else
                {
                    try
                    {
                        ((System.Collections.ArrayList)list).Sort(Comparator);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        throw new System.InvalidCastException(e.Message);
                    }
                }
            }

            /// <summary>
            /// Shuffles the list randomly.
            /// </summary>
            /// <param name="List">The list to be shuffled.</param>
            public static void Shuffle(System.Collections.IList List)
            {
                System.Random RandomList = new System.Random(unchecked((int)System.DateTime.Now.Ticks));
                Shuffle(List, RandomList);
            }

            /// <summary>
            /// Shuffles the list randomly.
            /// </summary>
            /// <param name="List">The list to be shuffled.</param>
            /// <param name="RandomList">The random to use to shuffle the list.</param>
            public static void Shuffle(System.Collections.IList List, System.Random RandomList)
            {
                System.Object source = null;
                int target = 0;

                for (int i = 0; i < List.Count; i++)
                {
                    target = RandomList.Next(List.Count);
                    source = (System.Object)List[i];
                    List[i] = List[target];
                    List[target] = source;
                }
            }
        }


        /*******************************/
        /// <summary>
        /// Writes the exception stack trace to the received stream
        /// </summary>
        /// <param name="throwable">Exception to obtain information from</param>
        /// <param name="stream">Output sream used to write to</param>
        public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }

        /*******************************/
        /// <summary>
        /// Recieves a form and an integer value representing the operation to perform when the closing 
        /// event is fired.
        /// </summary>
        /// <param name="form">The form that fire the event.</param>
        /// <param name="operation">The operation to do while the form is closing.</param>
        public static void CloseOperation(System.Windows.Forms.Form form, int operation)
        {
            switch (operation)
            {
                case 0:
                    break;
                case 1:
                    form.Hide();
                    break;
                case 2:
                    form.Dispose();
                    break;
                case 3:
                    form.Dispose();
                    System.Windows.Forms.Application.Exit();
                    break;
            }
        }


        /*******************************/
        /// <summary>
        /// This class provides functionality not found in .NET collection-related interfaces.
        /// </summary>
        public class ICollectionSupport
        {
            /// <summary>
            /// Adds a new element to the specified collection.
            /// </summary>
            /// <param name="c">Collection where the new element will be added.</param>
            /// <param name="obj">Object to add.</param>
            /// <returns>true</returns>
            public static bool Add(System.Collections.ICollection c, System.Object obj)
            {
                bool added = false;
                //Reflection. Invoke either the "add" or "Add" method.
                System.Reflection.MethodInfo method;
                try
                {
                    //Get the "add" method for proprietary classes
                    method = c.GetType().GetMethod("Add");
                    if (method == null)
                        method = c.GetType().GetMethod("add");
                    int index = (int)method.Invoke(c, new System.Object[] { obj });
                    if (index >= 0)
                        added = true;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                return added;
            }

            /// <summary>
            /// Adds all of the elements of the "c" collection to the "target" collection.
            /// </summary>
            /// <param name="target">Collection where the new elements will be added.</param>
            /// <param name="c">Collection whose elements will be added.</param>
            /// <returns>Returns true if at least one element was added, false otherwise.</returns>
            public static bool AddAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = new System.Collections.ArrayList(c).GetEnumerator();
                bool added = false;

                //Reflection. Invoke "addAll" method for proprietary classes
                System.Reflection.MethodInfo method;
                try
                {
                    method = target.GetType().GetMethod("addAll");

                    if (method != null)
                        added = (bool)method.Invoke(target, new System.Object[] { c });
                    else
                    {
                        method = target.GetType().GetMethod("Add");
                        while (e.MoveNext() == true)
                        {
                            bool tempBAdded = (int)method.Invoke(target, new System.Object[] { e.Current }) >= 0;
                            added = added ? added : tempBAdded;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                return added;
            }

            /// <summary>
            /// Removes all the elements from the collection.
            /// </summary>
            /// <param name="c">The collection to remove elements.</param>
            public static void Clear(System.Collections.ICollection c)
            {
                //Reflection. Invoke "Clear" method or "clear" method for proprietary classes
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("Clear");

                    if (method == null)
                        method = c.GetType().GetMethod("clear");

                    method.Invoke(c, new System.Object[] { });
                }
                catch (System.Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Determines whether the collection contains the specified element.
            /// </summary>
            /// <param name="c">The collection to check.</param>
            /// <param name="obj">The object to locate in the collection.</param>
            /// <returns>true if the element is in the collection.</returns>
            public static bool Contains(System.Collections.ICollection c, System.Object obj)
            {
                bool contains = false;

                //Reflection. Invoke "contains" method for proprietary classes
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("Contains");

                    if (method == null)
                        method = c.GetType().GetMethod("contains");

                    contains = (bool)method.Invoke(c, new System.Object[] { obj });
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                return contains;
            }

            /// <summary>
            /// Determines whether the collection contains all the elements in the specified collection.
            /// </summary>
            /// <param name="target">The collection to check.</param>
            /// <param name="c">Collection whose elements would be checked for containment.</param>
            /// <returns>true id the target collection contains all the elements of the specified collection.</returns>
            public static bool ContainsAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = c.GetEnumerator();

                bool contains = false;

                //Reflection. Invoke "containsAll" method for proprietary classes or "Contains" method for each element in the collection
                System.Reflection.MethodInfo method;
                try
                {
                    method = target.GetType().GetMethod("containsAll");

                    if (method != null)
                        contains = (bool)method.Invoke(target, new Object[] { c });
                    else
                    {
                        method = target.GetType().GetMethod("Contains");
                        while (e.MoveNext() == true)
                        {
                            if ((contains = (bool)method.Invoke(target, new Object[] { e.Current })) == false)
                                break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                return contains;
            }

            /// <summary>
            /// Removes the specified element from the collection.
            /// </summary>
            /// <param name="c">The collection where the element will be removed.</param>
            /// <param name="obj">The element to remove from the collection.</param>
            public static bool Remove(System.Collections.ICollection c, System.Object obj)
            {
                bool changed = false;

                //Reflection. Invoke "remove" method for proprietary classes or "Remove" method
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("remove");

                    if (method != null)
                        method.Invoke(c, new System.Object[] { obj });
                    else
                    {
                        method = c.GetType().GetMethod("Contains");
                        changed = (bool)method.Invoke(c, new System.Object[] { obj });
                        method = c.GetType().GetMethod("Remove");
                        method.Invoke(c, new System.Object[] { obj });
                    }
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                return changed;
            }

            /// <summary>
            /// Removes all the elements from the specified collection that are contained in the target collection.
            /// </summary>
            /// <param name="target">Collection where the elements will be removed.</param>
            /// <param name="c">Elements to remove from the target collection.</param>
            /// <returns>true</returns>
            public static bool RemoveAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.ArrayList al = ToArrayList(c);
                System.Collections.IEnumerator e = al.GetEnumerator();

                //Reflection. Invoke "removeAll" method for proprietary classes or "Remove" for each element in the collection
                System.Reflection.MethodInfo method;
                try
                {
                    method = target.GetType().GetMethod("removeAll");

                    if (method != null)
                        method.Invoke(target, new System.Object[] { al });
                    else
                    {
                        method = target.GetType().GetMethod("Remove");
                        System.Reflection.MethodInfo methodContains = target.GetType().GetMethod("Contains");

                        while (e.MoveNext() == true)
                        {
                            while ((bool)methodContains.Invoke(target, new System.Object[] { e.Current }) == true)
                                method.Invoke(target, new System.Object[] { e.Current });
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                return true;
            }

            /// <summary>
            /// Retains the elements in the target collection that are contained in the specified collection
            /// </summary>
            /// <param name="target">Collection where the elements will be removed.</param>
            /// <param name="c">Elements to be retained in the target collection.</param>
            /// <returns>true</returns>
            public static bool RetainAll(System.Collections.ICollection target, System.Collections.ICollection c)
            {
                System.Collections.IEnumerator e = new System.Collections.ArrayList(target).GetEnumerator();
                System.Collections.ArrayList al = new System.Collections.ArrayList(c);

                //Reflection. Invoke "retainAll" method for proprietary classes or "Remove" for each element in the collection
                System.Reflection.MethodInfo method;
                try
                {
                    method = c.GetType().GetMethod("retainAll");

                    if (method != null)
                        method.Invoke(target, new System.Object[] { c });
                    else
                    {
                        method = c.GetType().GetMethod("Remove");

                        while (e.MoveNext() == true)
                        {
                            if (al.Contains(e.Current) == false)
                                method.Invoke(target, new System.Object[] { e.Current });
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                return true;
            }

            /// <summary>
            /// Returns an array containing all the elements of the collection.
            /// </summary>
            /// <returns>The array containing all the elements of the collection.</returns>
            public static System.Object[] ToArray(System.Collections.ICollection c)
            {
                int index = 0;
                System.Object[] objects = new System.Object[c.Count];
                System.Collections.IEnumerator e = c.GetEnumerator();

                while (e.MoveNext())
                    objects[index++] = e.Current;

                return objects;
            }

            /// <summary>
            /// Obtains an array containing all the elements of the collection.
            /// </summary>
            /// <param name="objects">The array into which the elements of the collection will be stored.</param>
            /// <returns>The array containing all the elements of the collection.</returns>
            public static System.Object[] ToArray(System.Collections.ICollection c, System.Object[] objects)
            {
                int index = 0;

                System.Type type = objects.GetType().GetElementType();
                System.Object[] objs = (System.Object[])Array.CreateInstance(type, c.Count);

                System.Collections.IEnumerator e = c.GetEnumerator();

                while (e.MoveNext())
                    objs[index++] = e.Current;

                //If objects is smaller than c then do not return the new array in the parameter
                if (objects.Length >= c.Count)
                    objs.CopyTo(objects, 0);

                return objs;
            }

            /// <summary>
            /// Converts an ICollection instance to an ArrayList instance.
            /// </summary>
            /// <param name="c">The ICollection instance to be converted.</param>
            /// <returns>An ArrayList instance in which its elements are the elements of the ICollection instance.</returns>
            public static System.Collections.ArrayList ToArrayList(System.Collections.ICollection c)
            {
                System.Collections.ArrayList tempArrayList = new System.Collections.ArrayList();
                System.Collections.IEnumerator tempEnumerator = c.GetEnumerator();
                while (tempEnumerator.MoveNext())
                    tempArrayList.Add(tempEnumerator.Current);
                return tempArrayList;
            }
        }


        /*******************************/
        /// <summary>
        /// Converts the specified collection to its string representation.
        /// </summary>
        /// <param name="c">The collection to convert to string.</param>
        /// <returns>A string representation of the specified collection.</returns>
        public static System.String CollectionToString(System.Collections.ICollection c)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();

            if (c != null)
            {

                System.Collections.ArrayList l = new System.Collections.ArrayList(c);

                bool isDictionary = (c is System.Collections.BitArray || c is System.Collections.Hashtable || c is System.Collections.IDictionary || c is System.Collections.Specialized.NameValueCollection || (l.Count > 0 && l[0] is System.Collections.DictionaryEntry));
                for (int index = 0; index < l.Count; index++)
                {
                    if (l[index] == null)
                        s.Append("null");
                    else if (!isDictionary)
                        s.Append(l[index]);
                    else
                    {
                        isDictionary = true;
                        if (c is System.Collections.Specialized.NameValueCollection)
                            s.Append(((System.Collections.Specialized.NameValueCollection)c).GetKey(index));
                        else
                            s.Append(((System.Collections.DictionaryEntry)l[index]).Key);
                        s.Append("=");
                        if (c is System.Collections.Specialized.NameValueCollection)
                            s.Append(((System.Collections.Specialized.NameValueCollection)c).GetValues(index)[0]);
                        else
                            s.Append(((System.Collections.DictionaryEntry)l[index]).Value);

                    }
                    if (index < l.Count - 1)
                        s.Append(", ");
                }

                if (isDictionary)
                {
                    if (c is System.Collections.ArrayList)
                        isDictionary = false;
                }
                if (isDictionary)
                {
                    s.Insert(0, "{");
                    s.Append("}");
                }
                else
                {
                    s.Insert(0, "[");
                    s.Append("]");
                }
            }
            else
                s.Insert(0, "null");
            return s.ToString();
        }

        /// <summary>
        /// Tests if the specified object is a collection and converts it to its string representation.
        /// </summary>
        /// <param name="obj">The object to convert to string</param>
        /// <returns>A string representation of the specified object.</returns>
        public static System.String CollectionToString(System.Object obj)
        {
            System.String result = "";

            if (obj != null)
            {
                if (obj is System.Collections.ICollection)
                    result = CollectionToString((System.Collections.ICollection)obj);
                else
                    result = obj.ToString();
            }
            else
                result = "null";

            return result;
        }
    }

}