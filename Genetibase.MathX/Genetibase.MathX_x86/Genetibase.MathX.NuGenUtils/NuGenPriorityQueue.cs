using System;
using System.Collections;


namespace Genetibase.MathX.NuGenUtils
{

    public class NuGenPriorityQueue
    {

        public void Insert(IComparable item)
        {
            data.Add(item);
            size = data.Count;

            int i = size - 1;
            int j = size / 2 - 1;

            while (i > 0 && ((IComparable)data[i]).CompareTo((IComparable)data[j]) < 0)
            {
                object tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;

                i = j;
                j = (i - 1) / 2;
            }
        }

        public IComparable Min
        {
            get
            {
                return (size > 0) ? (IComparable)data[0] : null;
            }
        }

        public IComparable RemoveMin()
        {

            if (size == 0) return null;

            IComparable result = (IComparable)data[0];
            data[0] = data[size - 1];
            data.RemoveAt(size - 1);
            size--;

            int i = 0;
            int j = 1;

            while (j < size && ((IComparable)data[i]).CompareTo((IComparable)data[j]) > 0)
            {
                object tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;

                i = j;
                j = 2 * i + 1;
            }

            return result;
        }

        public int Size
        {
            get
            {
                return size;
            }
        }

        protected int size = 0;
        protected ArrayList data = new ArrayList();

    }

}
