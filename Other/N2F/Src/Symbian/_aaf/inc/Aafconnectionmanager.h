/*
============================================================================
Name        : Aafconnectionmanager.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Internet access points manager: internet access point selection dialog etc
============================================================================
*/

#include "common.h"

#ifndef CONNECTIONMANAGER_H
#define CONNECTIONMANAGER_H

// INCLUDES
#include <e32std.h>
#include <e32base.h>
#include <es_sock.h>

// Singleton class
class CAafConnectionManager
{
	friend class CAafAppUi;

public:
	/**
	*
	*/
	static CAafConnectionManager* GetInstanceL();

	/**
	*
	*/
	~CAafConnectionManager();

	/**
	* Performs IAP selection routine
	*/
	void SelectIAPL();

	/**
	* Install internet connection using current IAP id value
	*/
	TInt InstallConnectionL();

	/**
	* Get currently used IAP id
	*/
	TUint32 GetIAPId()
	{
		return iIAPId;
	}

	/**
	* Get pointer to the currently used socket service
	*/
	RSocketServ* GetSocketServ() const
	{
		return iSocketServ;
	}

	/**
	* Get pointer to the currently used connection
	*/
	RConnection* GetConnection() const
	{
		return iConnection;
	}

protected:
	/**
	* Read stored IAP id from the settings file
	*/
	TBool ReadSavedIAPL();

	/**
	* Save IAP id to the settings file
	*/
	TBool SaveIAPL();

private:
	/**
	* Default constructor
	*/
	CAafConnectionManager();

	/**
	* Two-phase constructor
	*/
	void ConstructL();

private:
	/**
	* For internal usage
	*/
	class TIapData
	{
		public:
			TBuf<128> iName;
			TUint32 iIap;
	};

	/**
	* Self instance pointer
	*/
	static CAafConnectionManager* iSelfInstance;

	/**
	* Id of currently used internet access point
	*/
	TUint32 iIAPId;

	/**
	*  Path to the setting file
	*/
	TFileName iSettingsFile;

	/**
	* Socket service
	*/
	RSocketServ* iSocketServ;

	/**
	* Connection object
	*/
	RConnection* iConnection;
};

#endif // CONNECTIONMANAGER_H
