using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TextureBlending
{
	
	public struct Vertex
	{
		Vector3 pos;
		Vector3 nor;		
		float tu,tv;
		float b1,b2,b3,b4;		
		public Vertex(Vector3 p,Vector3 n,float u,float v,float B1,float B2,float B3, float B4, bool normalize)
		{
			pos = p;nor = n;tu = u;tv = v;b1=B1; b2=B2; b3=B3;b4 = B4;
			float total = b1 + b2 + b3 + b4;
			if ( normalize)
			{
				// The total of all belnd values must be 1
				b1 /= total;
				b2 /= total;
				b3 /= total;
				b4 /= total;
			}
		}
		public Vector3 Normal
		{
			get { return nor; }
			set { nor = value; }
		}
		public Vector3 Position
		{
			get { return pos; }
			set { pos = value; }
		}
		public static VertexFormats Format = VertexFormats.Position | VertexFormats.Normal | VertexFormats.Texture0 | VertexFormats.Texture1; 
	}
	
	public class Terrain
	{
		private Device device;
		private VertexDeclaration decl;
		private VertexBuffer vb;
		private IndexBuffer ib;
		private int numvertices,numindices,numtriangles;
		float[,] heights;
		float dx,dy;
		private bool Created = false;
		private int numWidth,numHeight;
		public static float size = 150f;
		private Vector3[] normallist;
		public bool renderNormals = false;

		public unsafe Terrain( Device d, float Min, float Max)
		{			
			device = d;
			// Load the heightmap
			Bitmap b = new Bitmap(@"..\..\heightmap.bmp");
			// Store the number of pixels 
			int numWidth = b.Width; int numHeight = b.Height;
			// Create an array of floats
			heights  = new float[numWidth,numHeight];			
			
			// Lock the data from the bitmap so it can be accessed unsafely
			BitmapData data = b.LockBits(new Rectangle(0,0,numWidth,numHeight),ImageLockMode.ReadOnly,PixelFormat.Format24bppRgb);
			// Assigh the address of the first pixel to the pointer
			byte * p = (byte * ) data.Scan0;
			byte lowest = 255; byte highest = 0;
			for ( int i = 0; i < numWidth; i ++)
			{
				for ( int j = 0; j < numHeight ; j ++)
				{
					if ( *p < lowest ) lowest = *p;
					if ( *p > highest ) highest = *p;
					p += 3;
				}
			}
			p = (byte * ) data.Scan0;
			for ( int i = 0; i < numWidth; i ++)
			{
				for ( int j = 0; j < numHeight ; j ++)
				{
					heights[i,j] = (float) (*p - lowest) / ( (float) (highest - lowest) ) * (Max - Min) + Min;
					p += 3;
				}
			}
			b.UnlockBits(data);

			numvertices = numWidth*numHeight;
			numindices = 6*(numWidth-1)*(numHeight-1);
			numtriangles = 2*(numWidth-1)*(numHeight-1);

			// Create on array of vertices
			Vertex[] verts = new Vertex[numvertices];
			// Create on array of indices
			int[] ind = new int[numindices];

			int n = 0;int x = 0;
			dx = size / (float) numWidth;
			dy = size / (float) numWidth;

			for ( int i = 0; i < numHeight; i ++)
			{
				for ( int j = 0; j < numWidth; j ++)
				{					
					// Calculate the blend value by dividing the height by the maximal height
					float blend = (heights[j,i]-Min) / (Max - Min) ;
					// Normals will be calculated after this so assign a upward normal for now
					verts[i*numWidth+j] = new Vertex( new Vector3((float)j*dx -size/2f,heights[j,i],(float)i*dy -size/2f), new Vector3(0,1,0),(float) j / (float) numWidth*size/16f,(float) i / (float) numHeight*size/16f,1 - blend,blend,0,0,true);  
				}
			}
			for ( int i = 1; i < numHeight-1; i ++)
			{
				for ( int j = 1; j < numWidth-1; j ++)
				{
					// Calculate the real normals by using the cross product of the vertex' neighbours
					Vector3 X = Vector3.Subtract(verts[i*numWidth+j+1].Position,verts[i*numWidth+j-1].Position);
					Vector3 Z = Vector3.Subtract(verts[(i+1)*numWidth+j].Position,verts[(i-1)*numWidth+j].Position);
					Vector3 Normal = Vector3.Cross(Z,X);
					Normal.Normalize();
					verts[i*numWidth+j].Normal = Normal;
				}
			}

			// Create on array of Vector3's for each vertex storing the position and the normal added to the position so the normals can be rendered 
			normallist = new Vector3[numvertices*2];
			for ( int i = 0; i < numvertices; i ++)
			{
				normallist[2*i] = verts[i].Position;
				normallist[2*i+1] = Vector3.Add( verts[i].Position , Vector3.Multiply(verts[i].Normal,3) );
			}
			
			// Fill the array of indices
			for ( int i = 0; i < numWidth-1; i ++)
			{
				for ( int j = 0; j < numHeight-1; j ++)
				{
					x = i * numWidth + j;
					ind[n++] = x; 
					ind[n++] = x+1;
					ind[n++] = x+numWidth+1;
					ind[n++] = x;
					ind[n++] = x+numWidth;
					ind[n++] = x+numWidth+1;
				}
			}

			// Create the vertex- and indexbuffer and set their data
			vb = new VertexBuffer(typeof(Vertex),numvertices,device,Usage.None,Vertex.Format,Pool.Default);
			vb.SetData(verts,0,0);
			ib = new IndexBuffer(typeof(int),numindices,device,0,0);
			ib.SetData(ind,0,0);
			
			// Create the vertexdeclaration
			VertexElement[] v = new VertexElement[] { new VertexElement(0,0,DeclarationType.Float3,DeclarationMethod.Default,DeclarationUsage.Position,0), 
														new VertexElement(0,12,DeclarationType.Float3,DeclarationMethod.Default,DeclarationUsage.Normal,0),
														new VertexElement(0,24,DeclarationType.Float2,DeclarationMethod.Default,DeclarationUsage.TextureCoordinate,0),
														new VertexElement(0,32,DeclarationType.Float4,DeclarationMethod.Default,DeclarationUsage.TextureCoordinate,1), 
														VertexElement.VertexDeclarationEnd}; 
			decl = new VertexDeclaration(device,v);	
	
		}

		public void Draw()
		{
			// Set the VertexDeclaration, StreamSource and Indices 
			device.VertexDeclaration = decl;	
			device.SetStreamSource(0,vb,0);
			device.Indices = ib;
			
			// Draw the terrain
			device.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,numvertices,0,numtriangles);
						
			if ( ! renderNormals ) return;
			// Render the normal list
			using ( Line l = new Line(device) )
			{
				l.Antialias = true;
				l.Width = 2;
				l.Begin(); 
				for ( int i = 0; i < normallist.Length; i += 2) 
					l.DrawTransform(new Vector3[] { normallist[i],normallist[i+1]} ,Form1.viewMatrix* Form1.projMatrix,Color.Red);
				l.End();
			}
		}


	}
}
