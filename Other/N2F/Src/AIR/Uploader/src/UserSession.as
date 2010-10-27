package
{
	import flash.events.Event;
	import flash.utils.ByteArray;
	
	import com.next2friends.*;
	
	import mx.controls.Alert;
	import mx.rpc.events.FaultEvent;

	public class UserSession
	{
		private var service : PhotoOrganise;

		public function UserSession( faultListener : Function = null ) : void
		{
			var onFault : Function = function OnFault( event : FaultEvent ) : void
			{
				if ( faultListener != null ) faultListener.call( this, event );
			}

			service = new PhotoOrganise();
			service.addPhotoOrganiseFaultEventListener( onFault );
		}

		private function WrapListeners( type : String, listener : Function, faultListener : Function ) : Function
		{
			var onFault : Function;

			var removeListeners : Function = function RemoveListeners() : void
			{
				service.removeEventListener( type, onComplete );
				service.removeEventListener( FaultEvent.FAULT, onFault );
			}

			onFault = function OnFault( event : FaultEvent ) : void
			{
				removeListeners();
				if ( faultListener != null ) faultListener( event );
			}

			var onComplete : Function = function OnComplete( event : Event ) : void
			{
				removeListeners();
				if ( listener != null ) listener( event );
			}

			service.addPhotoOrganiseFaultEventListener( onFault );
			return onComplete;
		}

		public function Login( email : String, password : String, listener : Function, faultListener : Function = null ) : void
		{
			service.addloginEventListener( WrapListeners(
				LoginResultEvent.Login_RESULT,
				listener,
				faultListener ) );

			service.login( email, password );
		}

		public function GetCollections( listener : Function, faultListener : Function = null ) : void
		{
			service.addgetCollectionsEventListener( WrapListeners(
				GetCollectionsResultEvent.GetCollections_RESULT,
				listener,
				faultListener ) );

			service.getCollections();
		}

		public function GetCollection( collectionID : String, listener : Function, faultListener : Function = null ) : void
		{
			if ( collectionID == null ) return;

			service.addgetPhotosByCollectionEventListener( WrapListeners(
				GetPhotosByCollectionResultEvent.GetPhotosByCollection_RESULT,
				listener,
				faultListener ) );

			service.getPhotosByCollection( collectionID );
		}

		public function CreateNewCollection( newName : String, listener : Function, faultListener : Function = null ) : void
		{
			service.addcreateNewCollectionEventListener( WrapListeners(
				CreateNewCollectionResultEvent.CreateNewCollection_RESULT,
				listener,
				faultListener ) );

			service.createNewCollection( newName );
		}

		public function RenameCollection( collectionID : String, newName : String, listener : Function, faultListener : Function = null ) : void
		{
			service.addrenameCollectionEventListener( WrapListeners(
				RenameCollectionResultEvent.RenameCollection_RESULT,
				listener,
				faultListener ) );

			service.renameCollection( collectionID, newName );
		}

		public function UploadPhoto( collectionID : String, image : ByteArray,
			listener : Function, faultListener : Function = null ) : void
		{
			service.adduploadPhotoEventListener( WrapListeners(
				UploadPhotoResultEvent.UploadPhoto_RESULT,
				listener,
				faultListener ) );

			service.uploadPhoto( collectionID, image ); 
		}

		public function DeletePhoto( photoID : String, listener : Function, faultListener : Function = null ) : void
		{
			if ( photoID == null ) return;

			service.adddeletePhotoEventListener( WrapListeners(
				DeletePhotoResultEvent.DeletePhoto_RESULT,
				listener,
				faultListener ) );

			service.deletePhoto( photoID );			
		}
	}
}