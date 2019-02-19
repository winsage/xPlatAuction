package md514062de1e369991ec0c3d948c12c4951;


public class FirebaseRegistrationService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("xPlatAuction.Droid.FirebaseRegistrationService, xPlatAuction.Android", FirebaseRegistrationService.class, __md_methods);
	}


	public FirebaseRegistrationService ()
	{
		super ();
		if (getClass () == FirebaseRegistrationService.class)
			mono.android.TypeManager.Activate ("xPlatAuction.Droid.FirebaseRegistrationService, xPlatAuction.Android", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
