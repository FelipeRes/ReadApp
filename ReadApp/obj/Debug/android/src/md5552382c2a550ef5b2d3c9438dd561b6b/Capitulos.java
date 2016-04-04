package md5552382c2a550ef5b2d3c9438dd561b6b;


public class Capitulos
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("ReadApp.Capitulos, ReadApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Capitulos.class, __md_methods);
	}


	public Capitulos () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Capitulos.class)
			mono.android.TypeManager.Activate ("ReadApp.Capitulos, ReadApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
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
