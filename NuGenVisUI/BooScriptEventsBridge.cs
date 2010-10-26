using System.Reflection;
using System.Windows.Forms;

namespace Genetibase.VisUI.Scripting
{
    class BooScriptEventsBridge
    {
        object obj;
        MethodInfo handlerMethod;

        /// <summary>
        /// Initializes a new instance of the BooScriptEventsBridge class.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="handlerMethod"></param>
        public BooScriptEventsBridge(object obj, MethodInfo handlerMethod)
        {
            this.obj = obj;
            this.handlerMethod = handlerMethod;
        }

        public void MouseHandler(object obj, MouseEventArgs args)
        {
            handlerMethod.Invoke(this.obj, new object[] { obj, args });
        }
    }
}