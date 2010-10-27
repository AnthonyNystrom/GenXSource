#include "ScreenTitle.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"

#include "GUIAlert.h"
#include "N2FNewsManager.h"
#include "N2FDraftsManager.h"
#include "N2FInboxManager.h"
#include "N2FOutboxManager.h"

#include "SkinProperties.h"

ScreenTitle::ScreenTitle(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID)
:BaseScreen(newRect, pAppMng, screenID)
{
	SetStretch(ECS_STRETCH);
	
	img = new GUIImage(IDB_TITLE, this, newRect);
	img->SetSelectable(false);
	img->SetAlignType(EA_VCENTRE|EA_HCENTRE);
	img->SetControlRect(ControlRect(0, 0, img->GetImageWidth(), img->GetImageHeight(), img->GetImageWidth(), img->GetImageHeight()));
	skinInitCounter = 0;

	loading = new GUIControl(this, ControlRect(25, 190, 50, 7));
	loading->SetMargins(25, 0, 190, 0);
	loading->SetDrawType(EDT_LOADING_BAR);
}

ScreenTitle::~ScreenTitle(void)
{
}


void ScreenTitle::Update()
{

	{
		if (skinInitCounter < skinTilesCount)
		{
			SetTiles();
		}
		else if (skinInitCounter < skinTilesCount + (int32)ESN_COUNT)
		{
			pManager->CreateScreens(skinInitCounter - skinTilesCount);
			skinInitCounter++;
			
			
		}
		else
		{
			if (pManager->GetAlert() != GUISystem::Instance()->GetActiveControl())
			{
				if (skinInitCounter == skinTilesCount + ESN_COUNT)
				{
					pManager->GetDraftsManager()->Init();
					skinInitCounter++;
				}
				else if (skinInitCounter == skinTilesCount + ESN_COUNT + 1)
				{
					pManager->GetOutboxManager()->Init();
					skinInitCounter++;
				}
				else if (skinInitCounter == skinTilesCount + ESN_COUNT + 2)
				{
					pManager->GetNewsManager()->Init();
					skinInitCounter++;
				}
				else if (skinInitCounter == skinTilesCount + ESN_COUNT + 3)
				{
					pManager->GetInboxManager()->Init();
					skinInitCounter++;
				}
				else
				{
					//creating footer and header

					GUISystem::Instance()->SetBackground(pManager->GetBackSprite());

					GUISystem::Instance()->AddControl(pManager->GetHeader());
					GUISystem::Instance()->AddControl(pManager->GetFooter());

					pManager->OnApplicationLoaded();
				}
			}
		}
	}

	int32 xdx = LOADBAR_WIDTH * skinInitCounter / (skinTilesCount + ESN_COUNT + 4);
	loading->SetControlRect(ControlRect( img->GetRect().x + LOADBAR_X_OFFSET, img->GetRect().y + LOADBAR_Y_OFFSET, xdx, 7));
	//SetStretch(ECS_NOT_STRETCH);

	BaseScreen::Update();
}

void ScreenTitle::SetTiles()
{
	UTILS_TRACE("Load TILES = %d", skinInitCounter);

	for (int i = 0; i < LOAD_SKINS_PER_FRAME && skinInitCounter < skinTilesCount; i++)
	{
		Sprite *pSpriteArray[9];
		eDrawType dt = (eDrawType)skinProp[skinInitCounter][0];
		GUISkinLocal::eDrawScheme ds = (GUISkinLocal::eDrawScheme)skinProp[skinInitCounter][1];
		int32 frame = skinProp[skinInitCounter][2];
		int32 palette = skinProp[skinInitCounter][3];
		for (int j = 0; j < drawSchemeCount[ds]; j++)
		{
			if ((ds == GUISkinLocal::ES_9_GRADIENT || ds == GUISkinLocal::ES_9_FILL || ds == GUISkinLocal::ES_FILL) && j == GUISkinLocal::ESS9_CENTER_CENTER)
			{
				int32 val = skinProp[skinInitCounter][4 + j];
				GUISkinLocal::Gradient *pGr = new GUISkinLocal::Gradient(gradientProps[val][0],gradientProps[val][1],gradientProps[val][2],gradientProps[val][3],gradientProps[val][4],gradientProps[val][5],gradientProps[val][6]);
				pSpriteArray[j] = (Sprite*)pGr;
				continue;
			}
			if ((ds == GUISkinLocal::ES_LINE_BOTTOM) && j == GUISkinLocal::ESS9_CENTER_CENTER)
			{
				int32 val = skinProp[skinInitCounter][4 + j];
				GUISkinLocal::Line *pGr = new GUISkinLocal::Line(lineProps[val][0],lineProps[val][1],lineProps[val][2],lineProps[val][3],lineProps[val][4]);
				pSpriteArray[j] = (Sprite*)pGr;
				continue;
			}
			if (skinProp[skinInitCounter][4 + j] == 0)
			{
				pSpriteArray[j] = NULL;
				continue;
			}
			pSpriteArray[j] = pManager->GetGraphicRes()->CreateSprite((int16)skinProp[skinInitCounter][4 + j]);
		}

		pManager->GetGUISystem()->GetSkin()->SetTiles(dt, (Sprite**)pSpriteArray, frame, ds, (uint8)palette);
		for (int j = 0; j < drawSchemeCount[ds]; j++)
		{
			if ((ds == GUISkinLocal::ES_9_GRADIENT || ds == GUISkinLocal::ES_9_FILL || ds == GUISkinLocal::ES_FILL || ds == GUISkinLocal::ES_LINE_BOTTOM) && j == GUISkinLocal::ESS9_CENTER_CENTER)
			{
				continue;
			}
			if (pSpriteArray[j])
			{
				pSpriteArray[j]->Release();
			}
		}
		skinInitCounter++;
	}
}

bool ScreenTitle::PopUpShouldOpen()
{
	return false;
}
