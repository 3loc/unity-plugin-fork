using AIRENS.MiniJSON;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.AIRE;
using UnityEngine.Networking;

#if (UNITY_IPHONE || UNITY_TVOS)
using System.Runtime.InteropServices;
#endif

public class AIRE {
	private static readonly string UnityLibraryName = "aire-unity";
	private static readonly string UnityLibraryVersion = "1.6.0";

	private static Dictionary<string, AIRE> instances;
	private static readonly object instanceLock = new object();

#if UNITY_ANDROID
	private static readonly string androidPluginName = "com.aire.unity.plugins.AIREPlugin";
	private AndroidJavaClass pluginClass;
#endif

	public bool logging = false;
	private string instanceName = null;

#if (UNITY_IPHONE || UNITY_TVOS)
	[DllImport ("__Internal")]
	private static extern void _AIRE_init(string instanceName, string apiKey, string userId);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setTrackingOptions(string instanceName, string trackingOptionsJson);
	[DllImport ("__Internal")]
	private static extern void _AIRE_logEvent(string instanceName, string evt, string propertiesJson);
	[DllImport ("__Internal")]
	private static extern void _AIRE_logOutOfSessionEvent(string instanceName, string evt, string propertiesJson);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOffline(string instanceName, bool offline);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserId(string instanceName, string userId);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setDeviceId(string instanceName, string deviceId);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserProperties(string instanceName, string propertiesJson);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOptOut(string instanceName, bool enabled);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setMinTimeBetweenSessionsMillis(string instanceName, long minTimeBetweenSessionsMillis);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setLibraryName(string instanceName, string libraryName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setLibraryVersion(string instanceName, string libraryVersion);
	[DllImport ("__Internal")]
	private static extern void _AIRE_enableCoppaControl(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_disableCoppaControl(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setServerUrl(string instanceName, string serverUrl);
	[DllImport ("__Internal")]
	private static extern void _AIRE_logRevenueAmount(string instanceName, double amount);
	[DllImport ("__Internal")]
	private static extern void _AIRE_logRevenue(string instanceName, string productIdentifier, int quantity, double price);
	[DllImport ("__Internal")]
	private static extern void _AIRE_logRevenueWithReceipt(string instanceName, string productIdentifier, int quantity, double price, string receipt);
	[DllImport ("__Internal")]
	private static extern void _AIRE_logRevenueWithReceiptAndProperties(string instanceName, string productIdentifier, int quantity, double price, string receipt, string revenueType, string propertiesJson);
	[DllImport ("__Internal")]
	private static extern string _AIRE_getDeviceId(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_regenerateDeviceId(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_useAdvertisingIdForDeviceId(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_trackingSessionEvents(string instanceName, bool enabled);
	[DllImport ("__Internal")]
	private static extern long _AIRE_getSessionId(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_uploadEvents(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_clearUserProperties(string instanceName);
	[DllImport ("__Internal")]
	private static extern void _AIRE_unsetUserProperty(string instanceName, string property);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyBool(string instanceName, string property, bool value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyDouble(string instanceName, string property, double value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyFloat(string instanceName, string property, float value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyInt(string instanceName, string property, int value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyLong(string instanceName, string property, long value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyString(string instanceName, string property, string value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyDict(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyList(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyBoolArray(string instanceName, string property, bool[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyDoubleArray(string instanceName, string property, double[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyFloatArray(string instanceName, string property, float[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyIntArray(string instanceName, string property, int[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyLongArray(string instanceName, string property, long[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setOnceUserPropertyStringArray(string instanceName, string property, string[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyBool(string instanceName, string property, bool value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyDouble(string instanceName, string property, double value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyFloat(string instanceName, string property, float value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyInt(string instanceName, string property, int value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyLong(string instanceName, string property, long value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyString(string instanceName, string property, string value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyDict(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyList(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyBoolArray(string instanceName, string property, bool[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyDoubleArray(string instanceName, string property, double[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyFloatArray(string instanceName, string property, float[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyIntArray(string instanceName, string property, int[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyLongArray(string instanceName, string property, long[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_setUserPropertyStringArray(string instanceName, string property, string[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_addUserPropertyDouble(string instanceName, string property, double value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_addUserPropertyFloat(string instanceName, string property, float value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_addUserPropertyInt(string instanceName, string property, int value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_addUserPropertyLong(string instanceName, string property, long value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_addUserPropertyString(string instanceName, string property, string value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_addUserPropertyDict(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyBool(string instanceName, string property, bool value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyDouble(string instanceName, string property, double value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyFloat(string instanceName, string property, float value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyInt(string instanceName, string property, int value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyLong(string instanceName, string property, long value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyString(string instanceName, string property, string value);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyDict(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyList(string instanceName, string property, string values);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyBoolArray(string instanceName, string property, bool[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyDoubleArray(string instanceName, string property, double[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyFloatArray(string instanceName, string property, float[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyIntArray(string instanceName, string property, int[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyLongArray(string instanceName, string property, long[] value, int length);
	[DllImport ("__Internal")]
	private static extern void _AIRE_appendUserPropertyStringArray(string instanceName, string property, string[] value, int length);
#endif

	public static AIRE getInstance() {
		return getInstance(null);
	}
	public static AIRE getInstance(string instanceName) {
		string instanceKey = instanceName;
		if (string.IsNullOrEmpty(instanceKey)) {
			instanceKey = "$default_instance";
		}

		lock(instanceLock)
		{
			if (instances == null) {
				instances = new Dictionary<string, AIRE>();
			}

			AIRE instance;
			if (instances.TryGetValue(instanceKey, out instance)) {
				// No logic
			} else {
				instance = new AIRE(instanceName);
				instances.Add(instanceKey, instance);
			}
			return instance;
		}
	}

	public static AIRE Instance {
		get
		{
			return getInstance();
		}
	}

	public AIRE(string instanceName) : base() {
		this.instanceName = instanceName;

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			Debug.Log ("construct instance");
			pluginClass = new AndroidJavaClass(androidPluginName);
		}
#endif

		this.setLibraryName(UnityLibraryName);
		this.setLibraryVersion(UnityLibraryVersion);

		this.trackSessionEvents(true);
		this.useAdvertisingIdForDeviceId();
	}

	protected void Log(string message) {
		if(!logging) return;

		Debug.Log(message);
	}

	protected void Log<T>(string message, string property, IEnumerable<T> array) {
		Log (string.Format("{0} {1}, {2}: [{3}]", message, property, array, string.Join(", ", array)));
	}

	public void init(string apiKey) {
		Log (string.Format("C# init {0}", apiKey));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_init(instanceName, apiKey, null);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			using(AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using(AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
					using(AndroidJavaObject unityApplication = unityActivity.Call<AndroidJavaObject>("getApplication")) {
						pluginClass.CallStatic("init", instanceName, unityActivity, apiKey);
						pluginClass.CallStatic("enableForegroundTracking", instanceName, unityApplication);
					}
				}
			}
		}
#endif
	}

	/// <summary>
	/// Initialize the SDK.
	/// </summary>
	/// <param name="apiKey">API key</param>
	/// <param name="userId">user Id</param>
	public void init(string apiKey, string userId) {
		Log (string.Format("C# init {0} with userId {1}", apiKey, userId));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_init(instanceName, apiKey, userId);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			using(AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using(AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
					using (AndroidJavaObject unityApplication = unityActivity.Call<AndroidJavaObject>("getApplication")) {
						pluginClass.CallStatic("init", instanceName, unityActivity, apiKey, userId);
						pluginClass.CallStatic("enableForegroundTracking", instanceName, unityApplication);
					}
				}
			}
		}
#endif
	}

	public void setTrackingOptions(IDictionary<string, bool> trackingOptions) {
		if (trackingOptions != null) {
			string trackingOptionsJson = Json.Serialize(trackingOptions);

			Log(string.Format("C# setting tracking options {0}", trackingOptionsJson));
#if (UNITY_IPHONE || UNITY_TVOS)
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
				_AIRE_setTrackingOptions(instanceName, trackingOptionsJson);
			}
#endif

#if UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android) {
				pluginClass.CallStatic("setTrackingOptions", instanceName, trackingOptionsJson);
			}
#endif
		}
	}

	/// <summary>
	/// Tracks an event. Events are saved locally.
	/// Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.
	/// </summary>
	/// <param name="evt">event type</param>
	private void logEvent(string evt) {
		Log (string.Format("C# sendEvent {0}", evt));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_logEvent(instanceName, evt, null);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logEvent", instanceName, evt);
		}
#endif
	}



	/// <summary>
	/// Get a recommended bundle
	/// Usage: StartCoroutine(aire.GetRecommendedBundle(bundle => doSomething(bundle)));
	/// </summary>
	/// <param name="onDataAction">A delegate that is called when the Bundle is fetched</param>
	public IEnumerator GetRecommendedBundle(System.Action<Bundle> onDataAction)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get("https://bj6e8w5vhd.execute-api.eu-west-1.amazonaws.com/prod/bundles"))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				Debug.Log("Error getting recommended bundle: " + webRequest.error);
			}
			else
			{
				Bundle bundle = deserializeBundle(webRequest.downloadHandler.text);
				onDataAction(bundle);
			}
		}
	}

	/// <summary>
	/// Tracks a bundle view open
	/// </summary>
	/// <param name="bundle">a bundle</param>
	public void bundleViewOpen(Bundle bundle) {
		bundle.view_event = new Bundle.BundleViewEvent(bundle.bundle_id);

		IDictionary<string, object> bundleProps = new Dictionary<string, object>();
		bundleProps.Add("bundle", serializeBundle(bundle));
		bundleProps.Add("event_group_id", bundle.view_event.event_group_id);

		logEvent("bundle.view.open", bundleProps);
		uploadEvents();
	}

	/// <summary>
	/// Tracks a bundle view close
	/// </summary>
	/// <param name="bundle">a bundle</param>
	public void bundleViewClose(Bundle bundle)
	{
		if(bundle.view_event != null)
        {
			bundle.view_event.duration = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - bundle.view_event.start_time;
			IDictionary<string, object> bundleProps = new Dictionary<string, object>();
			bundleProps.Add("bundle", serializeBundle(bundle));
			bundleProps.Add("duration", bundle.view_event.duration);
			bundleProps.Add("event_group_id", bundle.view_event.event_group_id);

			logEvent("bundle.view.close", bundleProps);
			uploadEvents();
			bundle.view_event = null;
        }
	}
	
	/// <summary>
	/// Tracks the beginning of a Bundle Purchase
	/// </summary>
	/// <param name="bundle">a bundle</param>
	public void bundlePurchaseBegin(Bundle bundle)
	{
		bundle.purchase_event = new Bundle.BundlePurchaseEvent(bundle.bundle_id);	

		IDictionary<string, object> bundleProps = new Dictionary<string, object>();
		bundleProps.Add("bundle", serializeBundle(bundle));
		bundleProps.Add("event_group_id", bundle.purchase_event.event_group_id);

		logEvent("bundle.purchase.begin", bundleProps);
		uploadEvents();
	}

	/// <summary>
	/// Tracks an aborted Bundle Purchase
	/// </summary>
	/// <param name="bundle">a bundle</param>
	public void bundlePurchaseAborted(Bundle bundle)
	{
		if (bundle.purchase_event != null)
		{
			bundle.purchase_event.duration = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - bundle.purchase_event.start_time;

			IDictionary<string, object> bundleProps = new Dictionary<string, object>();
			bundleProps.Add("bundle", serializeBundle(bundle));
			bundleProps.Add("duration", bundle.purchase_event.duration);
			bundleProps.Add("event_group_id", bundle.purchase_event.event_group_id);

			logEvent("bundle.purchase.aborted", bundleProps);
			uploadEvents();
		}

		bundle.purchase_event = null;
	}

	/// <summary>
	/// Tracks an aborted Bundle Purchase
	/// </summary>
	/// <param name="bundle">a bundle</param>
	/// <param name="receipt">a receipt string</param>
	public void bundlePurchaseCompleted(Bundle bundle, string receipt) {
		// event_group_id (same as bundle view)

		if (bundle.purchase_event != null)
		{
			bundle.purchase_event.duration = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - bundle.purchase_event.start_time;

			IDictionary<string, object> bundleProps = new Dictionary<string, object>();
			bundleProps.Add("bundle", serializeBundle(bundle));
			bundleProps.Add("receipt", receipt);
			bundleProps.Add("duration", bundle.purchase_event.duration);
			bundleProps.Add("event_group_id", bundle.purchase_event.event_group_id);

			logEvent("bundle.purchase.completed", bundleProps);
			uploadEvents();
		}

		bundle.purchase_event = null;
	}

	/// <summary>
	/// Tracks consumption of a bundle
	/// </summary>
	/// <param name="bundle">a bundle</param>
	public void bundleConsume(Bundle bundle) {
		IDictionary<string, object> bundleProps = new Dictionary<string, object>();
		bundleProps.Add("bundle", serializeBundle(bundle));

		logEvent("bundleConsume", bundleProps);
	}

	/// <summary>
	/// Tracks the user progress level
	/// </summary>
	/// <param name="level">user progress level</param>
	public void setUserProgressLevel(int level) {
		setUserProperty("progressLevel", level);
	}

	/// <summary>
    /// Serializes a bundle to a JSON string
    /// </summary>
    /// <param name="bundle">a bundle</param>
    public string serializeBundle(Bundle bundle)
    {
		Dictionary<string, object> dict = new Dictionary<string, object>();
		dict.Add("bundle_id", bundle.bundle_id);
		dict.Add("price", bundle.price);
		dict.Add("currency", bundle.currency);
		dict.Add("payment_type", bundle.payment_type);
		dict.Add("display_price", bundle.display_price);
		dict.Add("data", bundle.data);

		if(bundle.view_event != null)
        {
			dict.Add("bundle_view_start_time", bundle.view_event.start_time);
			dict.Add("bundle_view_duration", bundle.view_event.duration);
		}

		if (bundle.purchase_event != null)
		{
			dict.Add("bundle_purchase_start_time", bundle.purchase_event.start_time);
			dict.Add("bundle_purchase_duration", bundle.purchase_event.duration);
		}

		var bundleJson = Json.Serialize(dict);

		return bundleJson;
	}

	/// <summary>
	/// Deserializes a bundle from a JSON string
	/// </summary>
	/// <param name="json">a json string representing a bundle</param>
	public Bundle deserializeBundle(string json)
	{
		var dict = Json.Deserialize(json) as Dictionary<string, object>;
		Bundle bundle = new Bundle();

		bundle.bundle_id = (string) dict["bundle_id"];
		bundle.price = new decimal((float) dict["price"]);
		bundle.currency = (string) dict["currency"];
		bundle.payment_type = (string) dict["payment_type"];
		bundle.display_price = (string) dict["display_price"];
		bundle.data = Json.Serialize(dict["data"]);

		return bundle;
	}

	/// <summary>
	/// Tracks an event. Events are saved locally.
	/// Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.
	/// </summary>
	/// <param name="evt">event type</param>
	/// <param name="properties">event properties</param>
	private void logEvent(string evt, IDictionary<string, object> properties) {
		string propertiesJson;
		if (properties != null) {
			propertiesJson = Json.Serialize(properties);
		} else {
			propertiesJson = Json.Serialize(new Dictionary<string, object>());
		}

		Log(string.Format("C# sendEvent {0} with properties {1}", evt, propertiesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_logEvent(instanceName, evt, propertiesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logEvent", instanceName, evt, propertiesJson);
		}
#endif
	}

	/// <summary>
	/// Tracks an event. Events are saved locally.
	/// Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.
	/// </summary>
	/// <param name="evt">event type</param>
	/// <param name="properties">event properties</param>
	/// <param name="outOfSession">if this event belongs to current session</param>
	private void logEvent(string evt, IDictionary<string, object> properties, bool outOfSession) {
		string propertiesJson;
		if (properties != null) {
			propertiesJson = Json.Serialize(properties);
		} else {
			propertiesJson = Json.Serialize(new Dictionary<string, object>());
		}

		Log(string.Format("C# sendEvent {0} with properties {1} and outOfSession {2}", evt, propertiesJson, outOfSession));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			if (outOfSession) {
				_AIRE_logOutOfSessionEvent(instanceName, evt, propertiesJson);
			} else {
				_AIRE_logEvent(instanceName, evt, propertiesJson);
			}
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logEvent", instanceName, evt, propertiesJson, outOfSession);
		}
#endif
	}

	/// <summary>
	/// Sets offline. If offline is true, then the SDK will not upload events to AIRE servers.
	/// However, it will still log events.
	/// </summary>
	/// <param name="offline"></param>
	public void setOffline(bool offline) {
		Log (string.Format("C# setOffline {0}", offline));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOffline(instanceName, offline);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOffline", instanceName, offline);
		}
#endif
	}

	/// <summary>
	/// Set user Id
	/// </summary>
	/// <param name="userId"></param>
	public void setUserId(string userId) {
		Log (string.Format("C# setUserId {0}", userId));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserId(instanceName, userId);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserId", instanceName, userId);
		}
#endif
	}

	/// <summary>
	/// Set user properties
	/// </summary>
	/// <param name="properties">properties dictionary</param>
	public void setUserProperties(IDictionary<string, object> properties) {
		string propertiesJson;
		if (properties != null) {
			propertiesJson = Json.Serialize(properties);
		} else {
			propertiesJson = Json.Serialize(new Dictionary<string, object>());
		}

		Log (string.Format("C# setUserProperties {0}", propertiesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserProperties(instanceName, propertiesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperties", instanceName, propertiesJson);
		}
#endif
	}

	/// <summary>
	/// Enables tracking opt out.
	/// If the user wants to opt out of all tracking, use this method to enable opt out for them.
	/// Once opt out is enabled, no events will be saved locally or sent to the server.
	/// </summary>
	/// <param name="enabled">enable opt out</param>
	public void setOptOut(bool enabled) {
		Log (string.Format("C# setOptOut {0}", enabled));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOptOut(instanceName, enabled);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOptOut", instanceName, enabled);
		}
#endif
	}

	/// <summary>
	/// When a user closes and reopens the app within minTimeBetweenSessionsMillis milliseconds,
	/// the reopen is considered part of the same session and the session continues.
	/// Otherwise, a new session is created. The default is 5 minutes.
	/// </summary>
	/// <param name="minTimeBetweenSessionsMillis">minimum time (milliseconds) between sessions</param>
	public void setMinTimeBetweenSessionsMillis(long minTimeBetweenSessionsMillis) {
		Log (string.Format("C# minTimeBetweenSessionsMillis {0}", minTimeBetweenSessionsMillis));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setMinTimeBetweenSessionsMillis(instanceName, minTimeBetweenSessionsMillis);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setMinTimeBetweenSessionsMillis", instanceName, minTimeBetweenSessionsMillis);
		}
#endif
	}

	/// <summary>
	/// If your app has its own system for tracking devices, you can set the deviceId.
	///
	/// NOTE: not recommended unless you know what you are doing.
	/// </summary>
	/// <param name="deviceId">device Id</param>
	public void setDeviceId(string deviceId) {
		Log (string.Format("C# setDeviceId {0}", deviceId));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setDeviceId(instanceName, deviceId);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setDeviceId", instanceName, deviceId);
		}
#endif
	}

	/// <summary>
	/// Enable COPPA (Children's Online Privacy Protection Act) restrictions on IDFA, IDFV, city, IP address and location tracking.
	/// This can be used by any customer that does not want to collect IDFA, IDFV, city, IP address and location tracking.
	/// </summary>
	public void enableCoppaControl() {
		Log ("C# enableCoppaControl");
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_enableCoppaControl(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("enableCoppaControl", instanceName);
		}
#endif
	}

	/// <summary>
	/// Disable COPPA (Children's Online Privacy Protection Act) restrictions on IDFA, IDFV, city, IP address and location tracking.
	/// </summary>
	public void disableCoppaControl() {
		Log (string.Format("C# disableCoppaControl"));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_disableCoppaControl(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("disableCoppaControl", instanceName);
		}
#endif
	}

	/// <summary>
	/// Customize server url events will be forwarded to.
	/// </summary>
	public void setServerUrl(string serverUrl) {
		Log (string.Format("C# setServerUrl"));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setServerUrl(instanceName, serverUrl);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setServerUrl", instanceName, serverUrl);
		}
#endif
	}

	[System.Obsolete("Please call setUserProperties instead", false)]
	public void setGlobalUserProperties(IDictionary<string, object> properties) {
		setUserProperties(properties);
	}

	public void logRevenue(double amount) {
		Log (string.Format("C# logRevenue {0}", amount));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_logRevenueAmount(instanceName, amount);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logRevenue", instanceName, amount);
		}
#endif
	}

	public void logRevenue(string productId, int quantity, double price) {
		Log (string.Format("C# logRevenue {0}, {1}, {2}", productId, quantity, price));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_logRevenue(instanceName, productId, quantity, price);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logRevenue", instanceName, productId, quantity, price);
		}
#endif
	}

	public void logRevenue(string productId, int quantity, double price, string receipt, string receiptSignature) {
		Log (string.Format("C# logRevenue {0}, {1}, {2} (with receipt)", productId, quantity, price));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_logRevenueWithReceipt(instanceName, productId, quantity, price, receipt);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logRevenue", instanceName, productId, quantity, price, receipt, receiptSignature);
		}
#endif
	}

	public void logRevenue(string productId, int quantity, double price, string receipt, string receiptSignature, string revenueType, IDictionary<string, object> eventProperties) {
		string propertiesJson;
		if (eventProperties != null) {
			propertiesJson = Json.Serialize(eventProperties);
		} else {
			propertiesJson = Json.Serialize(new Dictionary<string, object>());
		}

		Log (string.Format("C# logRevenue {0}, {1}, {2}, {3}, {4} (with receipt)", productId, quantity, price, revenueType, propertiesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_logRevenueWithReceiptAndProperties(instanceName, productId, quantity, price, receipt, revenueType, propertiesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("logRevenue", instanceName, productId, quantity, price, receipt, receiptSignature, revenueType, propertiesJson);
		}
#endif
	}

	/// <summary>
	/// Get current device Id.
	/// </summary>
	/// <returns></returns>
	public string getDeviceId() {
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			return _AIRE_getDeviceId(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			return pluginClass.CallStatic<string>("getDeviceId", instanceName);
		}
#endif
		return null;
	}

	/// <summary>
	/// Regenerates a new random deviceId for current user.
	/// Note: this is not recommended unless you know what you are doing. This can be used in conjunction with setUserId(null) to anonymize
	/// users after they log out.
	/// With a null userId and a completely new deviceId, the current user would appear as a brand new user in dashboard.
	/// </summary>
	public void regenerateDeviceId() {
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_regenerateDeviceId(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("regenerateDeviceId", instanceName);
		}
#endif
	}

	/// <summary>
	/// iOS:
	/// Use advertisingIdentifier instead of identifierForVendor as the device ID.
	/// Apple prohibits the use of advertisingIdentifier if your app does not have advertising.
	///
	/// Android:
	/// Whether to use the Android advertising ID (ADID) as the user's device ID.
	///
	/// **NOTE:** Must be called before `initializeApiKey`.
	/// </summary>
	public void useAdvertisingIdForDeviceId() {
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_useAdvertisingIdForDeviceId(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("useAdvertisingIdForDeviceId", instanceName);
		}
#endif
	}

	/// <summary>
	/// Whether to automatically log start and end session events corresponding to the start and end of a user's session.
	/// </summary>
	/// <param name="enabled"></param>
	public void trackSessionEvents(bool enabled) {
		Log (string.Format("C# trackSessionEvents {0}", enabled));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_trackingSessionEvents(instanceName, enabled);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("trackSessionEvents", instanceName, enabled);
		}
#endif
	}

	/// <summary>
	/// Get session Id
	/// </summary>
	/// <returns>sessionId</returns>
	public long getSessionId() {

#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			return _AIRE_getSessionId(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			return pluginClass.CallStatic<long>("getSessionId", instanceName);
		}
#endif
		return -1;
	}

	/// <summary>
	/// Manually forces the instance to immediately upload all unsent events.
	/// Use this method to force the class to immediately upload all queued events.
	/// </summary>
	private void uploadEvents() {
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_uploadEvents(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("uploadEvents", instanceName);
		}
#endif
	}

// User Property Operations
// ClearUserProperties
	public void clearUserProperties() {
		Log (string.Format("C# clearUserProperties"));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_clearUserProperties(instanceName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("clearUserProperties", instanceName);
		}
#endif
	}

// Unset
	public void unsetUserProperty(string property) {
		Log (string.Format("C# unsetUserProperty {0}", property));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_unsetUserProperty(instanceName, property);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("unsetUserProperty", instanceName, property);
		}
#endif
	}

// setOnce
	public void setOnceUserProperty(string property, bool value) {
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyBool(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setOnceUserProperty(string property, double value) {
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyDouble(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setOnceUserProperty(string property, float value) {
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyFloat(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setOnceUserProperty(string property, int value) {
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyInt(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setOnceUserProperty(string property, long value) {
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyLong(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setOnceUserProperty(string property, string value) {
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyString(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setOnceUserProperty(string property, IDictionary<string, object> values) {
		if (values == null) {
			return;
		}

		string valuesJson = Json.Serialize (values);
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyDict(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserPropertyDict", instanceName, property, valuesJson);
		}
#endif
	}

	public void setOnceUserProperty<T>(string property, IList<T> values) {
		if (values == null) {
			return;
		}

		Dictionary<string, object> wrapper = new Dictionary<string, object>()
		{
			{"list", values}
		};
		string valuesJson = Json.Serialize (wrapper);
		Log (string.Format("C# setOnceUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyList(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserPropertyList", instanceName, property, valuesJson);
		}
#endif
	}

	public void setOnceUserProperty(string property, bool[] array) {
		Log ("C# setOnceUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyBoolArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setOnceUserProperty(string property, double[] array) {
		Log ("C# setOnceUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyDoubleArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setOnceUserProperty(string property, float[] array) {
		Log ("C# setOnceUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyFloatArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setOnceUserProperty(string property, int[] array) {
		Log ("C# setOnceUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyIntArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setOnceUserProperty(string property, long[] array) {
		Log ("C# setOnceUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyLongArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setOnceUserProperty(string property, string[] array) {
		Log ("C# setOnceUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setOnceUserPropertyStringArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setOnceUserProperty", instanceName, property, array);
		}
#endif
	}

// set
	public void setUserProperty(string property, bool value) {
		Log (string.Format("C# setUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyBool(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setUserProperty(string property, double value) {
		Log (string.Format("C# setUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyDouble(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setUserProperty(string property, float value) {
		Log (string.Format("C# setUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyFloat(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setUserProperty(string property, int value) {
		Log (string.Format("C# setUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyInt(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setUserProperty(string property, long value) {
		Log (string.Format("C# setUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyLong(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setUserProperty(string property, string value) {
		Log (string.Format("C# setUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyString(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, value);
		}
#endif
	}

	public void setUserProperty(string property, IDictionary<string, object> values) {
		if (values == null) {
			return;
		}

		string valuesJson = Json.Serialize (values);
		Log (string.Format("C# setUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyDict(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, valuesJson);
		}
#endif
	}

	public void setUserProperty<T>(string property, IList<T> values) {
		if (values == null) {
			return;
		}

		Dictionary<string, object> wrapper = new Dictionary<string, object>()
		{
			{"list", values}
		};
		string valuesJson = Json.Serialize (wrapper);
		Log (string.Format("C# setUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyList(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserPropertyList", instanceName, property, valuesJson);
		}
#endif
	}

	public void setUserProperty(string property, bool[] array) {
		Log ("C# setUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyBoolArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setUserProperty(string property, double[] array) {
		Log ("C# setUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyDoubleArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setUserProperty(string property, float[] array) {
		Log ("C# setUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyFloatArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setUserProperty(string property, int[] array) {
		Log ("C# setUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyIntArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setUserProperty(string property, long[] array) {
		Log ("C# setUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyLongArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
#endif
	}

	public void setUserProperty(string property, string[] array) {
		Log ("C# setUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setUserPropertyStringArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setUserProperty", instanceName, property, array);
		}
#endif
	}


// add
	public void addUserProperty(string property, double value) {
		Log (string.Format("C# addUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_addUserPropertyDouble(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
#endif
	}

	public void addUserProperty(string property, float value) {
		Log (string.Format("C# addUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_addUserPropertyFloat(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
#endif
	}

	public void addUserProperty(string property, int value) {
		Log (string.Format("C# addUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_addUserPropertyInt(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
#endif
	}

	public void addUserProperty(string property, long value) {
		Log (string.Format("C# addUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_addUserPropertyLong(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
#endif
	}

	public void addUserProperty(string property, string value) {
		Log (string.Format("C# addUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_addUserPropertyString(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("addUserProperty", instanceName, property, value);
		}
#endif
	}

	public void addUserProperty(string property, IDictionary<string, object> values) {
		if (values == null) {
			return;
		}

		string valuesJson = Json.Serialize (values);
		Log (string.Format("C# addUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_addUserPropertyDict(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("addUserPropertyDict", instanceName, property, valuesJson);
		}
#endif
	}

// append
	public void appendUserProperty(string property, bool value) {
		Log (string.Format("C# appendUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyBool(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
#endif
	}

	public void appendUserProperty(string property, double value) {
		Log (string.Format("C# appendUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyDouble(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
#endif
	}

	public void appendUserProperty(string property, float value) {
		Log (string.Format("C# appendUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyFloat(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
#endif
	}

	public void appendUserProperty(string property, int value) {
		Log (string.Format("C# appendUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyInt(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
#endif
	}

	public void appendUserProperty(string property, long value) {
		Log (string.Format("C# appendUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyLong(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
#endif
	}

	public void appendUserProperty(string property, string value) {
		Log (string.Format("C# appendUserProperty {0}, {1}", property, value));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyString(instanceName, property, value);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, value);
		}
#endif
	}

	public void appendUserProperty(string property, IDictionary<string, object> values) {
		if (values == null) {
			return;
		}

		string valuesJson = Json.Serialize (values);
		Log (string.Format("C# appendUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyDict(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserPropertyDict", instanceName, property, valuesJson);
		}
#endif
	}

	public void appendUserProperty<T>(string property, IList<T> values) {
		if (values == null) {
			return;
		}

		Dictionary<string, object> wrapper = new Dictionary<string, object>()
		{
			{"list", values}
		};
		string valuesJson = Json.Serialize (wrapper);
		Log (string.Format("C# appendUserProperty {0}, {1}", property, valuesJson));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyList(instanceName, property, valuesJson);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserPropertyList", instanceName, property, valuesJson);
		}
#endif
	}

	public void appendUserProperty(string property, bool[] array) {
		Log ("C# appendUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyBoolArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
#endif
	}

	public void appendUserProperty(string property, double[] array) {
		Log ("C# appendUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyDoubleArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
#endif
	}

	public void appendUserProperty(string property, float[] array) {
		Log ("C# appendUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyFloatArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
#endif
	}

	public void appendUserProperty(string property, int[] array) {
		Log ("C# appendUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyIntArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
#endif
	}

	public void appendUserProperty(string property, long[] array) {
		Log ("C# appendUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyLongArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
#endif
	}

	public void appendUserProperty(string property, string[] array) {
		Log ("C# appendUserProperty", property, array);
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_appendUserPropertyStringArray(instanceName, property, array, array.Length);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("appendUserProperty", instanceName, property, array);
		}
#endif
	}

	private void setLibraryName(string libraryName) {
		Log (string.Format("C# setLibraryName {0}", libraryName));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setLibraryName(instanceName, libraryName);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setLibraryName", instanceName, libraryName);
		}
#endif
	}

	private void setLibraryVersion(string libraryVersion) {
		Log (string.Format("C# setLibraryVersion {0}", libraryVersion));
#if (UNITY_IPHONE || UNITY_TVOS)
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
			_AIRE_setLibraryVersion(instanceName, libraryVersion);
		}
#endif

#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			pluginClass.CallStatic("setLibraryVersion", instanceName, libraryVersion);
		}
#endif
	}

	// This method is deprecated
	public void startSession() { return; }

	// This method is deprecated
	public void endSession() { return; }
}
