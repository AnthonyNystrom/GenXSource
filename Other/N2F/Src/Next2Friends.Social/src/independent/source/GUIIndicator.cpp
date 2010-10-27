#include "GUIIndicator.h"
#include "GUISystem.h"
#include "GUISkinLocal.h"
#include "VList.h"


GUIIndicator::GUIIndicator( GUIControlContainer *parent /*= NULL*/, const ControlRect &cr/* = ControlRect()*/ )
: GUIControlContainer(parent, cr)
{

}

GUIIndicator::~GUIIndicator( void )
{

}

void GUIIndicator::Show()
{
	if (indicatorDelegate)
	{
		indicatorDelegate->IndicatorMustShow(this);
	}
}

void GUIIndicator::Hide()
{
	if (indicatorDelegate)
	{
		indicatorDelegate->IndicatorMustHide(this);
	}
}

void GUIIndicator::SetDelegate( IndicatorDelegate *pDelegate )
{
	indicatorDelegate = pDelegate;
}

GUIIndicator::eIndicatorType GUIIndicator::GetType()
{
	return type;
}