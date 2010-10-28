using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


/*
 * 
 *  File:		OpenGlControl.cs
 *  Class:		PopovYuri.Visualization.OpenGlControl
 *  Version:	1.0
 *  Date:		30.04.04
 *  Author:		Popov Yuri
 *  e-mail:		Yuri32@nm.ru
 * 
 */

namespace Demo
{
	/// <summary>
	/// The OpenGLControl class provides the basic functionality for user-defined 
	/// OpenGL controls.
	/// </summary>
	public class OpenGLControl : System.Windows.Forms.UserControl
	{
		#region Imports
		#region Defines from gl.h
		// Pixel types
		public const byte
			PFD_TYPE_RGBA		= 0,
			PFD_TYPE_COLORINDEX	= 1;
		
		// Layer types
		public const sbyte
			PFD_MAIN_PLANE		= 0,
			PFD_OVERLAY_PLANE	= 1,
			PFD_UNDERLAY_PLANE	= -1;

		// PIXELFORMATDESCRIPTOR flags
		public const System.UInt32
			PFD_DOUBLEBUFFER		= 0x00000001,
			PFD_STEREO				= 0x00000002,
			PFD_DRAW_TO_WINDOW		= 0x00000004,
			PFD_DRAW_TO_BITMAP		= 0x00000008,
			PFD_SUPPORT_GDI			= 0x00000010,
			PFD_SUPPORT_OPENGL		= 0x00000020,
			PFD_GENERIC_FORMAT		= 0x00000040,
			PFD_NEED_PALETTE		= 0x00000080,
			PFD_NEED_SYSTEM_PALETTE = 0x00000100,
			PFD_SWAP_EXCHANGE		= 0x00000200,
			PFD_SWAP_COPY			= 0x00000400,
			PFD_SWAP_LAYER_BUFFERS	= 0x00000800,
			PFD_GENERIC_ACCELERATED = 0x00001000,
			PFD_SUPPORT_DIRECTDRAW	= 0x00002000;
			
		/* PIXELFORMATDESCRIPTOR flags for use in ChoosePixelFormat only */
		public const System.UInt32
			PFD_DEPTH_DONTCARE          = 0x20000000,
			PFD_DOUBLEBUFFER_DONTCARE   = 0x40000000,
			PFD_STEREO_DONTCARE         = 0x80000000;
		
		/* Version */
		public const System.UInt32
			GL_VERSION_1_1 = 1;

		/* AccumOp */
		public const System.UInt32
			GL_ACCUM        = 0x0100,
			GL_LOAD         = 0x0101,
			GL_RETURN       = 0x0102,
			GL_MULT         = 0x0103,
			GL_ADD			= 0x0104;

		/* AlphaFunction */
		public const System.UInt32
			GL_NEVER	= 0x0200,
			GL_LESS		= 0x0201,
			GL_EQUAL	= 0x0202,
			GL_LEQUAL	= 0x0203,
			GL_GREATER	= 0x0204,
			GL_NOTEQUAL	= 0x0205,
			GL_GEQUAL	= 0x0206,
			GL_ALWAY	= 0x0207;

		/* AttribMask */
		public const System.UInt32
			GL_CURRENT_BIT			= 0x00000001,
			GL_POINT_BIT            = 0x00000002,
			GL_LINE_BIT				= 0x00000004,
			GL_POLYGON_BIT			= 0x00000008,
			GL_POLYGON_STIPPLE_BIT	= 0x00000010,
			GL_PIXEL_MODE_BIT		= 0x00000020,
			GL_LIGHTING_BIT         = 0x00000040,
			GL_FOG_BIT              = 0x00000080,
			GL_DEPTH_BUFFER_BIT     = 0x00000100,
			GL_ACCUM_BUFFER_BIT     = 0x00000200,
			GL_STENCIL_BUFFER_BIT   = 0x00000400,
			GL_VIEWPORT_BIT         = 0x00000800,
			GL_TRANSFORM_BIT        = 0x00001000,
			GL_ENABLE_BIT           = 0x00002000,
			GL_COLOR_BUFFER_BIT     = 0x00004000,
			GL_HINT_BIT             = 0x00008000,
			GL_EVAL_BIT             = 0x00010000,
			GL_LIST_BIT             = 0x00020000,
			GL_TEXTURE_BIT          = 0x00040000,
			GL_SCISSOR_BIT          = 0x00080000,
			GL_ALL_ATTRIB_BITS      = 0x000fffff;


		/* BeginMode */
		public const System.UInt32
			GL_POINTS           = 0x0000,
			GL_LINES            = 0x0001,
			GL_LINE_LOOP        = 0x0002,
			GL_LINE_STRIP       = 0x0003,
			GL_TRIANGLES		= 0x0004,
			GL_TRIANGLE_STRIP	= 0x0005,
			GL_TRIANGLE_FAN     = 0x0006,
			GL_QUADS            = 0x0007,
			GL_QUAD_STRIP       = 0x0008,
			GL_POLYGON          = 0x0009;

		/* BlendingFactorDest */
		public const System.UInt32
			GL_ZERO                 = 0,
			GL_ONE                  = 1,
			GL_SRC_COLOR            = 0x0300,
			GL_ONE_MINUS_SRC_COLOR  = 0x0301,
			GL_SRC_ALPHA            = 0x0302,
			GL_ONE_MINUS_SRC_ALPHA  = 0x0303,
			GL_DST_ALPHA            = 0x0304,
			GL_ONE_MINUS_DST_ALPHA	= 0x0305;

		/* BlendingFactorSrc */
		public const System.UInt32
			GL_DST_COLOR            = 0x0306,
			GL_ONE_MINUS_DST_COLOR	= 0x0307,
			GL_SRC_ALPHA_SATURATE   = 0x0308;

		/* Boolean */
		public const System.UInt32
			GL_TRUE		= 1,
			GL_FALSE    = 0;

		/* ClipPlaneName */
		public const System.UInt32
			GL_CLIP_PLANE0	= 0x3000,
			GL_CLIP_PLANE1	= 0x3001,
			GL_CLIP_PLANE2	= 0x3002,
			GL_CLIP_PLANE3	= 0x3003,
			GL_CLIP_PLANE4	= 0x3004,
			GL_CLIP_PLANE5	= 0x3005;
		
		/* DataType */
		public const System.UInt32
			GL_BYTE             = 0x1400,
			GL_UNSIGNED_BYTE	= 0x1401,
			GL_SHORT			= 0x1402,
			GL_UNSIGNED_SHORT	= 0x1403,
			GL_INT              = 0x1404,
			GL_UNSIGNED_INT     = 0x1405,
			GL_FLOAT            = 0x1406,
			GL_2_BYTES          = 0x1407,
			GL_3_BYTES          = 0x1408,
			GL_4_BYTES          = 0x1409,
			GL_DOUBLE           = 0x140A;

		/* DrawBufferMode */
		public const System.UInt32
			GL_NONE             = 0,
			GL_FRONT_LEFT       = 0x0400,
			GL_FRONT_RIGHT      = 0x0401,
			GL_BACK_LEFT        = 0x0402,
			GL_BACK_RIGHT       = 0x0403,
			GL_FRONT            = 0x0404,
			GL_BACK             = 0x0405,
			GL_LEFT             = 0x0406,
			GL_RIGHT			= 0x0407,
			GL_FRONT_AND_BACK	= 0x0408,
			GL_AUX0             = 0x0409,
			GL_AUX1             = 0x040A,
			GL_AUX2             = 0x040B,
			GL_AUX3             = 0x040C;
		
		/* ErrorCode */
		public const System.UInt32
			GL_NO_ERROR             = 0,
			GL_INVALID_ENUM         = 0x0500,
			GL_INVALID_VALUE        = 0x0501,
			GL_INVALID_OPERATION	= 0x0502,
			GL_STACK_OVERFLOW       = 0x0503,
			GL_STACK_UNDERFLOW      = 0x0504,
			GL_OUT_OF_MEMORY        = 0x0505;

		/* FeedBackMode */
		public const System.UInt32
			GL_2D               = 0x0600,
			GL_3D               = 0x0601,
			GL_3D_COLOR         = 0x0602,
			GL_3D_COLOR_TEXTURE	= 0x0603,
			GL_4D_COLOR_TEXTURE	= 0x0604;

		/* FeedBackToken */
		public const System.UInt32
			GL_PASS_THROUGH_TOKEN	= 0x0700,
			GL_POINT_TOKEN         = 0x0701,
			GL_LINE_TOKEN          = 0x0702,
			GL_POLYGON_TOKEN       = 0x0703,
			GL_BITMAP_TOKEN        = 0x0704,
			GL_DRAW_PIXEL_TOKEN    = 0x0705,
			GL_COPY_PIXEL_TOKEN    = 0x0706,
			GL_LINE_RESET_TOKEN    = 0x0707;

		/* FogMode */
		public const System.Int32
			GL_EXP	= 0x0800,
			GL_EXP2	= 0x0801;
		/* FrontFaceDirection */
		public const System.UInt32
			GL_CW	= 0x0900,
			GL_CCW	= 0x0901;

		/* GetMapTarget */
		public const System.UInt32
			GL_COEFF    = 0x0A00,
			GL_ORDER    = 0x0A01,
			GL_DOMAIN	= 0x0A02;

		/* GetTarget */
		public const System.UInt32
			GL_CURRENT_COLOR                 = 0x0B00,
			GL_CURRENT_INDEX                 = 0x0B01,
			GL_CURRENT_NORMAL                = 0x0B02,
			GL_CURRENT_TEXTURE_COORDS        = 0x0B03,
			GL_CURRENT_RASTER_COLOR          = 0x0B04,
			GL_CURRENT_RASTER_INDEX          = 0x0B05,
			GL_CURRENT_RASTER_TEXTURE_COORDS = 0x0B06,
			GL_CURRENT_RASTER_POSITION       = 0x0B07,
			GL_CURRENT_RASTER_POSITION_VALID = 0x0B08,
			GL_CURRENT_RASTER_DISTANCE       = 0x0B09,
			GL_POINT_SMOOTH                  = 0x0B10,
			GL_POINT_SIZE                    = 0x0B11,
			GL_POINT_SIZE_RANGE              = 0x0B12,
			GL_POINT_SIZE_GRANULARITY        = 0x0B13,
			GL_LINE_SMOOTH                   = 0x0B20,
			GL_LINE_WIDTH                    = 0x0B21,
			GL_LINE_WIDTH_RANGE              = 0x0B22,
			GL_LINE_WIDTH_GRANULARITY        = 0x0B23,
			GL_LINE_STIPPLE                  = 0x0B24,
			GL_LINE_STIPPLE_PATTERN          = 0x0B25,
			GL_LINE_STIPPLE_REPEAT           = 0x0B26,
			GL_LIST_MODE                     = 0x0B30,
			GL_MAX_LIST_NESTING              = 0x0B31,
			GL_LIST_BASE                     = 0x0B32,
			GL_LIST_INDEX                    = 0x0B33,
			GL_POLYGON_MODE                  = 0x0B40,
			GL_POLYGON_SMOOTH                = 0x0B41,
			GL_POLYGON_STIPPLE               = 0x0B42,
			GL_EDGE_FLAG                     = 0x0B43,
			GL_CULL_FACE                     = 0x0B44,
			GL_CULL_FACE_MODE                = 0x0B45,
			GL_FRONT_FACE                    = 0x0B46,
			GL_LIGHTING                      = 0x0B50,
			GL_LIGHT_MODEL_LOCAL_VIEWER      = 0x0B51,
			GL_LIGHT_MODEL_TWO_SIDE          = 0x0B52,
			GL_LIGHT_MODEL_AMBIENT           = 0x0B53,
			GL_SHADE_MODEL                   = 0x0B54,
			GL_COLOR_MATERIAL_FACE           = 0x0B55,
			GL_COLOR_MATERIAL_PARAMETER      = 0x0B56,
			GL_COLOR_MATERIAL                = 0x0B57,
			GL_FOG                           = 0x0B60,
			GL_FOG_INDEX                     = 0x0B61,
			GL_FOG_DENSITY                   = 0x0B62,
			GL_FOG_START                     = 0x0B63,
			GL_FOG_END                       = 0x0B64,
			GL_FOG_MODE                      = 0x0B65,
			GL_FOG_COLOR                     = 0x0B66,
			GL_DEPTH_RANGE                   = 0x0B70,
			GL_DEPTH_TEST                    = 0x0B71,
			GL_DEPTH_WRITEMASK               = 0x0B72,
			GL_DEPTH_CLEAR_VALUE             = 0x0B73,
			GL_DEPTH_FUNC                    = 0x0B74,
			GL_ACCUM_CLEAR_VALUE             = 0x0B80,
			GL_STENCIL_TEST                  = 0x0B90,
			GL_STENCIL_CLEAR_VALUE           = 0x0B91,
			GL_STENCIL_FUNC                  = 0x0B92,
			GL_STENCIL_VALUE_MASK            = 0x0B93,
			GL_STENCIL_FAIL                  = 0x0B94,
			GL_STENCIL_PASS_DEPTH_FAIL       = 0x0B95,
			GL_STENCIL_PASS_DEPTH_PASS       = 0x0B96,
			GL_STENCIL_REF                   = 0x0B97,
			GL_STENCIL_WRITEMASK             = 0x0B98,
			GL_MATRIX_MODE                   = 0x0BA0,
			GL_NORMALIZE                     = 0x0BA1,
			GL_VIEWPORT                      = 0x0BA2,
			GL_MODELVIEW_STACK_DEPTH         = 0x0BA3,
			GL_PROJECTION_STACK_DEPTH        = 0x0BA4,
			GL_TEXTURE_STACK_DEPTH           = 0x0BA5,
			GL_MODELVIEW_MATRIX              = 0x0BA6,
			GL_PROJECTION_MATRIX             = 0x0BA7,
			GL_TEXTURE_MATRIX                = 0x0BA8,
			GL_ATTRIB_STACK_DEPTH            = 0x0BB0,
			GL_CLIENT_ATTRIB_STACK_DEPTH     = 0x0BB1,
			GL_ALPHA_TEST                    = 0x0BC0,
			GL_ALPHA_TEST_FUNC               = 0x0BC1,
			GL_ALPHA_TEST_REF                = 0x0BC2,
			GL_DITHER                        = 0x0BD0,
			GL_BLEND_DST                     = 0x0BE0,
			GL_BLEND_SRC                     = 0x0BE1,
			GL_BLEND                         = 0x0BE2,
			GL_LOGIC_OP_MODE                 = 0x0BF0,
			GL_INDEX_LOGIC_OP                = 0x0BF1,
			GL_COLOR_LOGIC_OP                = 0x0BF2,
			GL_AUX_BUFFERS                   = 0x0C00,
			GL_DRAW_BUFFER                   = 0x0C01,
			GL_READ_BUFFER                   = 0x0C02,
			GL_SCISSOR_BOX                   = 0x0C10,
			GL_SCISSOR_TEST                  = 0x0C11,
			GL_INDEX_CLEAR_VALUE             = 0x0C20,
			GL_INDEX_WRITEMASK               = 0x0C21,
			GL_COLOR_CLEAR_VALUE             = 0x0C22,
			GL_COLOR_WRITEMASK               = 0x0C23,
			GL_INDEX_MODE                    = 0x0C30,
			GL_RGBA_MODE                     = 0x0C31,
			GL_DOUBLEBUFFER                  = 0x0C32,
			GL_STEREO                        = 0x0C33,
			GL_RENDER_MODE                   = 0x0C40,
			GL_PERSPECTIVE_CORRECTION_HINT   = 0x0C50,
			GL_POINT_SMOOTH_HINT             = 0x0C51,
			GL_LINE_SMOOTH_HINT              = 0x0C52,
			GL_POLYGON_SMOOTH_HINT           = 0x0C53,
			GL_FOG_HINT                      = 0x0C54,
			GL_TEXTURE_GEN_S                 = 0x0C60,
			GL_TEXTURE_GEN_T                 = 0x0C61,
			GL_TEXTURE_GEN_R                 = 0x0C62,
			GL_TEXTURE_GEN_Q                 = 0x0C63,
			GL_PIXEL_MAP_I_TO_I              = 0x0C70,
			GL_PIXEL_MAP_S_TO_S              = 0x0C71,
			GL_PIXEL_MAP_I_TO_R              = 0x0C72,
			GL_PIXEL_MAP_I_TO_G              = 0x0C73,
			GL_PIXEL_MAP_I_TO_B              = 0x0C74,
			GL_PIXEL_MAP_I_TO_A              = 0x0C75,
			GL_PIXEL_MAP_R_TO_R              = 0x0C76,
			GL_PIXEL_MAP_G_TO_G              = 0x0C77,
			GL_PIXEL_MAP_B_TO_B              = 0x0C78,
			GL_PIXEL_MAP_A_TO_A              = 0x0C79,
			GL_PIXEL_MAP_I_TO_I_SIZE         = 0x0CB0,
			GL_PIXEL_MAP_S_TO_S_SIZE         = 0x0CB1,
			GL_PIXEL_MAP_I_TO_R_SIZE         = 0x0CB2,
			GL_PIXEL_MAP_I_TO_G_SIZE         = 0x0CB3,
			GL_PIXEL_MAP_I_TO_B_SIZE         = 0x0CB4,
			GL_PIXEL_MAP_I_TO_A_SIZE         = 0x0CB5,
			GL_PIXEL_MAP_R_TO_R_SIZE         = 0x0CB6,
			GL_PIXEL_MAP_G_TO_G_SIZE         = 0x0CB7,
			GL_PIXEL_MAP_B_TO_B_SIZE         = 0x0CB8,
			GL_PIXEL_MAP_A_TO_A_SIZE         = 0x0CB9,
			GL_UNPACK_SWAP_BYTES             = 0x0CF0,
			GL_UNPACK_LSB_FIRST              = 0x0CF1,
			GL_UNPACK_ROW_LENGTH             = 0x0CF2,
			GL_UNPACK_SKIP_ROWS              = 0x0CF3,
			GL_UNPACK_SKIP_PIXELS            = 0x0CF4,
			GL_UNPACK_ALIGNMENT              = 0x0CF5,
			GL_PACK_SWAP_BYTES               = 0x0D00,
			GL_PACK_LSB_FIRST                = 0x0D01,
			GL_PACK_ROW_LENGTH               = 0x0D02,
			GL_PACK_SKIP_ROWS                = 0x0D03,
			GL_PACK_SKIP_PIXELS              = 0x0D04,
			GL_PACK_ALIGNMENT                = 0x0D05,
			GL_MAP_COLOR                     = 0x0D10,
			GL_MAP_STENCIL                   = 0x0D11,
			GL_INDEX_SHIFT                   = 0x0D12,
			GL_INDEX_OFFSET                  = 0x0D13,
			GL_RED_SCALE                     = 0x0D14,
			GL_RED_BIAS                      = 0x0D15,
			GL_ZOOM_X                        = 0x0D16,
			GL_ZOOM_Y                        = 0x0D17,
			GL_GREEN_SCALE                   = 0x0D18,
			GL_GREEN_BIAS                    = 0x0D19,
			GL_BLUE_SCALE                    = 0x0D1A,
			GL_BLUE_BIAS                     = 0x0D1B,
			GL_ALPHA_SCALE                   = 0x0D1C,
			GL_ALPHA_BIAS                    = 0x0D1D,
			GL_DEPTH_SCALE                   = 0x0D1E,
			GL_DEPTH_BIAS                    = 0x0D1F,
			GL_MAX_EVAL_ORDER                = 0x0D30,
			GL_MAX_LIGHTS                    = 0x0D31,
			GL_MAX_CLIP_PLANES               = 0x0D32,
			GL_MAX_TEXTURE_SIZE              = 0x0D33,
			GL_MAX_PIXEL_MAP_TABLE           = 0x0D34,
			GL_MAX_ATTRIB_STACK_DEPTH        = 0x0D35,
			GL_MAX_MODELVIEW_STACK_DEPTH     = 0x0D36,
			GL_MAX_NAME_STACK_DEPTH          = 0x0D37,
			GL_MAX_PROJECTION_STACK_DEPTH    = 0x0D38,
			GL_MAX_TEXTURE_STACK_DEPTH       = 0x0D39,
			GL_MAX_VIEWPORT_DIMS             = 0x0D3A,
			GL_MAX_CLIENT_ATTRIB_STACK_DEPTH = 0x0D3B,
			GL_SUBPIXEL_BITS                 = 0x0D50,
			GL_INDEX_BITS                    = 0x0D51,
			GL_RED_BITS                      = 0x0D52,
			GL_GREEN_BITS                    = 0x0D53,
			GL_BLUE_BITS                     = 0x0D54,
			GL_ALPHA_BITS                    = 0x0D55,
			GL_DEPTH_BITS                    = 0x0D56,
			GL_STENCIL_BITS                  = 0x0D57,
			GL_ACCUM_RED_BITS                = 0x0D58,
			GL_ACCUM_GREEN_BITS              = 0x0D59,
			GL_ACCUM_BLUE_BITS               = 0x0D5A,
			GL_ACCUM_ALPHA_BITS              = 0x0D5B,
			GL_NAME_STACK_DEPTH              = 0x0D70,
			GL_AUTO_NORMAL                   = 0x0D80,
			GL_MAP1_COLOR_4                  = 0x0D90,
			GL_MAP1_INDEX                    = 0x0D91,
			GL_MAP1_NORMAL                   = 0x0D92,
			GL_MAP1_TEXTURE_COORD_1          = 0x0D93,
			GL_MAP1_TEXTURE_COORD_2          = 0x0D94,
			GL_MAP1_TEXTURE_COORD_3          = 0x0D95,
			GL_MAP1_TEXTURE_COORD_4          = 0x0D96,
			GL_MAP1_VERTEX_3                 = 0x0D97,
			GL_MAP1_VERTEX_4                 = 0x0D98,
			GL_MAP2_COLOR_4                  = 0x0DB0,
			GL_MAP2_INDEX                    = 0x0DB1,
			GL_MAP2_NORMAL                   = 0x0DB2,
			GL_MAP2_TEXTURE_COORD_1          = 0x0DB3,
			GL_MAP2_TEXTURE_COORD_2          = 0x0DB4,
			GL_MAP2_TEXTURE_COORD_3          = 0x0DB5,
			GL_MAP2_TEXTURE_COORD_4          = 0x0DB6,
			GL_MAP2_VERTEX_3                 = 0x0DB7,
			GL_MAP2_VERTEX_4                 = 0x0DB8,
			GL_MAP1_GRID_DOMAIN              = 0x0DD0,
			GL_MAP1_GRID_SEGMENTS            = 0x0DD1,
			GL_MAP2_GRID_DOMAIN              = 0x0DD2,
			GL_MAP2_GRID_SEGMENTS            = 0x0DD3,
			GL_TEXTURE_1D                    = 0x0DE0,
			GL_TEXTURE_2D                    = 0x0DE1,
			GL_FEEDBACK_BUFFER_POINTER       = 0x0DF0,
			GL_FEEDBACK_BUFFER_SIZE          = 0x0DF1,
			GL_FEEDBACK_BUFFER_TYPE          = 0x0DF2,
			GL_SELECTION_BUFFER_POINTER      = 0x0DF3,
			GL_SELECTION_BUFFER_SIZE         = 0x0DF4;

		/* GetTextureParameter */
		public const System.UInt32
			GL_TEXTURE_WIDTH            = 0x1000,
			GL_TEXTURE_HEIGHT           = 0x1001,
			GL_TEXTURE_INTERNAL_FORMAT	= 0x1003,
			GL_TEXTURE_BORDER_COLOR     = 0x1004,
			GL_TEXTURE_BORDER           = 0x1005;

		/* HintMode */
		public const System.UInt32
			GL_DONT_CARE	= 0x1100,
			GL_FASTEST      = 0x1101,
			GL_NICEST       = 0x1102;

		/* LightName */
		public const System.UInt32
			GL_LIGHT0	= 0x4000,
			GL_LIGHT1	= 0x4001,
			GL_LIGHT2	= 0x4002,
			GL_LIGHT3   = 0x4003,
			GL_LIGHT4   = 0x4004,
			GL_LIGHT5   = 0x4005,
			GL_LIGHT6   = 0x4006,
			GL_LIGHT7   = 0x4007;

		/* LightParameter */
		public const System.UInt32
			GL_AMBIENT                  = 0x1200,
			GL_DIFFUSE                  = 0x1201,
			GL_SPECULAR                 = 0x1202,
			GL_POSITION                 = 0x1203,
			GL_SPOT_DIRECTION           = 0x1204,
			GL_SPOT_EXPONENT            = 0x1205,
			GL_SPOT_CUTOFF              = 0x1206,
			GL_CONSTANT_ATTENUATION     = 0x1207,
			GL_LINEAR_ATTENUATION       = 0x1208,
			GL_QUADRATIC_ATTENUATION	= 0x1209;

		/* ListMode */
		public const System.UInt32
			GL_COMPILE                  = 0x1300,
			GL_COMPILE_AND_EXECUTE      = 0x1301;

		/* LogicOp */
		public const System.UInt32
			GL_CLEAR            = 0x1500,
			GL_AND              = 0x1501,
			GL_AND_REVERSE      = 0x1502,
			GL_COPY             = 0x1503,
			GL_AND_INVERTED     = 0x1504,
			GL_NOOP             = 0x1505,
			GL_XOR              = 0x1506,
			GL_OR               = 0x1507,
			GL_NOR              = 0x1508,
			GL_EQUIV            = 0x1509,
			GL_INVERT           = 0x150A,
			GL_OR_REVERSE       = 0x150B,
			GL_COPY_INVERTED	= 0x150C,
			GL_OR_INVERTED      = 0x150D,
			GL_NAND             = 0x150E,
			GL_SET              = 0x150F;

		/* MaterialParameter */
		public const System.UInt32
			GL_EMISSION             = 0x1600,
			GL_SHININESS            = 0x1601,
			GL_AMBIENT_AND_DIFFUSE	= 0x1602,
			GL_COLOR_INDEXES        = 0x1603;

		/* MatrixMode */
		public const System.UInt32
			GL_MODELVIEW    = 0x1700,
			GL_PROJECTION	= 0x1701,
			GL_TEXTURE      = 0x1702;

		/* PixelCopyType */
		public const System.UInt32
			GL_COLOR        = 0x1800,
			GL_DEPTH        = 0x1801,
			GL_STENCIL		= 0x1802;

		/* PixelFormat */
		public const System.UInt32
			GL_COLOR_INDEX      = 0x1900,
			GL_STENCIL_INDEX    = 0x1901,
			GL_DEPTH_COMPONENT	= 0x1902,
			GL_RED              = 0x1903,
			GL_GREEN            = 0x1904,
			GL_BLUE             = 0x1905,
			GL_ALPHA            = 0x1906,
			GL_RGB              = 0x1907,
			GL_RGBA             = 0x1908,
			GL_LUMINANCE        = 0x1909,
			GL_LUMINANCE_ALPHA  = 0x190A;

		/* PixelType */
		public const System.UInt32
			GL_BITMAP	= 0x1A00;

		/* PolygonMode */
		public const System.UInt32
			GL_POINT	= 0x1B00,
			GL_LINE     = 0x1B01,
			GL_FILL     = 0x1B02;

		/* RenderingMode */
		public const System.UInt32
			GL_RENDER   = 0x1C00,
			GL_FEEDBACK	= 0x1C01,
			GL_SELECT   = 0x1C02;

		/* ShadingModel */
		public const System.UInt32
			GL_FLAT     = 0x1D00,
			GL_SMOOTH	= 0x1D01;

		/* StencilOp */
		public const System.UInt32
			GL_KEEP     = 0x1E00,
			GL_REPLACE	= 0x1E01,
			GL_INCR     = 0x1E02,
			GL_DECR     = 0x1E03;

		/* StringName */
		public const System.UInt32
			GL_VENDOR       = 0x1F00,
			GL_RENDERER     = 0x1F01,
			GL_VERSION      = 0x1F02,
			GL_EXTENSIONS	= 0x1F03;

		/* TextureCoordName */
		public const System.UInt32
			GL_S	= 0x2000,
			GL_T    = 0x2001,
			GL_R    = 0x2002,
			GL_Q    = 0x2003;

		public const System.UInt32
			GL_MODULATE	= 0x2100,
			GL_DECAL    = 0x2101;

		/* TextureEnvParameter */
		public const System.UInt32
			GL_TEXTURE_ENV_MODE     = 0x2200,
			GL_TEXTURE_ENV_COLOR	= 0x2201;

		/* TextureEnvTarget */
		public const System.UInt32
			GL_TEXTURE_ENV	= 0x2300;

		/* TextureGenMode */
		public const System.UInt32
			GL_EYE_LINEAR       = 0x2400,
			GL_OBJECT_LINEAR	= 0x2401,
			GL_SPHERE_MAP       = 0x2402;

		/* TextureGenParameter */
		public const System.UInt32
			GL_TEXTURE_GEN_MODE	= 0x2500,
			GL_OBJECT_PLANE     = 0x2501,
			GL_EYE_PLANE        = 0x2502;

		/* TextureMagFilter */
		public const System.Int32
			GL_NEAREST	= 0x2600,
			GL_LINEAR   = 0x2601;

		/* TextureMinFilter */
		public const System.UInt32
			GL_NEAREST_MIPMAP_NEAREST	= 0x2700,
			GL_LINEAR_MIPMAP_NEAREST    = 0x2701,
			GL_NEAREST_MIPMAP_LINEAR    = 0x2702,
			GL_LINEAR_MIPMAP_LINEAR     = 0x2703;

		/* TextureParameterName */
		public const System.UInt32
			GL_TEXTURE_MAG_FILTER	= 0x2800,
			GL_TEXTURE_MIN_FILTER   = 0x2801,
			GL_TEXTURE_WRAP_S       = 0x2802,
			GL_TEXTURE_WRAP_T       = 0x2803;

		/* TextureWrapMode */
		public const System.UInt32
			GL_CLAMP    = 0x2900,
			GL_REPEAT	= 0x2901;

		/* ClientAttribMask */
		public const System.UInt32
			GL_CLIENT_PIXEL_STORE_BIT         = 0x00000001,
			GL_CLIENT_VERTEX_ARRAY_BIT        = 0x00000002,
			GL_CLIENT_ALL_ATTRIB_BITS         = 0xffffffff;

		/* polygon_offset */
		public const System.Int32
			GL_POLYGON_OFFSET_FACTOR          = 0x8038,
			GL_POLYGON_OFFSET_UNITS           = 0x2A00,
			GL_POLYGON_OFFSET_POINT           = 0x2A01,
			GL_POLYGON_OFFSET_LINE            = 0x2A02,
			GL_POLYGON_OFFSET_FILL            = 0x8037;

		/* texture */
		public const System.Int32
			GL_ALPHA4                         = 0x803B,
			GL_ALPHA8                         = 0x803C,
			GL_ALPHA12                        = 0x803D,
			GL_ALPHA16                        = 0x803E,
			GL_LUMINANCE4                     = 0x803F,
			GL_LUMINANCE8                     = 0x8040,
			GL_LUMINANCE12                    = 0x8041,
			GL_LUMINANCE16                    = 0x8042,
			GL_LUMINANCE4_ALPHA4              = 0x8043,
			GL_LUMINANCE6_ALPHA2              = 0x8044,
			GL_LUMINANCE8_ALPHA8              = 0x8045,
			GL_LUMINANCE12_ALPHA4             = 0x8046,
			GL_LUMINANCE12_ALPHA12            = 0x8047,
			GL_LUMINANCE16_ALPHA16            = 0x8048,
			GL_INTENSITY                      = 0x8049,
			GL_INTENSITY4                     = 0x804A,
			GL_INTENSITY8                     = 0x804B,
			GL_INTENSITY12                    = 0x804C,
			GL_INTENSITY16                    = 0x804D,
			GL_R3_G3_B2                       = 0x2A10,
			GL_RGB4                           = 0x804F,
			GL_RGB5                           = 0x8050,
			GL_RGB8                           = 0x8051,
			GL_RGB10                          = 0x8052,
			GL_RGB12                          = 0x8053,
			GL_RGB16                          = 0x8054,
			GL_RGBA2                          = 0x8055,
			GL_RGBA4                          = 0x8056,
			GL_RGB5_A1                        = 0x8057,
			GL_RGBA8                          = 0x8058,
			GL_RGB10_A2                       = 0x8059,
			GL_RGBA12                         = 0x805A,
			GL_RGBA16                         = 0x805B,
			GL_TEXTURE_RED_SIZE               = 0x805C,
			GL_TEXTURE_GREEN_SIZE             = 0x805D,
			GL_TEXTURE_BLUE_SIZE              = 0x805E,
			GL_TEXTURE_ALPHA_SIZE             = 0x805F,
			GL_TEXTURE_LUMINANCE_SIZE         = 0x8060,
			GL_TEXTURE_INTENSITY_SIZE         = 0x8061,
			GL_PROXY_TEXTURE_1D               = 0x8063,
			GL_PROXY_TEXTURE_2D               = 0x8064;

		/* texture_object */
		public const System.Int32
			GL_TEXTURE_PRIORITY               = 0x8066,
			GL_TEXTURE_RESIDENT               = 0x8067,
			GL_TEXTURE_BINDING_1D             = 0x8068,
			GL_TEXTURE_BINDING_2D             = 0x8069;

		/* vertex_array */
		public const System.Int32
			GL_VERTEX_ARRAY                   = 0x8074,
			GL_NORMAL_ARRAY                   = 0x8075,
			GL_COLOR_ARRAY                    = 0x8076,
			GL_INDEX_ARRAY                    = 0x8077,
			GL_TEXTURE_COORD_ARRAY            = 0x8078,
			GL_EDGE_FLAG_ARRAY                = 0x8079,
			GL_VERTEX_ARRAY_SIZE              = 0x807A,
			GL_VERTEX_ARRAY_TYPE              = 0x807B,
			GL_VERTEX_ARRAY_STRIDE            = 0x807C,
			GL_NORMAL_ARRAY_TYPE              = 0x807E,
			GL_NORMAL_ARRAY_STRIDE            = 0x807F,
			GL_COLOR_ARRAY_SIZE               = 0x8081,
			GL_COLOR_ARRAY_TYPE               = 0x8082,
			GL_COLOR_ARRAY_STRIDE             = 0x8083,
			GL_INDEX_ARRAY_TYPE               = 0x8085,
			GL_INDEX_ARRAY_STRIDE             = 0x8086,
			GL_TEXTURE_COORD_ARRAY_SIZE       = 0x8088,
			GL_TEXTURE_COORD_ARRAY_TYPE       = 0x8089,
			GL_TEXTURE_COORD_ARRAY_STRIDE     = 0x808A,
			GL_EDGE_FLAG_ARRAY_STRIDE         = 0x808C,
			GL_VERTEX_ARRAY_POINTER           = 0x808E,
			GL_NORMAL_ARRAY_POINTER           = 0x808F,
			GL_COLOR_ARRAY_POINTER            = 0x8090,
			GL_INDEX_ARRAY_POINTER            = 0x8091,
			GL_TEXTURE_COORD_ARRAY_POINTER    = 0x8092,
			GL_EDGE_FLAG_ARRAY_POINTER        = 0x8093,
			GL_V2F                            = 0x2A20,
			GL_V3F                            = 0x2A21,
			GL_C4UB_V2F                       = 0x2A22,
			GL_C4UB_V3F                       = 0x2A23,
			GL_C3F_V3F                        = 0x2A24,
			GL_N3F_V3F                        = 0x2A25,
			GL_C4F_N3F_V3F                    = 0x2A26,
			GL_T2F_V3F                        = 0x2A27,
			GL_T4F_V4F                        = 0x2A28,
			GL_T2F_C4UB_V3F                   = 0x2A29,
			GL_T2F_C3F_V3F                    = 0x2A2A,
			GL_T2F_N3F_V3F                    = 0x2A2B,
			GL_T2F_C4F_N3F_V3F                = 0x2A2C,
			GL_T4F_C4F_N3F_V4F                = 0x2A2D;

		/* Extensions */
		public const byte
			GL_EXT_vertex_array               = 1,
			GL_EXT_bgra                       = 1,
			GL_EXT_paletted_texture           = 1,
			GL_WIN_swap_hint                  = 1,
			GL_WIN_draw_range_elements        = 1;

		/* EXT_vertex_array */
		public const System.UInt32
			GL_VERTEX_ARRAY_EXT               = 0x8074,
			GL_NORMAL_ARRAY_EXT               = 0x8075,
			GL_COLOR_ARRAY_EXT                = 0x8076,
			GL_INDEX_ARRAY_EXT                = 0x8077,
			GL_TEXTURE_COORD_ARRAY_EXT        = 0x8078,
			GL_EDGE_FLAG_ARRAY_EXT            = 0x8079,
			GL_VERTEX_ARRAY_SIZE_EXT          = 0x807A,
			GL_VERTEX_ARRAY_TYPE_EXT          = 0x807B,
			GL_VERTEX_ARRAY_STRIDE_EXT        = 0x807C,
			GL_VERTEX_ARRAY_COUNT_EXT         = 0x807D,
			GL_NORMAL_ARRAY_TYPE_EXT          = 0x807E,
			GL_NORMAL_ARRAY_STRIDE_EXT        = 0x807F,
			GL_NORMAL_ARRAY_COUNT_EXT         = 0x8080,
			GL_COLOR_ARRAY_SIZE_EXT           = 0x8081,
			GL_COLOR_ARRAY_TYPE_EXT           = 0x8082,
			GL_COLOR_ARRAY_STRIDE_EXT         = 0x8083,
			GL_COLOR_ARRAY_COUNT_EXT          = 0x8084,
			GL_INDEX_ARRAY_TYPE_EXT           = 0x8085,
			GL_INDEX_ARRAY_STRIDE_EXT         = 0x8086,
			GL_INDEX_ARRAY_COUNT_EXT          = 0x8087,
			GL_TEXTURE_COORD_ARRAY_SIZE_EXT   = 0x8088,
			GL_TEXTURE_COORD_ARRAY_TYPE_EXT   = 0x8089,
			GL_TEXTURE_COORD_ARRAY_STRIDE_EXT = 0x808A,
			GL_TEXTURE_COORD_ARRAY_COUNT_EXT  = 0x808B,
			GL_EDGE_FLAG_ARRAY_STRIDE_EXT     = 0x808C,
			GL_EDGE_FLAG_ARRAY_COUNT_EXT      = 0x808D,
			GL_VERTEX_ARRAY_POINTER_EXT       = 0x808E,
			GL_NORMAL_ARRAY_POINTER_EXT       = 0x808F,
			GL_COLOR_ARRAY_POINTER_EXT        = 0x8090,
			GL_INDEX_ARRAY_POINTER_EXT        = 0x8091,
			GL_TEXTURE_COORD_ARRAY_POINTER_EXT = 0x8092,
			GL_EDGE_FLAG_ARRAY_POINTER_EXT    = 0x8093,
			GL_DOUBLE_EXT                     = GL_DOUBLE;

		/* EXT_bgra */
		public const System.Int32
			GL_BGR_EXT                        = 0x80E0,
			GL_BGRA_EXT                       = 0x80E1;

		/* These must match the GL_COLOR_TABLE_*_SGI enumerants */
		public const System.Int32
			GL_COLOR_TABLE_FORMAT_EXT         = 0x80D8,
			GL_COLOR_TABLE_WIDTH_EXT          = 0x80D9,
			GL_COLOR_TABLE_RED_SIZE_EXT       = 0x80DA,
			GL_COLOR_TABLE_GREEN_SIZE_EXT     = 0x80DB,
			GL_COLOR_TABLE_BLUE_SIZE_EXT      = 0x80DC,
			GL_COLOR_TABLE_ALPHA_SIZE_EXT     = 0x80DD,
			GL_COLOR_TABLE_LUMINANCE_SIZE_EXT = 0x80DE,
			GL_COLOR_TABLE_INTENSITY_SIZE_EXT = 0x80DF,

			GL_COLOR_INDEX1_EXT               = 0x80E2,
			GL_COLOR_INDEX2_EXT               = 0x80E3,
			GL_COLOR_INDEX4_EXT               = 0x80E4,
			GL_COLOR_INDEX8_EXT               = 0x80E5,
			GL_COLOR_INDEX12_EXT              = 0x80E6,
			GL_COLOR_INDEX16_EXT              = 0x80E7,

			/* WIN_draw_range_elements */
			GL_MAX_ELEMENTS_VERTICES_WIN      = 0x80E8,
			GL_MAX_ELEMENTS_INDICES_WIN       = 0x80E9,

			/* WIN_phong_shading */
			GL_PHONG_WIN                      = 0x80EA,
			GL_PHONG_HINT_WIN                 = 0x80EB,

			/* WIN_specular_fog */
			GL_FOG_SPECULAR_TEXTURE_WIN       = 0x80EC;
		#endregion

		#region Functions from opengl32.dll
		[DllImport("opengl32.dll", SetLastError=true)] public static extern System.IntPtr wglGetCurrentContext();
		[DllImport("opengl32.dll", SetLastError=true)] public static extern System.IntPtr wglGetCurrentDC();
		[DllImport("opengl32.dll", SetLastError=true)] public static extern System.IntPtr wglCreateContext(IntPtr hdc);
		[DllImport("opengl32.dll", SetLastError=true)] public static extern System.Int32 wglMakeCurrent(IntPtr hdc, IntPtr hglrc);
		[DllImport("opengl32.dll", SetLastError=true)] public static extern System.Int32 wglDeleteContext(IntPtr hglrc);

		[DllImport("opengl32.dll")] public static extern void glAccum(uint op, float value);
		[DllImport("opengl32.dll")] public static extern void glAlphaFunc(uint func, float aRef);
		[DllImport("opengl32.dll")] unsafe public static extern byte glAreTexturesResident(int n, uint *textures, byte *residences);
		[DllImport("opengl32.dll")] public static extern void glArrayElement(int i);
		[DllImport("opengl32.dll")] public static extern void glBegin(uint mode);
		[DllImport("opengl32.dll")] public static extern void glBindTexture(uint target, uint texture);

		[DllImport("opengl32.dll")] unsafe public static extern void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte *bitmap);
		[DllImport("opengl32.dll")] public static extern void glBlendFunc(uint sfactor, uint dfactor);
		[DllImport("opengl32.dll")] public static extern void glCallList(uint list);
		[DllImport("opengl32.dll")] unsafe public static extern void glCallLists(int n, uint type,  void *lists);
		[DllImport("opengl32.dll")] public static extern void glClear(uint mask);
		[DllImport("opengl32.dll")] public static extern void glClearAccum(float red, float green, float blue, float alpha);
		[DllImport("opengl32.dll")] public static extern void glClearColor(float red, float green, float blue, float alpha);
		[DllImport("opengl32.dll")] public static extern void glClearDepth(double depth);
		[DllImport("opengl32.dll")] public static extern void glClearIndex(float c);
		[DllImport("opengl32.dll")] public static extern void glClearStencil(int s);
		[DllImport("opengl32.dll")] unsafe public static extern void glClipPlane(uint plane,  double *equation);
		[DllImport("opengl32.dll")] public static extern void glColor3b(sbyte red, sbyte green, sbyte blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3bv( sbyte *v);
		[DllImport("opengl32.dll")] public static extern void glColor3d(double red, double green, double blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glColor3f(float red, float green, float blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glColor3i(int red, int green, int blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glColor3s(short red, short green, short blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glColor3ub(byte red, byte green, byte blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3ubv( byte *v);
		[DllImport("opengl32.dll")] public static extern void glColor3ui(uint red, uint green, uint blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3uiv( uint *v);
		[DllImport("opengl32.dll")] public static extern void glColor3us(ushort red, ushort green, ushort blue);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor3usv( ushort *v);
		[DllImport("opengl32.dll")] public static extern void glColor4b(sbyte red, sbyte green, sbyte blue, sbyte alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4bv( sbyte *v);
		[DllImport("opengl32.dll")] public static extern void glColor4d(double red, double green, double blue, double alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glColor4f(float red, float green, float blue, float alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glColor4i(int red, int green, int blue, int alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glColor4s(short red, short green, short blue, short alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glColor4ub(byte red, byte green, byte blue, byte alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4ubv( byte *v);
		[DllImport("opengl32.dll")] public static extern void glColor4ui(uint red, uint green, uint blue, uint alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4uiv( uint *v);
		[DllImport("opengl32.dll")] public static extern void glColor4us(ushort red, ushort green, ushort blue, ushort alpha);
		[DllImport("opengl32.dll")] unsafe public static extern void glColor4usv( ushort *v);
		[DllImport("opengl32.dll")] public static extern void glColorMask(byte red, byte green, byte blue, byte alpha);
		[DllImport("opengl32.dll")] public static extern void glColorMaterial(uint face, uint mode);
		[DllImport("opengl32.dll")] unsafe public static extern void glColorPointer(int size, uint type, int stride,  void *pointer);
		[DllImport("opengl32.dll")] public static extern void glCopyPixels(int x, int y, int width, int height, uint type);
		[DllImport("opengl32.dll")] public static extern void glCopyTexImage1D(uint target, int level, uint internalformat, int x, int y, int width, int border);
		[DllImport("opengl32.dll")] public static extern void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border);
		[DllImport("opengl32.dll")] public static extern void glCopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width);
		[DllImport("opengl32.dll")] public static extern void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
		[DllImport("opengl32.dll")] public static extern void glCullFace(uint mode);
		[DllImport("opengl32.dll")] public static extern void glDeleteLists(uint list, int range);
		[DllImport("opengl32.dll")] unsafe public static extern void glDeleteTextures(int n,  uint *textures);
		[DllImport("opengl32.dll")] public static extern void glDepthFunc(uint func);
		[DllImport("opengl32.dll")] public static extern void glDepthMask(byte flag);
		[DllImport("opengl32.dll")] public static extern void glDepthRange(double zNear, double zFar);
		[DllImport("opengl32.dll")] public static extern void glDisable(uint cap);
		[DllImport("opengl32.dll")] public static extern void glDisableClientState(uint array);
		[DllImport("opengl32.dll")] public static extern void glDrawArrays(uint mode, int first, int count);
		[DllImport("opengl32.dll")] public static extern void glDrawBuffer(uint mode);
		[DllImport("opengl32.dll")] unsafe public static extern void glDrawElements(uint mode, int count, uint type,  void *indices);
		[DllImport("opengl32.dll")] unsafe public static extern void glDrawPixels(int width, int height, uint format, uint type,  void *pixels);
		[DllImport("opengl32.dll")] public static extern void glEdgeFlag(byte flag);
		[DllImport("opengl32.dll")] unsafe public static extern void glEdgeFlagPointer(int stride,  byte *pointer);
		[DllImport("opengl32.dll")] unsafe public static extern void glEdgeFlagv( byte *flag);
		[DllImport("opengl32.dll")] public static extern void glEnable(uint cap);
		[DllImport("opengl32.dll")] public static extern void glEnableClientState(uint array);
		[DllImport("opengl32.dll")] public static extern void glEnd();
		[DllImport("opengl32.dll")] public static extern void glEndList();
		[DllImport("opengl32.dll")] public static extern void glEvalCoord1d(double u);
		[DllImport("opengl32.dll")] unsafe public static extern void glEvalCoord1dv( double *u);
		[DllImport("opengl32.dll")] public static extern void glEvalCoord1f(float u);
		[DllImport("opengl32.dll")] unsafe public static extern void glEvalCoord1fv( float *u);
		[DllImport("opengl32.dll")] unsafe public static extern void glEvalCoord2d(double u, double v);
		[DllImport("opengl32.dll")] unsafe public static extern void glEvalCoord2dv( double *u);
		[DllImport("opengl32.dll")] public static extern void glEvalCoord2f(float u, float v);
		[DllImport("opengl32.dll")] unsafe public static extern void glEvalCoord2fv( float *u);
		[DllImport("opengl32.dll")] public static extern void glEvalMesh1(uint mode, int i1, int i2);
		[DllImport("opengl32.dll")] public static extern void glEvalMesh2(uint mode, int i1, int i2, int j1, int j2);
		[DllImport("opengl32.dll")] public static extern void glEvalPoint1(int i);
		[DllImport("opengl32.dll")] public static extern void glEvalPoint2(int i, int j);
		[DllImport("opengl32.dll")] unsafe public static extern void glFeedbackBuffer(int size, uint type, float *buffer);
		[DllImport("opengl32.dll")] public static extern void glFinish();
		[DllImport("opengl32.dll")] public static extern void glFlush();
		[DllImport("opengl32.dll")] public static extern void glFogf(uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glFogfv(uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glFogi(uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glFogiv(uint pname,  int *someParams);
		[DllImport("opengl32.dll")] public static extern void glFrontFace(uint mode);
		[DllImport("opengl32.dll")] public static extern void glFrustum(double left, double right, double bottom, double top, double zNear, double zFar);
		[DllImport("opengl32.dll")] public static extern uint glGenLists(int range);
		[DllImport("opengl32.dll")] unsafe public static extern void glGenTextures(int n, uint *textures);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetBooleanv(uint pname, byte *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetClipPlane(uint plane, double *equation);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetDoublev(uint pname, double *someParams);
		[DllImport("opengl32.dll")] public static extern uint glGetError();
		[DllImport("opengl32.dll")] unsafe public static extern void glGetFloatv(uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetIntegerv(uint pname, int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetLightfv(uint light, uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetLightiv(uint light, uint pname, int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetMapdv(uint target, uint query, double *v);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetMapfv(uint target, uint query, float *v);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetMapiv(uint target, uint query, int *v);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetMaterialfv(uint face, uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetMaterialiv(uint face, uint pname, int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetPixelMapfv(uint map, float *values);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetPixelMapuiv(uint map, uint *values);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetPixelMapusv(uint map, ushort *values);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetPointerv(uint pname, void* *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetPolygonStipple(byte *mask);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexEnvfv(uint target, uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexEnviv(uint target, uint pname, int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexGendv(uint coord, uint pname, double *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexGenfv(uint coord, uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexGeniv(uint coord, uint pname, int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexImage(uint target, int level, uint format, uint type, void *pixels);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexLevelParameterfv(uint target, int level, uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexLevelParameteriv(uint target, int level, uint pname, int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexParameterfv(uint target, uint pname, float *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glGetTexParameteriv(uint target, uint pname, int *someParams);
		[DllImport("opengl32.dll")] public static extern void glHint(uint target, uint mode);
		[DllImport("opengl32.dll")] public static extern void glIndexMask(uint mask);
		[DllImport("opengl32.dll")] unsafe public static extern void glIndexPointer(uint type, int stride,  void *pointer);
		[DllImport("opengl32.dll")] public static extern void glIndexd(double c);
		[DllImport("opengl32.dll")] unsafe public static extern void glIndexdv( double *c);
		[DllImport("opengl32.dll")] public static extern void glIndexf(float c);
		[DllImport("opengl32.dll")] unsafe public static extern void glIndexfv( float *c);
		[DllImport("opengl32.dll")] public static extern void glIndexi(int c);
		[DllImport("opengl32.dll")] unsafe public static extern void glIndexiv( int *c);
		[DllImport("opengl32.dll")] public static extern void glIndexs(short c);
		[DllImport("opengl32.dll")] unsafe public static extern void glIndexsv( short *c);
		[DllImport("opengl32.dll")] public static extern void glIndexub(byte c);
		[DllImport("opengl32.dll")] public static extern void glLightModelf(uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glLightModelfv(uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glLightModeli(uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glLightModeliv(uint pname,  int *someParams);
		[DllImport("opengl32.dll")] public static extern void glLightf(uint light, uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glLightfv(uint light, uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glLighti(uint light, uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glLightiv(uint light, uint pname,  int *someParams);
		[DllImport("opengl32.dll")] public static extern void glLineStipple(int factor, ushort pattern);
		[DllImport("opengl32.dll")] public static extern void glLineWidth(float width);
		[DllImport("opengl32.dll")] public static extern void glListBase(uint aBase);
		[DllImport("opengl32.dll")] public static extern void glLoadIdentity();
		[DllImport("opengl32.dll")] unsafe public static extern void glLoadMatrixd(double *m);
		[DllImport("opengl32.dll")] unsafe public static extern void glLoadMatrixf(float *m);
		[DllImport("opengl32.dll")] public static extern void glLoadName(uint name);
		[DllImport("opengl32.dll")] public static extern void glLogicOp(uint opcode);
		[DllImport("opengl32.dll")] unsafe public static extern void glMap1d(uint target, double u1, double u2, int stride, int order,  double *points);
		[DllImport("opengl32.dll")] unsafe public static extern void glMap1f(uint target, float u1, float u2, int stride, int order,  float *points);
		[DllImport("opengl32.dll")] unsafe public static extern void glMap2d(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder,  double *points);
		[DllImport("opengl32.dll")] unsafe public static extern void glMap2f(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder,  float *points);
		[DllImport("opengl32.dll")] public static extern void glMapGrid1d(int un, double u1, double u2);
		[DllImport("opengl32.dll")] public static extern void glMapGrid1f(int un, float u1, float u2);
		[DllImport("opengl32.dll")] public static extern void glMapGrid2d(int un, double u1, double u2, int vn, double v1, double v2);
		[DllImport("opengl32.dll")] public static extern void glMapGrid2f(int un, float u1, float u2, int vn, float v1, float v2);
		[DllImport("opengl32.dll")] public static extern void glMaterialf(uint face, uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glMaterialfv(uint face, uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glMateriali(uint face, uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glMaterialiv(uint face, uint pname,  int *someParams);
		[DllImport("opengl32.dll")] public static extern void glMatrixMode(uint mode);
		[DllImport("opengl32.dll")] unsafe public static extern void glMultMatrixd( double *m);
		[DllImport("opengl32.dll")] unsafe public static extern void glMultMatrixf( float *m);
		[DllImport("opengl32.dll")] public static extern void glNewList(uint list, uint mode);
		[DllImport("opengl32.dll")] public static extern void glNormal3b(sbyte nx, sbyte ny, sbyte nz);
		[DllImport("opengl32.dll")] unsafe public static extern void glNormal3bv( sbyte *v);
		[DllImport("opengl32.dll")] public static extern void glNormal3d(double nx, double ny, double nz);
		[DllImport("opengl32.dll")] unsafe public static extern void glNormal3dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glNormal3f(float nx, float ny, float nz);
		[DllImport("opengl32.dll")] unsafe public static extern void glNormal3fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glNormal3i(int nx, int ny, int nz);
		[DllImport("opengl32.dll")] unsafe public static extern void glNormal3iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glNormal3s(short nx, short ny, short nz);
		[DllImport("opengl32.dll")] unsafe public static extern void glNormal3sv( short *v);
		[DllImport("opengl32.dll")] unsafe public static extern void glNormalPointer(uint type, int stride,  void *pointer);
		[DllImport("opengl32.dll")] public static extern void glOrtho(double left, double right, double bottom, double top, double zNear, double zFar);
		[DllImport("opengl32.dll")] public static extern void glPassThrough(float token);
		[DllImport("opengl32.dll")] unsafe public static extern void glPixelMapfv(uint map, int mapsize,  float *values);
		[DllImport("opengl32.dll")] unsafe public static extern void glPixelMapuiv(uint map, int mapsize,  uint *values);
		[DllImport("opengl32.dll")] unsafe public static extern void glPixelMapusv(uint map, int mapsize,  ushort *values);
		[DllImport("opengl32.dll")] public static extern void glPixelStoref(uint pname, float param);
		[DllImport("opengl32.dll")] public static extern void glPixelStorei(uint pname, int param);
		[DllImport("opengl32.dll")] public static extern void glPixelTransferf(uint pname, float param);
		[DllImport("opengl32.dll")] public static extern void glPixelTransferi(uint pname, int param);
		[DllImport("opengl32.dll")] public static extern void glPixelZoom(float xfactor, float yfactor);
		[DllImport("opengl32.dll")] public static extern void glPointSize(float size);
		[DllImport("opengl32.dll")] public static extern void glPolygonMode(uint face, uint mode);
		[DllImport("opengl32.dll")] public static extern void glPolygonOffset(float factor, float units);
		[DllImport("opengl32.dll")] unsafe public static extern void glPolygonStipple( byte *mask);
		[DllImport("opengl32.dll")] public static extern void glPopAttrib();
		[DllImport("opengl32.dll")] public static extern void glPopClientAttrib();
		[DllImport("opengl32.dll")] public static extern void glPopMatrix();
		[DllImport("opengl32.dll")] public static extern void glPopName();
		[DllImport("opengl32.dll")] unsafe public static extern void glPrioritizeTextures(int n,  uint *textures,  float *priorities);
		[DllImport("opengl32.dll")] public static extern void glPushAttrib(uint mask);
		[DllImport("opengl32.dll")] public static extern void glPushClientAttrib(uint mask);
		[DllImport("opengl32.dll")] public static extern void glPushMatrix();
		[DllImport("opengl32.dll")] public static extern void glPushName(uint name);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2d(double x, double y);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos2dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2f(float x, float y);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos2fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2i(int x, int y);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos2iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2s(short x, short y);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos2sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3d(double x, double y, double z);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos3dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3f(float x, float y, float z);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos3fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3i(int x, int y, int z);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos3iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3s(short x, short y, short z);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos3sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4d(double x, double y, double z, double w);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos4dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4f(float x, float y, float z, float w);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos4fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4i(int x, int y, int z, int w);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos4iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4s(short x, short y, short z, short w);
		[DllImport("opengl32.dll")] unsafe public static extern void glRasterPos4sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glReadBuffer(uint mode);
		[DllImport("opengl32.dll")] unsafe public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, void *pixels);
		[DllImport("opengl32.dll")] public static extern void glRectd(double x1, double y1, double x2, double y2);
		[DllImport("opengl32.dll")] unsafe public static extern void glRectdv( double *v1,  double *v2);
		[DllImport("opengl32.dll")] public static extern void glRectf(float x1, float y1, float x2, float y2);
		[DllImport("opengl32.dll")] unsafe public static extern void glRectfv( float *v1,  float *v2);
		[DllImport("opengl32.dll")] public static extern void glRecti(int x1, int y1, int x2, int y2);
		[DllImport("opengl32.dll")] unsafe public static extern void glRectiv( int *v1,  int *v2);
		[DllImport("opengl32.dll")] public static extern void glRects(short x1, short y1, short x2, short y2);
		[DllImport("opengl32.dll")] unsafe public static extern void glRectsv( short *v1,  short *v2);
		[DllImport("opengl32.dll")] public static extern int glRenderMode(uint mode);
		[DllImport("opengl32.dll")] public static extern void glRotated(double angle, double x, double y, double z);
		[DllImport("opengl32.dll")] public static extern void glRotatef(float angle, float x, float y, float z);
		[DllImport("opengl32.dll")] public static extern void glScaled(double x, double y, double z);
		[DllImport("opengl32.dll")] public static extern void glScalef(float x, float y, float z);
		[DllImport("opengl32.dll")] public static extern void glScissor(int x, int y, int width, int height);
		[DllImport("opengl32.dll")] unsafe public static extern void glSelectBuffer(int size, uint *buffer);
		[DllImport("opengl32.dll")] public static extern void glShadeModel(uint mode);
		[DllImport("opengl32.dll")] public static extern void glStencilFunc(uint func, int aRef, uint mask);
		[DllImport("opengl32.dll")] public static extern void glStencilMask(uint mask);
		[DllImport("opengl32.dll")] public static extern void glStencilOp(uint fail, uint zfail, uint zpass);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1d(double s);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord1dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1f(float s);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord1fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1i(int s);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord1iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1s(short s);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord1sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2d(double s, double t);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord2dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2f(float s, float t);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord2fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2i(int s, int t);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord2iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2s(short s, short t);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord2sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3d(double s, double t, double r);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord3dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3f(float s, float t, float r);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord3fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3i(int s, int t, int r);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord3iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3s(short s, short t, short r);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord3sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4d(double s, double t, double r, double q);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord4dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4f(float s, float t, float r, float q);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord4fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4i(int s, int t, int r, int q);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord4iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4s(short s, short t, short r, short q);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoord4sv( short *v);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexCoordPointer(int size, uint type, int stride,  void *pointer);
		[DllImport("opengl32.dll")] public static extern void glTexEnvf(uint target, uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexEnvfv(uint target, uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glTexEnvi(uint target, uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexEnviv(uint target, uint pname,  int *someParams);
		[DllImport("opengl32.dll")] public static extern void glTexGend(uint coord, uint pname, double param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexGendv(uint coord, uint pname,  double *someParams);
		[DllImport("opengl32.dll")] public static extern void glTexGenf(uint coord, uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexGenfv(uint coord, uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glTexGeni(uint coord, uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexGeniv(uint coord, uint pname,  int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type,  void *pixels);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexImage2D(uint target, int level, int components, int width, int height, int border, uint format, uint type,  void *pixels);
		[DllImport("opengl32.dll")] public static extern void glTexParameterf(uint target, uint pname, float param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexParameterfv(uint target, uint pname,  float *someParams);
		[DllImport("opengl32.dll")] public static extern void glTexParameteri(uint target, uint pname, int param);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexParameteriv(uint target, uint pname,  int *someParams);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type,  void *pixels);
		[DllImport("opengl32.dll")] unsafe public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type,  void *pixels);
		[DllImport("opengl32.dll")] public static extern void glTranslated(double x, double y, double z);
		[DllImport("opengl32.dll")] public static extern void glTranslatef(float x, float y, float z);
		[DllImport("opengl32.dll")] public static extern void glVertex2d(double x, double y);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex2dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glVertex2f(float x, float y);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex2fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glVertex2i(int x, int y);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex2iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glVertex2s(short x, short y);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex2sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glVertex3d(double x, double y, double z);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex3dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glVertex3f(float x, float y, float z);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex3fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glVertex3i(int x, int y, int z);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex3iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glVertex3s(short x, short y, short z);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex3sv( short *v);
		[DllImport("opengl32.dll")] public static extern void glVertex4d(double x, double y, double z, double w);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex4dv( double *v);
		[DllImport("opengl32.dll")] public static extern void glVertex4f(float x, float y, float z, float w);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex4fv( float *v);
		[DllImport("opengl32.dll")] public static extern void glVertex4i(int x, int y, int z, int w);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex4iv( int *v);
		[DllImport("opengl32.dll")] public static extern void glVertex4s(short x, short y, short z, short w);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertex4sv( short *v);
		[DllImport("opengl32.dll")] unsafe public static extern void glVertexPointer(int size, uint type, int stride,  void *pointer);
		[DllImport("opengl32.dll")] public static extern void glViewport(int x, int y, int width, int height);

		[DllImport("opengl32.dll")] unsafe public static extern void glIndexubv( byte *c);
		[DllImport("opengl32.dll")] public static extern void glInitNames();
		[DllImport("opengl32.dll")] unsafe public static extern void glInterleavedArrays(uint format, int stride,  void *pointer);
		[DllImport("opengl32.dll")] public static extern byte glIsEnabled(uint cap);
		[DllImport("opengl32.dll")] public static extern byte glIsList(uint list);
		[DllImport("opengl32.dll")] public static extern byte glIsTexture(uint texture);

		[DllImport("opengl32.dll")] public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, float[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, int[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, uint[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage1D(uint target, int level, int components, int width, int border, uint format, uint type, float[,] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage2D(uint target, int level, int components, int width, int height, int border, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glGetTexImage(uint target, int level, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage2D(uint target, int level, int components, int width, int height, int border, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glGetTexImage(uint target, int level, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, uint[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, byte[,,] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexImage2D(uint target, int level, int components, int width, int height, int border, uint format, uint type, uint[] pixels);
		[DllImport("opengl32.dll")] public static extern void glGetTexImage(uint target, int level, uint format, uint type, uint[] pixels);
		[DllImport("opengl32.dll")] public static extern void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, uint[] pixels);
		[DllImport("opengl32.dll")] public static extern void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte[] bitmap);
		[DllImport("opengl32.dll")] public static extern void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, IntPtr bitmap);
		
		[DllImport("opengl32.dll")] public static extern void glTexParameteri(uint target, uint pname, uint param);
		[DllImport("opengl32.dll")] public static extern void glGetTexLevelParameterfv(uint target, int level, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexLevelParameteriv(uint target, int level, uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexParameterfv(uint target, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexParameteriv(uint target, uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexLevelParameterfv(uint target, int level, uint pname, out float someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexLevelParameteriv(uint target, int level, uint pname, out int someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexParameterfv(uint target, uint pname, out float someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexParameteriv(uint target, uint pname, out int someParams);
	
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, byte[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, sbyte[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, short[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, ushort[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, int[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, uint[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, float[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, char[] lists);
		[DllImport("opengl32.dll")] public static extern void glCallLists(int n, uint type, [MarshalAs(UnmanagedType.LPWStr)] string lists);
   
		[DllImport("opengl32.dll")] public static extern void glRasterPos2dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos2sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos3sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glRasterPos4sv(short[] v);
		
		[DllImport("opengl32.dll")] public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, ushort[] pixels);
		[DllImport("opengl32.dll")] public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, uint[] pixels);
		[DllImport("opengl32.dll")] public static extern void glDrawPixels(int width, int height, uint format, uint type, IntPtr pixels);
		[DllImport("opengl32.dll")] public static extern void glDrawPixels(int width, int height, uint format, uint type, byte[] pixels);
		[DllImport("opengl32.dll")] public static extern void glDrawPixels(int width, int height, uint format, uint type, ushort[] pixels);
		[DllImport("opengl32.dll")] public static extern void glDrawPixels(int width, int height, uint format, uint type, uint[] pixels);
		
		[DllImport("opengl32.dll")] public static extern void glGenTextures(int n, uint[] textures);
		[DllImport("opengl32.dll")] public static extern void glDeleteTextures(int n,  uint[] textures);
		[DllImport("opengl32.dll")] public static extern void glNormal3bv(sbyte[] v);
		[DllImport("opengl32.dll")] public static extern void glNormal3dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glNormal3fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glNormal3iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glNormal3sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex2dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex2fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex2iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex2sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex3dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex3fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex3iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex3sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex4dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex4fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex4iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glVertex4sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glLightModelfv(uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glLightModeliv(uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glLightfv(uint light, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glLightiv(uint light, uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glColor3bv(sbyte[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3ubv(byte[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3uiv(uint[] v);
		[DllImport("opengl32.dll")] public static extern void glColor3usv(ushort[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4bv(sbyte[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4ubv(byte[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4uiv(uint[] v);
		[DllImport("opengl32.dll")] public static extern void glColor4usv(ushort[] v);
		[DllImport("opengl32.dll")] public static extern void glGetBooleanv(uint pname, byte[] someParams );
		[DllImport("opengl32.dll")] public static extern void glGetDoublev(uint pname, double[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetFloatv(uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetIntegerv(uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetIntegerv(uint pname, uint[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetBooleanv(uint pname, out byte someParam);
		[DllImport("opengl32.dll")] public static extern void glGetDoublev(uint pname, out double someParam);
		[DllImport("opengl32.dll")] public static extern void glGetFloatv(uint pname, out float someParam);
		[DllImport("opengl32.dll")] public static extern void glGetIntegerv(uint pname, out int someParam);
		[DllImport("opengl32.dll")] public static extern void glGetIntegerv(uint pname, out uint someParam);
		[DllImport("opengl32.dll")] public static extern void glLoadMatrixd(double[] m);
		[DllImport("opengl32.dll")] public static extern void glLoadMatrixf(float[] m);
		[DllImport("opengl32.dll")] public static extern void glMultMatrixd(double[] m);
		[DllImport("opengl32.dll")] public static extern void glMultMatrixf(float[] m);

		[DllImport("opengl32.dll")] public static extern void glEdgeFlagv(ref byte flag);
		[DllImport("opengl32.dll")] public static extern void glEdgeFlagv(byte[] flag);
		
		[DllImport("opengl32.dll")] public static extern void glMaterialfv(uint face, uint pname,  ref float someParams);
		[DllImport("opengl32.dll")] public static extern void glMaterialfv(uint face, uint pname,  float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glMaterialfv(uint face, uint pname,  IntPtr someParams);
		[DllImport("opengl32.dll")] public static extern void glMaterialiv(uint face, uint pname,  ref int someParams);
		[DllImport("opengl32.dll")] public static extern void glMaterialiv(uint face, uint pname,  int[] someParams);
		
		[DllImport("opengl32.dll")] public static extern void glTexCoord1dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord1sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord2sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord3sv(short[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4dv(double[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4fv(float[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4iv(int[] v);
		[DllImport("opengl32.dll")] public static extern void glTexCoord4sv(short[] v);
		
		[DllImport("opengl32.dll")] public static extern void glSelectBuffer(int size, uint[] buffer);
		[DllImport("opengl32.dll")] public static extern void glSelectBuffer(int size, IntPtr buffer);
		
		[DllImport("opengl32.dll")] public static extern void glFeedbackBuffer(int size, uint type, float[] buffer);
		[DllImport("opengl32.dll")] public static extern void glFeedbackBuffer(int size, uint type, IntPtr buffer);
		
		[DllImport("opengl32.dll")] public static extern byte glAreTexturesResident(int n, out uint textures, out byte residences);
		[DllImport("opengl32.dll")] public static extern byte glAreTexturesResident(int n, uint[] textures, byte[] residences);
		[DllImport("opengl32.dll")] public static extern void glPrioritizeTextures(int n, uint[] textures, float[] priorities);
		
		[DllImport("opengl32.dll")] public static extern void glClipPlane(uint plane, double[] equation);
		[DllImport("opengl32.dll")] public static extern void glGetClipPlane(uint plane, double[] equation);
		
		[DllImport("opengl32.dll")] public static extern void glEvalCoord2dv(double[] u);
		[DllImport("opengl32.dll")] public static extern void glEvalCoord2fv(float[] u);
		[DllImport("opengl32.dll")] public static extern void glEvalCoord1dv(double[] u);
		[DllImport("opengl32.dll")] public static extern void glEvalCoord1fv(float[] u);
		
		[DllImport("opengl32.dll")] public static extern void glFogfv(uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glFogiv(uint pname, int[] someParams);
		
		[DllImport("opengl32.dll")] public static extern void glGetLightfv(uint light, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetLightiv(uint light, uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetLightfv(uint light, uint pname, out float someParams);
		[DllImport("opengl32.dll")] public static extern void glGetLightiv(uint light, uint pname, out int someParams);
		
		[DllImport("opengl32.dll")] public static extern void glGetMaterialfv(uint face, uint pname, out float someParams);
		[DllImport("opengl32.dll")] public static extern void glGetMaterialiv(uint face, uint pname, out int someParams);
		[DllImport("opengl32.dll")] public static extern void glGetMaterialfv(uint face, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetMaterialiv(uint face, uint pname, int[] someParams);
		
		[DllImport("opengl32.dll")] public static extern void glGetPolygonStipple(byte[] mask);
		[DllImport("opengl32.dll")] public static extern void glPolygonStipple(byte[] mask);

		[DllImport("opengl32.dll")] public static extern void glGetTexEnvfv(uint target, uint pname, out float someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexEnviv(uint target, uint pname, out int someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexEnvfv(uint target, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexEnviv(uint target, uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glTexEnvfv(uint target, uint pname,  float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glTexEnviv(uint target, uint pname,  int[] someParams);

		[DllImport("opengl32.dll")] public static extern void glGetTexGendv(uint coord, uint pname, out double someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexGenfv(uint coord, uint pname, out float someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexGeniv(uint coord, uint pname, out int someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexGendv(uint coord, uint pname, double[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexGenfv(uint coord, uint pname, float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glGetTexGeniv(uint coord, uint pname, int[] someParams);
		[DllImport("opengl32.dll")] public static extern void glTexGendv(uint coord, uint pname,  double[] someParams);
		[DllImport("opengl32.dll")] public static extern void glTexGenfv(uint coord, uint pname,  float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glTexGeniv(uint coord, uint pname,  int[] someParams);

		[DllImport("opengl32.dll")] public static extern void glIndexdv(double[] c);
		[DllImport("opengl32.dll")] public static extern void glIndexfv(float[] c);
		[DllImport("opengl32.dll")] public static extern void glIndexiv(int[] c);
		[DllImport("opengl32.dll")] public static extern void glIndexsv(short[] c);
		[DllImport("opengl32.dll")] public static extern void glIndexubv(byte[] c);
		[DllImport("opengl32.dll")] public static extern void glIndexdv(ref double c);
		[DllImport("opengl32.dll")] public static extern void glIndexfv(ref float c);
		[DllImport("opengl32.dll")] public static extern void glIndexiv(ref int c);
		[DllImport("opengl32.dll")] public static extern void glIndexsv(ref short c);
		[DllImport("opengl32.dll")] public static extern void glIndexubv(ref byte c);
		
		[DllImport("opengl32.dll")] public static extern void glGetMapdv(uint target, uint query, double[] v);
		[DllImport("opengl32.dll")] public static extern void glGetMapfv(uint target, uint query, float[] v);
		[DllImport("opengl32.dll")] public static extern void glGetMapiv(uint target, uint query, int[] v);
		[DllImport("opengl32.dll")] public static extern void glMap1d(uint target, double u1, double u2, int stride, int order,  double[] points);
		[DllImport("opengl32.dll")] public static extern void glMap1f(uint target, float u1, float u2, int stride, int order,  float[] points);
		[DllImport("opengl32.dll")] public static extern void glMap2d(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double[] points);
		[DllImport("opengl32.dll")] public static extern void glMap2f(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder,  float[] points);
		[DllImport("opengl32.dll")] public static extern void glPixelMapfv(uint map, int mapsize,  float[] values);
		[DllImport("opengl32.dll")] public static extern void glPixelMapuiv(uint map, int mapsize,  uint[] values);
		[DllImport("opengl32.dll")] public static extern void glPixelMapusv(uint map, int mapsize,  ushort[] values);
		[DllImport("opengl32.dll")] public static extern void glGetPixelMapfv(uint map, float[] values);
		[DllImport("opengl32.dll")] public static extern void glGetPixelMapuiv(uint map, uint[] values);
		[DllImport("opengl32.dll")] public static extern void glGetPixelMapusv(uint map, ushort[] values);

		[DllImport("opengl32.dll")] public static extern void glRectdv(double[] v1, double[] v2);
		[DllImport("opengl32.dll")] public static extern void glRectfv(float[] v1, float[] v2);
		[DllImport("opengl32.dll")] public static extern void glRectiv(int[] v1, int[] v2);
		[DllImport("opengl32.dll")] public static extern void glRectsv(short[] v1, short[] v2);
		
		[DllImport("opengl32.dll")] public static extern void glTexParameterfv(uint target, uint pname,  float[] someParams);
		[DllImport("opengl32.dll")] public static extern void glTexParameteriv(uint target, uint pname,  int[] someParams);
	
		[DllImport("opengl32.dll")] public static extern void glColorPointer(int size, uint type, int stride, IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glColorPointer(int size, uint type, int stride, float[,] pointer);
		[DllImport("opengl32.dll")] public static extern void glEdgeFlagPointer(int stride,  IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glIndexPointer(uint type, int stride, IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glNormalPointer(uint type, int stride,  IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glTexCoordPointer(int size, uint type, int stride, IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glVertexPointer(int size, uint type, int stride, IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glVertexPointer(int size, uint type, int stride, float[,] pointer);
		[DllImport("opengl32.dll")] public static extern void glInterleavedArrays(uint format, int stride,  IntPtr pointer);
		[DllImport("opengl32.dll")] public static extern void glDrawElements(uint mode, int count, uint type, IntPtr indices);
		[DllImport("opengl32.dll")] public static extern void glDrawElements(uint mode, int count, uint type, byte[,] indices);
		[DllImport("opengl32.dll")] public static extern void glGetPointerv(uint pname, out IntPtr someParams);
		#endregion

		#region Functions from glu32.dll
		[DllImport("glu32.dll")] public static extern void gluOrtho2D(System.Double left, System.Double right, System.Double bottom, System.Double top);
		[DllImport("glu32.dll")] public static extern void gluPerspective(System.Double fovy, System.Double aspect, System.Double zNear, System.Double zFar);
		[DllImport("glu32.dll")] public static extern void gluLookAt(System.Double eyex, System.Double eyey, System.Double eyez, System.Double centerx, System.Double centery, System.Double centerz, System.Double upx, System.Double upy, System.Double upz);
		[DllImport("glu32.dll")] public unsafe static extern void gluProject(System.Double objx, System.Double objy, System.Double objz,  System.Double *modelMatrix, System.Double *projMatrix, System.Int32 *viewport, System.Double *winx, System.Double *winy, System.Double *winz);
		[DllImport("glu32.dll")] public unsafe static extern void gluUnProject(System.Double winx, System.Double winy, System.Double winz, System.Double *modelMatrix, System.Double *projMatrix, System.Int32 *viewport, System.Double *objx, System.Double *objy, System.Double *objz);
		#endregion

		

		#region Defines from gdi.h
		/// <summary>
		/// Structure PIXELFORMATDESCRIPTOR.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct PIXELFORMATDESCRIPTOR
		{
			public ushort nSize, nVersion;
			public uint dwFlags;
			public byte
				iPixelType, cColorBits, cRedBits, cRedShift,
				cGreenBits, cGreenShift, cBlueBits, cBlueShift,
				cAlphaBits, cAlphaShift, cAccumBits, cAccumRedBits,
				cAccumGreenBits, cAccumBlueBits, cAccumAlphaBits,
				cDepthBits, cStencilBits, cAuxBuffers, iLayerType, bReserved;
			public uint dwLayerMask, dwVisibleMask, dwDamageMask;

			
			/// <summary>
			/// Initializes a current instance of the PIXELFORMATDESCRIPTOR
			/// structure with default values.
			/// </summary>
			public void Initialize()
			{
				nSize = (ushort)Marshal.SizeOf(this);
				nVersion = 1;
				dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER;
				iPixelType = PFD_TYPE_RGBA;
				cColorBits = 24;
				cRedBits = 24; cRedShift = 0;
				cGreenBits = 24; cGreenShift = 0; 
				cBlueBits = 24; cBlueShift = 0;
				cAlphaBits = 0; cAlphaShift = 0;
				cAccumBits = cAccumRedBits = cAccumGreenBits = cAccumBlueBits = cAccumAlphaBits = 0;
				cDepthBits = 32;
				cStencilBits = 0;
				cAuxBuffers = 0;
				iLayerType = 0;
				bReserved = 0;
				dwLayerMask = dwVisibleMask = dwDamageMask = 0;
			}
		}
		#endregion

		#region Functions from gdi32.dll 
		[DllImport("gdi32.dll", SetLastError=true)] public unsafe static extern System.Int32 ChoosePixelFormat(IntPtr hdc, ref PIXELFORMATDESCRIPTOR ppfd);
		[DllImport("gdi32.dll", SetLastError=true)] public unsafe static extern System.Int32 SetPixelFormat(IntPtr hdc, System.Int32 iPixelFormat, ref PIXELFORMATDESCRIPTOR ppfd);
		[DllImport("gdi32.dll", SetLastError=true)] public static extern System.Int32 SwapBuffers(IntPtr hdc);
		#endregion

		#region Functions from user32.dll
		[DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport("user32.dll")] public static extern System.Int32 ReleaseDC(IntPtr hWnd, IntPtr hDC);
		#endregion
		#endregion


		#region Initializing & Disposing
		/// <summary>
		/// Intializes a new instance of the OpenGLControl class.
		/// </summary>
		public OpenGLControl()
		{
			InitializeComponent();
		
			SetStyle(ControlStyles.ResizeRedraw, true);
			
			// Creates an OpenGL rendering context.
			CreateOpenGLContext();
		}

		/// <summary>
		/// Creates the OpenGL rendering context.
		/// </summary>
		protected virtual void CreateOpenGLContext()
		{
			try 
			{
				// Fill in the pixel format descriptor.
				PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
				pfd.Initialize();

				// Allow the user to correct the pixel format descriptor.
				OnPreparePFD(ref pfd);

				_hDC = GetDC(Handle);
				if (_hDC == IntPtr.Zero)
					throw new Win32Exception(Marshal.GetLastWin32Error());
				
				// Choose appropriate pixel format supported by a _hDC context
				int iPixelformat = ChoosePixelFormat(_hDC, ref pfd);
				if (iPixelformat == 0)
					throw new Win32Exception(Marshal.GetLastWin32Error());

				// Set the pixel format
				if (SetPixelFormat(_hDC, iPixelformat, ref pfd) == 0)
					throw new Win32Exception(Marshal.GetLastWin32Error());
				
				// Create a new OpenGL rendering context
				_hRC = wglCreateContext(_hDC);
				if (_hRC == IntPtr.Zero)
					throw new Win32Exception(Marshal.GetLastWin32Error());

				// Make _hRC rendering context as a current context
				if (wglMakeCurrent(_hDC, _hRC) == 0) 
					throw new Win32Exception(Marshal.GetLastWin32Error());
				
				// Allow the user to tune the scene
				OnInitScene();

				// Release the OpenGL rendering context 
				wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
			} 
			catch (Exception) 
			{
				if (_hRC != IntPtr.Zero)
					wglDeleteContext(_hRC);
	
				if (_hDC != IntPtr.Zero)
					ReleaseDC(Handle, _hDC);

				throw;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (_bDisposed)
				return;

			try 
			{
				// Release the OpenGL rendering context 
				wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
				
				// Delete the OpenGL rendering context.
				if (_hRC != IntPtr.Zero)
					wglDeleteContext(_hRC);
		
				// Release the device context
				if (_hDC != IntPtr.Zero)
					ReleaseDC(Handle, _hDC);

				_bDisposed = true;
				_hDC = _hRC = IntPtr.Zero;
			} 
			finally 
			{
				base.Dispose(disposing);
			}
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// OpenGLControl
			// 
			this.Name = "OpenGLControl";
			this.Size = new System.Drawing.Size(300, 208);

		}
		#endregion

		#region Operations
		/// <summary>
		/// Activates the OpenGL context.
		/// This function should be called before execution OpenGL commands.
		/// </summary>
		protected void ActivateContext()
		{
			System.Diagnostics.Debug.Assert(wglGetCurrentContext() == IntPtr.Zero && wglGetCurrentDC() == IntPtr.Zero, "The OpenGL context is already activated.");
			wglMakeCurrent(_hDC, _hRC);
		}

		/// <summary>
		/// Deactivates the OpenGL context.
		/// This function should be called after execution OpenGL commands.
		/// </summary>
		protected void DeactivateContext()
		{
			System.Diagnostics.Debug.Assert(wglGetCurrentContext() != IntPtr.Zero && wglGetCurrentDC() != IntPtr.Zero, "The OpenGL context is already deactivated.");
			wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
		}

		/// <summary>
		/// Exchanges the front and back buffers.
		/// </summary>
		protected void SwapBuffers()
		{
			SwapBuffers(_hDC);
		}

		/// <summary>
		/// Saves OpenGL window to the bitmap with pixel format PixelFormat.Format24bppRgb.
		/// </summary>
		/// <param name="pf"></param>
		/// <returns>A Bitmap object, that contains a screen shot of the OpenGL window.</returns>
		protected Bitmap ToBitmap()
		{
			return ToBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		}

		/// <summary>
		/// Saves OpenGL window to the bitmap with specified pixel format.
		/// </summary>
		/// <param name="pf">The format of the pixel in the created bitmap.</param>
		/// <returns>A Bitmap object, that contains a screen shot of the OpenGL window.</returns>
		protected Bitmap ToBitmap(System.Drawing.Imaging.PixelFormat pf)
		{
			ActivateContext();

			int[] nViewPort = new int[4];
			glGetIntegerv(GL_VIEWPORT, nViewPort);
			if (nViewPort[2] < 1 || nViewPort[3] < 1)
				return null;

			Rectangle rect = new Rectangle(0, 0, nViewPort[2], nViewPort[3]);
			Bitmap bmp = new Bitmap(rect.Width, rect.Height, pf);
			System.Drawing.Imaging.BitmapData bdata = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, pf);
			glReadPixels(0, 0, rect.Width, rect.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bdata.Scan0);
			bmp.UnlockBits(bdata);
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

			DeactivateContext();
			return bmp;
		}
		#endregion

		#region Overridables
		/// <summary>
		/// Called before creation OpenGL window. By overriding this function,
		/// user can modify fields of the PIXELFORMATDESCRIPTOR structure.
		/// </summary>
		/// <param name="pfd">A reference to the PIXELFORMATDESCRIPTOR structure.</param>
		protected virtual void OnPreparePFD(ref PIXELFORMATDESCRIPTOR pfd)
		{
		}

		/// <summary>
		/// Called after successfully creation of the OpenGL window. By overriding
		/// this function, user can perform special preporation of the OpenGL scene.
		/// In this function it is not required to activate or deactivate the OpenGL
		/// rendering context.
		/// </summary>
		protected virtual void OnInitScene()
		{
		}

		/// <summary>
		/// Called for setting the viewport area. By default, 
		/// viewport area is equal to the all window area. By overriding 
		/// this function, user can set the viewport area. In this function it is not 
		/// required to activate or deactivate the OpenGL rendering context.
		/// </summary>
		protected virtual void OnSetViewport()
		{
			glViewport(0, 0, Width, Height);
		}

		/// <summary>
		/// Called for setting the projection. By default, a perspective 
		/// projection are setted. By overriding this function, user can set
		/// projection type itself. In this function it is not required to 
		/// activate or deactivate the OpenGL rendering context.
		/// </summary>
		protected virtual void SetProjection()
		{
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();

			// 
			float dAspect = Width <= Height ? (float)Height / Width : (float)Width / Height;
			
			// Set up a perspective projection
			gluPerspective(45.0f, dAspect, 0.1, 100.0f);
		}
		#endregion

		#region Event handlers
		/// <summary>
		/// Occurs when the Size property value changes.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);

			// Activate context
			wglMakeCurrent(_hDC, _hRC);

			// Set viewport
			OnSetViewport();
			// Set projection
			SetProjection();

			// Deactivate context
			wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
		{
		}
		#endregion


		#region Fields
		/// <summary>Track whether Dispose has been called.</summary>
		private bool _bDisposed = false;
		/// <summary>A handle of the window.</summary>
		protected IntPtr _hDC = IntPtr.Zero;
		/// <summary>A OpenGL rendering context.</summary>
		protected IntPtr _hRC = IntPtr.Zero;
		#endregion
	}
}
