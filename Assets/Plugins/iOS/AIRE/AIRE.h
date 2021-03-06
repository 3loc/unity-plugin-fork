//
//  AIRE.h
//  Copyright (c) 2013 Amplitude Inc. (https://amplitude.com/)
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//

#import <Foundation/Foundation.h>
#import "AIREIdentify.h"
#import "AIRERevenue.h"
#import "AIRETrackingOptions.h"

/**
 AIRE iOS SDK.

 Use the AIRE SDK to track events in your application.

 Setup:

 1. In every file that uses analytics, import AIRE.h at the top `#import "AIRE.h"`
 2. Be sure to initialize the API in your didFinishLaunchingWithOptions delegate `[[AIRE instance] initializeApiKey:@"YOUR_API_KEY_HERE"];`
 3. Track an event anywhere in the app `[[AIRE instance] logEvent:@"EVENT_IDENTIFIER_HERE"];`
 4. You can attach additional data to any event by passing a NSDictionary object:

        NSMutableDictionary *eventProperties = [NSMutableDictionary dictionary];
        [eventProperties setValue:@"VALUE_GOES_HERE" forKey:@"KEY_GOES_HERE"];
        [[AIRE instance] logEvent:@"Compute Hash" withEventProperties:eventProperties];

 **Note:** you should call SDK methods on an AIRE instance, for example logging events with the default instance: `[[AIRE instance] logEvent:@"testEvent"];`

 **Note:** the SDK supports tracking data to multiple AIRE apps, via separate named instances. For example: `[[AIRE instanceWithName:@"app1"] logEvent:@"testEvent"];` See [Tracking Events to Multiple Apps](https://github.com/aire/aire-ios#tracking-events-to-multiple-aire-apps).

 For more details on the setup and usage, be sure to checkout the [README](https://github.com/aire/AIRE-iOS#aire-ios-sdk)
 */
@interface AIRE : NSObject

#pragma mark - Properties

/**
 API key for your AIRE App.
 */
@property (nonatomic, copy, readonly) NSString *apiKey;

/**
 Identifier for the current user.
 */
@property (nonatomic, copy, readonly) NSString *userId;

/**
 Identifier for the current device.
 */
@property (nonatomic, copy, readonly) NSString *deviceId;

/**
 Name of the SDK instance (ex: no name for default instance, or custom name for a named instance)
 */
@property (nonatomic, copy, readonly) NSString *instanceName;
@property (nonatomic, copy, readonly) NSString *propertyListPath;

/**
 Whether or to opt the current user out of tracking. If true then this blocks the logging of any events and properties, and blocks the sending of events to AIRE servers.
 */
@property (nonatomic, assign, readwrite) BOOL optOut;


/**-----------------------------------------------------------------------------
 * @name Configurable SDK thresholds and parameters
 * -----------------------------------------------------------------------------
 */

/**
 The maximum number of events that can be stored locally before forcing an upload. The default is 30 events.
 */
@property (nonatomic, assign) int eventUploadThreshold;

/**
 The maximum number of events that can be uploaded in a single request. The default is 100 events.
 */
@property (nonatomic, assign) int eventUploadMaxBatchSize;

/**
 The maximum number of events that can be stored lcoally. The default is 1000 events.
 */
@property (nonatomic, assign) int eventMaxCount;

/**
 The amount of time after an event is logged that events will be batched before being uploaded to the server. The default is 30 seconds.
 */
@property (nonatomic, assign) int eventUploadPeriodSeconds;

/**
 When a user closes and reopens the app within minTimeBetweenSessionsMillis milliseconds, the reopen is considered part of the same session and the session continues. Otherwise, a new session is created. The default is 5 minutes.
 */
@property (nonatomic, assign) long minTimeBetweenSessionsMillis;

/**
 Whether to automatically log start and end session events corresponding to the start and end of a user's session.
 */
@property (nonatomic, assign) BOOL trackingSessionEvents;

/**
 Library name is default as `aire-ios`.
 Notice: You will only want to set it when following conditions are met.
 1. You develop your own library which bridges AIRE iOS native library.
 2. You want to track your library as one of the data sources.
 */
@property (nonatomic, copy, readwrite) NSString *libraryName;

/**
 Library version is default as the latest AIRE iOS SDK version.
 Notice: You will only want to set it when following conditions are met.
 1. You develop your own library which bridges AIRE iOS native library.
 2. You want to track your library as one of the data sources.
*/
@property (nonatomic, copy, readwrite) NSString *libraryVersion;

#pragma mark - Methods

/**-----------------------------------------------------------------------------
 * @name Fetching AIRE SDK instance
 * -----------------------------------------------------------------------------
 */

/**
 This fetches the default SDK instance. Recommended if you are only logging events to a single app.

 @returns the default AIRE SDK instance
 */
+ (AIRE *)instance;

/**
 This fetches a named SDK instance. Use this if logging events to multiple AIRE apps.

 @param instanceName the name of the SDK instance to fetch.

 @returns the AIRE SDK instance corresponding to `instanceName`

 @see [Tracking Events to Multiple AIRE Apps](https://github.com/aire/aire-ios#tracking-events-to-multiple-aire-apps)
 */
+ (AIRE *)instanceWithName:(NSString*) instanceName;

/**-----------------------------------------------------------------------------
 * @name Initialize the AIRE SDK with your AIRE API Key
 * -----------------------------------------------------------------------------
 */

/**
 Initializes the AIRE instance with your AIRE API key

 We recommend you first initialize your class within your "didFinishLaunchingWithOptions" method inside your app delegate.

 **Note:** this is required before you can log any events.

 @param apiKey Your AIRE key obtained from your dashboard at https://aire.com/settings
 */
- (void)initializeApiKey:(NSString*) apiKey;

/**
 Initializes the AIRE instance with your AIRE API key and sets a user identifier for the current user.

 We recommend you first initialize your class within your "didFinishLaunchingWithOptions" method inside your app delegate.

 **Note:** this is required before you can log any events.

 @param apiKey Your AIRE key obtained from your dashboard at https://aire.com/settings

 @param userId If your app has its own login system that you want to track users with, you can set the userId.

*/
- (void)initializeApiKey:(NSString*) apiKey userId:(NSString*) userId;


/**-----------------------------------------------------------------------------
 * @name Logging Events
 * -----------------------------------------------------------------------------
 */

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)
 */
- (void)logEvent:(NSString*) eventType;

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.
 @param eventProperties          You can attach additional data to any event by passing a NSDictionary object with property: value pairs.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)
 */
- (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties;

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.
 @param eventProperties          You can attach additional data to any event by passing a NSDictionary object with property: value pairs.
 @param outOfSession             If YES, will track the event as out of session. Useful for push notification events.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)
 @see [Tracking Sessions](https://github.com/aire/AIRE-iOS#tracking-sessions)
 */
- (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties outOfSession:(BOOL) outOfSession;

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.
 @param eventProperties          You can attach additional data to any event by passing a NSDictionary object with property: value pairs.
 @param groups                   You can specify event-level groups for this user by passing a NSDictionary object with groupType: groupName pairs. Note the keys need to be strings, and the values can either be strings or an array of strings.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)

 @see [Setting Groups](https://github.com/aire/AIRE-iOS#setting-groups)
 */
- (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties withGroups:(NSDictionary*) groups;

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.
 @param eventProperties          You can attach additional data to any event by passing a NSDictionary object with property: value pairs.
 @param groups                   You can specify event-level groups for this user by passing a NSDictionary object with groupType: groupName pairs. Note the keys need to be strings, and the values can either be strings or an array of strings.
 @param outOfSession             If YES, will track the event as out of session. Useful for push notification events.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)

 @see [Setting Groups](https://github.com/aire/AIRE-iOS#setting-groups)

 @see [Tracking Sessions](https://github.com/aire/AIRE-iOS#tracking-sessions)
 */
- (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties withGroups:(NSDictionary*) groups outOfSession:(BOOL) outOfSession;

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.
 @param eventProperties          You can attach additional data to any event by passing a NSDictionary object with property: value pairs.
 @param groups                   You can specify event-level groups for this user by passing a NSDictionary object with groupType: groupName pairs. Note the keys need to be strings, and the values can either be strings or an array of strings.
 @param longLongTimestamp        You can specify a custom timestamp by passing the milliseconds since epoch UTC time as a long long.
 @param outOfSession             If YES, will track the event as out of session. Useful for push notification events.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)

 @see [Setting Groups](https://github.com/aire/AIRE-iOS#setting-groups)

 @see [Tracking Sessions](https://github.com/aire/AIRE-iOS#tracking-sessions)
 */
- (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties withGroups:(NSDictionary*) groups withLongLongTimestamp:(long long) longLongTimestamp outOfSession:(BOOL)outOfSession;

/**
 Tracks an event. Events are saved locally.

 Uploads are batched to occur every 30 events or every 30 seconds (whichever comes first), as well as on app close.

 @param eventType                The name of the event you wish to track.
 @param eventProperties          You can attach additional data to any event by passing a NSDictionary object with property: value pairs.
 @param groups                   You can specify event-level groups for this user by passing a NSDictionary object with groupType: groupName pairs. Note the keys need to be strings, and the values can either be strings or an array of strings.
 @param timestamp                You can specify a custom timestamp by passing an NSNumber representing the milliseconds since epoch UTC time. We recommend using [NSNumber numberWithLongLong:milliseconds] to create the value. If nil is passed in, then the event will be timestamped with the current time.
 @param outOfSession             If YES, will track the event as out of session. Useful for push notification events.

 @see [Tracking Events](https://github.com/aire/aire-ios#tracking-events)

 @see [Setting Groups](https://github.com/aire/AIRE-iOS#setting-groups)

 @see [Tracking Sessions](https://github.com/aire/AIRE-iOS#tracking-sessions)
 */
- (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties withGroups:(NSDictionary*) groups withTimestamp:(NSNumber*) timestamp outOfSession:(BOOL)outOfSession;

/**-----------------------------------------------------------------------------
 * @name Logging Revenue
 * -----------------------------------------------------------------------------
 */

/**
 **Note: this is deprecated** - please use `logRevenueV2` and `AIRERevenue`

 Tracks revenue.

 To track revenue from a user, call [[AIRE instance] logRevenue:[NSNumber numberWithDouble:3.99]] each time the user generates revenue. logRevenue: takes in an NSNumber with the dollar amount of the sale as the only argument. This allows us to automatically display data relevant to revenue on the AIRE website, including average revenue per daily active user (ARPDAU), 7, 30, and 90 day revenue, lifetime value (LTV) estimates, and revenue by advertising campaign cohort and daily/weekly/monthly cohorts.

 @param amount                   The amount of revenue to track, e.g. "3.99".

 @see [LogRevenue Backwards Compatability](https://github.com/aire/AIRE-iOS#backwards-compatibility)
 */
- (void)logRevenue:(NSNumber*) amount;

/**
 **Note: this is deprecated** - please use `logRevenueV2` and `AIRERevenue`

 Tracks revenue. This allows us to automatically display data relevant to revenue on the AIRE website, including average revenue per daily active user (ARPDAU), 7, 30, and 90 day revenue, lifetime value (LTV) estimates, and revenue by advertising campaign cohort and daily/weekly/monthly cohorts.

 @param productIdentifier        The identifier for the product in the transaction, e.g. "com.aire.productId"
 @param quantity                 The number of products in the transaction. Revenue amount is calculated as quantity * price
 @param price                    The price of the products in the transaction. Revenue amount is calculated as quantity * price

 @see [LogRevenueV2](https://github.com/aire/AIRE-iOS#tracking-revenue)
 @see [LogRevenue Backwards Compatability](https://github.com/aire/AIRE-iOS#backwards-compatibility)
 */
- (void)logRevenue:(NSString*) productIdentifier quantity:(NSInteger) quantity price:(NSNumber*) price;

/**
 **Note: this is deprecated** - please use `logRevenueV2` and `AIRERevenue`

 Tracks revenue. This allows us to automatically display data relevant to revenue on the AIRE website, including average revenue per daily active user (ARPDAU), 7, 30, and 90 day revenue, lifetime value (LTV) estimates, and revenue by advertising campaign cohort and daily/weekly/monthly cohorts.

 For validating revenue, use [[AIRE instance] logRevenue:@"com.company.app.productId" quantity:1 price:[NSNumber numberWithDouble:3.99] receipt:transactionReceipt]

 @param productIdentifier        The identifier for the product in the transaction, e.g. "com.aire.productId"
 @param quantity                 The number of products in the transaction. Revenue amount is calculated as quantity * price
 @param price                    The price of the products in the transaction. Revenue amount is calculated as quantity * price
 @param receipt                  The receipt data from the App Store. Required if you want to verify this revenue event.

 @see [LogRevenueV2](https://github.com/aire/AIRE-iOS#tracking-revenue)
 @see [LogRevenue Backwards Compatability](https://github.com/aire/AIRE-iOS#backwards-compatibility)
 @see [Revenue Verification](https://github.com/aire/AIRE-iOS#revenue-verification)
 */
- (void)logRevenue:(NSString*) productIdentifier quantity:(NSInteger) quantity price:(NSNumber*) price receipt:(NSData*) receipt;

/**
 Tracks revenue - API v2. This uses the `AIRERevenue` object to store transaction properties such as quantity, price, and revenue type. This is the recommended method for tracking revenue in AIRE.

 For validating revenue, make sure the receipt data is set on the AIRERevenue object.

 To track revenue from a user, create an AIRERevenue object each time the user generates revenue, and set the revenue properties (productIdentifier, price, quantity). logRevenuev2: takes in an AIRERevenue object. This allows us to automatically display data relevant to revenue on the AIRE website, including average revenue per daily active user (ARPDAU), 7, 30, and 90 day revenue, lifetime value (LTV) estimates, and revenue by advertising campaign cohort and daily/weekly/monthly cohorts.

 @param revenue AIRERevenue object       revenue object contains all revenue information

 @see [Tracking Revenue](https://github.com/aire/AIRE-iOS#tracking-revenue)
 */
- (void)logRevenueV2:(AIRERevenue*) revenue;

/**-----------------------------------------------------------------------------
 * @name User Properties and User Property Operations
 * -----------------------------------------------------------------------------
 */

/**
 Update user properties using operations provided via Identify API.

 To update user properties, first create an AIREIdentify object. For example if you wanted to set a user's gender, and then increment their karma count by 1, you would do:

    AIREIdentify *identify = [[[AIREIdentify identify] set:@"gender" value:@"male"] add:@"karma" value:[NSNumber numberWithInt:1]];

 Then you would pass this AIREIdentify object to the identify function to send to the server:

    [[AIRE instance] identify:identify];

 @param identify                   An AIREIdentify object with the intended user property operations

 @see [User Properties and User Property Operations](https://github.com/aire/AIRE-iOS#user-properties-and-user-property-operations)

 */

- (void)identify:(AIREIdentify *)identify;

/**
 Update user properties using operations provided via Identify API. If outOfSession is `YES` then the identify event is logged with a session id of -1 and does not trigger any session-handling logic.

 To update user properties, first create an AIREIdentify object. For example if you wanted to set a user's gender, and then increment their karma count by 1, you would do:

 AIREIdentify *identify = [[[AIREIdentify identify] set:@"gender" value:@"male"] add:@"karma" value:[NSNumber numberWithInt:1]];

 Then you would pass this AIREIdentify object to the identify function to send to the server:

 [[AIRE instance] identify:identify outOfSession:YES];

 @param identify                   An AIREIdentify object with the intended user property operations
 @param outOfSession               Whether to log identify event out of session

 @see [User Properties and User Property Operations](https://github.com/aire/AIRE-iOS#user-properties-and-user-property-operations)

 */

- (void)identify:(AIREIdentify *)identify outOfSession:(BOOL) outOfSession;

/**

 Adds properties that are tracked on the user level.

 **Note:** Property keys must be <code>NSString</code> objects and values must be serializable.

 @param userProperties          An NSDictionary containing any additional data to be tracked.

 @see [Setting Multiple Properties with setUserProperties](https://github.com/aire/AIRE-iOS#setting-multiple-properties-with-setuserproperties)
 */
- (void)setUserProperties:(NSDictionary*) userProperties;

/**

 **NOTE: this method is deprecated** - use `setUserProperties` instead. In earlier versions of the SDK, replace = YES replaced the in-memory userProperties dictionary with the input; however, now userProperties are no longer stored in memory, so the flag does nothing.

 Adds properties that are tracked on the user level.

 **Note:** Property keys must be <code>NSString</code> objects and values must be serializable.

 @param userProperties          An NSDictionary containing any additional data to be tracked.
 @param replace                 This is deprecated. In earlier versions of this SDK, this replaced the in-memory userProperties dictionary with the input, but now userProperties are no longer stored in memory.

 @see [Setting Multiple Properties with setUserProperties](https://github.com/aire/AIRE-iOS#setting-multiple-properties-with-setuserproperties)
 */
- (void)setUserProperties:(NSDictionary*) userProperties replace:(BOOL) replace;

/**
 Clears all properties that are tracked on the user level.

 **Note: the result is irreversible!**

 @see [Clearing user properties](https://github.com/aire/AIRE-iOS#clearing-user-properties-with-clearuserproperties)
 */

- (void)clearUserProperties;

/**
 Adds a user to a group or groups. You need to specify a groupType and groupName(s).

 For example you can group people by their organization. In that case groupType is "orgId", and groupName would be the actual ID(s). groupName can be a string or an array of strings to indicate a user in multiple groups.

 You can also call setGroup multiple times with different groupTypes to track multiple types of groups (up to 5 per app).

 **Note:** this will also set groupType: groupName as a user property.

 @param groupType               You need to specify a group type (for example "orgId").

 @param groupName               The value for the group name, can be a string or an array of strings, (for example for groupType orgId, the groupName would be the actual id number, like 15).

 @see [Setting Groups](https://github.com/aire/AIRE-iOS#setting-groups)
 */

- (void)setGroup:(NSString*) groupType groupName:(NSObject*) groupName;

- (void)groupIdentifyWithGroupType:(NSString*) groupType groupName:(NSObject*) groupName groupIdentify:(AIREIdentify *) groupIdentify;

- (void)groupIdentifyWithGroupType:(NSString*) groupType groupName:(NSObject*) groupName groupIdentify:(AIREIdentify *) groupIdentify outOfSession:(BOOL) outOfSession;

/**-----------------------------------------------------------------------------
 * @name Setting User and Device Identifiers
 * -----------------------------------------------------------------------------
 */

/**
 Sets the userId.
 @param userId                  If your app has its own login system that you want to track users with, you can set the userId.
 @see [Setting Custom UserIds](https://github.com/aire/AIRE-iOS#setting-custom-user-ids)
 */

- (void)setUserId:(NSString*) userId;

/**
 Sets the userId and starts a new session. The previous session for the previous user will be terminated and a new session will begin for the new user id.

 @param userId                  If your app has its own login system that you want to track users with, you can set the userId.

 @see [Setting Custom UserIds](https://github.com/aire/AIRE-iOS#setting-custom-user-ids)
 */
- (void)setUserId:(NSString*) userId startNewSession:(BOOL) startNewSession;

/**
 Sets the deviceId.

 **NOTE: not recommended unless you know what you are doing**

 @param deviceId                  If your app has its own system for tracking devices, you can set the deviceId.

 @see [Setting Custom Device Ids](https://github.com/aire/AIRE-iOS#custom-device-ids)
 */
- (void)setDeviceId:(NSString*) deviceId;

/**-----------------------------------------------------------------------------
 * @name Configuring the SDK instance
 * -----------------------------------------------------------------------------
 */

/**
 Enables tracking opt out.

 If the user wants to opt out of all tracking, use this method to enable opt out for them. Once opt out is enabled, no events will be saved locally or sent to the server. Calling this method again with enabled set to NO will turn tracking back on for the user.

 @param enabled                  Whether tracking opt out should be enabled or disabled.
 */
- (void)setOptOut:(BOOL)enabled;

/**
 Disables sending logged events to AIRE servers.

 If you want to stop logged events from being sent to AIRE severs, use this method to set the client to offline. Once offline is enabled, logged events will not be sent to the server until offline is disabled. Calling this method again with offline set to NO will allow events to be sent to server and the client will attempt to send events that have been queued while offline.

 @param offline                  Whether logged events should be sent to AIRE servers.
 */
- (void)setOffline:(BOOL)offline;

/**
 Enables location tracking.

 If the user has granted your app location permissions, the SDK will also grab the location of the user. AIRE will never prompt the user for location permissions itself, this must be done by your app.

 **Note:** the user's location is only fetched once per session. Use `updateLocation` to force the SDK to fetch the user's latest location.
 */
- (void)enableLocationListening;

/**
 Disables location tracking. If you want location tracking disabled on startup of the app, call disableLocationListening before you call initializeApiKey.
 */
- (void)disableLocationListening;

/**
 Forces the SDK to update with the user's last known location if possible.

 If you want to manually force the SDK to update with the user's last known location, call updateLocation.
 */
- (void)updateLocation;

/**
 Uses advertisingIdentifier instead of identifierForVendor as the device ID

 Apple prohibits the use of advertisingIdentifier if your app does not have advertising. Useful for tying together data from advertising campaigns to anlaytics data.

 **NOTE:** Must be called before initializeApiKey: is called to function.
 */
- (void)useAdvertisingIdForDeviceId;

/**
 Disables tracking of advertisingIdentifier by the SDK

 **NOTE:** Must be called before initializeApiKey: is called to function.
 */
- (void)disableIdfaTracking;

- (void)setTrackingOptions:(AIRETrackingOptions*) options;

/**
 Enable COPPA (Children's Online Privacy Protection Act) restrictions on IDFA, IDFV, city, IP address and location tracking.
 This can be used by any customer that does not want to collect IDFA, IDFV, city, IP address and location tracking.
 */
- (void)enableCoppaControl;

/**
 Disable COPPA (Children's Online Privacy Protection Act) restrictions on IDFA, IDFV, city, IP address and location tracking.
 */
- (void)disableCoppaControl;

- (void)setServerUrl:(NSString*) serverUrl;

- (void)setBearerToken:(NSString *) token;

/**-----------------------------------------------------------------------------
 * @name Other Methods
 * -----------------------------------------------------------------------------
 */

/**
 Prints the number of events in the queue.

 Debugging method to find out how many events are being stored locally on the device.
 */
- (void)printEventsCount;

/**
 Fetches the deviceId, a unique identifier shared between multiple users using the same app on the same device.

 @returns the deviceId.
 */
- (NSString*)getDeviceId;

/**
 Regenerates a new random deviceId for current user. Note: this is not recommended unless you know what you are doing. This can be used in conjunction with setUserId:nil to anonymize users after they log out. With a nil userId and a completely new deviceId, the current user would appear as a brand new user in dashboard.

 @see [Logging Out Users](https://github.com/aire/AIRE-iOS#logging-out-and-anonymous-users)
 */
- (void)regenerateDeviceId;

/**
 Fetches the current sessionId, an identifier used by AIRE to group together events tracked during the same session.

 @returns the current session id

 @see [Tracking Sessions](https://github.com/aire/AIRE-iOS#tracking-sessions)
 */
- (long long)getSessionId;

/**
 Manually forces the instance to immediately upload all unsent events.

 Events are saved locally. Uploads are batched to occur every 30 events and every 30 seconds, as well as on app close. Use this method to force the class to immediately upload all queued events.
 */
- (void)uploadEvents;

/**
 Call to check if the SDK is ready to start a new session at timestamp. Returns YES if a new session was started, otherwise NO and current session is extended. Only use if you know what you are doing. Recommended to use current time in UTC milliseconds for timestamp.
 */
- (BOOL)startOrContinueSession:(long long) timestamp;

#pragma mark - Deprecated methods

- (void)initializeApiKey:(NSString*) apiKey userId:(NSString*) userId startSession:(BOOL)startSession __attribute((deprecated()));

- (void)startSession __attribute((deprecated()));

+ (void)initializeApiKey:(NSString*) apiKey __attribute((deprecated()));

+ (void)initializeApiKey:(NSString*) apiKey userId:(NSString*) userId __attribute((deprecated()));

+ (void)logEvent:(NSString*) eventType __attribute((deprecated()));

+ (void)logEvent:(NSString*) eventType withEventProperties:(NSDictionary*) eventProperties __attribute((deprecated()));

+ (void)logRevenue:(NSNumber*) amount __attribute((deprecated()));

+ (void)logRevenue:(NSString*) productIdentifier quantity:(NSInteger) quantity price:(NSNumber*) price __attribute((deprecated())) __attribute((deprecated()));

+ (void)logRevenue:(NSString*) productIdentifier quantity:(NSInteger) quantity price:(NSNumber*) price receipt:(NSData*) receipt __attribute((deprecated()));

+ (void)uploadEvents __attribute((deprecated()));

+ (void)setUserProperties:(NSDictionary*) userProperties __attribute((deprecated()));

+ (void)setUserId:(NSString*) userId __attribute((deprecated()));

+ (void)enableLocationListening __attribute((deprecated()));

+ (void)disableLocationListening __attribute((deprecated()));

+ (void)useAdvertisingIdForDeviceId __attribute((deprecated()));

+ (void)printEventsCount __attribute((deprecated()));

+ (NSString*)getDeviceId __attribute((deprecated()));
@end

#pragma mark - constants

extern NSString *const kAIRESessionStartEvent;
extern NSString *const kAIRESessionEndEvent;
extern NSString *const kAIRERevenueEvent;
