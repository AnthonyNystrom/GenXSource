#ifndef _DIAGRAMPROPERTYDLG_H_
#define _DIAGRAMPROPERTYDLG_H_

#include "DiagramEntity.h"

class CDiagramEntity;
class IThumbnailerStorage;


class CDiagramPropertyDlg : public CDialog
{

public:
	CDiagramPropertyDlg( UINT res, IThumbnailerStorage* parent );
	BOOL Create( UINT nIDTemplate, IThumbnailerStorage* pParentWnd );

	void			SetEntity( CDiagramEntity* entity );
	CDiagramEntity*	GetEntity() const;

	virtual IThumbnailerStorage*	GetRedrawWnd();
	virtual void	SetRedrawWnd( IThumbnailerStorage* redrawWnd );
	virtual void	Redraw();

	virtual void	SetValues() = 0;

private:
	CDiagramEntity*	m_entity;
	IThumbnailerStorage*	m_redrawWnd;

};

#endif // _DIAGRAMPROPERTYDLG_H_