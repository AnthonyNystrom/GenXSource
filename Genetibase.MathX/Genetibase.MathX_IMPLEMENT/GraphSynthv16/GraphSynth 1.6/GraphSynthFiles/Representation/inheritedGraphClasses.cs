using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GraphSynth.Representation
{
    /* Here is an example of classes that inherits from arc and node for a particular application.
     * Many consider edge and arc, and vertex and node to be synonymous but I prefer to think of 
     * edge and vertices as arcs and nodes with spatial information. 
     * As you will notice from the declarations below, we can add any number of new fields, properties
     * or functions. However, we must include in our inherited classes two things:
     *      -the constructors that invoke the base (node and arc) constructors,
     *      -the xmlstatements at the beginning of these classes as well as at the end for the 
     *       base classes. */
    public class edge : arc
    {
        public double slope
        {
            get
            {
                try /* this is in case the connecting nodes are not vertices */
                {
                    vertex v1 = (vertex)this.From;
                    vertex v2 = (vertex)this.To;
                    return (v2.y - v1.y) / (v2.x - v1.x);
                }
                catch
                { return 0.0; }
            }
        }
        public double length
        {
            get
            {
                try /* this is in case the connecting nodes are not vertices */
                {
                    vertex v1 = (vertex)this.From;
                    vertex v2 = (vertex)this.To;
                    return Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y));
                }
                catch
                { return 0.0; }
            }
        }


        public edge(string name, node from, node to) : base(name, from, to) { }
        public edge() : base() { }

        public override arc copy()
        {
            edge copyOfEdge = new edge();
            base.copy(copyOfEdge);
            copyOfEdge.name = this.name;

            return copyOfEdge;
        }
    }

    public class vertex : node
    {
        public double x;
        public double y;
        public double z;

        public vertex(string name) : base(name) { }
        public vertex() : base() { }

        public override node copy()
        {
            vertex copyOfVertex = new vertex(this.name);
            base.copy(copyOfVertex);
            copyOfVertex.x = this.x;
            copyOfVertex.y = this.y;
            copyOfVertex.z = this.z;

            return copyOfVertex;
        }

    }

    /* in order to "show" xml serialization that these are inherited from node and arc,
     * we need to add these two XmlInclude lines to the declaration of node and arc.
     * If you are developing your own inherited classes, remember to add this. NOTE: there
     * sometimes can be a problem with compilation if the compilers sees this before
     * it sees node and arc (or something like that - might also be interference with 
     * ruleNode and ruleArc. */
    [XmlInclude(typeof(GraphSynth.Representation.vertex))]
    public partial class node { }
    [XmlInclude(typeof(GraphSynth.Representation.edge))]
    public partial class arc { }
}
