using Microsoft.DirectX;
using OcTree;

namespace Test
{
    class TestObj : OcTreeItem
    {
        public TestObj(Vector3 position, Vector3 dimensions)
            : base(position, dimensions)
        { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OcTree<TestObj> tree = new OcTree<TestObj>(20, new Vector3(0, 0, 0));
            tree.Insert(new TestObj(Vector3.Empty, new Vector3(1, 1, 1)));
            tree.Insert(new TestObj(new Vector3(5, 0, 0), new Vector3(1, 1, 1)));
        }
    }
}
