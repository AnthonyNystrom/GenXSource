#include "EncoderObject.h"
#include "EncoderObjectWM.h"


EncoderObject * EncoderObject::Create()
{
	return new EncoderObjectWM();
}

EncoderObject::EncoderObject()
{
}

EncoderObject::~EncoderObject()
{

}

void EncoderObject::SetListener( EncoderListener *newLisener )
{
	listener = newLisener;
}

void EncoderObject::OnEncodingSuccess(int32 size)
{
	if (listener)
	{
		listener->OnEncodingSuccess(size);
	}
}

void EncoderObject::OnEncodingCanceled()
{
	if (listener)
	{
		listener->OnEncodingCanceled();
	}
}

void EncoderObject::OnEncodingFailed()
{
	if (listener)
	{
		listener->OnEncodingFailed();
	}
}