#include "TextBalancer.h"
#include "GUIText.h"
#include "Font.h"

TextBalancer::TextBalancer( GUIText *textToBalance )
{
	FASSERT(textToBalance);
	text = textToBalance;
	Reset();

}

TextBalancer::~TextBalancer()
{

}

void TextBalancer::Reset()
{
	state = ETBS_PAUSED;
	waitCounter = 0;
}

void TextBalancer::Update()
{
	if (text->align & Font::EAP_HCENTER  || text->align & Font::EAP_RIGHT)
	{
		text->align &= ~Font::EAP_HCENTER;
		text->align &= ~Font::EAP_RIGHT;
		text->align |= Font::EAP_LEFT;
		text->dx = text->marginLeft;
	}

	text->align |= Font::EAP_POINTS;

	switch(state)
	{
	case ETBS_PAUSED:
		{
			waitCounter++;
			if (waitCounter > 60)
			{
				state = ETBS_MOVE_TO_SHOW;
			}
		}
		break;
	case ETBS_MOVE_TO_SHOW:
		{
			text->dx--;
			if (text->dx + text->textWidth < text->GetRect().dx >> 1)
			{
				state = ETBS_MOVE_BACK;
			}
			text->Invalidate();
		}
		break;
	case ETBS_MOVE_BACK:
		{
			if (text->dx != text->marginLeft)
			{
				int32 spd = (text->marginLeft - text->dx) / 4;
				if (text->marginLeft > text->dx)
				{
					spd++;
					text->dx += spd;
					if (text->marginLeft < text->dx)
					{
						text->dx = text->marginLeft;
					}
				}
				else
				{
					spd--;
					text->dx += spd;
					if (text->marginLeft > text->dx)
					{
						text->dx = text->marginLeft;
					}
				}
			}
			else
			{
				state = ETBS_PAUSED;
				waitCounter = 0;
			}
			text->Invalidate();
		}
		break;
	}
}