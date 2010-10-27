package n2f.sup.ui;

public class LocaleUI 
{
	private LocaleUI(){}

	//TODO: introduce codes;
	
	//External Events
	public static final String REPLY_READY = "reply.alert.text";
	public static final String QUESTION_READY = "question.alert.text";
	public static final String OPERATION_FAILED="failed.alert.text";
	public static final String COMMAND_YES="alert.command.yes";
	public static final String COMMAND_NO="alert.command.no";
	public static final String ALERT_TITLE="alert.title";
	
	//errors
	public static final String ERROR_GENERAL_TEXT = "error.general.text";
	public static final String ERROR_CAMERA_TEXT = "error.camera.text";
	public static final String ERROR_OPENINIT_TEXT = "error.openinit.text";
	public static final String ERROR_SAVE_TEXT = "error.save.text";
	public static final String ERROR_PREVIEW_TEXT = "error.preview.text";
	public static final String ERROR_SCALE_TEXT = "error.scale.text";
	public static final String ERROR_SUBMIT_TEXT = "error.submit.text";
	public static final String ERROR_ATTACH_TEXT = "error.attach.text";
	public static final String ERROR_COMPLETE_TEXT = "error.complete.text";
	public static final String ERROR_QUESTIONS_TEXT = "error.getquestions.text";
	public static final String ERROR_DETAILS_TEXT = "error.getdetails.text";
	public static final String ERROR_COMMENTS_TEXT = "error.getcomments.text";
	public static final String ERROR_CREDENTIALS_TEXT = "error.credentials.text";
	public static final String ERROR_UPLOAD_TEXT = "error.upload.text";

	/* Settings Screen*/
	public static final String FORM_SETTINGS_CAPTION = "form.settings.caption";
	public static final String SETTINGS_ALERT_TEXT = "alert.settings.text";
	public static final String SETINGS_COMMAND_PROCEED = "form.settings.command.proceed";
	public static final String SETINGS_COMMAND_QUIT = "form.settings.command.quit";
	public static final String SETINGS_COMMAND_CANCEL = "form.settings.command.cancel";
	public static final String SETINGS_TEXTFIELD_LOGIN = "form.settings.textfield.login";
	public static final String SETINGS_TEXTFIELD_PASSWORD = "form.settings.textfield.password";

	//FileSelectorScreen
	public static final String FORM_FILE_SELECTOR_CAPTION = "form.fileselector.caption";
	public static final String FILE_SELECTOR_ALERT_TEXT = "alert.fileselector.text";
	public static final String FILE_SELECTOR_COMMAND_ASK_A_QUESTION = "form.fileselector.command.askaquestion";
	public static final String FILE_SELECTOR_COMMAND_CANCEL = "form.fileselector.command.cancel";
	public static final String FILE_SELECTOR_COMMAND_GOTOCART = "form.fileselector.command.gotocart";
	
	public static final String FILE_SELECTOR_COMMAND_OPEN = "form.fileselector.command.open";
	public static final String FILE_SELECTOR_COMMAND_PREVIEW = "form.fileselector.command.preview";
	public static final String FILE_SELECTOR_ALERT = "alert.fileselector.text";
	public static final String FILE_SELECTOR_COMMAND_ADD2CART = "form.fileselector.command.add2cart";
	public static final String FILE_SELECTOR_COMMAND_SELECT = "form.fileselector.command.select";
	public static final String ALERT_SELECTOR_COMMAND_PREVIEW_TEXT = "alert.fileselector.preview.text";
	public static final String ALERT_SELECTOR_THIS_ITEM_CAN_T_BE_OPENED = "alert.fileselector.not_file";
	public static final String ALERT_SELECTOR_PLEASE_SELECT_AN_IMAGE = "alert.fileselector.not_image";
	
	//StartScreen
	public static final String FORM_START_CAPTION = "form.start.caption";
	public static final String START_CITEM_EXPLORE = "form.start.citem.explore";
	public static final String START_CITEM_CAMERA = "form.start.citem.camera";
	public static final String START_CITEM_SETTINGS = "form.start.citem.settings";
	public static final String START_CITEM_RESPONSES = "form.start.citem.responses";
	public static final String START_CITEM_EXIT = "form.start.citem.exit";
	public static final String START_CITEM_PRIVATE_QUESTIONS = "form.start.citem.private_questions";
	public static final String START_CITEM_CART="form.start.citem.cart";
	public static final String START_CITEM_ASK="form.start.citem.ask";
	public static final String START_COMMAND_SELECT="form.start.command.select";
	
	//WaitScreen
	public static final String FORM_WAIT_CAPTION = "form.wait.caption";
	public static final String WAIT_COMMAND_DONE = "form.wait.command.done";
	public static final String WAIT_COMMAND_BACK = "form.wait.command.back";
	public static final String WAIT_TEXT = "form.wait.text";
	
	//preview screen
	public static final String FORM_PREVIEW_CAPTION = "form.preview.caption";
	public static final String ALERT_PREVIEW_TEXT = "alert.preview.text";
	public static final String FORM_PREVIEW_COMMAND_BACK = "form.preview.command.back";
	public static final String FORM_PREVIEW_COMMAND_ADD_2_CART = "form.preview.command.add2cart";
	public static final String FORM_PREVIEW_COMMAND_REMOVE_FROM_CART = "form.preview.command.remove2cart";

	//splash screen
	public static final String FORM_SPLASH_CAPTION = "form.splash";

	//proceed screen 
	public static final String SCREEN_PROCEED = "form.proceed";	
	
	//PrivateQuestionsSreen
//	public static final String FORM_SAVE = "form.save.caption";	

	//Image cart
	public static final String FORM_IMAGE_CART_CAPTION = "form.cart.caption";
	public static final String IMAGE_CART_COMMAND_NEXT = "form.cart.command.next";
	public static final String IMAGE_CART_COMMAND_ASK = "form.cart.command.ask";
	public static final String IMAGE_CART_COMMAND_DELETE = "form.cart.command.delete";
	public static final String IMAGE_CART_COMMAND_DELETEALL = "form.cart.command.deleteAll";
	public static final String IMAGE_CART_COMMAND_SELECT = "form.cart.command.select";
	public static final String ALERT_IMAGE_CART_TEXT = "alert.cart.preview_one";
	public static final String IMAGE_CART_TEXT = "form.cart.text";
	public static final String CARD_COMMAND_BACK = "form.cart.command.back";
	
	//OptionsChooserScreen
	public static final String FORM_BROWS_OPTIONS_CAPTION = "form.brows_screen.caption";	
	public static final String FORM_BROWS_OPTIONS_COMMAND_SELECT = "form.brows_screen.command.select";
	public static final String FORM_BROWS_OPTIONS_COMMAND_BACK = "form.brows_screen.command.back";
	public static final String FORM_BROWS_OPTIONS_COMMAND_PREVIEW = "form.brows_screen.command.preview";
	public static final String FORM_BROWS_OPTIONS_COMMAND_ADD_CARD = "form.brows_screen.command.add_card";
	
	public static final String FORM_CARD_OPTIONS_CAPTION = "form.card_options.caption";
	public static final String FORM_CARD_OPTIONS_COMMAND_SELECT = "form.card_options.command.select";
	public static final String FORM_CARD_OPTIONS_COMMAND_BACK = "form.card_options.command.back";
	public static final String FORM_CARD_OPTIONS_COMMAND_DELETE = "form.card_options.command.delete";
	public static final String FORM_CARD_OPTIONS_CHOICE_MARK = "form.card_options.choice.mark";
	public static final String FORM_CARD_OPTIONS_CHOICE_UNMARK = "form.card_options.choice.unmark";
	public static final String FORM_CARD_OPTIONS_COMMAND_PREVIEW = "form.card_options.command.preview";	
	
	
	///SUP
	//SUP_StartScreen
	public static final String FORM_SUP_START_CAPTION = "form.sup_start.caption";//"Snap-Up"
	//Image cart
	public static final String FORM_SUP_IMAGE_CART_CAPTION = "form.sup_cart.caption";//SUP Cart
	public static final String IMAGE_SUP_CART_TEXT = "form.sup_cart.text";//Manage your cart
	//Image cart
	public static final String IMAGE_SUP_CART_COMMAND_UPLOAD = "form.sup_cart.command.upload";//Upload
	public static final String IMAGE_SUP_CART_ALERT1 = "alert.sup_cart.alert1";//The Card is Empty	
	public static final String IMAGE_SUP_CART_MEMORY = "alert.sup_cart.alert.memory";//"MEMORY ISSUE TRY LATER ON"
	public static final String IMAGE_SUP_CART_GENERAL = "alert.sup_cart.alert.general";//"GENERAL ERROR"	
}