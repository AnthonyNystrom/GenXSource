package {
	import flash.utils.ByteArray;
	import flash.utils.Endian;
	
	public class TIFF {
		public static var EXIF_TAGS:Object = {
			256: "ImageWidth",
			257: "ImageLength",
			258: "BitsPerSample",
			259: "Compression",
			262: "PhotometricInterpretation",
			266: "FillOrder",
			269: "DocumentName",
			270: "ImageDescription",			
			271: "Make",
			272: "Model",
			273: "StripOffsets",
			274: "Orientation",
			277: "SamplesPerPixel",
			278: "RowsPerStrip",
			279: "StripByteCounts",
			282: "XResolution",
			283: "YResolution",
			284: "PlanarConfiguration",
			296: "ResolutionUnit",
			301: "TransferFunction",
			305: "Software",
			306: "DateTime",
			315: "Artist",
			318: "WhitePoint",
			319: "PrimaryChromacities",
			342: "TransferRange",
			512: "JPEGProc",
			513: "JPEGInterchangeFormat",
			514: "JPEGInterchangeFormatLength",
			529: "YCbCrCoefficients",
			530: "YCbCrSubSampling",
			531: "YCbCrPositioning",
			532: "ReferenceBlackWhite",
			33421: "CFARepeatPatternDim",
			33422: "CFAPattern",
			33423: "BatteryLevel",
			33432: "Copyright",
			33434: "ExposureTime",
			33437: "FNumber",
			33723: "IPTC/NAA",
			34665: "ExifOffset",
			34675: "InterColorProfile",
			34850: "ExposureProgram",
			34852: "SpectralSensitivity",
			34853: "GPSInfo",
			34855: "ISOSpeedRatings",
			34856: "OECF",
			36864: "ExifVersion",
			36867: "DateTimeOriginal",
			36868: "DateTimeDigitized",
			37121: "ComponentsConfiguration",
			37122: "CompressedBitsPerPixel",
			37377: "ShutterSpeedValue",
			37378: "ApertureValue",
			37379: "BrightnessValue",
			37380: "ExposureBiasValue",
			37381: "MaxApertureValue",
			37382: "SubjectDistance",
			37383: "MeteringMode",
			37384: "LightSource",
			37385: "Flash",
			37386: "FocalLength",
			37500: "MakerNote",
			37510: "UserComment",
			37520: "SubSecTime",
			37521: "SubSecTimeOriginal",
			37522: "SubSecTimeDigitized",
			40960: "FlashPixVersion",
			40961: "ColorSpace",
			40962: "ExifImageWidth",
			40963: "ExifImageLength",
			40965: "InteroperabilityOffset",
			41483: "FlashEnergy",
			41484: "SpatialFrequencyResponse",
			41486: "FocalPlaneXResolution",
			41487: "FocalPlaneYResolution",
			41488: "FocalPlaneResolutionUnit",
			41492: "SubjectLocation",
			41493: "ExposureIndex",
			41495: "SensingMethod",
			41728: "FileSource",
			41729: "SceneType"
		};

		public static var INTEROP_TAGS:Object = {
			1: "InteroperabilityIndex",
			2: "InteroperabilityVersion",
			4096: "RelatedImageFileFormat",
			4097: "RelatedImageWidth",
			4098: "RelatedImageLength"
		}
		
		private var data:ByteArray = null;
		
		public function TIFF( data:ByteArray ) {
			this.data = data;

			if( data[0] == 73 ) {
				data.endian = Endian.LITTLE_ENDIAN;
			} else {
				data.endian = Endian.BIG_ENDIAN;				
			}	
		}
		
		public function first():uint {
			data.position = 4;
			return data.readUnsignedInt();
		}
		
		public function next( ifd:uint ):uint {
			var entries:uint = data.readUnsignedShort();
			
			data.position = ifd + 2 + 12 * entries;
			
			return data.readUnsignedInt();
		}
		
		public function list():Array {
			var offsets:Array = new Array();
			
			offsets.push( first() );
			offsets.push( next( offsets[0] ) );
			
			return offsets;	
		}
		
		public function dump( offset:uint ):Array {
			var all:Array = new Array();
			var types:Array = new Array( 1, 1, 2, 4, 8, 1, 1, 2, 4, 8 );
			var values:Object = null;
			var value:Object = null;
			var special:ByteArray = null;
			var rational:Fraction = null;
			var count:uint = 0;			
			var entries:uint = 0;
			var mark:uint = 0;
			var tag:uint = 0;
			var type:uint = 0;
			var typelen:int = 0;
			
			data.position = offset;
			entries = data.readUnsignedShort();
			
			for( var e:int = 0; e < entries; e++ ) {
				data.position = offset + 2 + 12 * e;
				
				tag = data.readUnsignedShort();
				type = data.readUnsignedShort();
				
				if( type <= 1 && type >= 10 ) {
					continue;	
				}
				
				typelen = types[ type - 1 ];
				count = data.readUnsignedInt();
		
				mark = 0;
		
				if( count * typelen > 4 ) {
					mark = data.readUnsignedInt();	
				}

				if( type == 2 ) {
					special = new ByteArray();					
					
					if( tag == 1 ) {
						data.readBytes( special, 0, Math.min( data.bytesAvailable, count * typelen ) );												
					} else {
						data.position = mark;
						data.readBytes( special, 0, Math.min( data.bytesAvailable, mark + count - 1 ) );						
					}
					
					values = special.toString();					
				} else {
					values = new Array();
					
					for( var j:int = 0; j < count; j++ ) {
						switch( type ) {
							case 1:
								value = data.readUnsignedByte();
								break;
								
							case 3:
								value = data.readUnsignedShort();
								break;
								
							case 4:
								value = data.readUnsignedInt();
								break;
								
							case 5:
								value = new Fraction();
								
								data.position = mark;
								value.setNumerator( data.readUnsignedInt() );
								value.setDenominator( data.readUnsignedInt() );								
								break;							

							case 7:
								if( mark != 0 ) {
									data.position = mark + j;
								}
								
								value = data.readUnsignedByte();
								break;
													
							case 10:
								value = new Fraction();
								
								data.position = mark;
								value.setNumerator( data.readInt() );
								value.setDenominator( data.readInt() );								
								break;
								
							case 6:
								value = data.readByte();
								break;
								
							case 8:
								value = data.readShort();
								break;
								
							case 9:
								value = data.readInt();
								break;
								
							case 11:
								value = data.readFloat();
								break;
								
							case 12:
								value = data.readDouble();
								break;
								
						}
						
						values.push( value );
					}
				}
				
				all.push( new IFD( tag, type, values ) );
			}
			
			return all;			
		}
		
		public function print( values:Array, tags:Object ):void {
			var types:Array = new Array( "B",  "A", "S",  "L",  "R",
										 "SB", "U", "SS", "SL", "SR" );
			var stag:String = null;
			
			for( var t:int = 0; t < values.length; t++ ) {
				if( values[t].getValues() is Array ) {
					if( values[t].getValues()[0] is Fraction ) {
						stag = "[" + values[t].getValues()[0].getNumerator() + "/" +
						       values[t].getValues()[0].getDenominator() + "]";
					} else {
						stag = "[" + values[t].getValues().join( "," ) + "]";
					}
				} else {
					stag = values[t].getValues().toString();						
				}
				
//				trace( tags[values[t].getTag()] + "(" + types[values[t].getType() - 1] + ")=" + stag );
			}										 
		}		
	}
}