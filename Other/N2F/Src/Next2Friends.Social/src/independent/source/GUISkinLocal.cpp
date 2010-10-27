#include "GUISkinLocal.h"
#include "Framework.h"
#include "Sprite.h"

GUISkinLocal::GUISkinLocal()
{
	graphicsSystem = GetApplication()->GetGraphicsSystem();
}

GUISkinLocal::~GUISkinLocal()
{
	if (!controlStyleList.Empty())
	{
		for (VList::Iterator styleIt = controlStyleList.Begin(); styleIt != controlStyleList.End(); styleIt++ )
		{
					int32 i = 0;
					GUISkinLocal::TypeTilesStyle* style = ((GUISkinLocal::TypeTilesStyle*)(*styleIt));
					while (i < drawSchemeCount[style->scheme])
					{
						if (i == ESS9_CENTER_CENTER && (style->scheme == ES_9_GRADIENT || style->scheme == ES_FILL || style->scheme == ES_9_FILL || style->scheme == ES_LINE_BOTTOM))
						{
							Gradient *pGr = (Gradient*)style->spriteArray[i];
							SAFE_DELETE(pGr);
							i++;
							continue;
						}
						if (style->spriteArray[i])
						{
							style->spriteArray[i]->Release();
						}
						i++;
					}
					SAFE_DELETE(style->spriteArray);

					SAFE_DELETE(style);
		}
	}
	if (!fontList.Empty())
	{
		for (VList::Iterator styleIt = fontList.Begin(); styleIt != fontList.End(); styleIt++ )
		{
			int32 i = 0;
			GUISkinLocal::TypeFontStyle* style = ((GUISkinLocal::TypeFontStyle*)(*styleIt));
			style->pFont->Release();

			SAFE_DELETE(style);
		}
	}

}


GUISkinLocal::TypeTilesStyle *GUISkinLocal::GetTiles(eDrawType drawType)
{
	if (controlStyleList.Empty())
	{
		return NULL;
	}
	for (VList::Iterator styleIt = controlStyleList.Begin(); styleIt != controlStyleList.End(); styleIt++ )
	{
		eDrawType drwType = ((GUISkinLocal::TypeTilesStyle*)(*styleIt))->drawType;
		if (drwType == drawType)
		{	
			return (GUISkinLocal::TypeTilesStyle*)(*styleIt);
		}
	}
	return (GUISkinLocal::TypeTilesStyle*)(*(controlStyleList.Begin()));
}

Font * GUISkinLocal::GetFont( eDrawType drawType, uint8 &palette /*= NULL*/ )
{
	if (fontList.Empty())
	{
		return NULL;
	}
	GUISkinLocal::TypeFontStyle *fnt = NULL;
	for (VList::Iterator styleIt = fontList.Begin(); styleIt != fontList.End(); styleIt++ )
	{
		eDrawType drwType = ((GUISkinLocal::TypeTilesStyle*)(*styleIt))->drawType;
		if (drwType == drawType)
		{	
			fnt = (GUISkinLocal::TypeFontStyle*)(*(styleIt));
			break;
		}
	}
	if (!fnt)
	{
		fnt = (GUISkinLocal::TypeFontStyle*)(*(fontList.Begin()));
	}

	palette = fnt->palette;
	return fnt->pFont;
}




void GUISkinLocal::SetTiles(eDrawType drawType, Sprite **spriteArray
								   , uint32 frame, GUISkinLocal::eDrawScheme drawScheme, uint8 palNumber/* = 0*/)
{
	int32 i = 0;
	Sprite **ppSprArray;
	if (drawScheme != ES_NONE)
	{
		ppSprArray = new Sprite*[drawSchemeCount[drawScheme]];
	}
	else
	{
		ppSprArray = NULL;
	}
	while (i < drawSchemeCount[drawScheme])
	{
		if (i == ESS9_CENTER_CENTER && (drawScheme == ES_9_GRADIENT || drawScheme == ES_FILL || drawScheme == ES_9_FILL || drawScheme == ES_LINE_BOTTOM))
		{
			ppSprArray[i] = spriteArray[i];
			i++;
			continue;
		}
		ppSprArray[i] = spriteArray[i];
		if (ppSprArray[i])
		{
			ppSprArray[i]->AddReference();
		}
		i++;
	}
	if (!controlStyleList.Empty())
	{
		for (VList::Iterator styleIt = controlStyleList.Begin(); styleIt != controlStyleList.End(); styleIt++ )
		{
			GUISkinLocal::TypeTilesStyle* st= (GUISkinLocal::TypeTilesStyle*)(*styleIt);
			if (st->drawType == drawType)
			{
				st->frame = (uint8)frame;
				i = 0;
				while (i < drawSchemeCount[st->scheme])
				{
					if (i == ESS9_CENTER_CENTER && (drawScheme == ES_9_GRADIENT || drawScheme == ES_FILL || drawScheme == ES_9_FILL || drawScheme == ES_LINE_BOTTOM))
					{
						ppSprArray[i] = spriteArray[i];
						i++;
						continue;
					}
					if (st->spriteArray[i])
					{
						st->spriteArray[i]->Release();
					}
					i++;
				}
				SAFE_DELETE(st->spriteArray);
				st->spriteArray = ppSprArray;
				st->scheme = drawScheme;
				st->palette = palNumber;
				return;
			}
		}
	}

	GUISkinLocal::TypeTilesStyle *newType = new GUISkinLocal::TypeTilesStyle();
	newType->drawType = drawType;
	newType->frame = (uint8)frame;
	newType->spriteArray = ppSprArray;
	newType->scheme = drawScheme;
	newType->palette = palNumber;

	if (drawType == EDT_DEFAULT)
	{
		controlStyleList.PushFront(newType);
		return;
	}
	controlStyleList.PushBack(newType);
}


void GUISkinLocal::SetFont( eDrawType drawType, Font *font, uint8 palNumber /*= 0*/ )
{
	int32 i = 0;
	font->AddReference();
	if (!fontList.Empty())
	{
		for (VList::Iterator styleIt = fontList.Begin(); styleIt != fontList.End(); styleIt++ )
		{
			if (((GUISkinLocal::TypeFontStyle*)(*styleIt))->drawType == drawType)
			{
				((GUISkinLocal::TypeFontStyle*)(*styleIt))->pFont->Release();
				((GUISkinLocal::TypeFontStyle*)(*styleIt))->pFont = font;
				((GUISkinLocal::TypeFontStyle*)(*styleIt))->palette = palNumber;
				return;
			}
		}
	}

	GUISkinLocal::TypeFontStyle *newType = new GUISkinLocal::TypeFontStyle();
	newType->drawType = drawType;
	newType->pFont = font;
	newType->palette = palNumber;

	if (drawType == EDT_DEFAULT)
	{
		fontList.PushFront(newType);
		return;
	}
	fontList.PushBack(newType);
}


void GUISkinLocal::DrawControl(eDrawType drawType, const Rect & rect)
{
	GUISkinLocal::TypeTilesStyle *cs = GetTiles(drawType);
	if (!cs)
	{
		return;
	}
	DrawTiled(cs, rect);
}


void GUISkinLocal::DrawTiled (GUISkinLocal::TypeTilesStyle *controlStyle, const Rect &rect)
{
	int32 clipX, clipDX, clipY, clipDY;
	graphicsSystem->GetClip(&clipX, &clipY, &clipDX, &clipDY);


	
	for (int32 i = 0; i < drawSchemeCount[controlStyle->scheme]; i++)
	{//выставл€ем необходимую палитру дл€ спрайтов
		if (i == ESS9_CENTER_CENTER && (controlStyle->scheme == ES_9_GRADIENT || controlStyle->scheme == ES_FILL || controlStyle->scheme == ES_9_FILL || controlStyle->scheme == ES_LINE_BOTTOM))
		{
			continue;
		}
		if (controlStyle->spriteArray[i])
		{
			controlStyle->spriteArray[i]->SetCurrentPalette(controlStyle->palette);
		}
	}

	int32 frame = controlStyle->frame;
	switch (controlStyle->scheme)
	{
	case GUISkinLocal::ES_NONE:
		return;
	case GUISkinLocal::ES_1:
		{
			controlStyle->spriteArray[0]->Draw
				(
				rect.x + ((rect.dx - controlStyle->spriteArray[0]->GetWidth()) >> 1),
				rect.y + ((rect.dy - controlStyle->spriteArray[0]->GetHeight()) >> 1),
				frame
				);

		}break;

	case GUISkinLocal::ES_V3:
		{
			//width of  sprite
			int32 topWidth = controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->GetWidth();
			//height of top sprite
			int32 topHeight = controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->GetHeight();
			int32 bottomHeight = controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->GetHeight();

			//height of top and central parts
			int32 htc = rect.dy - bottomHeight;

			//y coordinate of top part
			int32 yt = rect.y;
			//y coordinate of bottom part
			int32 yb = rect.y + htc;

			int32 xc = rect.x + ((rect.dx - controlStyle->spriteArray[GUISkinLocal::ESSV3_CENTER]->GetWidth()) >> 1);
			int32 xt = rect.x + ((rect.dx - controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->GetWidth()) >> 1);
			int32 xb = rect.x + ((rect.dx - controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->GetWidth()) >> 1);


			{//draw center part
				//height of central part
				int32 hc = htc - topHeight;
				//y coordinate of current middle part
				int32 yc = yt + topHeight;


				Sprite *cntr = controlStyle->spriteArray[GUISkinLocal::ESSV3_CENTER];

				//height of central part
				int32 centerHeight = cntr->GetHeight();

				graphicsSystem->SetClipIntersect(clipX, yc, clipDX, yb - yc);

				//truncate num of middle parts
				int32 n = hc / centerHeight;
				if (yc < clipY)
				{
					int32 hn = (clipY - yc) / centerHeight;
					yc += centerHeight * hn;
					n -= hn;
				}

				if (yc + n * centerHeight > clipY + clipDY)
				{
					int32 hn = ((yc + n * centerHeight) - (clipY + clipDY)) / centerHeight;

					if (hn > 0)
					{
						n -= hn;
					}
				}

				for (int32 i = 0; i <= n; ++i)
				{//draw cycle
					cntr->Draw(xc, yc, frame);
					yc += centerHeight;
				}
				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
			}

			//draw top and bottom parts
			int32 over = rect.dy - controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->GetHeight();
			if (over < 0)
			{
				over = -over;
				int32 cl = over>>1;
				graphicsSystem->SetClipIntersect(xt, yt, controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->GetWidth(), controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->GetHeight() - cl);
				controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->Draw(xt, yt, frame);
				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
				
				cl = over - (over>>1);
				graphicsSystem->SetClipIntersect(xb, yb + cl, controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->GetWidth(), controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->GetHeight());
				controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->Draw(xb, yb, frame);
				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
			}
			else
			{
				controlStyle->spriteArray[GUISkinLocal::ESSV3_TOP]->Draw(xt, yt, frame);
				controlStyle->spriteArray[GUISkinLocal::ESSV3_BOTTOM]->Draw(xb, yb, frame);
			}

		}
		break;

	case GUISkinLocal::ES_H3:
		{
			//height of control sprites
			int32 sprHeight = controlStyle->spriteArray[GUISkinLocal::ESSH3_CENTER]->GetHeight();

			//y coordinate of vertical aligned control
			int32 y = rect.y + ( (rect.dy - sprHeight) >> 1 );

			//x coordinate of left sprite
			int32 xl = rect.x;

			//width of left and central parts
			int32 wlc = rect.dx - controlStyle->spriteArray[GUISkinLocal::ESSH3_RIGHT]->GetWidth();

			//x coordinate of right sprite
			int32 xr = xl + wlc;

			{//draw center part
				//width of central part
				int32 wc = wlc - controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->GetWidth();

				Sprite *cntr = controlStyle->spriteArray[GUISkinLocal::ESSH3_CENTER];
				//truncate num of middle parts
				int32 cw = cntr->GetWidth();
				int32 n = wc / cw;

				//x coordinate of current middle part
				int32 xc = xl + controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->GetWidth();
				graphicsSystem->SetClipIntersect(xc, clipY, xr - xc, clipDY);
				if (xc < clipX)
				{
					int32 wn = (clipX - xc) / cw;
					xc += cw * wn;
					n -= wn;
				}

				if (xc + n * cw > clipX + clipDX)
				{
					int32 wn = ((xc + n * cw) - (clipX + clipDX)) / cw;

					if (wn > 0)
					{
						n -= wn;
					}
				}

				for (int32 i = 0; i <= n; ++i)
				{//draw cycle
					cntr->Draw(xc, y, frame);
					xc += cw;
				}

				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
			}

			//draw left and right parts
			int32 over = rect.dx - controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESSH3_RIGHT]->GetWidth();
			if (over < 0)
			{
				over = -over;
				int32 cl = over>>1;
				graphicsSystem->SetClipIntersect(xl, y, controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->GetWidth() - cl, controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->GetHeight());
				controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->Draw(xl, y, frame);
				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);

				cl = over - (over>>1);
				graphicsSystem->SetClipIntersect(xr + cl, y, controlStyle->spriteArray[GUISkinLocal::ESSH3_RIGHT]->GetWidth(), controlStyle->spriteArray[GUISkinLocal::ESSH3_RIGHT]->GetHeight());
				controlStyle->spriteArray[GUISkinLocal::ESSH3_RIGHT]->Draw(xr, y, frame);
				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
			}
			else
			{
				controlStyle->spriteArray[GUISkinLocal::ESSH3_LEFT]->Draw(xl, y, frame);
				controlStyle->spriteArray[GUISkinLocal::ESSH3_RIGHT]->Draw(xr, y, frame);
			}
		}
		break;

	case GUISkinLocal::ES_9:
	case GUISkinLocal::ES_9_FILL:
	case GUISkinLocal::ES_9_GRADIENT:
	case GUISkinLocal::ES_9_TOP:
	case GUISkinLocal::ES_9_BOTTOM:
	case GUISkinLocal::ES_9_LEFT:
	case GUISkinLocal::ES_9_RIGHT:
	case GUISkinLocal::ES_9_BORDER:
		{


			if (controlStyle->scheme == GUISkinLocal::ES_9 || controlStyle->scheme == GUISkinLocal::ES_9_GRADIENT || controlStyle->scheme == GUISkinLocal::ES_9_FILL)
			{//draw central part

				//y coordinate of current middle part
				int32 yt = MIN(
					MIN(controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER]->GetHeight()
					, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight())
					, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetHeight()
					);
				int32 yc = rect.y + yt;
				int32 clw = MIN(
					MIN(controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT]->GetWidth()
					, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetWidth())
					, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetWidth()
					);

				if (controlStyle->scheme != GUISkinLocal::ES_9_GRADIENT && controlStyle->scheme != GUISkinLocal::ES_9_FILL)
				{
					Sprite *s_cc = controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_CENTER];
					int32 ccw = s_cc->GetWidth();
					int32 cch = s_cc->GetHeight();

					int32 xl = rect.x + clw;

					int32 nx = (rect.dx - clw - MIN(
						MIN(controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_RIGHT]->GetWidth()
						, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth())
						, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth()
						)) / ccw;
					int32 ny = (rect.dy - yt - MIN(
						MIN(controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_CENTER]->GetHeight()
						, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight())
						, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight()
						)) / cch;

					graphicsSystem->SetClipIntersect(
						rect.x + controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT]->GetWidth()
						, rect.y + controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER]->GetHeight()
						, rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_RIGHT]->GetWidth()
						, rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_CENTER]->GetHeight() );

					int32 xc;
					if (yc < clipY)
					{
						int32 hn = (clipY - yc) / cch;
						yc += cch * hn;
						ny -= hn;
					}

					if (yc + ny * cch > clipY + clipDY)
					{
						int32 hn = ((yc + ny * cch) - (clipY + clipDY)) / cch;

						if (hn > 0)
						{
							ny -= hn;
						}
					}
					if (xl < clipX)
					{
						int32 wn = (clipX - (rect.x + clw)) / ccw;
						xl += ccw * wn;
						nx -= wn;
					}

					if (xl + nx * ccw > clipX + clipDX)
					{
						int32 wn = ((xl + nx * ccw) - (clipX + clipDX)) / ccw;

						if (wn > 0)
						{
							nx -= wn;
						}
					}

					for (int32 i = 0; i <= ny; ++i)
					{
						//x coordinate of current middle part
						xc = xl;

						for (int32 j = 0; j <= nx; ++j)
						{
							s_cc->Draw(xc, yc, frame);
							xc += ccw;
						}

						yc += cch;
					}
					graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
				}
				else
				{
					Rect grRect(rect.x + controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT]->GetWidth()
						, rect.y + controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER]->GetHeight()
						, rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_RIGHT]->GetWidth()
						, rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_CENTER]->GetHeight() + 1 );
					grRect.dy--;
					Gradient *gr = (Gradient*)controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_CENTER];
					if (controlStyle->scheme == GUISkinLocal::ES_9_FILL)
					{
						graphicsSystem->SetColor(gr->startR, gr->startG, gr->startB);
						if (gr->alpha)
						{
							graphicsSystem->SetAlpha(gr->alpha);
							graphicsSystem->SetBlendMode(GraphicsSystem::EBM_ALPHA);
							graphicsSystem->FillRect(grRect.x, grRect.y, grRect.dx, grRect.dy);
							graphicsSystem->SetBlendMode(GraphicsSystem::EBM_NONE);
						}
						else
						{
							graphicsSystem->FillRect(grRect.x, grRect.y, grRect.dx, grRect.dy);
						}
					}
					else
					{
						graphicsSystem->SetColor(gr->startR, gr->startG, gr->startB);
						if (gr->alpha)
						{
							graphicsSystem->SetAlpha(gr->alpha);
							graphicsSystem->SetBlendMode(GraphicsSystem::EBM_ALPHA);
							graphicsSystem->FillGradientV(grRect, gr->endR, gr->endG, gr->endB);
							graphicsSystem->SetBlendMode(GraphicsSystem::EBM_NONE);
						}
						else
						{
							graphicsSystem->FillGradientV(grRect, gr->endR, gr->endG, gr->endB);
						}
					}
				}
			}

			if (controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM && controlStyle->scheme != GUISkinLocal::ES_9_TOP)
			{//draw central-left & central-right parts

				//y coordinate of current middle part

				int32 over = 0;
				if (controlStyle->scheme != GUISkinLocal::ES_9_LEFT && controlStyle->scheme != GUISkinLocal::ES_9_RIGHT)
				{
					int32 over = rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_RIGHT]->GetWidth();
					if (over < 0)
					{
						over = -over;
					}
					else
					{
						over = 0;
					}
				}
				Sprite *s_cl = controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_LEFT];
				if ( controlStyle->scheme != GUISkinLocal::ES_9_RIGHT
					&& !(rect.x + s_cl->GetWidth() < clipX || rect.x > clipX + clipDX))
				{
					int32 yc = rect.y + controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight();

					int32 clh = s_cl->GetHeight();
					int32 ny = (rect.dy
						- controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight()
						- controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight())
						/ clh;

					int32 cl = over >>1;
					graphicsSystem->SetClipIntersect(rect.x, yc, s_cl->GetWidth() - cl, rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight());

					if (yc < clipY)
					{
						int32 hn = (clipY - yc) / clh;
						yc += clh * hn;
						ny -= hn;
					}

					if (yc + ny * clh > clipY + clipDY)
					{
						int32 hn = ((yc + ny * clh) - (clipY + clipDY)) / clh;

						if (hn > 0)
						{
							ny -= hn;
						}
					}

					for (int32 i = 0; i <= ny; ++i)
					{//draw cycle
						s_cl->Draw(rect.x, yc, frame);
						yc += clh;
					}
					graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);

				}

				Sprite *s_cr = controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_RIGHT];
				if ( controlStyle->scheme != GUISkinLocal::ES_9_LEFT
					&& !(rect.x + rect.dx - s_cr->GetWidth() > clipX + clipDX || rect.x + rect.dx < clipX) )
				{
					int32 xr = rect.x + rect.dx - s_cr->GetWidth();
					int32 yc = rect.y + controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetHeight();
					int32 crh = s_cr->GetHeight();
					int32 ny = (rect.dy
						- controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetHeight()
						- controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight())
						/ crh;

					int32 cl = over - (over >>1);
					graphicsSystem->SetClipIntersect(xr + cl, yc, s_cr->GetWidth(), rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight());

					if (yc < clipY)
					{
						int32 hn = (clipY - yc) / crh;
						yc += crh * hn;
						ny -= hn;
					}

					if (yc + ny * crh > clipY + clipDY)
					{
						int32 hn = ((yc + ny * crh) - (clipY + clipDY)) / crh;

						if (hn > 0)
						{
							ny -= hn;
						}
					}

					for (int32 i = 0; i <= ny; ++i)
					{//draw cycle
						s_cr->Draw(xr, yc, frame);
						yc += crh;
					}
				}
				graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);

			}


			if (controlStyle->scheme != GUISkinLocal::ES_9_LEFT && controlStyle->scheme != GUISkinLocal::ES_9_RIGHT)
			{//draw central-top & central-bottom parts

				//x coordinate of current middle part
				int32 over = 0;
				if (controlStyle->scheme != GUISkinLocal::ES_9_TOP && controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM)
				{
					over = rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_CENTER]->GetHeight();
					if (over < 0)
					{
						over = -over;
					}
					else
					{
						over = 0;
					}
				}
				Sprite *s_tc = controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_CENTER];
				if (controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM
					&& !(rect.y + s_tc->GetHeight() < clipY || rect.y > clipY + clipDY))
				{
					int32 xc = rect.x + controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetWidth();

					int32 tcw = s_tc->GetWidth();
					int32 nx = (rect.dx
						- controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetWidth()
						- controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth())
						/ tcw;
					int32 cl = over >> 1;
					graphicsSystem->SetClipIntersect(xc, rect.y, rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth(), s_tc->GetHeight() - cl);
					if (xc < clipX)
					{
						int32 wn = (clipX - xc) / tcw;
						xc += tcw * wn;
						nx -= wn;
					}

					if (xc + nx * tcw > clipX + clipDX)
					{
						int32 wn = ((xc + nx * tcw) - (clipX + clipDX)) / tcw;

						if (wn > 0)
						{
							nx -= wn;
						}
					}

					for (int32 i = 0; i <= nx; ++i)
					{//draw cycle
						s_tc->Draw(xc, rect.y, frame);
						xc += tcw;
					}
					graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);

				}


				Sprite *s_bc = controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_CENTER];
				if (controlStyle->scheme != GUISkinLocal::ES_9_TOP
					&& !(rect.y + rect.dy < clipY || rect.y + rect.dy - s_bc->GetHeight() > clipY + clipDY))
				{
					int32 yb = rect.y + rect.dy - s_bc->GetHeight();
					int32 xc = rect.x + controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetWidth();

					int32 bcw = s_bc->GetWidth();
					int32 nx = (rect.dx
						- controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetWidth()
						- controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth())
						/ bcw;
					int32 cl = over - (over >> 1);
					graphicsSystem->SetClipIntersect(xc, yb + cl, rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth(), s_bc->GetHeight());
					if (xc < clipX)
					{
						int32 wn = (clipX - xc) / bcw;
						xc += bcw * wn;
						nx -= wn;
					}

					if (xc + nx * bcw > clipX + clipDX)
					{
						int32 wn = ((xc + nx * bcw) - (clipX + clipDX)) / bcw;

						if (wn > 0)
						{
							nx -= wn;
						}
					}

					for (int32 i = 0; i <= nx; ++i)
					{//draw cycle
						s_bc->Draw(xc, yb, frame);
						xc += bcw;
					}
					graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);


				}
			}


			//draw corners
			{
				bool isOver = false;
				int32 overTopX = 0;
				if (controlStyle->scheme == GUISkinLocal::ES_9_TOP || controlStyle->scheme == GUISkinLocal::ES_9
					|| controlStyle->scheme == GUISkinLocal::ES_9_GRADIENT || controlStyle->scheme == GUISkinLocal::ES_9_BORDER
					 || controlStyle->scheme == GUISkinLocal::ES_9_FILL)
				{
					overTopX = rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth();
					if (overTopX < 0)
					{
						overTopX = -overTopX;
						isOver = true;
					}
					else
					{
						overTopX = 0;
					}
				}
				int32 overBottomX = 0;
				if (controlStyle->scheme == GUISkinLocal::ES_9_BOTTOM || controlStyle->scheme == GUISkinLocal::ES_9
					|| controlStyle->scheme == GUISkinLocal::ES_9_GRADIENT || controlStyle->scheme == GUISkinLocal::ES_9_BORDER
					 || controlStyle->scheme == GUISkinLocal::ES_9_FILL)
				{
					overBottomX = rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetWidth() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth();
					if (overBottomX < 0)
					{
						overBottomX = -overBottomX;
						isOver = true;
					}
					else
					{
						overBottomX = 0;
					}
				}
				int32 overLeftY = 0;
				if (controlStyle->scheme == GUISkinLocal::ES_9_LEFT || controlStyle->scheme == GUISkinLocal::ES_9
					|| controlStyle->scheme == GUISkinLocal::ES_9_GRADIENT || controlStyle->scheme == GUISkinLocal::ES_9_BORDER
					 || controlStyle->scheme == GUISkinLocal::ES_9_FILL)
				{
					overLeftY = rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight();
					if (overLeftY < 0)
					{
						overLeftY = -overLeftY;
						isOver = true;
					}
					else
					{
						overLeftY = 0;
					}
				}
				int32 overRightY = 0;
				if (controlStyle->scheme == GUISkinLocal::ES_9_RIGHT || controlStyle->scheme == GUISkinLocal::ES_9
					|| controlStyle->scheme == GUISkinLocal::ES_9_GRADIENT || controlStyle->scheme == GUISkinLocal::ES_9_BORDER
					|| controlStyle->scheme == GUISkinLocal::ES_9_FILL)
				{
					overRightY = rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetHeight() - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight();
					if (overRightY < 0)
					{
						overRightY = -overRightY;
						isOver = true;
					}
					else
					{
						overRightY = 0;
					}
				}
				if (isOver)
				{
					int32 clx;
					int32 cly;
					if (controlStyle->scheme != GUISkinLocal::ES_9_RIGHT && controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM)
					{
						clx = overTopX >>1;
						cly = overLeftY >> 1;
						graphicsSystem->SetClipIntersect
							( rect.x
							, rect.y
							, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetWidth() - clx
							, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->GetHeight() - cly );
						controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->Draw
							( rect.x
							, rect.y
							, frame);
						graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
					}

					if (controlStyle->scheme != GUISkinLocal::ES_9_LEFT&& controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM)
					{
						clx = overTopX - (overTopX >>1);
						cly = overRightY >> 1;
						graphicsSystem->SetClipIntersect
							( rect.x + rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth() + clx
							, rect.y
							, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth()
							, controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetHeight() - cly );
						controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->Draw
							( rect.x + rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth()
							, rect.y
							, frame);
						graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
					}

					if (controlStyle->scheme != GUISkinLocal::ES_9_RIGHT && controlStyle->scheme != GUISkinLocal::ES_9_TOP)
					{
						clx = overBottomX >>1;
						cly = overLeftY - (overLeftY >> 1);
						graphicsSystem->SetClipIntersect
							( rect.x
							, rect.y + rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight() + cly
							, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetWidth() - clx
							, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight() );
						controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->Draw
							( rect.x
							, rect.y + rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight()
							, frame);
						graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
					}

					if (controlStyle->scheme != GUISkinLocal::ES_9_LEFT && controlStyle->scheme != GUISkinLocal::ES_9_TOP)
					{
						clx = overBottomX - (overBottomX >>1);
						cly = overRightY - (overRightY >> 1);
						graphicsSystem->SetClipIntersect
							( rect.x + rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth() + clx
							, rect.y + rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight() + cly
							, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth() + clx
							, controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight() - cly );
						controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->Draw
							( rect.x + rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth()
							, rect.y + rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight()
							, frame);
						graphicsSystem->SetClip(clipX, clipY, clipDX, clipDY);
					}
				}
				else
				{
					if (controlStyle->scheme != GUISkinLocal::ES_9_RIGHT && controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM)
					{
						controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_LEFT]->Draw
							( rect.x
							, rect.y
							, frame);
					}
					if (controlStyle->scheme != GUISkinLocal::ES_9_LEFT && controlStyle->scheme != GUISkinLocal::ES_9_BOTTOM)
					{
						controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->Draw
							( rect.x + rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_TOP_RIGHT]->GetWidth()
							, rect.y
							, frame);
					}
					if (controlStyle->scheme != GUISkinLocal::ES_9_RIGHT && controlStyle->scheme != GUISkinLocal::ES_9_TOP)
					{
						controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->Draw
							( rect.x
							, rect.y + rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_LEFT]->GetHeight()
							, frame);
					}
					if (controlStyle->scheme != GUISkinLocal::ES_9_LEFT && controlStyle->scheme != GUISkinLocal::ES_9_TOP)
					{
						controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->Draw
							( rect.x + rect.dx - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetWidth()
							, rect.y + rect.dy - controlStyle->spriteArray[GUISkinLocal::ESS9_BOTTOM_RIGHT]->GetHeight()
							, frame);
					}
				}

			}

		}break;
	case GUISkinLocal::ES_FILL:
		{
			Gradient *gr = (Gradient*)controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_CENTER];
			graphicsSystem->SetColor(gr->startR, gr->startG, gr->startB);
			if (gr->alpha)
			{
				graphicsSystem->SetAlpha(gr->alpha);
				graphicsSystem->SetBlendMode(GraphicsSystem::EBM_ALPHA);
 				graphicsSystem->FillRect(rect.x, rect.y, rect.dx, rect.dy);
				graphicsSystem->SetBlendMode(GraphicsSystem::EBM_NONE);
			}
			else
			{
 				graphicsSystem->FillRect(rect.x, rect.y, rect.dx, rect.dy);
			}
		}break;
	case GUISkinLocal::ES_LINE_BOTTOM:
		{
			Line *line = (Line*)controlStyle->spriteArray[GUISkinLocal::ESS9_CENTER_CENTER];
			graphicsSystem->SetColor(line->r, line->g, line->b);
			if (line->alpha)
			{
				graphicsSystem->SetAlpha(line->alpha);
				graphicsSystem->SetBlendMode(GraphicsSystem::EBM_ALPHA);
				graphicsSystem->DrawHLine(rect.x, rect.y + rect.dy - line->bottomOffset, rect.x + rect.dx);
				graphicsSystem->SetBlendMode(GraphicsSystem::EBM_NONE);
			}
			else
			{
				graphicsSystem->DrawHLine(rect.x, rect.y + rect.dy - line->bottomOffset, rect.x + rect.dx);
			}
		}break;
	}

}

Sprite	* GUISkinLocal::GetSprite( eDrawType drawType, int32 schemeID )
{
	GUISkinLocal::TypeTilesStyle *cs = GetTiles(drawType);
	if (!cs || schemeID >= drawSchemeCount[cs->scheme])
	{
		return NULL;
	}


	if (schemeID == ESS9_CENTER_CENTER && (cs->scheme == ES_9_GRADIENT || cs->scheme == ES_FILL 
		|| cs->scheme == ES_9_FILL || cs->scheme == ES_LINE_BOTTOM || cs->scheme == ES_9_BORDER))
	{
		return NULL;
	}

	return cs->spriteArray[schemeID];
}
