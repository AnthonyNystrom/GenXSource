#include "EncoderObjectSis9.h"

#include "Graphics.h"
#include "FileSystem.h"
extern "C" 
{
	#include "jpeglib.h"
	#include "jinclude.h"
	#include "jerror.h"
	#include "jpegint.h"
}


//struct my_error_mgr
//{
//	jpeg_error_mgr	pub;			// "public" fields
//	jmp_buf			setjmp_buffer;	// for return to caller 
//};

// callbacks required by jpeglib

//
// Initialize source --- called by jpeg_read_header before any data is actually read.
//

static	void	InitSourceForBuffer ( j_decompress_ptr cinfo)
{

}
static	void	InitSourceForFile ( j_decompress_ptr cinfo)
{
	((EncoderObjectSis*)(cinfo->client_data))->currentFile->Seek(0, File::ESO_START);
}

static	void	InitDest ( j_compress_ptr compPtr)
{

}

//
// Fill the input buffer --- called whenever buffer is emptied. should never happen
//

static	boolean	FillInputBufferForBuffer ( j_decompress_ptr cinfo)
{
	return TRUE;
}

static	boolean	FillInputBufferForFile ( j_decompress_ptr cinfo)
{
	jpeg_source_mgr *src = cinfo->src;
	int nbytes;

	nbytes = ((EncoderObjectSis*)(cinfo->client_data))->currentFile->Read(((EncoderObjectSis*)(cinfo->client_data))->innerBuffer, INNER_BUFFER_SIZE);
	if (nbytes <= 0) 
	{
		//((EncoderObjectBrew*)(cinfo->client_data))->innerBuffer[0] = (JOCTET) 0xFF;
		//((EncoderObjectBrew*)(cinfo->client_data))->innerBuffer[1] = (JOCTET) JPEG_EOI;
		//nbytes = 2;
		//ERREXIT(cinfo, JERR_INPUT_EMPTY);
		src->next_input_byte = NULL;
		src->bytes_in_buffer = 0;
		return TRUE;
	}

	src->next_input_byte = ((EncoderObjectSis*)(cinfo->client_data))->innerBuffer;
	src->bytes_in_buffer = nbytes;
	return TRUE;
}


static	boolean	EmptyOutputBuffer ( j_compress_ptr cinfo)
{
	return FALSE;
}

//
// Skip data --- used to skip over a potentially large amount of
// uninteresting data (such as an APPn marker).
//

static	void	SkipInputDataForBuffer ( j_decompress_ptr cinfo, long count )
{
	jpeg_source_mgr * src = cinfo -> src;

	if ( count > 0 )
	{
		src -> bytes_in_buffer -= count;
		src -> next_input_byte += count;
	}
}

static	void	SkipInputDataForFile ( j_decompress_ptr cinfo, long count )
{
	jpeg_source_mgr * src = cinfo -> src;


	/* Just a dumb implementation for now.  Could use fseek() except
	* it doesn't work on pipes.  Not clear that being smart is worth
	* any trouble anyway --- large skips are infrequent.
	*/
	if (count > 0) 
	{
		while (count > (long) src->bytes_in_buffer) 
		{
			count -= (long) src->bytes_in_buffer;
			FillInputBufferForFile(cinfo);
		}
		src->next_input_byte += (int) count;
		src->bytes_in_buffer -= (int) count;
	}
}

//
// Terminate source --- called by jpeg_finish_decompress
// after all data has been read.  Often a no-op.
//
// NB: *not* called by jpeg_abort or jpeg_destroy; surrounding
// application must deal with any cleanup that should happen even
// for error exit.
//

static	void	TermSourceForBuffer ( j_decompress_ptr cinfo)
{

}

static	void	TermDest ( j_compress_ptr cinfo)
{

}

static void MyErrorExit (j_common_ptr cinfo)
{
	((EncoderObjectSis*)(cinfo->client_data))->isOk = false;
	cinfo->err->isError = true;
}


EncoderObjectSis::EncoderObjectSis( /*EncoderObjectListener *aListener*/ )
: EncoderObject(/*aListener*/)
{

	decompressInfo = new jpeg_decompress_struct;
	compressInfo = new jpeg_compress_struct;
	jpegErr = new jpeg_error_mgr;
	jsrc = new jpeg_source_mgr;
	jdst = new jpeg_destination_mgr;

	// allocate and initialize JPEG decompression object
	// we set up the normal JPEG error routines, then override error_exit


	compressInfo->err = decompressInfo->err		= jpeg_std_error ( jpegErr );
	compressInfo->err->error_exit = MyErrorExit;


	//// now we can initialize the JPEG decompression object
	jpeg_create_decompress ( decompressInfo );
	jpeg_create_compress(compressInfo);


}

EncoderObjectSis::~EncoderObjectSis()
{

	if (state == EES_DECOMPRESS || state == EES_DECOMPRESS_CANCELED || state == EES_RESIZE || state == EES_RESIZE_CANCELED)
	{
		jpeg_abort_decompress(decompressInfo);
	}

	if (state == EES_COMPRESS || state == EES_COMPRESS_CANCELED || state == EES_RESIZE || state == EES_RESIZE_CANCELED)
	{
		jpeg_abort_compress(compressInfo);
	}

	jpeg_destroy_decompress ( decompressInfo );
	jpeg_destroy_compress ( compressInfo );

	delete jpegErr;
	delete decompressInfo;
	delete compressInfo;
	delete jsrc;
	delete jdst;

}


const ImageInfo * EncoderObjectSis::GetInfo( char8 *dataBuffer, int32 bufferSize ) /*= 0*/
{
	
	//// set up data pointer
	SetSourceToBuffer(dataBuffer, bufferSize);


	//// read file parameters with jpeg_read_header()
	jpeg_read_header ( decompressInfo, TRUE );
	jpeg_abort_decompress(decompressInfo);

	if (!isOk)
	{
		info.width = 0;
		info.height = 0;
		return &info;
	}

	info.width = decompressInfo->image_width;
	info.height = decompressInfo->image_height;


	return &info;
}

const ImageInfo * EncoderObjectSis::GetInfo( File *file ) /*= 0*/
{

	//// set up data pointer
	SetSourceToFile(file);


	//// read file parameters with jpeg_read_header()
	jpeg_read_header ( decompressInfo, TRUE );
	jpeg_abort_decompress(decompressInfo);

	if (!isOk)
	{
		info.width = 0;
		info.height = 0;
		return &info;
	}


	info.width = decompressInfo->image_width;
	info.height = decompressInfo->image_height;


	return &info;
}

bool EncoderObjectSis::GetSurface( char8 *srcBuffer, int32 bufferSize, GraphicsSystem::Surface *destSurface, ImageInfo *pDestProps /*= NULL*/ ) /*= 0*/
{
	if (state != EES_IDLE)
	{
		return false;
	}
	state = EES_DECOMPRESS;


	//// set up data pointer
	SetSourceToBuffer(srcBuffer, bufferSize);


	LoadData(destSurface, pDestProps);
	if (!isOk)
	{
		jpeg_abort_decompress(decompressInfo);
		OnEncodingFailed();
		state = EES_IDLE;
		return false;
	}

	return true;
}

bool EncoderObjectSis::GetSurface( File *srcFile, GraphicsSystem::Surface *destSurface, ImageInfo *pDestProps /*= NULL*/ ) /*= 0*/
{
	if (state != EES_IDLE)
	{
		return false;
	}
	state = EES_DECOMPRESS;


	SetSourceToFile(srcFile);


	LoadData(destSurface, pDestProps);
	if (!isOk)
	{
		jpeg_abort_decompress(decompressInfo);
		OnEncodingFailed();
		state = EES_IDLE;
		return false;
	}

	return true;
}

bool EncoderObjectSis::GetData( GraphicsSystem::Surface *sourceSurface, char8 *destBuffer, int32 bufferSize ) /*= 0*/
{

	jdst->next_output_byte = (JOCTET*)destBuffer;
	jdst->free_in_buffer = bufferSize;
	jdst->init_destination = InitDest;
	jdst->empty_output_buffer = EmptyOutputBuffer;
	jdst->term_destination = TermDest;


	compressInfo->dest			= jdst;
	compressInfo->client_data	= this;	

	compressInfo->image_width = sourceSurface->width;
	compressInfo->image_height = sourceSurface->height;
	compressInfo->input_components = 3;
	compressInfo->in_color_space = JCS_RGB;
	jpeg_set_defaults(compressInfo);

	jpeg_start_compress(compressInfo, TRUE);


	JSAMPARRAY lineBuffer = (*compressInfo->mem->alloc_sarray) ((j_common_ptr) compressInfo, JPOOL_IMAGE, sourceSurface->width * 3, 1);


	// Here we use the library's state variable cinfo.output_scanline as the
	// loop counter, so that we don't have to keep track ourselves.


	int32	y = 0;

	GraphicsSystem *gr = GetApplication()->GetGraphicsSystem();
	while ( compressInfo->next_scanline < compressInfo->image_height )
	{
		uint16 *buf = (uint16*)(sourceSurface->pData) + y * sourceSurface->width;
		uint8 *ptr = (uint8*)*lineBuffer;
		for (int i = 0; i < sourceSurface->width; i++)
		{
			gr->Native2Rgb(*buf, ptr[0], ptr[1], ptr[2]);
			buf++;
			ptr += 3;
		}
	    jpeg_write_scanlines(compressInfo, lineBuffer, 1); 
		y++;
	}
	jpeg_finish_compress(compressInfo);

	int32 size = bufferSize - jdst->free_in_buffer;

	jpeg_abort_compress(compressInfo);

	return true;
}



void EncoderObjectSis::LoadData(GraphicsSystem::Surface *destSurface, ImageInfo *img)
{

	outWidth = destSurface->width;
	outHeight = destSurface->height;
	if (img)
	{
		if (img->width <= outWidth && img->height <= outHeight)
		{
			outWidth = img->width;
			outHeight = img->height;
		}
	}
	xAdd = (destSurface->width - outWidth) / 2;
	yAdd = (destSurface->height - outHeight) / 2;


	//// read file parameters with jpeg_read_header()
	jpeg_read_header ( decompressInfo, TRUE );
	if (!isOk)
	{
		return;
	}

	if (decompressInfo->image_width / outWidth >= 8)
	{
		decompressInfo->scale_denom = 8;
	}
	else if (decompressInfo->image_width / outWidth >= 4)
	{
		decompressInfo->scale_denom = 4;
	}
	else if (decompressInfo->image_width / outWidth >= 2)
	{
		decompressInfo->scale_denom = 2;
	}

	if ( decompressInfo->jpeg_color_space != JCS_GRAYSCALE )
	{
		decompressInfo->out_color_space = JCS_RGB;
	}

	// Recalculate output image dimensions
	jpeg_calc_output_dimensions ( decompressInfo );
	if (!isOk)
	{
		return;
	}

	// Start decompresser
	jpeg_start_decompress ( decompressInfo );
	if (!isOk)
	{
		return;
	}

	int32		width       = decompressInfo->output_width;
	int32		height      = decompressInfo->output_height;
	int32		rowSpan     = decompressInfo->image_width * decompressInfo->num_components;
	bool	isGreyScale = decompressInfo->jpeg_color_space == JCS_GRAYSCALE;



	// Make a one-row-high sample array that will go away when done with image
	jBuffer = (*decompressInfo->mem->alloc_sarray) ((j_common_ptr) decompressInfo, JPOOL_IMAGE, rowSpan, 1);

	// Here we use the library's state variable cinfo.output_scanline as the
	// loop counter, so that we don't have to keep track ourselves.

	fxHeight = Int2Fx(0);
	widthDx =  FxDiv(Int2Fx(outWidth), Int2Fx(width));
	heightDx = FxDiv(Int2Fx(outHeight), Int2Fx(height));


	yBorder = -1;

	surface = destSurface;


}


void EncoderObjectSis::DecompressLines()
{
	GraphicsSystem *gr = GetApplication()->GetGraphicsSystem();
	gr->SetCurrentSurface(surface);

	int32 toDecompress = DECOMPRESS_PER_UPDATE;

	while ( decompressInfo->output_scanline < decompressInfo->output_height )
	{
		if (toDecompress <= 0)
		{
			gr->SetBlendMode(GraphicsSystem::EBM_NONE);
			gr->SetCurrentSurface(NULL);
			return;
		}
		// jpeg_read_scanlines expects an array of pointers to scanlines.
		// Here the array is only one element long, but you could ask for
		// more than one scanline at a time if that's more convenient.

		Fixed fxWidth = Int2Fx(0);

		int32 linesRead = jpeg_read_scanlines ( decompressInfo, (JSAMPARRAY)jBuffer, 1 );
		if (!isOk)
		{
			gr->SetBlendMode(GraphicsSystem::EBM_NONE);
			gr->SetCurrentSurface(NULL);
			OnEncodingFailed();
			state = EES_IDLE;
			return;
		}
		toDecompress -= linesRead * decompressInfo->output_width;

		int32 heightFrom = Fx2Int(fxHeight);
		fxHeight += heightDx;
		int32 heightTo = MIN(Fx2Int(fxHeight), outHeight - 1);
		gr->SetAlpha(MAX_ALPHA>>1);
		while (heightFrom <= heightTo)
		{
			Fixed fxWidth = Int2Fx(0);
			uint8 *ptr = (uint8*)*((JSAMPARRAY)jBuffer);
			int32 xBorder = -1;
			bool isAlpha = false;
			if (heightFrom <= yBorder)
			{
				isAlpha = true;
			}
			else
			{
				yBorder = heightFrom;
			}
			for ( int i = decompressInfo->output_width; i > 0; i-- )
			{
				int32 widthFrom = Fx2Int(fxWidth);
				fxWidth += widthDx;
				int32 widthTo = MIN(Fx2Int(fxWidth), outWidth - 1);
				while (widthFrom <= widthTo)
				{
					if (isAlpha)
					{
						gr->SetBlendMode(GraphicsSystem::EBM_ALPHA);
					}
					else
					{
						if (widthFrom <= xBorder)
						{
							gr->SetBlendMode(GraphicsSystem::EBM_ALPHA);
						}
						else
						{
							xBorder = widthFrom;
							gr->SetBlendMode(GraphicsSystem::EBM_NONE);
						}
					}
					if ( decompressInfo->output_components == 1 )		// paletted or grayscale image
					{
						if ( decompressInfo->quantize_colors )		// paletted image
						{
							gr->SetColor(decompressInfo->colormap [2][*ptr], decompressInfo->colormap [1][*ptr], decompressInfo->colormap [0][*ptr]);
							gr->DrawPixel(widthFrom + xAdd, heightFrom + yAdd);
						}
						else								// grayscale image
						{ 
							gr->SetColor(*ptr, *ptr, *ptr);
							gr->DrawPixel(widthFrom + xAdd, heightFrom + yAdd);
						}
					}
					else									// rgb image
					{
						gr->SetColor(ptr [0], ptr [1], ptr [2]);
						gr->DrawPixel(widthFrom + xAdd, heightFrom + yAdd);
					}

					widthFrom++;
				}
				if ( decompressInfo->output_components == 1 )		// paletted or grayscale image
				{
					ptr++;
				}
				else
				{
					ptr += 3;
				}
			}

			heightFrom++;
		}

	}

	gr->SetBlendMode(GraphicsSystem::EBM_NONE);
	gr->SetCurrentSurface(NULL);

	// Finish decompression
	jpeg_finish_decompress ( decompressInfo );

	jpeg_abort_decompress(decompressInfo);



	state = EES_IDLE;

	OnEncodingSuccess(0);
}


void EncoderObjectSis::SetSourceToBuffer( char8 *buffer, int32 size )
{
	isOk = true;

	jsrc->bytes_in_buffer   = size;
	jsrc->next_input_byte   = (JOCTET*)buffer;
	jsrc->init_source = InitSourceForBuffer;
	jsrc->fill_input_buffer = FillInputBufferForBuffer;
	jsrc->skip_input_data = SkipInputDataForBuffer;
	jsrc->resync_to_restart = jpeg_resync_to_restart; /* use default method */
	jsrc->term_source = TermSourceForBuffer;

	decompressInfo->src            = jsrc;
	decompressInfo->client_data	= this;
}

void EncoderObjectSis::SetSourceToFile( File *file )
{
	isOk = true;

	currentFile = file;
	jsrc->bytes_in_buffer   = 0;
	jsrc->next_input_byte   = NULL;
	jsrc->init_source = InitSourceForFile;
	jsrc->fill_input_buffer = FillInputBufferForFile;
	jsrc->skip_input_data = SkipInputDataForFile;
	jsrc->resync_to_restart = jpeg_resync_to_restart; /* use default method */
	jsrc->term_source = TermSourceForBuffer;

	decompressInfo->src            = jsrc;
	decompressInfo->client_data	= this;
}

void EncoderObjectSis::Update()
{
	if (state == EES_DECOMPRESS)
	{
		DecompressLines();
	}
	if (state == EES_DECOMPRESS_CANCELED)
	{
		jpeg_abort_decompress(decompressInfo);

		state = EES_IDLE;

		OnEncodingCanceled();
	}
	if (state == EES_RESIZE)
	{
		ResizeLines();
	}
	if (state == EES_RESIZE_CANCELED)
	{
		jpeg_abort_decompress(decompressInfo);
		jpeg_abort_compress(compressInfo);

		state = EES_IDLE;

		OnEncodingCanceled();
	}
}

void EncoderObjectSis::Cancel()
{
	if (state == EES_DECOMPRESS)
	{
		state = EES_DECOMPRESS_CANCELED;
	}
	if (state == EES_RESIZE)
	{
		state = EES_RESIZE_CANCELED;
	}
}

bool EncoderObjectSis::IsFree()
{
	return state == EES_IDLE;
}

bool EncoderObjectSis::Resize( File *srcFile, char8 *destBuffer, int32 bufferSize, ImageInfo *pDestProps )
{
	if (state != EES_IDLE)
	{
		return false;
	}
	state = EES_RESIZE;

	compressBufferSize = bufferSize;

	SetSourceToFile(srcFile);


	outWidth = pDestProps->width;
	outHeight = pDestProps->height;


	//// read file parameters with jpeg_read_header()
	jpeg_read_header ( decompressInfo, TRUE );
	if (!isOk)
	{
		return false;
	}

	if (decompressInfo->image_width / outWidth >= 8)
	{
		decompressInfo->scale_denom = 8;
	}
	else if (decompressInfo->image_width / outWidth >= 4)
	{
		decompressInfo->scale_denom = 4;
	}
	else if (decompressInfo->image_width / outWidth >= 2)
	{
		decompressInfo->scale_denom = 2;
	}

	if ( decompressInfo->jpeg_color_space != JCS_GRAYSCALE )
	{
		decompressInfo->out_color_space = JCS_RGB;
	}

	// Recalculate output image dimensions
	jpeg_calc_output_dimensions ( decompressInfo );
	if (!isOk)
	{
		jpeg_abort_decompress(decompressInfo);
		OnEncodingFailed();
		state = EES_IDLE;
		return false;
	}

	// Start decompresser
	jpeg_start_decompress ( decompressInfo );
	if (!isOk)
	{
		jpeg_abort_decompress(decompressInfo);
		OnEncodingFailed();
		state = EES_IDLE;
		return false;
	}

	int32		width       = decompressInfo->output_width;
	int32		height      = decompressInfo->output_height;
	int32		rowSpan     = decompressInfo->image_width * decompressInfo->num_components;
	bool	isGreyScale = decompressInfo->jpeg_color_space == JCS_GRAYSCALE;



	// Make a one-row-high sample array that will go away when done with image
	jBuffer = (*decompressInfo->mem->alloc_sarray) ((j_common_ptr) decompressInfo, JPOOL_IMAGE, rowSpan, 1);

	jOutBuffer = (*compressInfo->mem->alloc_sarray) ((j_common_ptr) compressInfo, JPOOL_IMAGE, outWidth * 3, 1);

	// Here we use the library's state variable cinfo.output_scanline as the
	// loop counter, so that we don't have to keep track ourselves.

	fxHeight = Int2Fx(0);
	widthDx =  FxDiv(Int2Fx(outWidth), Int2Fx(width));
	heightDx = FxDiv(Int2Fx(outHeight), Int2Fx(height));


	yBorder = -1;

	jdst->next_output_byte = (JOCTET*)destBuffer;
	jdst->free_in_buffer = bufferSize;
	jdst->init_destination = InitDest;
	jdst->empty_output_buffer = EmptyOutputBuffer;
	jdst->term_destination = TermDest;


	compressInfo->dest			= jdst;
	compressInfo->client_data	= this;	

	compressInfo->image_width = outWidth;
	compressInfo->image_height = outHeight;
	compressInfo->input_components = 3;
	compressInfo->in_color_space = JCS_RGB;
	jpeg_set_defaults(compressInfo);

	jpeg_start_compress(compressInfo, TRUE);


	return true;
}




void EncoderObjectSis::ResizeLines()
{

	int32 toDecompress = DECOMPRESS_PER_UPDATE;

	while ( decompressInfo->output_scanline < decompressInfo->output_height )
	{
		if (toDecompress <= 0)
		{
			return;
		}
		// jpeg_read_scanlines expects an array of pointers to scanlines.
		// Here the array is only one element long, but you could ask for
		// more than one scanline at a time if that's more convenient.

		Fixed fxWidth = Int2Fx(0);

		int32 linesRead = jpeg_read_scanlines ( decompressInfo, (JSAMPARRAY)jBuffer, 1 );
		if (!isOk)
		{
			OnEncodingFailed();
			state = EES_IDLE;
			return;
		}
		toDecompress -= linesRead * decompressInfo->output_width;

		int32 heightFrom = Fx2Int(fxHeight);
		fxHeight += heightDx;
		int32 heightTo = MIN(Fx2Int(fxHeight), outHeight - 1);
		while (heightFrom <= heightTo)
		{
			Fixed fxWidth = Int2Fx(0);
			uint8 *ptr = (uint8*)*((JSAMPARRAY)jBuffer);
			for ( int i = decompressInfo->output_width; i > 0; i-- )
			{
				int32 widthFrom = Fx2Int(fxWidth);
				fxWidth += widthDx;
				int32 widthTo = MIN(Fx2Int(fxWidth), outWidth - 1);
				while (widthFrom <= widthTo)
				{
					if ( decompressInfo->output_components == 1 )		// paletted or grayscale image
					{
						if ( decompressInfo->quantize_colors )		// paletted image
						{
							((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3] = decompressInfo->colormap [2][*ptr];
							((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3 + 1] = decompressInfo->colormap [1][*ptr];
							((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3 + 2] = decompressInfo->colormap [0][*ptr];
						}
						else								// grayscale image
						{ 
							((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3] = *ptr;
							((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3 + 1] = *ptr;
							((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3 + 2] = *ptr;
						}
					}
					else									// rgb image
					{
						((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3] = ptr[0];
						((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3 + 1] = ptr[1];
						((uint8*)*((JSAMPARRAY)jOutBuffer))[widthFrom * 3 + 2] = ptr[2];
					}

					widthFrom++;
				}
				if ( decompressInfo->output_components == 1 )		// paletted or grayscale image
				{
					ptr++;
				}
				else
				{
					ptr += 3;
				}
			}

			if (yBorder < heightFrom)
			{
				jpeg_write_scanlines(compressInfo, (JSAMPARRAY)jOutBuffer, 1);
				yBorder = heightFrom;
			}

			heightFrom++;
		}

	}


	// Finish decompression
	jpeg_finish_decompress ( decompressInfo );

	jpeg_finish_compress(compressInfo);

	int32 size = compressBufferSize - jdst->free_in_buffer;

	jpeg_abort_compress(compressInfo);

	jpeg_abort_decompress(decompressInfo);



	state = EES_IDLE;

	OnEncodingSuccess(size);
}