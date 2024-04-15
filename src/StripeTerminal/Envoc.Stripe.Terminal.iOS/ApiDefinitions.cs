using System;
using Foundation;
using ObjCRuntime;

namespace StripeTerminal
{

	// @protocol SCPJSONDecodable <NSObject>
	/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
	[Protocol, Model]
	[BaseType(typeof(NSObject))]
	interface SCPJSONDecodable
	{
		// @required +(instancetype _Nullable)decodedObjectFromJSON:(NSDictionary * _Nullable)json;
		[Static, Abstract]
		[Export("decodedObjectFromJSON:")]
		[return: NullAllowed]
		SCPJSONDecodable DecodedObjectFromJSON([NullAllowed] NSDictionary json);

		// @required @property (readonly, nonatomic) NSDictionary * _Nonnull originalJSON;
		[Abstract]
		[Export("originalJSON")]
		NSDictionary OriginalJSON { get; }
	}

	public partial interface ISCPJSONDecodable { }

    // @interface SCPAddress : NSObject <SCPJSONDecodable>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPAddress : ISCPJSONDecodable
	{
		// @property (readonly, copy, nonatomic) NSString * _Nullable city;
		[NullAllowed, Export ("city")]
		string City { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable country;
		[NullAllowed, Export ("country")]
		string Country { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable line1;
		[NullAllowed, Export ("line1")]
		string Line1 { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable line2;
		[NullAllowed, Export ("line2")]
		string Line2 { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable postalCode;
		[NullAllowed, Export ("postalCode")]
		string PostalCode { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable state;
		[NullAllowed, Export ("state")]
		string State { get; }
	}

	// @interface SCPAmountDetails : NSObject <SCPJSONDecodable, NSCopying>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPAmountDetails : ISCPJSONDecodable, INSCopying
	{
		// @property (readonly, nonatomic) SCPTip * _Nullable tip;
		[NullAllowed, Export ("tip")]
		SCPTip Tip { get; }
	}

    // typedef void (^SCPConnectionTokenCompletionBlock)(NSString * _Nullable, NSError * _Nullable);
    delegate void SCPConnectionTokenCompletionBlock([NullAllowed] string token, [NullAllowed] NSError error);

    // typedef void (^SCPLogListenerBlock)(NSString * _Nonnull);
    delegate void SCPLogListenerBlock(string listener);

    // typedef void (^SCPPaymentMethodCompletionBlock)(SCPPaymentMethod * _Nullable, NSError * _Nullable);
    delegate void SCPPaymentMethodCompletionBlock([NullAllowed] SCPPaymentMethod paymentMethod, [NullAllowed] NSError error);

    // typedef void (^SCPErrorCompletionBlock)(NSError * _Nullable);
    delegate void SCPErrorCompletionBlock([NullAllowed] NSError error);

    // typedef void (^SCPConfirmPaymentIntentCompletionBlock)(SCPPaymentIntent * _Nullable, SCPConfirmPaymentIntentError * _Nullable);
    delegate void SCPConfirmPaymentIntentCompletionBlock([NullAllowed] SCPPaymentIntent paymentIntent, [NullAllowed] SCPConfirmPaymentIntentError error);

    // typedef void (^SCPConfirmRefundCompletionBlock)(SCPRefund * _Nullable, SCPConfirmRefundError * _Nullable);
    delegate void SCPConfirmRefundCompletionBlock([NullAllowed] SCPRefund refund, [NullAllowed] SCPConfirmRefundError error);

    // typedef void (^SCPRefundCompletionBlock)(SCPRefund * _Nullable, NSError * _Nullable);
    delegate void SCPRefundCompletionBlock([NullAllowed] SCPRefund refund, [NullAllowed] NSError error);

    // typedef void (^SCPPaymentIntentCompletionBlock)(SCPPaymentIntent * _Nullable, NSError * _Nullable);
    delegate void SCPPaymentIntentCompletionBlock([NullAllowed] SCPPaymentIntent paymentIntent, [NullAllowed] NSError error);

    // typedef void (^SCPSetupIntentCompletionBlock)(SCPSetupIntent * _Nullable, NSError * _Nullable);
    delegate void SCPSetupIntentCompletionBlock([NullAllowed] SCPSetupIntent setupIntent, [NullAllowed] NSError error);

    // typedef void (^SCPConfirmSetupIntentCompletionBlock)(SCPSetupIntent * _Nullable, SCPConfirmSetupIntentError * _Nullable);
    delegate void SCPConfirmSetupIntentCompletionBlock([NullAllowed] SCPSetupIntent setupIntent, [NullAllowed] SCPConfirmSetupIntentError error);

    // typedef void (^SCPLocationsCompletionBlock)(NSArray<SCPLocation *> * _Nullable, BOOL, NSError * _Nullable);
    delegate void SCPLocationsCompletionBlock([NullAllowed] SCPLocation[] locations, bool arg1, [NullAllowed] NSError error);

    // typedef void (^SCPReaderCompletionBlock)(SCPReader * _Nullable, NSError * _Nullable);
    delegate void SCPReaderCompletionBlock([NullAllowed] SCPReader reader, [NullAllowed] NSError error);

    // typedef void (^SCPCollectInputsCompletionBlock)(NSArray<SCPCollectInputsResult *> * _Nullable, NSError * _Nullable);
    delegate void SCPCollectInputsCompletionBlock([NullAllowed] SCPCollectInputsResult[] collectInputsResults, [NullAllowed] NSError error);

    // typedef void (^SCPReaderSettingsCompletionBlock)(SCPReaderSettings * _Nullable, NSError * _Nullable);
    delegate void SCPReaderSettingsCompletionBlock([NullAllowed] SCPReaderSettings settings, [NullAllowed] NSError error);

    // audit-objc-generics: @interface SCPBuilder<__covariant T> : NSObject
    [BaseType(typeof(NSObject))]
    interface SCPBuilder<T>
    {
        // -(T _Nullable)build:(NSError * _Nullable * _Nullable)error;
        [Export("build:")]
        [return: NullAllowed]
        T Build([NullAllowed] out NSError error);
    }

    // @interface SCPConnectionConfiguration : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPConnectionConfiguration
    {
    }

    // @protocol SCPBluetoothReaderDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPBluetoothReaderDelegate
	{
		// @required -(void)reader:(SCPReader * _Nonnull)reader didReportAvailableUpdate:(SCPReaderSoftwareUpdate * _Nonnull)update;
		[Abstract]
		[Export ("reader:didReportAvailableUpdate:")]
		void AvailableUpdate(SCPReader reader, SCPReaderSoftwareUpdate update);

		// @required -(void)reader:(SCPReader * _Nonnull)reader didStartInstallingUpdate:(SCPReaderSoftwareUpdate * _Nonnull)update cancelable:(SCPCancelable * _Nullable)cancelable;
		[Abstract]
		[Export ("reader:didStartInstallingUpdate:cancelable:")]
		void StartInstallingUpdate(SCPReader reader, SCPReaderSoftwareUpdate update, [NullAllowed] SCPCancelable cancelable);

		// @required -(void)reader:(SCPReader * _Nonnull)reader didReportReaderSoftwareUpdateProgress:(float)progress __attribute__((swift_name("reader(_:didReportReaderSoftwareUpdateProgress:)")));
		[Abstract]
		[Export ("reader:didReportReaderSoftwareUpdateProgress:")]
		void SoftwareUpdateProgress(SCPReader reader, float progress);

		// @required -(void)reader:(SCPReader * _Nonnull)reader didFinishInstallingUpdate:(SCPReaderSoftwareUpdate * _Nullable)update error:(NSError * _Nullable)error __attribute__((swift_name("reader(_:didFinishInstallingUpdate:error:)")));
		[Abstract]
		[Export ("reader:didFinishInstallingUpdate:error:")]
		void FinishInstallingUpdate(SCPReader reader, [NullAllowed] SCPReaderSoftwareUpdate update, [NullAllowed] NSError error);

		// @required -(void)reader:(SCPReader * _Nonnull)reader didRequestReaderInput:(SCPReaderInputOptions)inputOptions __attribute__((swift_name("reader(_:didRequestReaderInput:)")));
		[Abstract]
		[Export ("reader:didRequestReaderInput:")]
		void ReaderInput(SCPReader reader, SCPReaderInputOptions inputOptions);

		// @required -(void)reader:(SCPReader * _Nonnull)reader didRequestReaderDisplayMessage:(SCPReaderDisplayMessage)displayMessage __attribute__((swift_name("reader(_:didRequestReaderDisplayMessage:)")));
		[Abstract]
		[Export ("reader:didRequestReaderDisplayMessage:")]
		void ReaderDisplayMessage(SCPReader reader, SCPReaderDisplayMessage displayMessage);

		// @optional -(void)reader:(SCPReader * _Nonnull)reader didReportReaderEvent:(SCPReaderEvent)event info:(NSDictionary * _Nullable)info __attribute__((swift_name("reader(_:didReportReaderEvent:info:)")));
		[Export ("reader:didReportReaderEvent:info:")]
		void ReaderEvent(SCPReader reader, SCPReaderEvent @event, [NullAllowed] NSDictionary info);

		// @optional -(void)reader:(SCPReader * _Nonnull)reader didReportBatteryLevel:(float)batteryLevel status:(SCPBatteryStatus)status isCharging:(BOOL)isCharging __attribute__((swift_name("reader(_:didReportBatteryLevel:status:isCharging:)")));
		[Export ("reader:didReportBatteryLevel:status:isCharging:")]
		void BatteryLevel(SCPReader reader, float batteryLevel, SCPBatteryStatus status, bool isCharging);

		// @optional -(void)readerDidReportLowBatteryWarning:(SCPReader * _Nonnull)reader __attribute__((swift_name("readerDidReportLowBatteryWarning(_:)")));
		[Export ("readerDidReportLowBatteryWarning:")]
		void LowBatteryWarning (SCPReader reader);

        // @optional -(void)reader:(SCPReader * _Nonnull)reader didDisconnect:(SCPDisconnectReason)reason __attribute__((swift_name("reader(_:didDisconnect:)")));
        [Export("reader:didDisconnect:")]
        void Disconnect(SCPReader reader, SCPDisconnectReason reason);
    }

    // @interface SCPCart : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPCart
    {
        // @property (readonly, nonatomic, strong) NSArray<SCPCartLineItem *> * _Nonnull lineItems;
        [Export("lineItems", ArgumentSemantic.Strong)]
        SCPCartLineItem[] LineItems { get; }

        // @property (assign, readwrite, nonatomic) NSInteger tax;
        [Export("tax")]
        nint Tax { get; set; }

        // @property (assign, readwrite, nonatomic) NSInteger total;
        [Export("total")]
        nint Total { get; set; }

        // @property (copy, nonatomic) NSString * _Nonnull currency;
        [Export("currency")]
        string Currency { get; set; }
    }

    // @interface SCPCartBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPCart>))]
    [DisableDefaultCtor]
    interface SCPCartBuilder
    {
        // -(instancetype _Nonnull)initWithCurrency:(NSString * _Nonnull)currency __attribute__((objc_designated_initializer));
        [Export("initWithCurrency:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string currency);

        // -(SCPCartBuilder * _Nonnull)setCurrency:(NSString * _Nonnull)currency;
        [Export("setCurrency:")]
        SCPCartBuilder SetCurrency(string currency);

        // -(SCPCartBuilder * _Nonnull)setTax:(NSInteger)tax;
        [Export("setTax:")]
        SCPCartBuilder SetTax(nint tax);

        // -(SCPCartBuilder * _Nonnull)setTotal:(NSInteger)total;
        [Export("setTotal:")]
        SCPCartBuilder SetTotal(nint total);

        // -(SCPCartBuilder * _Nonnull)setLineItems:(NSArray<SCPCartLineItem *> * _Nonnull)lineItems;
        [Export("setLineItems:")]
        SCPCartBuilder SetLineItems(SCPCartLineItem[] lineItems);
    }

    // @interface SCPCartLineItem : NSObject
    [BaseType (typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPCartLineItem : INativeObject
    {
		// @property (assign, readwrite, nonatomic) NSInteger quantity;
		[Export ("quantity")]
		nint Quantity { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull displayName;
		[Export ("displayName")]
		string DisplayName { get; set; }

		// @property (assign, readwrite, nonatomic) NSInteger amount;
		[Export ("amount")]
		nint Amount { get; set; }
	}

    // @interface SCPCartLineItemBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPCartLineItem>))]
    [DisableDefaultCtor]
    interface SCPCartLineItemBuilder
    {
        // -(instancetype _Nonnull)initWithDisplayName:(NSString * _Nonnull)displayName __attribute__((objc_designated_initializer));
        [Export("initWithDisplayName:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string displayName);

        // -(SCPCartLineItemBuilder * _Nonnull)setDisplayName:(NSString * _Nonnull)displayName;
        [Export("setDisplayName:")]
        SCPCartLineItemBuilder SetDisplayName(string displayName);

        // -(SCPCartLineItemBuilder * _Nonnull)setQuantity:(NSInteger)quantity;
        [Export("setQuantity:")]
        SCPCartLineItemBuilder SetQuantity(nint quantity);

        // -(SCPCartLineItemBuilder * _Nonnull)setAmount:(NSInteger)amount;
        [Export("setAmount:")]
        SCPCartLineItemBuilder SetAmount(nint amount);
    }

    // @interface SCPCollectConfiguration : NSObject <NSCopying>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPCollectConfiguration : INSCopying
	{
		// @property (assign, nonatomic) BOOL skipTipping;
		[Export ("skipTipping")]
		bool SkipTipping { get; set; }

		// @property (nonatomic, strong) SCPTippingConfiguration * _Nullable tippingConfiguration;
		[NullAllowed, Export ("tippingConfiguration", ArgumentSemantic.Strong)]
		SCPTippingConfiguration TippingConfiguration { get; set; }

		// @property (assign, nonatomic) BOOL updatePaymentIntent;
		[Export ("updatePaymentIntent")]
		bool UpdatePaymentIntent { get; set; }

        // @property (readonly, assign, nonatomic) BOOL enableCustomerCancellation;
        [Export("enableCustomerCancellation")]
        bool EnableCustomerCancellation { get; }

        // @property (readonly, assign, nonatomic) BOOL requestDynamicCurrencyConversion;
        [Export("requestDynamicCurrencyConversion")]
        bool RequestDynamicCurrencyConversion { get; }
    }

    // @interface SCPCollectConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPCollectConfiguration>))]
    interface SCPCollectConfigurationBuilder
    {
        // -(SCPCollectConfigurationBuilder * _Nonnull)setSkipTipping:(BOOL)skipTipping;
        [Export("setSkipTipping:")]
        SCPCollectConfigurationBuilder SetSkipTipping(bool skipTipping);

        // -(SCPCollectConfigurationBuilder * _Nonnull)setTippingConfiguration:(SCPTippingConfiguration * _Nullable)tippingConfiguration;
        [Export("setTippingConfiguration:")]
        SCPCollectConfigurationBuilder SetTippingConfiguration([NullAllowed] SCPTippingConfiguration tippingConfiguration);

        // -(SCPCollectConfigurationBuilder * _Nonnull)setUpdatePaymentIntent:(BOOL)updatePaymentIntent;
        [Export("setUpdatePaymentIntent:")]
        SCPCollectConfigurationBuilder SetUpdatePaymentIntent(bool updatePaymentIntent);

        // -(SCPCollectConfigurationBuilder * _Nonnull)setEnableCustomerCancellation:(BOOL)enableCustomerCancellation;
        [Export("setEnableCustomerCancellation:")]
        SCPCollectConfigurationBuilder SetEnableCustomerCancellation(bool enableCustomerCancellation);

        // -(SCPCollectConfigurationBuilder * _Nonnull)setRequestDynamicCurrencyConversion:(BOOL)requestDynamicCurrencyConversion;
        [Export("setRequestDynamicCurrencyConversion:")]
        SCPCollectConfigurationBuilder SetRequestDynamicCurrencyConversion(bool requestDynamicCurrencyConversion);
    }

    // @interface SCPInput : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPInput
    {
        // @property (readonly, nonatomic) BOOL required;
        [Export("required")]
        bool Required { get; }
    }

    // @interface SCPCollectInputsParameters : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPCollectInputsParameters
    {
        // @property (readonly, copy, nonatomic) NSArray<SCPInput *> * _Nonnull inputs;
        [Export("inputs", ArgumentSemantic.Copy)]
        SCPInput[] Inputs { get; }
    }

    // @interface SCPCollectInputsParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPCollectInputsParameters>))]
    [DisableDefaultCtor]
    interface SCPCollectInputsParametersBuilder
    {
        // -(instancetype _Nonnull)initWithInputs:(NSArray<SCPInput *> * _Nonnull)inputs;
        [Export("initWithInputs:")]
        NativeHandle Constructor(SCPInput[] inputs);

        // -(SCPCollectInputsParametersBuilder * _Nonnull)setInputs:(NSArray<SCPInput *> * _Nonnull)inputs;
        [Export("setInputs:")]
        SCPCollectInputsParametersBuilder SetInputs(SCPInput[] inputs);
    }

    // @interface SCPCreateConfiguration : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPCreateConfiguration
    {
        // @property (readonly, nonatomic) SCPOfflineBehavior offlineBehavior;
        [Export("offlineBehavior")]
        SCPOfflineBehavior OfflineBehavior { get; }
    }

    // @interface SCPCreateConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPCreateConfiguration>))]
    interface SCPCreateConfigurationBuilder
    {
        // -(SCPCreateConfigurationBuilder * _Nonnull)setOfflineBehavior:(SCPOfflineBehavior)offlineBehavior;
        [Export("setOfflineBehavior:")]
        SCPCreateConfigurationBuilder SetOfflineBehavior(SCPOfflineBehavior offlineBehavior);
    }

    // @protocol SCPLocalMobileReaderDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPLocalMobileReaderDelegate
	{
		// @required -(void)localMobileReader:(SCPReader * _Nonnull)reader didStartInstallingUpdate:(SCPReaderSoftwareUpdate * _Nonnull)update cancelable:(SCPCancelable * _Nullable)cancelable __attribute__((swift_name("localMobileReader(_:didStartInstallingUpdate:cancelable:)")));
		[Abstract]
		[Export ("localMobileReader:didStartInstallingUpdate:cancelable:")]
		void LocalMobileReader (SCPReader reader, SCPReaderSoftwareUpdate update, [NullAllowed] SCPCancelable cancelable);

		// @required -(void)localMobileReader:(SCPReader * _Nonnull)reader didReportReaderSoftwareUpdateProgress:(float)progress __attribute__((swift_name("localMobileReader(_:didReportReaderSoftwareUpdateProgress:)")));
		[Abstract]
		[Export ("localMobileReader:didReportReaderSoftwareUpdateProgress:")]
		void LocalMobileReader (SCPReader reader, float progress);

		// @required -(void)localMobileReader:(SCPReader * _Nonnull)reader didFinishInstallingUpdate:(SCPReaderSoftwareUpdate * _Nullable)update error:(NSError * _Nullable)error __attribute__((swift_name("localMobileReader(_:didFinishInstallingUpdate:error:)")));
		[Abstract]
		[Export ("localMobileReader:didFinishInstallingUpdate:error:")]
		void LocalMobileReader (SCPReader reader, [NullAllowed] SCPReaderSoftwareUpdate update, [NullAllowed] NSError error);

		// @required -(void)localMobileReader:(SCPReader * _Nonnull)reader didRequestReaderInput:(SCPReaderInputOptions)inputOptions __attribute__((swift_name("localMobileReader(_:didRequestReaderInput:)")));
		[Abstract]
		[Export ("localMobileReader:didRequestReaderInput:")]
		void LocalMobileReader (SCPReader reader, SCPReaderInputOptions inputOptions);

		// @required -(void)localMobileReader:(SCPReader * _Nonnull)reader didRequestReaderDisplayMessage:(SCPReaderDisplayMessage)displayMessage __attribute__((swift_name("localMobileReader(_:didRequestReaderDisplayMessage:)")));
		[Abstract]
		[Export ("localMobileReader:didRequestReaderDisplayMessage:")]
		void LocalMobileReader (SCPReader reader, SCPReaderDisplayMessage displayMessage);

		// @optional -(void)localMobileReaderDidAcceptTermsOfService:(SCPReader * _Nonnull)reader __attribute__((swift_name("localMobileReaderDidAcceptTermsOfService(_:)")));
		[Export ("localMobileReaderDidAcceptTermsOfService:")]
		void LocalMobileReaderDidAcceptTermsOfService (SCPReader reader);
	}

    // @interface SCPOfflineStatusDetails : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPOfflineStatusDetails
    {
        // @property (readonly, nonatomic, strong) NSNumber * _Nullable paymentsCount;
        [NullAllowed, Export("paymentsCount", ArgumentSemantic.Strong)]
        NSNumber PaymentsCount { get; }

        // @property (readonly, nonatomic, strong) NSDictionary<NSString *,NSNumber *> * _Nonnull paymentAmountsByCurrency;
        [Export("paymentAmountsByCurrency", ArgumentSemantic.Strong)]
        NSDictionary<NSString, NSNumber> PaymentAmountsByCurrency { get; }

        // @property (readonly, nonatomic) SCPNetworkStatus networkStatus;
        [Export("networkStatus")]
        SCPNetworkStatus NetworkStatus { get; }
    }

    // @interface SCPOfflineStatus : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPOfflineStatus
    {
        // @property (readonly, nonatomic, strong) SCPOfflineStatusDetails * _Nullable reader;
        [NullAllowed, Export("reader", ArgumentSemantic.Strong)]
        SCPOfflineStatusDetails Reader { get; }

        // @property (readonly, nonatomic, strong) SCPOfflineStatusDetails * _Nonnull sdk;
        [Export("sdk", ArgumentSemantic.Strong)]
        SCPOfflineStatusDetails Sdk { get; }
    }

    // @protocol SCPOfflineDelegate
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SCPOfflineDelegate
    {
        // @required -(void)terminal:(SCPTerminal * _Nonnull)terminal didChangeOfflineStatus:(SCPOfflineStatus * _Nonnull)offlineStatus;
        [Abstract]
        [Export("terminal:didChangeOfflineStatus:")]
        void DidChangeOfflineStatus(SCPTerminal terminal, SCPOfflineStatus offlineStatus);

        // @required -(void)terminal:(SCPTerminal * _Nonnull)terminal didForwardPaymentIntent:(SCPPaymentIntent * _Nonnull)intent error:(NSError * _Nullable)error;
        [Abstract]
        [Export("terminal:didForwardPaymentIntent:error:")]
        void DidForwardPaymentIntent(SCPTerminal terminal, SCPPaymentIntent intent, [NullAllowed] NSError error);

        // @required -(void)terminal:(SCPTerminal * _Nonnull)terminal didReportForwardingError:(NSError * _Nonnull)error;
        [Abstract]
        [Export("terminal:didReportForwardingError:")]
        void DidReportForwardingError(SCPTerminal terminal, NSError error);
    }

    // @interface SCPCardPresentParameters : NSObject
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPCardPresentParameters
	{
		// @property (assign, nonatomic) BOOL requestExtendedAuthorization;
		[Export ("requestExtendedAuthorization")]
		bool RequestExtendedAuthorization { get; set; }

		// @property (assign, nonatomic) BOOL requestIncrementalAuthorizationSupport;
		[Export ("requestIncrementalAuthorizationSupport")]
		bool RequestIncrementalAuthorizationSupport { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable captureMethod;
		[NullAllowed, Export ("captureMethod", ArgumentSemantic.Strong)]
		NSNumber CaptureMethod { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable requestedPriority;
		[NullAllowed, Export ("requestedPriority", ArgumentSemantic.Strong)]
		NSNumber RequestedPriority { get; set; }
	}

    // @interface SCPCardPresentParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPCardPresentParameters>))]
    interface SCPCardPresentParametersBuilder
    {
        // -(SCPCardPresentParametersBuilder * _Nonnull)setRequestExtendedAuthorization:(BOOL)requestExtendedAuthorization;
        [Export("setRequestExtendedAuthorization:")]
        SCPCardPresentParametersBuilder SetRequestExtendedAuthorization(bool requestExtendedAuthorization);

        // -(SCPCardPresentParametersBuilder * _Nonnull)setRequestIncrementalAuthorizationSupport:(BOOL)requestIncrementalAuthorizationSupport;
        [Export("setRequestIncrementalAuthorizationSupport:")]
        SCPCardPresentParametersBuilder SetRequestIncrementalAuthorizationSupport(bool requestIncrementalAuthorizationSupport);

        // -(SCPCardPresentParametersBuilder * _Nonnull)setCaptureMethod:(SCPCardPresentCaptureMethod)captureMethod;
        [Export("setCaptureMethod:")]
        SCPCardPresentParametersBuilder SetCaptureMethod(SCPCardPresentCaptureMethod captureMethod);

        // -(SCPCardPresentParametersBuilder * _Nonnull)setRequestedPriority:(SCPCardPresentRouting)requestedPriority;
        [Export("setRequestedPriority:")]
        SCPCardPresentParametersBuilder SetRequestedPriority(SCPCardPresentRouting requestedPriority);
    }

    // @interface SCPPaymentMethodOptionsParameters : NSObject
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPPaymentMethodOptionsParameters
	{
		// @property (nonatomic, strong) SCPCardPresentParameters * _Nonnull cardPresentParameters;
		[Export ("cardPresentParameters", ArgumentSemantic.Strong)]
		SCPCardPresentParameters CardPresentParameters { get; }
    }

    // @interface SCPPaymentMethodOptionsParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPPaymentMethodOptionsParameters>))]
    [DisableDefaultCtor]
    interface SCPPaymentMethodOptionsParametersBuilder
    {
        // -(instancetype _Nonnull)initWithCardPresentParameters:(SCPCardPresentParameters * _Nonnull)cardPresentParameters;
        [Export("initWithCardPresentParameters:")]
        NativeHandle Constructor(SCPCardPresentParameters cardPresentParameters);

        // -(SCPPaymentMethodOptionsParametersBuilder * _Nonnull)setCardPresentParameters:(SCPCardPresentParameters * _Nonnull)cardPresentParameters;
        [Export("setCardPresentParameters:")]
        SCPPaymentMethodOptionsParametersBuilder SetCardPresentParameters(SCPCardPresentParameters cardPresentParameters);
    }

    // @interface SCPPaymentIntentParameters : NSObject
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPPaymentIntentParameters
	{
		// @property (readonly, nonatomic) NSUInteger amount;
		[Export ("amount")]
		nuint Amount { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull currency;
		[Export ("currency")]
		string Currency { get; }

		// @property (readonly, nonatomic) NSArray<NSString *> * _Nonnull paymentMethodTypes;
		[Export ("paymentMethodTypes")]
		string[] PaymentMethodTypes { get; }

		// @property (readonly, nonatomic) SCPCaptureMethod captureMethod;
		[Export ("captureMethod")]
		SCPCaptureMethod CaptureMethod { get; }

		// @property (readwrite, copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable metadata;
		[NullAllowed, Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> Metadata { get; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable stripeDescription;
		[NullAllowed, Export ("stripeDescription")]
		string StripeDescription { get; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable statementDescriptor;
		[NullAllowed, Export ("statementDescriptor")]
		string StatementDescriptor { get; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable statementDescriptorSuffix;
		[NullAllowed, Export ("statementDescriptorSuffix")]
		string StatementDescriptorSuffix { get; }

        // @property (readonly, nonatomic) NSString * _Nullable receiptEmail;
        [NullAllowed, Export("receiptEmail")]
        string ReceiptEmail { get; }

        // @property (readonly, nonatomic) NSString * _Nullable customer;
        [NullAllowed, Export("customer")]
        string Customer { get; }

        // @property (readonly, nonatomic) NSNumber * _Nullable applicationFeeAmount;
        [NullAllowed, Export("applicationFeeAmount")]
        NSNumber ApplicationFeeAmount { get; }

        // @property (readonly, nonatomic) NSString * _Nullable transferGroup;
        [NullAllowed, Export("transferGroup")]
        string TransferGroup { get; }

        // @property (readonly, nonatomic) NSString * _Nullable transferDataDestination;
        [NullAllowed, Export("transferDataDestination")]
        string TransferDataDestination { get; }

        // @property (readonly, nonatomic) NSString * _Nullable onBehalfOf;
        [NullAllowed, Export("onBehalfOf")]
        string OnBehalfOf { get; }

        // @property (readonly, nonatomic) NSString * _Nullable setupFutureUsage;
        [NullAllowed, Export("setupFutureUsage")]
        string SetupFutureUsage { get; }

        // @property (readonly, nonatomic) SCPPaymentMethodOptionsParameters * _Nonnull paymentMethodOptionsParameters;
        [Export("paymentMethodOptionsParameters")]
        SCPPaymentMethodOptionsParameters PaymentMethodOptionsParameters { get; }

        // @property (readonly, copy, nonatomic) DEPRECATED_MSG_ATTRIBUTE("Did you mean to use stripeDescription?") NSString * description __attribute__((deprecated("Did you mean to use stripeDescription?")));
        [Obsolete("Did you mean to use stripeDescription?")]
        [Export ("description")]
		string Description { get; }
	}

    // @interface SCPPaymentIntentParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPPaymentIntentParameters>))]
    [DisableDefaultCtor]
    interface SCPPaymentIntentParametersBuilder
    {
        // -(instancetype _Nonnull)initWithAmount:(NSUInteger)amount currency:(NSString * _Nonnull)currency;
        [Export("initWithAmount:currency:")]
        NativeHandle Constructor(nuint amount, string currency);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setAmount:(NSUInteger)amount;
        [Export("setAmount:")]
        SCPPaymentIntentParametersBuilder SetAmount(nuint amount);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setCurrency:(NSString * _Nonnull)currency;
        [Export("setCurrency:")]
        SCPPaymentIntentParametersBuilder SetCurrency(string currency);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setPaymentMethodTypes:(NSArray<NSString *> * _Nonnull)paymentMethodTypes;
        [Export("setPaymentMethodTypes:")]
        SCPPaymentIntentParametersBuilder SetPaymentMethodTypes(string[] paymentMethodTypes);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setCaptureMethod:(SCPCaptureMethod)captureMethod;
        [Export("setCaptureMethod:")]
        SCPPaymentIntentParametersBuilder SetCaptureMethod(SCPCaptureMethod captureMethod);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setMetadata:(NSDictionary<NSString *,NSString *> * _Nullable)metadata;
        [Export("setMetadata:")]
        SCPPaymentIntentParametersBuilder SetMetadata([NullAllowed] NSDictionary<NSString, NSString> metadata);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setStripeDescription:(NSString * _Nullable)stripeDescription;
        [Export("setStripeDescription:")]
        SCPPaymentIntentParametersBuilder SetStripeDescription([NullAllowed] string stripeDescription);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setStatementDescriptor:(NSString * _Nullable)statementDescriptor;
        [Export("setStatementDescriptor:")]
        SCPPaymentIntentParametersBuilder SetStatementDescriptor([NullAllowed] string statementDescriptor);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setStatementDescriptorSuffix:(NSString * _Nullable)statementDescriptorSuffix;
        [Export("setStatementDescriptorSuffix:")]
        SCPPaymentIntentParametersBuilder SetStatementDescriptorSuffix([NullAllowed] string statementDescriptorSuffix);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setReceiptEmail:(NSString * _Nullable)receiptEmail;
        [Export("setReceiptEmail:")]
        SCPPaymentIntentParametersBuilder SetReceiptEmail([NullAllowed] string receiptEmail);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setCustomer:(NSString * _Nullable)customer;
        [Export("setCustomer:")]
        SCPPaymentIntentParametersBuilder SetCustomer([NullAllowed] string customer);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setApplicationFeeAmount:(NSNumber * _Nullable)applicationFeeAmount;
        [Export("setApplicationFeeAmount:")]
        SCPPaymentIntentParametersBuilder SetApplicationFeeAmount([NullAllowed] NSNumber applicationFeeAmount);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setTransferGroup:(NSString * _Nullable)transferGroup;
        [Export("setTransferGroup:")]
        SCPPaymentIntentParametersBuilder SetTransferGroup([NullAllowed] string transferGroup);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setTransferDataDestination:(NSString * _Nullable)transferDataDestination;
        [Export("setTransferDataDestination:")]
        SCPPaymentIntentParametersBuilder SetTransferDataDestination([NullAllowed] string transferDataDestination);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setOnBehalfOf:(NSString * _Nullable)onBehalfOf;
        [Export("setOnBehalfOf:")]
        SCPPaymentIntentParametersBuilder SetOnBehalfOf([NullAllowed] string onBehalfOf);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setSetupFutureUsage:(NSString * _Nullable)setupFutureUsage;
        [Export("setSetupFutureUsage:")]
        SCPPaymentIntentParametersBuilder SetSetupFutureUsage([NullAllowed] string setupFutureUsage);

        // -(SCPPaymentIntentParametersBuilder * _Nonnull)setPaymentMethodOptionsParameters:(SCPPaymentMethodOptionsParameters * _Nonnull)paymentMethodOptionsParameters;
        [Export("setPaymentMethodOptionsParameters:")]
        SCPPaymentIntentParametersBuilder SetPaymentMethodOptionsParameters(SCPPaymentMethodOptionsParameters paymentMethodOptionsParameters);
    }

    // @protocol SCPReaderSettingsParameters <NSObject>
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface SCPReaderSettingsParameters
    {
    }

    // @interface SCPRefundConfiguration : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPRefundConfiguration : INSCopying
    {
        // @property (readonly, assign, nonatomic) BOOL enableCustomerCancellation;
        [Export("enableCustomerCancellation")]
        bool EnableCustomerCancellation { get; }
    }

    // @interface SCPRefundConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPRefundConfiguration>))]
    interface SCPRefundConfigurationBuilder
    {
        // -(SCPRefundConfigurationBuilder * _Nonnull)setEnableCustomerCancellation:(BOOL)enableCustomerCancellation;
        [Export("setEnableCustomerCancellation:")]
        SCPRefundConfigurationBuilder SetEnableCustomerCancellation(bool enableCustomerCancellation);
    }

    // @interface SCPPaymentIntent : NSObject <SCPJSONDecodable, NSCopying>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPPaymentIntent : ISCPJSONDecodable, INSCopying
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, nonatomic) NSDate * _Nonnull created;
		[Export ("created")]
		NSDate Created { get; }

		// @property (readonly, nonatomic) SCPPaymentIntentStatus status;
		[Export ("status")]
		SCPPaymentIntentStatus Status { get; }

		// @property (readonly, nonatomic) NSUInteger amount;
		[Export ("amount")]
		nuint Amount { get; }

		// @property (readonly, nonatomic) SCPCaptureMethod captureMethod;
		[Export ("captureMethod")]
		SCPCaptureMethod CaptureMethod { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull currency;
		[Export ("currency")]
		string Currency { get; }

		// @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable metadata;
		[NullAllowed, Export ("metadata")]
		NSDictionary<NSString, NSString> Metadata { get; }

		// @property (readonly, nonatomic) NSArray<SCPCharge *> * _Nonnull charges;
		[Export ("charges")]
		SCPCharge[] Charges { get; }

		// @property (readonly, nonatomic) SCPPaymentMethod * _Nullable paymentMethod;
		[NullAllowed, Export ("paymentMethod")]
		SCPPaymentMethod PaymentMethod { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable paymentMethodId;
        [NullAllowed, Export("paymentMethodId")]
        string PaymentMethodId { get; }

        // @property (readonly, nonatomic) SCPAmountDetails * _Nullable amountDetails;
        [NullAllowed, Export ("amountDetails")]
		SCPAmountDetails AmountDetails { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable amountTip;
		[NullAllowed, Export ("amountTip")]
		NSNumber AmountTip { get; }

		// @property (readonly, nonatomic) NSString * _Nullable statementDescriptor;
		[NullAllowed, Export ("statementDescriptor")]
		string StatementDescriptor { get; }

		// @property (readonly, nonatomic) NSString * _Nullable statementDescriptorSuffix;
		[NullAllowed, Export ("statementDescriptorSuffix")]
		string StatementDescriptorSuffix { get; }

        // @property (readonly, nonatomic) SCPOfflineDetails * _Nullable offlineDetails;
        [NullAllowed, Export("offlineDetails")]
        SCPOfflineDetails OfflineDetails { get; }
    }

	// @interface SCPRefundParameters : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPRefundParameters
	{
        // @property (readonly, nonatomic) NSString * _Nullable paymentIntentId;
        [NullAllowed, Export("paymentIntentId")]
        string PaymentIntentId { get; }

        // @property (readonly, nonatomic) NSString * _Nullable chargeId;
        [NullAllowed, Export("chargeId")]
        string ChargeId { get; }

        // @property (readonly, nonatomic) NSUInteger amount;
        [Export("amount")]
        nuint Amount { get; }

        // @property (readonly, nonatomic) NSString * _Nonnull currency;
        [Export("currency")]
        string Currency { get; }

        // @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable metadata;
        [NullAllowed, Export("metadata")]
        NSDictionary<NSString, NSString> Metadata { get; }

        // @property (readonly, nonatomic) NSNumber * _Nullable reverseTransfer;
        [NullAllowed, Export("reverseTransfer")]
        NSNumber ReverseTransfer { get; }

        // @property (readonly, nonatomic) NSNumber * _Nullable refundApplicationFee;
        [NullAllowed, Export("refundApplicationFee")]
        NSNumber RefundApplicationFee { get; }
    }

    // @interface SCPRefundParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPRefundParameters>))]
    [DisableDefaultCtor]
    interface SCPRefundParametersBuilder
    {
		//Duplicate Signature

        //// -(instancetype _Nonnull)initWithPaymentIntentId:(NSString * _Nonnull)paymentIntentId amount:(NSUInteger)amount currency:(NSString * _Nonnull)currency;
        //[Export("initWithPaymentIntentId:amount:currency:")]
        //NativeHandle Constructor(string paymentIntentId, nuint amount, string currency);

        //// -(instancetype _Nonnull)initWithChargeId:(NSString * _Nonnull)chargeId amount:(NSUInteger)amount currency:(NSString * _Nonnull)currency;
        //[Export("initWithChargeId:amount:currency:")]
        //NativeHandle Constructor(string chargeId, nuint amount, string currency);

        // -(SCPRefundParametersBuilder * _Nonnull)setChargeId:(NSString * _Nonnull)chargeId;
        [Export("setChargeId:")]
        SCPRefundParametersBuilder SetChargeId(string chargeId);

        // -(SCPRefundParametersBuilder * _Nonnull)setPaymentIntentId:(NSString * _Nonnull)paymentIntentId;
        [Export("setPaymentIntentId:")]
        SCPRefundParametersBuilder SetPaymentIntentId(string paymentIntentId);

        // -(SCPRefundParametersBuilder * _Nonnull)setAmount:(NSUInteger)amount;
        [Export("setAmount:")]
        SCPRefundParametersBuilder SetAmount(nuint amount);

        // -(SCPRefundParametersBuilder * _Nonnull)setCurrency:(NSString * _Nonnull)currency;
        [Export("setCurrency:")]
        SCPRefundParametersBuilder SetCurrency(string currency);

        // -(SCPRefundParametersBuilder * _Nonnull)setMetadata:(NSDictionary<NSString *,NSString *> * _Nullable)metadata;
        [Export("setMetadata:")]
        SCPRefundParametersBuilder SetMetadata([NullAllowed] NSDictionary<NSString, NSString> metadata);

        // -(SCPRefundParametersBuilder * _Nonnull)setReverseTransfer:(BOOL)reverseTransfer;
        [Export("setReverseTransfer:")]
        SCPRefundParametersBuilder SetReverseTransfer(bool reverseTransfer);

        // -(SCPRefundParametersBuilder * _Nonnull)setRefundApplicationFee:(BOOL)refundApplicationFee;
        [Export("setRefundApplicationFee:")]
        SCPRefundParametersBuilder SetRefundApplicationFee(bool refundApplicationFee);
    }

    // @interface SCPSetupIntentConfiguration : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPSetupIntentConfiguration : INSCopying
    {
        // @property (readonly, assign, nonatomic) BOOL enableCustomerCancellation;
        [Export("enableCustomerCancellation")]
        bool EnableCustomerCancellation { get; }
    }

    // @interface SCPSetupIntentConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPSetupIntentConfiguration>))]
    interface SCPSetupIntentConfigurationBuilder
    {
        // -(SCPSetupIntentConfigurationBuilder * _Nonnull)setEnableCustomerCancellation:(BOOL)enableCustomerCancellation;
        [Export("setEnableCustomerCancellation:")]
        SCPSetupIntentConfigurationBuilder SetEnableCustomerCancellation(bool enableCustomerCancellation);
    }

    // @interface SCPSimulatedCard : NSObject
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPSimulatedCard
	{
		// -(instancetype _Nonnull)initWithType:(SCPSimulatedCardType)type;
		[Export ("initWithType:")]
		NativeHandle Constructor (SCPSimulatedCardType type);

		// -(instancetype _Nonnull)initWithTestCardNumber:(NSString * _Nonnull)testCardNumber;
		[Export ("initWithTestCardNumber:")]
		NativeHandle Constructor (string testCardNumber);

		// -(BOOL)isOnlinePin;
		[Export ("isOnlinePin")]
		bool IsOnlinePin { get; }

		// -(BOOL)isOfflinePin;
		[Export ("isOfflinePin")]
		bool IsOfflinePin { get; }
	}

	// @interface SCPSimulatorConfiguration : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPSimulatorConfiguration
	{
		// @property (assign, nonatomic) SCPSimulateReaderUpdate availableReaderUpdate;
		[Export ("availableReaderUpdate", ArgumentSemantic.Assign)]
		SCPSimulateReaderUpdate AvailableReaderUpdate { get; set; }

		// @property (readwrite, nonatomic) SCPSimulatedCard * _Nonnull simulatedCard;
		[Export ("simulatedCard", ArgumentSemantic.Assign)]
		SCPSimulatedCard SimulatedCard { get; set; }

		// @property (readwrite, nonatomic) NSNumber * _Nullable simulatedTipAmount;
		[NullAllowed, Export ("simulatedTipAmount", ArgumentSemantic.Assign)]
		NSNumber SimulatedTipAmount { get; set; }
	}

	// @interface SCPTerminal : NSObject
	//[iOS (11,0)]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPTerminal
	{
		// +(void)setTokenProvider:(id<SCPConnectionTokenProvider> _Nonnull)tokenProvider __attribute__((swift_name("setTokenProvider(_:)")));
		[Static]
		[Export ("setTokenProvider:")]
		void SetTokenProvider (SCPConnectionTokenProvider tokenProvider);

		// +(BOOL)hasTokenProvider;
		[Static]
		[Export ("hasTokenProvider")]
		bool HasTokenProvider { get; }

		// +(void)setLogListener:(SCPLogListenerBlock _Nonnull)listener;
		[Static]
		[Export ("setLogListener:")]
		void SetLogListener (SCPLogListenerBlock listener);

		// @property (readonly, nonatomic, class) SCPTerminal * _Nonnull shared;
		[Static]
		[Export ("shared")]
		SCPTerminal Shared { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		SCPTerminalDelegate Delegate { get; set; }

		// @property (readwrite, nonatomic) id<SCPTerminalDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		// @property (readonly, nonatomic) SCPReader * _Nullable connectedReader;
		[NullAllowed, Export ("connectedReader")]
		SCPReader ConnectedReader { get; }

		// @property (readonly, nonatomic) SCPConnectionStatus connectionStatus;
		[Export ("connectionStatus")]
		SCPConnectionStatus ConnectionStatus { get; }

		// @property (assign, readwrite, nonatomic) SCPLogLevel logLevel;
		[Export ("logLevel", ArgumentSemantic.Assign)]
		SCPLogLevel LogLevel { get; set; }

		// @property (readonly, nonatomic) SCPSimulatorConfiguration * _Nonnull simulatorConfiguration;
		[Export ("simulatorConfiguration")]
		SCPSimulatorConfiguration SimulatorConfiguration { get; }

		// @property (readonly, nonatomic) SCPPaymentStatus paymentStatus;
		[Export ("paymentStatus")]
		SCPPaymentStatus PaymentStatus { get; }

		// -(void)clearCachedCredentials __attribute__((swift_name("clearCachedCredentials()")));
		[Export ("clearCachedCredentials")]
		void ClearCachedCredentials ();

		// -(BOOL)supportsReadersOfType:(SCPDeviceType)deviceType discoveryMethod:(SCPDiscoveryMethod)discoveryMethod simulated:(BOOL)simulated error:(NSError * _Nullable * _Nullable)error __attribute__((swift_private));
		[Export ("supportsReadersOfType:discoveryMethod:simulated:error:")]
		bool SupportsReadersOfType (SCPDeviceType deviceType, SCPDiscoveryMethod discoveryMethod, bool simulated, [NullAllowed] out NSError error);

		// -(SCPCancelable * _Nullable)discoverReaders:(SCPDiscoveryConfiguration * _Nonnull)configuration delegate:(id<SCPDiscoveryDelegate> _Nonnull)delegate completion:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("discoverReaders(_:delegate:completion:)")));
		[Export ("discoverReaders:delegate:completion:")]
		[return: NullAllowed]
		[Async(ResultTypeName = "DiscoverResult")]
		SCPCancelable DiscoverReaders (SCPDiscoveryConfiguration configuration, SCPDiscoveryDelegate @delegate, SCPErrorCompletionBlock completion);

		// -(void)connectBluetoothReader:(SCPReader * _Nonnull)reader delegate:(id<SCPBluetoothReaderDelegate> _Nonnull)delegate connectionConfig:(SCPBluetoothConnectionConfiguration * _Nonnull)connectionConfig completion:(SCPReaderCompletionBlock _Nonnull)completion __attribute__((swift_name("connectBluetoothReader(_:delegate:connectionConfig:completion:)")));
		[Export ("connectBluetoothReader:delegate:connectionConfig:completion:")]
        [Async(ResultTypeName = "ReaderResult")]
        void ConnectBluetoothReader (SCPReader reader, SCPBluetoothReaderDelegate @delegate, SCPBluetoothConnectionConfiguration connectionConfig, SCPReaderCompletionBlock completion);

		// -(void)connectInternetReader:(SCPReader * _Nonnull)reader connectionConfig:(SCPInternetConnectionConfiguration * _Nullable)connectionConfig completion:(SCPReaderCompletionBlock _Nonnull)completion __attribute__((swift_name("connectInternetReader(_:connectionConfig:completion:)")));
		[Export ("connectInternetReader:connectionConfig:completion:")]
        [Async(ResultTypeName = "ReaderResult")]
        void ConnectInternetReader (SCPReader reader, [NullAllowed] SCPInternetConnectionConfiguration connectionConfig, SCPReaderCompletionBlock completion);

		// -(void)connectLocalMobileReader:(SCPReader * _Nonnull)reader delegate:(id<SCPLocalMobileReaderDelegate> _Nonnull)delegate connectionConfig:(SCPLocalMobileConnectionConfiguration * _Nonnull)connectionConfig completion:(SCPReaderCompletionBlock _Nonnull)completion __attribute__((swift_name("connectLocalMobileReader(_:delegate:connectionConfig:completion:)")));
		[Export ("connectLocalMobileReader:delegate:connectionConfig:completion:")]
        [Async(ResultTypeName = "ReaderResult")]
        void ConnectLocalMobileReader (SCPReader reader, SCPLocalMobileReaderDelegate @delegate, SCPLocalMobileConnectionConfiguration connectionConfig, SCPReaderCompletionBlock completion);

		// -(void)listLocations:(SCPListLocationsParameters * _Nullable)parameters completion:(SCPLocationsCompletionBlock _Nonnull)completion __attribute__((swift_name("listLocations(parameters:completion:)")));
		[Export ("listLocations:completion:")]
        [Async(ResultTypeName="LocationResult")]
        void ListLocations ([NullAllowed] SCPListLocationsParameters parameters, SCPLocationsCompletionBlock completion);

		// -(void)installAvailableUpdate;
		[Export ("installAvailableUpdate")]
		void InstallAvailableUpdate ();

        // -(void)rebootReader:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("rebootReader(_:)")));
        [Export("rebootReader:")]
		[Async]
        void RebootReader(SCPErrorCompletionBlock completion);

        // -(void)disconnectReader:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("disconnectReader(_:)")));
        [Export ("disconnectReader:")]
        [Async]
        void DisconnectReader (SCPErrorCompletionBlock completion);

		// -(void)createPaymentIntent:(SCPPaymentIntentParameters * _Nonnull)parameters completion:(SCPPaymentIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("createPaymentIntent(_:completion:)")));
		[Export ("createPaymentIntent:completion:")]
        [Async(ResultTypeName = "PaymentResult")]
        void CreatePaymentIntent (SCPPaymentIntentParameters parameters, SCPPaymentIntentCompletionBlock completion);

		// -(void)retrievePaymentIntent:(NSString * _Nonnull)clientSecret completion:(SCPPaymentIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("retrievePaymentIntent(clientSecret:completion:)")));
		[Export ("retrievePaymentIntent:completion:")]
        [Async(ResultTypeName = "PaymentResult")]
        void RetrievePaymentIntent (string clientSecret, SCPPaymentIntentCompletionBlock completion);

		// -(SCPCancelable * _Nullable)collectPaymentMethod:(SCPPaymentIntent * _Nonnull)paymentIntent completion:(SCPPaymentIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("collectPaymentMethod(_:completion:)")));
		[Export ("collectPaymentMethod:completion:")]
		[return: NullAllowed]
        [Async(ResultTypeName = "PaymentResult")]
        SCPCancelable CollectPaymentMethod (SCPPaymentIntent paymentIntent, SCPPaymentIntentCompletionBlock completion);

		// -(SCPCancelable * _Nullable)collectPaymentMethod:(SCPPaymentIntent * _Nonnull)paymentIntent collectConfig:(SCPCollectConfiguration * _Nullable)collectConfig completion:(SCPPaymentIntentCompletionBlock _Nonnull)completion;
		[Export ("collectPaymentMethod:collectConfig:completion:")]
		[return: NullAllowed]
        [Async(ResultTypeName = "PaymentResult")]
        SCPCancelable CollectPaymentMethod (SCPPaymentIntent paymentIntent, [NullAllowed] SCPCollectConfiguration collectConfig, SCPPaymentIntentCompletionBlock completion);

        // -(void)confirmPaymentIntent:(SCPPaymentIntent * _Nonnull)paymentIntent completion:(SCPConfirmPaymentIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("confirmPaymentIntent(_:completion:)")));
        [Export("confirmPaymentIntent:completion:")]
        [Async(ResultTypeName = "PaymentProcessResult")]
        void ConfirmPaymentIntent(SCPPaymentIntent paymentIntent, SCPConfirmPaymentIntentCompletionBlock completion);

        // -(void)cancelPaymentIntent:(SCPPaymentIntent * _Nonnull)paymentIntent completion:(SCPPaymentIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("cancelPaymentIntent(_:completion:)")));
        [Export ("cancelPaymentIntent:completion:")]
        [Async(ResultTypeName = "PaymentResult")]
        void CancelPaymentIntent (SCPPaymentIntent paymentIntent, SCPPaymentIntentCompletionBlock completion);

		// -(void)createSetupIntent:(SCPSetupIntentParameters * _Nonnull)setupIntentParams completion:(SCPSetupIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("createSetupIntent(_:completion:)")));
		[Export ("createSetupIntent:completion:")]
        [Async(ResultTypeName = "SetupIntentResult")]
        void CreateSetupIntent (SCPSetupIntentParameters setupIntentParams, SCPSetupIntentCompletionBlock completion);

		// -(void)retrieveSetupIntent:(NSString * _Nonnull)clientSecret completion:(SCPSetupIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("retrieveSetupIntent(clientSecret:completion:)")));
		[Export ("retrieveSetupIntent:completion:")]
        [Async(ResultTypeName = "SetupIntentResult")]
        void RetrieveSetupIntent (string clientSecret, SCPSetupIntentCompletionBlock completion);

		// -(void)cancelSetupIntent:(SCPSetupIntent * _Nonnull)intent completion:(SCPSetupIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("cancelSetupIntent(_:completion:)")));
		[Export ("cancelSetupIntent:completion:")]
        [Async(ResultTypeName = "SetupIntentResult")]
        void CancelSetupIntent (SCPSetupIntent intent, SCPSetupIntentCompletionBlock completion);

		// -(SCPCancelable * _Nullable)collectSetupIntentPaymentMethod:(SCPSetupIntent * _Nonnull)setupIntent customerConsentCollected:(BOOL)customerConsentCollected completion:(SCPSetupIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("collectSetupIntentPaymentMethod(_:customerConsentCollected:completion:)")));
		[Export ("collectSetupIntentPaymentMethod:customerConsentCollected:completion:")]
		[return: NullAllowed]
        [Async(ResultTypeName = "SetupIntentResult")]
        SCPCancelable CollectSetupIntentPaymentMethod (SCPSetupIntent setupIntent, bool customerConsentCollected, SCPSetupIntentCompletionBlock completion);

        // -(SCPCancelable * _Nullable)collectSetupIntentPaymentMethod:(SCPSetupIntent * _Nonnull)setupIntent customerConsentCollected:(BOOL)customerConsentCollected setupConfig:(SCPSetupIntentConfiguration * _Nullable)setupConfig completion:(SCPSetupIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("collectSetupIntentPaymentMethod(_:customerConsentCollected:setupConfig:completion:)")));
        [Export("collectSetupIntentPaymentMethod:customerConsentCollected:setupConfig:completion:")]
        [return: NullAllowed]
        [Async(ResultTypeName = "SetupIntentResult")]
        SCPCancelable CollectSetupIntentPaymentMethod(SCPSetupIntent setupIntent, bool customerConsentCollected, [NullAllowed] SCPSetupIntentConfiguration setupConfig, SCPSetupIntentCompletionBlock completion);

        // -(void)confirmSetupIntent:(SCPSetupIntent * _Nonnull)setupIntent completion:(SCPConfirmSetupIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("confirmSetupIntent(_:completion:)")));
        [Export ("confirmSetupIntent:completion:")]
        [Async (ResultTypeName="SetupConfirmIntentResult")]
        void ConfirmSetupIntent (SCPSetupIntent setupIntent, SCPConfirmSetupIntentCompletionBlock completion);

		// -(SCPCancelable * _Nullable)collectRefundPaymentMethod:(SCPRefundParameters * _Nonnull)refundParams completion:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("collectRefundPaymentMethod(_:completion:)")));
		[Export ("collectRefundPaymentMethod:completion:")]
		[return: NullAllowed]
        [Async]
        SCPCancelable CollectRefundPaymentMethod (SCPRefundParameters refundParams, SCPErrorCompletionBlock completion);

        // -(SCPCancelable * _Nullable)collectRefundPaymentMethod:(SCPRefundParameters * _Nonnull)refundParams refundConfig:(SCPRefundConfiguration * _Nullable)refundConfig completion:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("collectRefundPaymentMethod(_:refundConfig:completion:)")));
        [Export("collectRefundPaymentMethod:refundConfig:completion:")]
        [return: NullAllowed]
		[Async]
        SCPCancelable CollectRefundPaymentMethod(SCPRefundParameters refundParams, [NullAllowed] SCPRefundConfiguration refundConfig, SCPErrorCompletionBlock completion);

        // -(void)confirmRefund:(SCPConfirmRefundCompletionBlock _Nonnull)completion __attribute__((swift_name("confirmRefund(completion:)")));
        [Export("confirmRefund:")]
        [Async(ResultTypeName = "RefundResult")]
        void ConfirmRefund(SCPConfirmRefundCompletionBlock completion);

        [Wrap("WeakOfflineDelegate")]
        [NullAllowed]
        SCPOfflineDelegate OfflineDelegate { get; set; }

        // @property (readwrite, nonatomic) id<SCPOfflineDelegate> _Nullable offlineDelegate;
        [NullAllowed, Export("offlineDelegate", ArgumentSemantic.Assign)]
        NSObject WeakOfflineDelegate { get; set; }

        // @property (readonly, nonatomic) SCPOfflineStatus * _Nonnull offlineStatus;
        [Export("offlineStatus")]
        SCPOfflineStatus OfflineStatus { get; }

        // -(void)createPaymentIntent:(SCPPaymentIntentParameters * _Nonnull)parameters createConfig:(SCPCreateConfiguration * _Nullable)createConfig completion:(SCPPaymentIntentCompletionBlock _Nonnull)completion __attribute__((swift_name("createPaymentIntent(_:createConfig:completion:)")));
        [Export("createPaymentIntent:createConfig:completion:")]
        [Async(ResultTypeName = "PaymentResult")]
        void CreatePaymentIntent(SCPPaymentIntentParameters parameters, [NullAllowed] SCPCreateConfiguration createConfig, SCPPaymentIntentCompletionBlock completion);

        // -(void)clearReaderDisplay:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("clearReaderDisplay(_:)")));
        [Export ("clearReaderDisplay:")]
        [Async]
        void ClearReaderDisplay (SCPErrorCompletionBlock completion);

		// -(void)setReaderDisplay:(SCPCart * _Nonnull)cart completion:(SCPErrorCompletionBlock _Nonnull)completion __attribute__((swift_name("setReaderDisplay(_:completion:)")));
		[Export ("setReaderDisplay:completion:")]
        [Async]
        void SetReaderDisplay (SCPCart cart, SCPErrorCompletionBlock completion);

        // -(void)setReaderSettings:(id<SCPReaderSettingsParameters> _Nonnull)params completion:(SCPReaderSettingsCompletionBlock _Nonnull)completion __attribute__((swift_name("setReaderSettings(_:completion:)")));
        [Export("setReaderSettings:completion:")]
		[Async(ResultTypeName = "ReaderSettingsResult")]
        void SetReaderSettings(SCPReaderSettingsParameters parameters, SCPReaderSettingsCompletionBlock completion);

        // -(void)retrieveReaderSettings:(SCPReaderSettingsCompletionBlock _Nonnull)completion __attribute__((swift_name("retrieveReaderSettings(_:)")));
        [Export("retrieveReaderSettings:")]
        [Async(ResultTypeName = "ReaderSettingsResult")]
        void RetrieveReaderSettings(SCPReaderSettingsCompletionBlock completion);

        // -(SCPCancelable * _Nullable)collectInputs:(SCPCollectInputsParameters * _Nonnull)collectInputsParams completion:(SCPCollectInputsCompletionBlock _Nonnull)completion __attribute__((swift_name("collectInputs(_:completion:)")));
        [Export("collectInputs:completion:")]
        [return: NullAllowed]
        [Async(ResultTypeName = "CollectInputsResult")]
        SCPCancelable CollectInputs(SCPCollectInputsParameters collectInputsParams, SCPCollectInputsCompletionBlock completion);

        // +(NSString * _Nonnull)stringFromReaderInputOptions:(SCPReaderInputOptions)options __attribute__((swift_name("stringFromReaderInputOptions(_:)")));
        [Static]
		[Export ("stringFromReaderInputOptions:")]
		string StringFromReaderInputOptions (SCPReaderInputOptions options);

		// +(NSString * _Nonnull)stringFromReaderDisplayMessage:(SCPReaderDisplayMessage)message __attribute__((swift_name("stringFromReaderDisplayMessage(_:)")));
		[Static]
		[Export ("stringFromReaderDisplayMessage:")]
		string StringFromReaderDisplayMessage (SCPReaderDisplayMessage message);

		// +(NSString * _Nonnull)stringFromReaderEvent:(SCPReaderEvent)event __attribute__((swift_name("stringFromReaderEvent(_:)")));
		[Static]
		[Export ("stringFromReaderEvent:")]
		string StringFromReaderEvent (SCPReaderEvent @event);

		// +(NSString * _Nonnull)stringFromConnectionStatus:(SCPConnectionStatus)status __attribute__((swift_name("stringFromConnectionStatus(_:)")));
		[Static]
		[Export ("stringFromConnectionStatus:")]
		string StringFromConnectionStatus (SCPConnectionStatus status);

		// +(NSString * _Nonnull)stringFromPaymentStatus:(SCPPaymentStatus)status __attribute__((swift_name("stringFromPaymentStatus(_:)")));
		[Static]
		[Export ("stringFromPaymentStatus:")]
		string StringFromPaymentStatus (SCPPaymentStatus status);

		// +(NSString * _Nonnull)stringFromDeviceType:(SCPDeviceType)deviceType __attribute__((swift_name("stringFromDeviceType(_:)")));
		[Static]
		[Export ("stringFromDeviceType:")]
		string StringFromDeviceType (SCPDeviceType deviceType);

		// +(NSString * _Nonnull)stringFromDiscoveryMethod:(SCPDiscoveryMethod)method __attribute__((swift_name("stringFromDiscoveryMethod(_:)")));
		[Static]
		[Export ("stringFromDiscoveryMethod:")]
		string StringFromDiscoveryMethod (SCPDiscoveryMethod method);

		// +(NSString * _Nonnull)stringFromCardBrand:(SCPCardBrand)cardBrand __attribute__((swift_name("stringFromCardBrand(_:)")));
		[Static]
		[Export ("stringFromCardBrand:")]
		string StringFromCardBrand (SCPCardBrand cardBrand);

		// +(NSString * _Nonnull)stringFromPaymentIntentStatus:(SCPPaymentIntentStatus)paymentIntentStatus __attribute__((swift_name("stringFromPaymentIntentStatus(_:)")));
		[Static]
		[Export ("stringFromPaymentIntentStatus:")]
		string StringFromPaymentIntentStatus (SCPPaymentIntentStatus paymentIntentStatus);

		// +(NSString * _Nonnull)stringFromCaptureMethod:(SCPCaptureMethod)captureMethod __attribute__((swift_name("stringFromCaptureMethod(_:)")));
		[Static]
		[Export ("stringFromCaptureMethod:")]
		string StringFromCaptureMethod (SCPCaptureMethod captureMethod);

        // +(NSString * _Nonnull)stringFromReadMethod:(SCPReadMethod)method __attribute__((swift_name("stringFromReadMethod(_:)")));
        [Static]
        [Export("stringFromReadMethod:")]
        string StringFromReadMethod(SCPReadMethod method);

        // +(NSString * _Nonnull)stringFromNetworkStatus:(SCPNetworkStatus)networkStatus __attribute__((swift_name("stringFromNetworkStatus(_:)")));
        [Static]
        [Export("stringFromNetworkStatus:")]
        string StringFromNetworkStatus(SCPNetworkStatus networkStatus);

        // +(NSString * _Nonnull)stringFromDisconnectReason:(SCPDisconnectReason)reason __attribute__((swift_name("stringFromDisconnectReason(_:)")));
        [Static]
        [Export("stringFromDisconnectReason:")]
        string StringFromDisconnectReason(SCPDisconnectReason reason);

        // +(NSString * _Nonnull)stringFromOfflineBehavior:(SCPOfflineBehavior)behavior __attribute__((swift_name("stringFromOfflineBehavior(_:)")));
        [Static]
        [Export("stringFromOfflineBehavior:")]
        string StringFromOfflineBehavior(SCPOfflineBehavior behavior);
    }

    // @protocol SCPReconnectionDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPReconnectionDelegate
	{
        // @required -(void)readerDidSucceedReconnect:(SCPReader * _Nonnull)reader __attribute__((swift_name("readerDidSucceedReconnect(_:)")));
        [Abstract]
        [Export("readerDidSucceedReconnect:")]
        void ReaderDidSucceedReconnect(SCPReader reader);

        // @required -(void)readerDidFailReconnect:(SCPReader * _Nonnull)reader __attribute__((swift_name("readerDidFailReconnect(_:)")));
        [Abstract]
        [Export("readerDidFailReconnect:")]
        void ReaderDidFailReconnect(SCPReader reader);

        // @optional -(void)reader:(SCPReader * _Nonnull)reader didStartReconnect:(SCPCancelable * _Nonnull)cancelable __attribute__((swift_name("reader(_:didStartReconnect:)"))) __attribute__((deprecated("Use reader:didStartReconnect:disconnectReason: instead.")));
        [Export("reader:didStartReconnect:")]
        void Reader(SCPReader reader, SCPCancelable cancelable);

        // @optional -(void)reader:(SCPReader * _Nonnull)reader didStartReconnect:(SCPCancelable * _Nonnull)cancelable disconnectReason:(SCPDisconnectReason)disconnectReason __attribute__((swift_name("reader(_:didStartReconnect:disconnectReason:)")));
        [Export("reader:didStartReconnect:disconnectReason:")]
        void Reader(SCPReader reader, SCPCancelable cancelable, SCPDisconnectReason disconnectReason);
    }

	// @interface SCPBluetoothConnectionConfiguration : SCPConnectionConfiguration
	[BaseType (typeof(SCPConnectionConfiguration))]
	interface SCPBluetoothConnectionConfiguration
	{
		// @property (readonly, nonatomic) NSString * _Nonnull locationId;
		[Export ("locationId")]
		string LocationId { get; }

		// @property (readonly, assign, nonatomic) BOOL autoReconnectOnUnexpectedDisconnect;
		[Export ("autoReconnectOnUnexpectedDisconnect")]
		bool AutoReconnectOnUnexpectedDisconnect { get; }

		[Wrap ("WeakAutoReconnectionDelegate")]
		[NullAllowed]
		SCPReconnectionDelegate AutoReconnectionDelegate { get; }

		// @property (readonly, nonatomic, weak) id<SCPReconnectionDelegate> _Nullable autoReconnectionDelegate;
		[NullAllowed, Export ("autoReconnectionDelegate", ArgumentSemantic.Weak)]
		NSObject WeakAutoReconnectionDelegate { get; }
	}

    // @interface SCPBluetoothConnectionConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPBluetoothConnectionConfiguration>))]
    interface SCPBluetoothConnectionConfigurationBuilder
    {
        // -(instancetype _Nonnull)initWithLocationId:(NSString * _Nonnull)locationId __attribute__((objc_designated_initializer));
        [Export("initWithLocationId:")]
        NativeHandle Constructor(string locationId);

        // -(SCPBluetoothConnectionConfigurationBuilder * _Nonnull)setLocationId:(NSString * _Nonnull)locationId;
        [Export("setLocationId:")]
        SCPBluetoothConnectionConfigurationBuilder SetLocationId(string locationId);

        // -(SCPBluetoothConnectionConfigurationBuilder * _Nonnull)setAutoReconnectOnUnexpectedDisconnect:(BOOL)autoReconnectOnUnexpectedDisconnect;
        [Export("setAutoReconnectOnUnexpectedDisconnect:")]
        SCPBluetoothConnectionConfigurationBuilder SetAutoReconnectOnUnexpectedDisconnect(bool autoReconnectOnUnexpectedDisconnect);

        // -(SCPBluetoothConnectionConfigurationBuilder * _Nonnull)setAutoReconnectionDelegate:(id<SCPReconnectionDelegate> _Nullable)autoReconnectionDelegate;
        [Export("setAutoReconnectionDelegate:")]
        SCPBluetoothConnectionConfigurationBuilder SetAutoReconnectionDelegate([NullAllowed] SCPReconnectionDelegate autoReconnectionDelegate);
    }


    public partial interface ISCPDiscoveryConfiguration { }

    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface SCPDiscoveryConfiguration //: ISCPDiscoveryConfiguration
    {
        // @required @property (readonly, nonatomic) SCPDiscoveryMethod discoveryMethod;
        [Abstract]
        [Export("discoveryMethod")]
        SCPDiscoveryMethod DiscoveryMethod { get; }

        // @required @property (readonly, nonatomic) BOOL simulated;
        [Abstract]
        [Export("simulated")]
        bool Simulated { get; }
    }

    // @interface SCPBluetoothProximityDiscoveryConfiguration : NSObject <SCPDiscoveryConfiguration>
    [BaseType(typeof(SCPDiscoveryConfiguration))]
    interface SCPBluetoothProximityDiscoveryConfiguration : ISCPDiscoveryConfiguration
    {
    }

    // @interface SCPBluetoothProximityDiscoveryConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPDiscoveryConfiguration>))]
    interface SCPBluetoothProximityDiscoveryConfigurationBuilder
    {
        // -(SCPBluetoothProximityDiscoveryConfigurationBuilder * _Nonnull)setSimulated:(BOOL)simulated;
        [Export("setSimulated:")]
        SCPBluetoothProximityDiscoveryConfigurationBuilder SetSimulated(bool simulated);
    }

    // @interface SCPBluetoothScanDiscoveryConfiguration : NSObject <SCPDiscoveryConfiguration>
    [BaseType(typeof(SCPDiscoveryConfiguration))]
    interface SCPBluetoothScanDiscoveryConfiguration : ISCPDiscoveryConfiguration
    {
        // @property (readonly, assign, nonatomic) NSUInteger timeout;
        [Export("timeout")]
        nuint Timeout { get; }
    }

    // @interface SCPBluetoothScanDiscoveryConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPDiscoveryConfiguration>))]
    interface SCPBluetoothScanDiscoveryConfigurationBuilder
    {
        // -(SCPBluetoothScanDiscoveryConfigurationBuilder * _Nonnull)setSimulated:(BOOL)simulated;
        [Export("setSimulated:")]
        SCPBluetoothScanDiscoveryConfigurationBuilder SetSimulated(bool simulated);

        // -(SCPBluetoothScanDiscoveryConfigurationBuilder * _Nonnull)setTimeout:(NSUInteger)timeout;
        [Export("setTimeout:")]
        SCPBluetoothScanDiscoveryConfigurationBuilder SetTimeout(nuint timeout);
    }

    // @interface SCPCancelable : NSObject
    [BaseType (typeof(NSObject))]
	interface SCPCancelable
	{
		// @property (readonly, nonatomic) BOOL completed;
		[Export ("completed")]
		bool Completed { get; }

		// -(void)cancel:(SCPErrorCompletionBlock _Nonnull)completion;
		[Export ("cancel:")]
		[Async]
		void Cancel (SCPErrorCompletionBlock completion);
	}

	// @interface SCPCardDetails : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPCardDetails : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) SCPCardBrand brand;
		[Export ("brand")]
		SCPCardBrand Brand { get; }

		// @property (readonly, nonatomic) NSString * _Nullable country;
		[NullAllowed, Export ("country")]
		string Country { get; }

		// @property (readonly, nonatomic) NSInteger expMonth;
		[Export ("expMonth")]
		nint ExpMonth { get; }

		// @property (readonly, nonatomic) NSInteger expYear;
		[Export ("expYear")]
		nint ExpYear { get; }

		// @property (readonly, nonatomic) SCPCardFundingType funding;
		[Export ("funding")]
		SCPCardFundingType Funding { get; }

		// @property (readonly, nonatomic) NSString * _Nullable last4;
		[NullAllowed, Export ("last4")]
		string Last4 { get; }
	}

	// @interface SCPCardPresentDetails : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPCardPresentDetails : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) NSString * _Nonnull last4;
		[Export ("last4")]
		string Last4 { get; }

		// @property (readonly, nonatomic) NSInteger expMonth;
		[Export ("expMonth")]
		nint ExpMonth { get; }

		// @property (readonly, nonatomic) NSInteger expYear;
		[Export ("expYear")]
		nint ExpYear { get; }

		// @property (readonly, nonatomic) NSString * _Nullable cardholderName;
		[NullAllowed, Export ("cardholderName")]
		string CardholderName { get; }

		// @property (readonly, nonatomic) SCPCardFundingType funding;
		[Export ("funding")]
		SCPCardFundingType Funding { get; }

		// @property (readonly, nonatomic) SCPCardBrand brand;
		[Export ("brand")]
		SCPCardBrand Brand { get; }

		// @property (readonly, nonatomic) NSString * _Nullable generatedCard;
		[NullAllowed, Export ("generatedCard")]
		string GeneratedCard { get; }

		// @property (readonly, nonatomic) SCPReceiptDetails * _Nullable receipt;
		[NullAllowed, Export ("receipt")]
		SCPReceiptDetails Receipt { get; }

		// @property (readonly, nonatomic) NSString * _Nullable emvAuthData;
		[NullAllowed, Export ("emvAuthData")]
		string EmvAuthData { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable country;
		[NullAllowed, Export ("country")]
		string Country { get; }

		// @property (readonly, copy, nonatomic) NSArray<NSString *> * _Nullable preferredLocales;
		[NullAllowed, Export ("preferredLocales", ArgumentSemantic.Copy)]
		string[] PreferredLocales { get; }

		// @property (readonly, copy, nonatomic) SCPNetworks * _Nullable networks;
		[NullAllowed, Export ("networks", ArgumentSemantic.Copy)]
		SCPNetworks Networks { get; }

        // @property (readonly, copy, nonatomic) NSNumber * _Nullable network;
        [NullAllowed, Export("network", ArgumentSemantic.Copy)]
        NSNumber Network { get; }

        // @property (readonly, assign, nonatomic) SCPIncrementalAuthorizationStatus incrementalAuthorizationStatus;
        [Export ("incrementalAuthorizationStatus", ArgumentSemantic.Assign)]
		SCPIncrementalAuthorizationStatus IncrementalAuthorizationStatus { get; }

        // @property (readonly, assign, nonatomic) SCPReadMethod readMethod;
        [Export("readMethod", ArgumentSemantic.Assign)]
        SCPReadMethod ReadMethod { get; }

        // @property (readonly, nonatomic) SCPWallet * _Nullable wallet;
        [NullAllowed, Export("wallet")]
        SCPWallet Wallet { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable iin;
        [NullAllowed, Export("iin")]
        string Iin { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable issuer;
        [NullAllowed, Export("issuer")]
        string Issuer { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }
    }

	// @interface SCPCharge : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPCharge : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) NSUInteger amount;
		[Export ("amount")]
		nuint Amount { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull currency;
		[Export ("currency")]
		string Currency { get; }

		// @property (readonly, nonatomic) SCPChargeStatus status;
		[Export ("status")]
		SCPChargeStatus Status { get; }

		// @property (readonly, nonatomic) SCPPaymentMethodDetails * _Nullable paymentMethodDetails;
		[NullAllowed, Export ("paymentMethodDetails")]
		SCPPaymentMethodDetails PaymentMethodDetails { get; }

		// @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
		[NullAllowed, Export ("stripeDescription")]
		string StripeDescription { get; }

		// @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nonnull metadata;
		[Export ("metadata")]
		NSDictionary<NSString, NSString> Metadata { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, nonatomic) NSString * _Nullable statementDescriptorSuffix;
		[NullAllowed, Export ("statementDescriptorSuffix")]
		string StatementDescriptorSuffix { get; }

		// @property (readonly, nonatomic) NSString * _Nullable calculatedStatementDescriptor;
		[NullAllowed, Export ("calculatedStatementDescriptor")]
		string CalculatedStatementDescriptor { get; }

        // @property (readonly, nonatomic) NSString * _Nullable authorizationCode;
        [NullAllowed, Export("authorizationCode")]
        string AuthorizationCode { get; }
    }

    // @interface SCPCollectInputsResult : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPCollectInputsResult
    {
        // @property (readonly, nonatomic) BOOL skipped;
        [Export("skipped")]
        bool Skipped { get; }
    }

    [Static]
	partial interface Constants
	{
		// extern NSString *const _Nonnull SCPErrorDomain __attribute__((swift_name("ErrorDomain")));
		[Field ("SCPErrorDomain", "__Internal")]
		NSString SCPErrorDomain { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyMessage;
		[Field ("SCPErrorKeyMessage", "__Internal")]
		NSString SCPErrorKeyMessage { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIDeclineCode;
		[Field ("SCPErrorKeyStripeAPIDeclineCode", "__Internal")]
		NSString SCPErrorKeyStripeAPIDeclineCode { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIFailureReason;
		[Field ("SCPErrorKeyStripeAPIFailureReason", "__Internal")]
		NSString SCPErrorKeyStripeAPIFailureReason { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIRequestId;
		[Field ("SCPErrorKeyStripeAPIRequestId", "__Internal")]
		NSString SCPErrorKeyStripeAPIRequestId { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIErrorCode;
		[Field ("SCPErrorKeyStripeAPIErrorCode", "__Internal")]
		NSString SCPErrorKeyStripeAPIErrorCode { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIErrorType;
		[Field ("SCPErrorKeyStripeAPIErrorType", "__Internal")]
		NSString SCPErrorKeyStripeAPIErrorType { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIDocUrl;
		[Field ("SCPErrorKeyStripeAPIDocUrl", "__Internal")]
		NSString SCPErrorKeyStripeAPIDocUrl { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIErrorParameter;
		[Field ("SCPErrorKeyStripeAPIErrorParameter", "__Internal")]
		NSString SCPErrorKeyStripeAPIErrorParameter { get; }

        // extern SCPErrorKey _Nonnull SCPErrorKeyOfflineDeclineReason;
        [Field("SCPErrorKeyOfflineDeclineReason", "__Internal")]
        NSString SCPErrorKeyOfflineDeclineReason { get; }

        // extern SCPErrorKey _Nonnull SCPErrorKeyHttpStatusCode;
        [Field ("SCPErrorKeyHttpStatusCode", "__Internal")]
		NSString SCPErrorKeyHttpStatusCode { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyStripeAPIPaymentIntent;
		[Field ("SCPErrorKeyStripeAPIPaymentIntent", "__Internal")]
		NSString SCPErrorKeyStripeAPIPaymentIntent { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyReaderMessage;
		[Field ("SCPErrorKeyReaderMessage", "__Internal")]
		NSString SCPErrorKeyReaderMessage { get; }

		// extern SCPErrorKey _Nonnull SCPErrorKeyDeviceBannedUntilDate;
		[Field ("SCPErrorKeyDeviceBannedUntilDate", "__Internal")]
		NSString SCPErrorKeyDeviceBannedUntilDate { get; }

        // extern SCPErrorKey _Nonnull SCPErrorKeyPrepareFailedReason;
        [Field("SCPErrorKeyPrepareFailedReason", "__Internal")]
        NSString SCPErrorKeyPrepareFailedReason { get; }
    }

    // @interface SCPConfirmPaymentIntentError : NSError
    [BaseType(typeof(NSError))]
    [DisableDefaultCtor]
    interface SCPConfirmPaymentIntentError
    {
        // @property (readonly, nonatomic) SCPPaymentIntent * _Nullable paymentIntent;
        [NullAllowed, Export("paymentIntent")]
        SCPPaymentIntent PaymentIntent { get; }

        // @property (readonly, nonatomic) NSError * _Nullable requestError;
        [NullAllowed, Export("requestError")]
        NSError RequestError { get; }

        // @property (readonly, nonatomic) NSString * _Nullable declineCode;
        [NullAllowed, Export("declineCode")]
        string DeclineCode { get; }
    }

    // @interface SCPConfirmRefundError : NSError
    [BaseType(typeof(NSError))]
    interface SCPConfirmRefundError
    {
        // @property (readonly, nonatomic) SCPRefund * _Nullable refund;
        [NullAllowed, Export("refund")]
        SCPRefund Refund { get; }

        // @property (readonly, nonatomic) NSError * _Nullable requestError;
        [NullAllowed, Export("requestError")]
        NSError RequestError { get; }
    }

    // @interface SCPConfirmSetupIntentError : NSError
    [BaseType (typeof(NSError))]
	[DisableDefaultCtor]
	interface SCPConfirmSetupIntentError
	{
		// @property (readonly, nonatomic) SCPSetupIntent * _Nullable setupIntent;
		[NullAllowed, Export ("setupIntent")]
		SCPSetupIntent SetupIntent { get; }

		// @property (readonly, nonatomic) NSError * _Nullable requestError;
		[NullAllowed, Export ("requestError")]
		NSError RequestError { get; }

		// @property (readonly, nonatomic) NSString * _Nullable declineCode;
		[NullAllowed, Export ("declineCode")]
		string DeclineCode { get; }
	}

    // @protocol SCPConnectionTokenProvider
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SCPConnectionTokenProvider
	{
		// @required -(void)fetchConnectionToken:(SCPConnectionTokenCompletionBlock _Nonnull)completion __attribute__((swift_name("fetchConnectionToken(_:)")));
		[Abstract]
		[Export ("fetchConnectionToken:")]
        [Async]
        void FetchConnectionToken (SCPConnectionTokenCompletionBlock completion);
    }

    // @protocol SCPDiscoveryDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SCPDiscoveryDelegate
    {
        // @required -(void)terminal:(SCPTerminal * _Nonnull)terminal didUpdateDiscoveredReaders:(NSArray<SCPReader *> * _Nonnull)readers __attribute__((swift_name("terminal(_:didUpdateDiscoveredReaders:)")));
        [Abstract]
        [Export("terminal:didUpdateDiscoveredReaders:")]
        void DidUpdateDiscoveredReaders(SCPTerminal terminal, SCPReader[] readers);
    }

    /*
	// @interface SCPDiscoveryConfiguration : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPDiscoveryConfiguration
	{
		// -(instancetype _Nonnull)initWithDiscoveryMethod:(SCPDiscoveryMethod)discoveryMethod simulated:(BOOL)simulated;
		[Export ("initWithDiscoveryMethod:simulated:")]
		NativeHandle Constructor (SCPDiscoveryMethod discoveryMethod, bool simulated);

		// -(instancetype _Nonnull)initWithDiscoveryMethod:(SCPDiscoveryMethod)discoveryMethod locationId:(NSString * _Nullable)locationId simulated:(BOOL)simulated;
		[Export ("initWithDiscoveryMethod:locationId:simulated:")]
		NativeHandle Constructor (SCPDiscoveryMethod discoveryMethod, [NullAllowed] string locationId, bool simulated);

		// @property (assign, readwrite, nonatomic) NSUInteger timeout;
		[Export ("timeout")]
		nuint Timeout { get; set; }

		// @property (readonly, nonatomic) SCPDiscoveryMethod discoveryMethod;
		[Export ("discoveryMethod")]
		SCPDiscoveryMethod DiscoveryMethod { get; }

		// @property (readonly, nonatomic) BOOL simulated;
		[Export ("simulated")]
		bool Simulated { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable locationId;
		[NullAllowed, Export ("locationId")]
		string LocationId { get; }
	}
	*/


    // @interface SCPToggle : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPToggle
    {
        // @property (readonly, nonatomic) NSString * _Nullable title;
        [NullAllowed, Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, assign, nonatomic) SCPToggleValue defaultValue;
        [Export("defaultValue", ArgumentSemantic.Assign)]
        SCPToggleValue DefaultValue { get; }
    }

    // @interface SCPToggleBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPToggle>))]
    [DisableDefaultCtor]
    interface SCPToggleBuilder
    {
        // -(instancetype _Nonnull)initWithDefaultValue:(SCPToggleValue)defaultValue __attribute__((objc_designated_initializer));
        [Export("initWithDefaultValue:")]
        [DesignatedInitializer]
        NativeHandle Constructor(SCPToggleValue defaultValue);

        // -(SCPToggleBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPToggleBuilder SetTitle(string title);

        // -(SCPToggleBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPToggleBuilder SetStripeDescription(string stripeDescription);

        // -(SCPToggleBuilder * _Nonnull)setDefaultValue:(SCPToggleValue)defaultValue;
        [Export("setDefaultValue:")]
        SCPToggleBuilder SetDefaultValue(SCPToggleValue defaultValue);
    }

    // @interface SCPEmailInput : SCPInput
    [BaseType(typeof(SCPInput))]
    [DisableDefaultCtor]
    interface SCPEmailInput
    {
        // @property (readonly, nonatomic) NSString * _Nonnull title;
        [Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, nonatomic) NSString * _Nullable skipButtonText;
        [NullAllowed, Export("skipButtonText")]
        string SkipButtonText { get; }

        // @property (readonly, nonatomic) NSString * _Nullable submitButtonText;
        [NullAllowed, Export("submitButtonText")]
        string SubmitButtonText { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPToggle *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        SCPToggle[] Toggles { get; }
    }

    // @interface SCPEmailInputBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPEmailInput>))]
    [DisableDefaultCtor]
    interface SCPEmailInputBuilder
    {
        // -(instancetype _Nonnull)initWithTitle:(NSString * _Nonnull)title __attribute__((objc_designated_initializer));
        [Export("initWithTitle:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string title);

        // -(SCPEmailInputBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPEmailInputBuilder SetTitle(string title);

        // -(SCPEmailInputBuilder * _Nonnull)setRequired:(BOOL)required;
        [Export("setRequired:")]
        SCPEmailInputBuilder SetRequired(bool required);

        // -(SCPEmailInputBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPEmailInputBuilder SetStripeDescription(string stripeDescription);

        // -(SCPEmailInputBuilder * _Nonnull)setSkipButtonText:(NSString * _Nonnull)skipButtonText;
        [Export("setSkipButtonText:")]
        SCPEmailInputBuilder SetSkipButtonText(string skipButtonText);

        // -(SCPEmailInputBuilder * _Nonnull)setSubmitButtonText:(NSString * _Nonnull)submitButtonText;
        [Export("setSubmitButtonText:")]
        SCPEmailInputBuilder SetSubmitButtonText(string submitButtonText);

        // -(SCPEmailInputBuilder * _Nonnull)setToggles:(NSArray<SCPToggle *> * _Nonnull)toggles;
        [Export("setToggles:")]
        SCPEmailInputBuilder SetToggles(SCPToggle[] toggles);
    }

    // @interface SCPEmailResult : SCPCollectInputsResult
    [BaseType(typeof(SCPCollectInputsResult))]
    [DisableDefaultCtor]
    interface SCPEmailResult
    {
        // @property (readonly, nonatomic) NSString * _Nullable email;
        [NullAllowed, Export("email")]
        string Email { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        NSNumber[] Toggles { get; }
    }

    // @interface SCPInternetConnectionConfiguration : SCPConnectionConfiguration
    [BaseType (typeof(SCPConnectionConfiguration))]
	interface SCPInternetConnectionConfiguration
	{
		// @property (readonly, nonatomic) BOOL failIfInUse;
		[Export ("failIfInUse")]
		bool FailIfInUse { get; }

		// @property (readonly, nonatomic) BOOL allowCustomerCancel;
		[Export ("allowCustomerCancel")]
		bool AllowCustomerCancel { get; }
	}

    // @interface SCPInternetConnectionConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPInternetConnectionConfiguration>))]
    interface SCPInternetConnectionConfigurationBuilder
    {
        // -(SCPInternetConnectionConfigurationBuilder * _Nonnull)setFailIfInUse:(BOOL)failIfInUse;
        [Export("setFailIfInUse:")]
        SCPInternetConnectionConfigurationBuilder SetFailIfInUse(bool failIfInUse);

        // -(SCPInternetConnectionConfigurationBuilder * _Nonnull)setAllowCustomerCancel:(BOOL)allowCustomerCancel;
        [Export("setAllowCustomerCancel:")]
        SCPInternetConnectionConfigurationBuilder SetAllowCustomerCancel(bool allowCustomerCancel);
    }

    // @interface SCPInternetDiscoveryConfiguration : NSObject <SCPDiscoveryConfiguration>
    [BaseType(typeof(SCPDiscoveryConfiguration))]
    interface SCPInternetDiscoveryConfiguration : ISCPDiscoveryConfiguration
    {
        // @property (readonly, copy, nonatomic) NSString * _Nullable locationId;
        [NullAllowed, Export("locationId")]
        string LocationId { get; }
    }

    // @interface SCPInternetDiscoveryConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPDiscoveryConfiguration>))]
    interface SCPInternetDiscoveryConfigurationBuilder
    {
        // -(SCPInternetDiscoveryConfigurationBuilder * _Nonnull)setSimulated:(BOOL)simulated;
        [Export("setSimulated:")]
        SCPInternetDiscoveryConfigurationBuilder SetSimulated(bool simulated);

        // -(SCPInternetDiscoveryConfigurationBuilder * _Nonnull)setLocationId:(NSString * _Nullable)locationId;
        [Export("setLocationId:")]
        SCPInternetDiscoveryConfigurationBuilder SetLocationId([NullAllowed] string locationId);
    }

    // @interface SCPListLocationsParameters : NSObject
    [BaseType (typeof(NSObject))]
	interface SCPListLocationsParameters
	{
		// @property (readwrite, nonatomic) NSNumber * _Nullable limit;
		[NullAllowed, Export ("limit", ArgumentSemantic.Assign)]
		NSNumber Limit { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable endingBefore;
		[NullAllowed, Export ("endingBefore")]
		string EndingBefore { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable startingAfter;
		[NullAllowed, Export ("startingAfter")]
		string StartingAfter { get; set; }
    }

    // @interface SCPListLocationsParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPListLocationsParameters>))]
    interface SCPListLocationsParametersBuilder
    {
        // -(SCPListLocationsParametersBuilder * _Nonnull)setLimit:(NSUInteger)limit;
        [Export("setLimit:")]
        SCPListLocationsParametersBuilder SetLimit(nuint limit);

        // -(SCPListLocationsParametersBuilder * _Nonnull)setEndingBefore:(NSString * _Nullable)endingBefore;
        [Export("setEndingBefore:")]
        SCPListLocationsParametersBuilder SetEndingBefore([NullAllowed] string endingBefore);

        // -(SCPListLocationsParametersBuilder * _Nonnull)setStartingAfter:(NSString * _Nullable)startingAfter;
        [Export("setStartingAfter:")]
        SCPListLocationsParametersBuilder SetStartingAfter([NullAllowed] string startingAfter);
    }

    // @interface SCPLocalMobileConnectionConfiguration : SCPConnectionConfiguration
    [BaseType (typeof(SCPConnectionConfiguration))]
	interface SCPLocalMobileConnectionConfiguration
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull locationId;
		[Export ("locationId")]
		string LocationId { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable onBehalfOf;
		[NullAllowed, Export ("onBehalfOf")]
		string OnBehalfOf { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable merchantDisplayName;
		[NullAllowed, Export ("merchantDisplayName")]
		string MerchantDisplayName { get; }

		// @property (readonly, getter = isTOSAcceptancePermitted, assign, nonatomic) BOOL tosAcceptancePermitted;
		[Export ("tosAcceptancePermitted")]
		bool TosAcceptancePermitted { [Bind ("isTOSAcceptancePermitted")] get; }

        // @property (readonly, getter = isReturnReadResultImmediatelyEnabled, assign, nonatomic) BOOL returnReadResultImmediatelyEnabled;
        [Export("returnReadResultImmediatelyEnabled")]
        bool ReturnReadResultImmediatelyEnabled { [Bind("isReturnReadResultImmediatelyEnabled")] get; }

        // @property (readonly, getter = isAutoReconnectOnUnexpectedDisconnect, assign, nonatomic) BOOL autoReconnectOnUnexpectedDisconnect;
        [Export("autoReconnectOnUnexpectedDisconnect")]
        bool AutoReconnectOnUnexpectedDisconnect { [Bind("isAutoReconnectOnUnexpectedDisconnect")] get; }

        [Wrap("WeakAutoReconnectionDelegate")]
        [NullAllowed]
        SCPReconnectionDelegate AutoReconnectionDelegate { get; }

        // @property (readonly, nonatomic, weak) id<SCPReconnectionDelegate> _Nullable autoReconnectionDelegate;
        [NullAllowed, Export("autoReconnectionDelegate", ArgumentSemantic.Weak)]
        NSObject WeakAutoReconnectionDelegate { get; }
    }

    // @interface SCPLocalMobileConnectionConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPLocalMobileConnectionConfiguration>))]
    [DisableDefaultCtor]
    interface SCPLocalMobileConnectionConfigurationBuilder
    {
        // -(instancetype _Nonnull)initWithLocationId:(NSString * _Nonnull)locationId __attribute__((objc_designated_initializer));
        [Export("initWithLocationId:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string locationId);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setLocationId:(NSString * _Nonnull)locationId;
        [Export("setLocationId:")]
        SCPLocalMobileConnectionConfigurationBuilder SetLocationId(string locationId);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setOnBehalfOf:(NSString * _Nullable)onBehalfOf;
        [Export("setOnBehalfOf:")]
        SCPLocalMobileConnectionConfigurationBuilder SetOnBehalfOf([NullAllowed] string onBehalfOf);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setMerchantDisplayName:(NSString * _Nullable)merchantDisplayName;
        [Export("setMerchantDisplayName:")]
        SCPLocalMobileConnectionConfigurationBuilder SetMerchantDisplayName([NullAllowed] string merchantDisplayName);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setTosAcceptancePermitted:(BOOL)tosAcceptancePermitted;
        [Export("setTosAcceptancePermitted:")]
        SCPLocalMobileConnectionConfigurationBuilder SetTosAcceptancePermitted(bool tosAcceptancePermitted);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setReturnReadResultImmediatelyEnabled:(BOOL)returnReadResultImmediatelyEnabled;
        [Export("setReturnReadResultImmediatelyEnabled:")]
        SCPLocalMobileConnectionConfigurationBuilder SetReturnReadResultImmediatelyEnabled(bool returnReadResultImmediatelyEnabled);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setAutoReconnectOnUnexpectedDisconnect:(BOOL)autoReconnectOnUnexpectedDisconnect;
        [Export("setAutoReconnectOnUnexpectedDisconnect:")]
        SCPLocalMobileConnectionConfigurationBuilder SetAutoReconnectOnUnexpectedDisconnect(bool autoReconnectOnUnexpectedDisconnect);

        // -(SCPLocalMobileConnectionConfigurationBuilder * _Nonnull)setAutoReconnectionDelegate:(id<SCPReconnectionDelegate> _Nullable)autoReconnectionDelegate;
        [Export("setAutoReconnectionDelegate:")]
        SCPLocalMobileConnectionConfigurationBuilder SetAutoReconnectionDelegate([NullAllowed] SCPReconnectionDelegate autoReconnectionDelegate);
    }

    // @interface SCPLocalMobileDiscoveryConfiguration : NSObject <SCPDiscoveryConfiguration>
    [BaseType(typeof(SCPDiscoveryConfiguration))]
    interface SCPLocalMobileDiscoveryConfiguration : ISCPDiscoveryConfiguration
    {
    }

    // @interface SCPLocalMobileDiscoveryConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPDiscoveryConfiguration>))]
    interface SCPLocalMobileDiscoveryConfigurationBuilder
    {
        // -(SCPLocalMobileDiscoveryConfigurationBuilder * _Nonnull)setSimulated:(BOOL)simulated;
        [Export("setSimulated:")]
        SCPLocalMobileDiscoveryConfigurationBuilder SetSimulated(bool simulated);
    }

    // @interface SCPLocation : NSObject <SCPJSONDecodable>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPLocation : ISCPJSONDecodable
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, nonatomic, strong) SCPAddress * _Nullable address;
		[NullAllowed, Export ("address", ArgumentSemantic.Strong)]
		SCPAddress Address { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable displayName;
		[NullAllowed, Export ("displayName")]
		string DisplayName { get; }

		// @property (readonly, assign, nonatomic) BOOL livemode;
		[Export ("livemode")]
		bool Livemode { get; }

		// @property (readonly, nonatomic, strong) NSDictionary<NSString *,NSString *> * _Nullable metadata;
		[NullAllowed, Export ("metadata", ArgumentSemantic.Strong)]
		NSDictionary<NSString, NSString> Metadata { get; }
	}

	// @interface SCPNetworks : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPNetworks : ISCPJSONDecodable
	{
		// @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nullable available;
		[NullAllowed, Export ("available", ArgumentSemantic.Copy)]
		NSNumber[] Available { get; }
    }

    // @interface SCPNumericInput : SCPInput
    [BaseType(typeof(SCPInput))]
    [DisableDefaultCtor]
    interface SCPNumericInput
    {
        // @property (readonly, nonatomic) NSString * _Nonnull title;
        [Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, nonatomic) NSString * _Nullable skipButtonText;
        [NullAllowed, Export("skipButtonText")]
        string SkipButtonText { get; }

        // @property (readonly, nonatomic) NSString * _Nullable submitButtonText;
        [NullAllowed, Export("submitButtonText")]
        string SubmitButtonText { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPToggle *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        SCPToggle[] Toggles { get; }
    }

    // @interface SCPNumericInputBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPNumericInput>))]
    [DisableDefaultCtor]
    interface SCPNumericInputBuilder
    {
        // -(instancetype _Nonnull)initWithTitle:(NSString * _Nonnull)title __attribute__((objc_designated_initializer));
        [Export("initWithTitle:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string title);

        // -(SCPNumericInputBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPNumericInputBuilder SetTitle(string title);

        // -(SCPNumericInputBuilder * _Nonnull)setRequired:(BOOL)required;
        [Export("setRequired:")]
        SCPNumericInputBuilder SetRequired(bool required);

        // -(SCPNumericInputBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPNumericInputBuilder SetStripeDescription(string stripeDescription);

        // -(SCPNumericInputBuilder * _Nonnull)setSkipButtonText:(NSString * _Nonnull)skipButtonText;
        [Export("setSkipButtonText:")]
        SCPNumericInputBuilder SetSkipButtonText(string skipButtonText);

        // -(SCPNumericInputBuilder * _Nonnull)setSubmitButtonText:(NSString * _Nonnull)submitButtonText;
        [Export("setSubmitButtonText:")]
        SCPNumericInputBuilder SetSubmitButtonText(string submitButtonText);

        // -(SCPNumericInputBuilder * _Nonnull)setToggles:(NSArray<SCPToggle *> * _Nonnull)toggles;
        [Export("setToggles:")]
        SCPNumericInputBuilder SetToggles(SCPToggle[] toggles);
    }

    // @interface SCPNumericResult : SCPCollectInputsResult
    [BaseType(typeof(SCPCollectInputsResult))]
    [DisableDefaultCtor]
    interface SCPNumericResult
    {
        // @property (readonly, nonatomic) NSString * _Nullable numericString;
        [NullAllowed, Export("numericString")]
        string NumericString { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        NSNumber[] Toggles { get; }
    }

    // @interface SCPOfflineCardPresentDetails : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPOfflineCardPresentDetails : INSCopying
    {
        // @property (readonly, assign, nonatomic) SCPCardBrand brand;
        [Export("brand", ArgumentSemantic.Assign)]
        SCPCardBrand Brand { get; }

        // @property (readonly, assign, nonatomic) NSInteger expMonth;
        [Export("expMonth")]
        nint ExpMonth { get; }

        // @property (readonly, assign, nonatomic) NSInteger expYear;
        [Export("expYear")]
        nint ExpYear { get; }

        // @property (readonly, nonatomic) NSString * _Nullable last4;
        [NullAllowed, Export("last4")]
        string Last4 { get; }

        // @property (readonly, assign, nonatomic) SCPReadMethod readMethod;
        [Export("readMethod", ArgumentSemantic.Assign)]
        SCPReadMethod ReadMethod { get; }

        // @property (readonly, nonatomic) NSString * _Nullable cardholderName;
        [NullAllowed, Export("cardholderName")]
        string CardholderName { get; }

        // @property (readonly, nonatomic, strong) SCPReceiptDetails * _Nullable receiptDetails;
        [NullAllowed, Export("receiptDetails", ArgumentSemantic.Strong)]
        SCPReceiptDetails ReceiptDetails { get; }
    }

    // @interface SCPOfflineDetails : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    interface SCPOfflineDetails : INSCopying
    {
        // @property (readonly, nonatomic, strong) NSDate * _Nullable collectedAt;
        [NullAllowed, Export("collectedAt", ArgumentSemantic.Strong)]
        NSDate CollectedAt { get; }

        // @property (readwrite, nonatomic) BOOL requiresUpload;
        [Export("requiresUpload")]
        bool RequiresUpload { get; set; }

        // @property (readonly, nonatomic, strong) SCPAmountDetails * _Nullable amountDetails;
        [NullAllowed, Export("amountDetails", ArgumentSemantic.Strong)]
        SCPAmountDetails AmountDetails { get; }

        // @property (readonly, nonatomic, strong) SCPOfflineCardPresentDetails * _Nullable cardPresentDetails;
        [NullAllowed, Export("cardPresentDetails", ArgumentSemantic.Strong)]
        SCPOfflineCardPresentDetails CardPresentDetails { get; }
    }

    // @interface SCPPaymentMethod : NSObject <SCPJSONDecodable>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPPaymentMethod : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, nonatomic) NSDate * _Nullable created;
		[NullAllowed, Export ("created")]
		NSDate Created { get; }

		// @property (readonly, nonatomic) SCPPaymentMethodType type;
		[Export ("type")]
		SCPPaymentMethodType Type { get; }

		// @property (readonly, nonatomic) SCPCardDetails * _Nullable card;
		[NullAllowed, Export ("card")]
		SCPCardDetails Card { get; }

		// @property (readonly, nonatomic) SCPCardPresentDetails * _Nullable cardPresent;
		[NullAllowed, Export ("cardPresent")]
		SCPCardPresentDetails CardPresent { get; }

		// @property (readonly, nonatomic) SCPCardPresentDetails * _Nullable interacPresent;
		[NullAllowed, Export ("interacPresent")]
		SCPCardPresentDetails InteracPresent { get; }

		// @property (readonly, nonatomic) NSString * _Nullable customer;
		[NullAllowed, Export ("customer")]
		string Customer { get; }

		// @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nonnull metadata;
		[Export ("metadata")]
		NSDictionary<NSString, NSString> Metadata { get; }
	}

	// @interface SCPPaymentMethodDetails : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPPaymentMethodDetails : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) SCPPaymentMethodType type;
		[Export ("type")]
		SCPPaymentMethodType Type { get; }

		// @property (readonly, nonatomic) SCPCardPresentDetails * _Nullable cardPresent;
		[NullAllowed, Export ("cardPresent")]
		SCPCardPresentDetails CardPresent { get; }

		// @property (readonly, nonatomic) SCPCardPresentDetails * _Nullable interacPresent;
		[NullAllowed, Export ("interacPresent")]
		SCPCardPresentDetails InteracPresent { get; }
	}

    // @interface SCPPhoneInput : SCPInput
    [BaseType(typeof(SCPInput))]
    [DisableDefaultCtor]
    interface SCPPhoneInput
    {
        // @property (readonly, nonatomic) NSString * _Nonnull title;
        [Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, nonatomic) NSString * _Nullable skipButtonText;
        [NullAllowed, Export("skipButtonText")]
        string SkipButtonText { get; }

        // @property (readonly, nonatomic) NSString * _Nullable submitButtonText;
        [NullAllowed, Export("submitButtonText")]
        string SubmitButtonText { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPToggle *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        SCPToggle[] Toggles { get; }
    }

    // @interface SCPPhoneInputBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPPhoneInput>))]
    [DisableDefaultCtor]
    interface SCPPhoneInputBuilder
    {
        // -(instancetype _Nonnull)initWithTitle:(NSString * _Nonnull)title __attribute__((objc_designated_initializer));
        [Export("initWithTitle:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string title);

        // -(SCPPhoneInputBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPPhoneInputBuilder SetTitle(string title);

        // -(SCPPhoneInputBuilder * _Nonnull)setRequired:(BOOL)required;
        [Export("setRequired:")]
        SCPPhoneInputBuilder SetRequired(bool required);

        // -(SCPPhoneInputBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPPhoneInputBuilder SetStripeDescription(string stripeDescription);

        // -(SCPPhoneInputBuilder * _Nonnull)setSkipButtonText:(NSString * _Nonnull)skipButtonText;
        [Export("setSkipButtonText:")]
        SCPPhoneInputBuilder SetSkipButtonText(string skipButtonText);

        // -(SCPPhoneInputBuilder * _Nonnull)setSubmitButtonText:(NSString * _Nonnull)submitButtonText;
        [Export("setSubmitButtonText:")]
        SCPPhoneInputBuilder SetSubmitButtonText(string submitButtonText);

        // -(SCPPhoneInputBuilder * _Nonnull)setToggles:(NSArray<SCPToggle *> * _Nonnull)toggles;
        [Export("setToggles:")]
        SCPPhoneInputBuilder SetToggles(SCPToggle[] toggles);
    }

    // @interface SCPPhoneResult : SCPCollectInputsResult
    [BaseType(typeof(SCPCollectInputsResult))]
    [DisableDefaultCtor]
    interface SCPPhoneResult
    {
        // @property (readonly, nonatomic) NSString * _Nullable phone;
        [NullAllowed, Export("phone")]
        string Phone { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        NSNumber[] Toggles { get; }
    }


    // @interface SCPReader : NSObject <SCPJSONDecodable>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPReader : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) SCPDeviceType deviceType;
		[Export ("deviceType")]
		SCPDeviceType DeviceType { get; }

		// @property (readonly, nonatomic) BOOL simulated;
		[Export ("simulated")]
		bool Simulated { get; }

		// @property (readonly, nonatomic) NSString * _Nullable stripeId;
		[NullAllowed, Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, atomic) NSString * _Nullable locationId;
		[NullAllowed, Export ("locationId")]
		string LocationId { get; }

		// @property (readonly, atomic) SCPLocationStatus locationStatus;
		[Export ("locationStatus")]
		SCPLocationStatus LocationStatus { get; }

		// @property (readonly, atomic) SCPLocation * _Nullable location;
		[NullAllowed, Export ("location")]
		SCPLocation Location { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull serialNumber;
		[Export ("serialNumber")]
		string SerialNumber { get; }

		// @property (readonly, atomic) NSString * _Nullable deviceSoftwareVersion;
		[NullAllowed, Export ("deviceSoftwareVersion")]
		string DeviceSoftwareVersion { get; }

		// @property (readonly, atomic) SCPReaderSoftwareUpdate * _Nullable availableUpdate;
		[NullAllowed, Export ("availableUpdate")]
		SCPReaderSoftwareUpdate AvailableUpdate { get; }

		// @property (readonly, atomic) NSNumber * _Nullable batteryLevel;
		[NullAllowed, Export ("batteryLevel")]
		NSNumber BatteryLevel { get; }

		// @property (readonly, atomic) SCPBatteryStatus batteryStatus;
		[Export ("batteryStatus")]
		SCPBatteryStatus BatteryStatus { get; }

		// @property (readonly, atomic) NSNumber * _Nullable isCharging;
		[NullAllowed, Export ("isCharging")]
        [return: BindAs(typeof(bool))]
        NSNumber IsCharging { get; }

		// @property (readonly, nonatomic) NSString * _Nullable ipAddress;
		[NullAllowed, Export ("ipAddress")]
		string IpAddress { get; }

		// @property (readonly, nonatomic) SCPReaderNetworkStatus status;
		[Export ("status")]
		SCPReaderNetworkStatus Status { get; }

		// @property (readonly, nonatomic) NSString * _Nullable label;
		[NullAllowed, Export ("label")]
		string Label { get; }
    }

    // @interface SCPReaderAccessibility : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPReaderAccessibility
    {
        // @property (readonly, nonatomic) SCPReaderTextToSpeechStatus textToSpeechStatus;
        [Export("textToSpeechStatus")]
        SCPReaderTextToSpeechStatus TextToSpeechStatus { get; }

        // @property (readonly, nonatomic) NSError * _Nullable error;
        [NullAllowed, Export("error")]
        NSError Error { get; }
    }

	public partial interface ISCPReaderSettingsParameters { }

    // @interface SCPReaderAccessibilityParameters : NSObject <SCPReaderSettingsParameters>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPReaderAccessibilityParameters : ISCPReaderSettingsParameters
    {
        // @property (readonly, assign, nonatomic) BOOL textToSpeechViaSpeakers;
        [Export("textToSpeechViaSpeakers")]
        bool TextToSpeechViaSpeakers { get; }
    }

    // @interface SCPReaderAccessibilityParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPReaderAccessibilityParameters>))]
    interface SCPReaderAccessibilityParametersBuilder
    {
        // -(SCPReaderAccessibilityParametersBuilder * _Nonnull)setTextToSpeechViaSpeakers:(BOOL)textToSpeechViaSpeakers;
        [Export("setTextToSpeechViaSpeakers:")]
        SCPReaderAccessibilityParametersBuilder SetTextToSpeechViaSpeakers(bool textToSpeechViaSpeakers);
    }

    // @interface SCPReaderSettings : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPReaderSettings
    {
        // @property (readonly, nonatomic) SCPReaderAccessibility * _Nonnull accessibility;
        [Export("accessibility")]
        SCPReaderAccessibility Accessibility { get; }
    }

    // @interface SCPReaderSoftwareUpdate : NSObject
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPReaderSoftwareUpdate
	{
		// @property (readonly, nonatomic) SCPUpdateTimeEstimate estimatedUpdateTime;
		[Export ("estimatedUpdateTime")]
		SCPUpdateTimeEstimate EstimatedUpdateTime { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull deviceSoftwareVersion;
		[Export ("deviceSoftwareVersion")]
		string DeviceSoftwareVersion { get; }

		// @property (readonly, nonatomic) SCPUpdateComponent components;
		[Export ("components")]
		SCPUpdateComponent Components { get; }

		// @property (readonly, nonatomic) NSDate * _Nonnull requiredAt;
		[Export ("requiredAt")]
		NSDate RequiredAt { get; }

		// +(NSString * _Nonnull)stringFromUpdateTimeEstimate:(SCPUpdateTimeEstimate)estimate;
		[Static]
		[Export ("stringFromUpdateTimeEstimate:")]
		string StringFromUpdateTimeEstimate (SCPUpdateTimeEstimate estimate);
	}

	// @interface SCPReceiptDetails : NSObject <SCPJSONDecodable, NSCopying>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPReceiptDetails : ISCPJSONDecodable, INSCopying
	{
		// @property (readonly, nonatomic) NSString * _Nullable accountType;
		[NullAllowed, Export ("accountType")]
		string AccountType { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull applicationPreferredName;
		[Export ("applicationPreferredName")]
		string ApplicationPreferredName { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull dedicatedFileName;
		[Export ("dedicatedFileName")]
		string DedicatedFileName { get; }

		// @property (readonly, nonatomic) NSString * _Nullable authorizationCode;
		[NullAllowed, Export ("authorizationCode")]
		string AuthorizationCode { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull authorizationResponseCode;
		[Export ("authorizationResponseCode")]
		string AuthorizationResponseCode { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull applicationCryptogram;
		[Export ("applicationCryptogram")]
		string ApplicationCryptogram { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull terminalVerificationResults;
		[Export ("terminalVerificationResults")]
		string TerminalVerificationResults { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull transactionStatusInformation;
		[Export ("transactionStatusInformation")]
		string TransactionStatusInformation { get; }
	}

	// @interface SCPRefund : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPRefund : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, nonatomic) NSUInteger amount;
		[Export ("amount")]
		nuint Amount { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull charge;
		[Export ("charge")]
		string Charge { get; }

		// @property (readonly, nonatomic) NSDate * _Nonnull created;
		[Export ("created")]
		NSDate Created { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull currency;
		[Export ("currency")]
		string Currency { get; }

		// @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nonnull metadata;
		[Export ("metadata")]
		NSDictionary<NSString, NSString> Metadata { get; }

		// @property (readonly, nonatomic) NSString * _Nullable reason;
		[NullAllowed, Export ("reason")]
		string Reason { get; }

		// @property (readonly, nonatomic) SCPRefundStatus status;
		[Export ("status")]
		SCPRefundStatus Status { get; }

		// @property (readonly, nonatomic) SCPPaymentMethodDetails * _Nullable paymentMethodDetails;
		[NullAllowed, Export ("paymentMethodDetails")]
		SCPPaymentMethodDetails PaymentMethodDetails { get; }

		// @property (readonly, nonatomic) NSString * _Nullable failureReason;
		[NullAllowed, Export ("failureReason")]
		string FailureReason { get; }
    }

    // @interface SCPSelectionButton : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPSelectionButton
    {
        // @property (readonly, assign, nonatomic) SCPSelectionButtonStyle style;
        [Export("style", ArgumentSemantic.Assign)]
        SCPSelectionButtonStyle Style { get; }

        // @property (readonly, nonatomic) NSString * _Nonnull text;
        [Export("text")]
        string Text { get; }
    }

    // @interface SCPSelectionButtonBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPSelectionButton>))]
    [DisableDefaultCtor]
    interface SCPSelectionButtonBuilder
    {
        // -(instancetype _Nonnull)initWithStyle:(SCPSelectionButtonStyle)style text:(NSString * _Nonnull)text __attribute__((objc_designated_initializer));
        [Export("initWithStyle:text:")]
        [DesignatedInitializer]
        NativeHandle Constructor(SCPSelectionButtonStyle style, string text);

        // -(SCPSelectionButtonBuilder * _Nonnull)setStyle:(SCPSelectionButtonStyle)style;
        [Export("setStyle:")]
        SCPSelectionButtonBuilder SetStyle(SCPSelectionButtonStyle style);

        // -(SCPSelectionButtonBuilder * _Nonnull)setText:(NSString * _Nonnull)text;
        [Export("setText:")]
        SCPSelectionButtonBuilder SetText(string text);
    }

    // @interface SCPSelectionInput : SCPInput
    [BaseType(typeof(SCPInput))]
    [DisableDefaultCtor]
    interface SCPSelectionInput
    {
        // @property (readonly, nonatomic) NSString * _Nonnull title;
        [Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, nonatomic) NSString * _Nullable skipButtonText;
        [NullAllowed, Export("skipButtonText")]
        string SkipButtonText { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPSelectionButton *> * _Nonnull selectionButtons;
        [Export("selectionButtons", ArgumentSemantic.Copy)]
        SCPSelectionButton[] SelectionButtons { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPToggle *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        SCPToggle[] Toggles { get; }
    }

    // @interface SCPSelectionInputBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPSelectionInput>))]
    [DisableDefaultCtor]
    interface SCPSelectionInputBuilder
    {
        // -(instancetype _Nonnull)initWithTitle:(NSString * _Nonnull)title __attribute__((objc_designated_initializer));
        [Export("initWithTitle:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string title);

        // -(SCPSelectionInputBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPSelectionInputBuilder SetTitle(string title);

        // -(SCPSelectionInputBuilder * _Nonnull)setRequired:(BOOL)required;
        [Export("setRequired:")]
        SCPSelectionInputBuilder SetRequired(bool required);

        // -(SCPSelectionInputBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPSelectionInputBuilder SetStripeDescription(string stripeDescription);

        // -(SCPSelectionInputBuilder * _Nonnull)setSkipButtonText:(NSString * _Nonnull)skipButtonText;
        [Export("setSkipButtonText:")]
        SCPSelectionInputBuilder SetSkipButtonText(string skipButtonText);

        // -(SCPSelectionInputBuilder * _Nonnull)setSelectionButtons:(NSArray<SCPSelectionButton *> * _Nonnull)selectionButton;
        [Export("setSelectionButtons:")]
        SCPSelectionInputBuilder SetSelectionButtons(SCPSelectionButton[] selectionButton);

        // -(SCPSelectionInputBuilder * _Nonnull)setToggles:(NSArray<SCPToggle *> * _Nonnull)toggles;
        [Export("setToggles:")]
        SCPSelectionInputBuilder SetToggles(SCPToggle[] toggles);
    }

    // @interface SCPSelectionResult : SCPCollectInputsResult
    [BaseType(typeof(SCPCollectInputsResult))]
    [DisableDefaultCtor]
    interface SCPSelectionResult
    {
        // @property (readonly, nonatomic) NSString * _Nullable selection;
        [NullAllowed, Export("selection")]
        string Selection { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        NSNumber[] Toggles { get; }
    }

    // @interface SCPSetupAttempt : NSObject <SCPJSONDecodable>
    [BaseType (typeof(NSObject))]
	interface SCPSetupAttempt : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) NSString * _Nullable application;
		[NullAllowed, Export ("application")]
		string Application { get; }

		// @property (readonly, nonatomic) NSDate * _Nonnull created;
		[Export ("created")]
		NSDate Created { get; }

		// @property (readonly, nonatomic) NSString * _Nullable customer;
		[NullAllowed, Export ("customer")]
		string Customer { get; }

		// @property (readonly, nonatomic) NSString * _Nullable onBehalfOf;
		[NullAllowed, Export ("onBehalfOf")]
		string OnBehalfOf { get; }

		// @property (readonly, nonatomic) NSString * _Nullable paymentMethod;
		[NullAllowed, Export ("paymentMethod")]
		string PaymentMethod { get; }

		// @property (readonly, nonatomic) SCPSetupAttemptPaymentMethodDetails * _Nullable paymentMethodDetails;
		[NullAllowed, Export ("paymentMethodDetails")]
		SCPSetupAttemptPaymentMethodDetails PaymentMethodDetails { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull setupIntent;
		[Export ("setupIntent")]
		string SetupIntent { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull status;
		[Export ("status")]
		string Status { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }
	}

	// @interface SCPSetupAttemptCardPresentDetails : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPSetupAttemptCardPresentDetails : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) NSString * _Nonnull generatedCard;
		[Export ("generatedCard")]
		string GeneratedCard { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull emvAuthData;
		[Export ("emvAuthData")]
		string EmvAuthData { get; }
	}

	// @interface SCPSetupAttemptPaymentMethodDetails : NSObject <SCPJSONDecodable>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPSetupAttemptPaymentMethodDetails : ISCPJSONDecodable
	{
		// @property (readonly, nonatomic) SCPPaymentMethodType type;
		[Export ("type")]
		SCPPaymentMethodType Type { get; }

		// @property (readonly, nonatomic) SCPSetupAttemptCardPresentDetails * _Nullable cardPresent;
		[NullAllowed, Export ("cardPresent")]
		SCPSetupAttemptCardPresentDetails CardPresent { get; }

		// @property (readonly, nonatomic) SCPSetupAttemptCardPresentDetails * _Nullable interacPresent;
		[NullAllowed, Export ("interacPresent")]
		SCPSetupAttemptCardPresentDetails InteracPresent { get; }
	}

	// @interface SCPSetupIntent : NSObject <SCPJSONDecodable, NSCopying>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPSetupIntent : ISCPJSONDecodable, INSCopying
	{
		// @property (readonly, nonatomic) NSString * _Nonnull stripeId;
		[Export ("stripeId")]
		string StripeId { get; }

		// @property (readonly, nonatomic) NSDate * _Nonnull created;
		[Export ("created")]
		NSDate Created { get; }

		// @property (readonly, nonatomic) NSString * _Nullable customer;
		[NullAllowed, Export ("customer")]
		string Customer { get; }

		// @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable metadata;
		[NullAllowed, Export ("metadata")]
		NSDictionary<NSString, NSString> Metadata { get; }

		// @property (readonly, nonatomic) SCPSetupIntentUsage usage;
		[Export ("usage")]
		SCPSetupIntentUsage Usage { get; }

		// @property (readonly, nonatomic) SCPSetupIntentStatus status;
		[Export ("status")]
		SCPSetupIntentStatus Status { get; }

		// @property (readonly, nonatomic) SCPSetupAttempt * _Nullable latestAttempt;
		[NullAllowed, Export ("latestAttempt")]
		SCPSetupAttempt LatestAttempt { get; }

        // @property (readonly, nonatomic) NSArray<NSString *> * _Nonnull paymentMethodTypes;
        [Export("paymentMethodTypes")]
        string[] PaymentMethodTypes { get; }
    }

	// @interface SCPSetupIntentParameters : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPSetupIntentParameters
	{
		// @property (readwrite, copy, nonatomic) NSString * _Nullable customer;
		[NullAllowed, Export ("customer")]
		string Customer { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable stripeDescription;
		[NullAllowed, Export ("stripeDescription")]
		string StripeDescription { get; set; }

		// @property (readwrite, copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable metadata;
		[NullAllowed, Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> Metadata { get; set; }

		// @property (readwrite, nonatomic) SCPSetupIntentUsage usage;
		[Export ("usage", ArgumentSemantic.Assign)]
		SCPSetupIntentUsage Usage { get; set; }

		// @property (readwrite, copy, nonatomic) NSString * _Nullable onBehalfOf;
		[NullAllowed, Export ("onBehalfOf")]
		string OnBehalfOf { get; set; }

        // @property (readonly, nonatomic) NSArray<NSString *> * _Nonnull paymentMethodTypes;
        [Export("paymentMethodTypes")]
        string[] PaymentMethodTypes { get; }

        // @property (readonly, copy, nonatomic) DEPRECATED_MSG_ATTRIBUTE("Did you mean to use stripeDescription?") NSString * description __attribute__((deprecated("Did you mean to use stripeDescription?")));
        [Obsolete("Did you mean to use stripeDescription?")]
        [Export("description")]
        string Description { get; }
    }

    // @interface SCPSetupIntentParametersBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPSetupIntentParameters>))]
    interface SCPSetupIntentParametersBuilder
    {
        // -(SCPSetupIntentParametersBuilder * _Nonnull)setCustomer:(NSString * _Nullable)customer;
        [Export("setCustomer:")]
        SCPSetupIntentParametersBuilder SetCustomer([NullAllowed] string customer);

        // -(SCPSetupIntentParametersBuilder * _Nonnull)setStripeDescription:(NSString * _Nullable)stripeDescription;
        [Export("setStripeDescription:")]
        SCPSetupIntentParametersBuilder SetStripeDescription([NullAllowed] string stripeDescription);

        // -(SCPSetupIntentParametersBuilder * _Nonnull)setMetadata:(NSDictionary<NSString *,NSString *> * _Nullable)metadata;
        [Export("setMetadata:")]
        SCPSetupIntentParametersBuilder SetMetadata([NullAllowed] NSDictionary<NSString, NSString> metadata);

        // -(SCPSetupIntentParametersBuilder * _Nonnull)setUsage:(SCPSetupIntentUsage)usage;
        [Export("setUsage:")]
        SCPSetupIntentParametersBuilder SetUsage(SCPSetupIntentUsage usage);

        // -(SCPSetupIntentParametersBuilder * _Nonnull)setOnBehalfOf:(NSString * _Nullable)onBehalfOf;
        [Export("setOnBehalfOf:")]
        SCPSetupIntentParametersBuilder SetOnBehalfOf([NullAllowed] string onBehalfOf);

        // -(SCPSetupIntentParametersBuilder * _Nonnull)setPaymentMethodTypes:(NSArray<NSString *> * _Nonnull)paymentMethodTypes;
        [Export("setPaymentMethodTypes:")]
        SCPSetupIntentParametersBuilder SetPaymentMethodTypes(string[] paymentMethodTypes);
    }

    // @interface SCPSignatureInput : SCPInput
    [BaseType(typeof(SCPInput))]
    [DisableDefaultCtor]
    interface SCPSignatureInput
    {
        // @property (readonly, nonatomic) NSString * _Nonnull title;
        [Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, nonatomic) NSString * _Nullable skipButtonText;
        [NullAllowed, Export("skipButtonText")]
        string SkipButtonText { get; }

        // @property (readonly, nonatomic) NSString * _Nullable submitButtonText;
        [NullAllowed, Export("submitButtonText")]
        string SubmitButtonText { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPToggle *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        SCPToggle[] Toggles { get; }
    }

    // @interface SCPSignatureInputBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPSignatureInput>))]
    [DisableDefaultCtor]
    interface SCPSignatureInputBuilder
    {
        // -(instancetype _Nonnull)initWithTitle:(NSString * _Nonnull)title __attribute__((objc_designated_initializer));
        [Export("initWithTitle:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string title);

        // -(SCPSignatureInputBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPSignatureInputBuilder SetTitle(string title);

        // -(SCPSignatureInputBuilder * _Nonnull)setRequired:(BOOL)required;
        [Export("setRequired:")]
        SCPSignatureInputBuilder SetRequired(bool required);

        // -(SCPSignatureInputBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPSignatureInputBuilder SetStripeDescription(string stripeDescription);

        // -(SCPSignatureInputBuilder * _Nonnull)setSkipButtonText:(NSString * _Nonnull)skipButtonText;
        [Export("setSkipButtonText:")]
        SCPSignatureInputBuilder SetSkipButtonText(string skipButtonText);

        // -(SCPSignatureInputBuilder * _Nonnull)setSubmitButtonText:(NSString * _Nonnull)submitButtonText;
        [Export("setSubmitButtonText:")]
        SCPSignatureInputBuilder SetSubmitButtonText(string submitButtonText);

        // -(SCPSignatureInputBuilder * _Nonnull)setToggles:(NSArray<SCPToggle *> * _Nonnull)toggles;
        [Export("setToggles:")]
        SCPSignatureInputBuilder SetToggles(SCPToggle[] toggles);
    }

    // @interface SCPSignatureResult : SCPCollectInputsResult
    [BaseType(typeof(SCPCollectInputsResult))]
    [DisableDefaultCtor]
    interface SCPSignatureResult
    {
        // @property (readonly, nonatomic) NSString * _Nullable signatureSvg;
        [NullAllowed, Export("signatureSvg")]
        string SignatureSvg { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        NSNumber[] Toggles { get; }
    }

    // @protocol SCPTerminalDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPTerminalDelegate
	{
		// @required -(void)terminal:(SCPTerminal * _Nonnull)terminal didReportUnexpectedReaderDisconnect:(SCPReader * _Nonnull)reader __attribute__((swift_name("terminal(_:didReportUnexpectedReaderDisconnect:)")));
		[Abstract]
		[Export ("terminal:didReportUnexpectedReaderDisconnect:")]
		void DidReportUnexpectedReaderDisconnect (SCPTerminal terminal, SCPReader reader);

		// @optional -(void)terminal:(SCPTerminal * _Nonnull)terminal didChangeConnectionStatus:(SCPConnectionStatus)status __attribute__((swift_name("terminal(_:didChangeConnectionStatus:)")));
		[Export ("terminal:didChangeConnectionStatus:")]
		void DidChangeConnectionStatus (SCPTerminal terminal, SCPConnectionStatus status);

		// @optional -(void)terminal:(SCPTerminal * _Nonnull)terminal didChangePaymentStatus:(SCPPaymentStatus)status __attribute__((swift_name("terminal(_:didChangePaymentStatus:)")));
		[Export ("terminal:didChangePaymentStatus:")]
		void DidChangePaymentStatus (SCPTerminal terminal, SCPPaymentStatus status);
    }

    // @interface SCPTextInput : SCPInput
    [BaseType(typeof(SCPInput))]
    [DisableDefaultCtor]
    interface SCPTextInput
    {
        // @property (readonly, nonatomic) NSString * _Nonnull title;
        [Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable stripeDescription;
        [NullAllowed, Export("stripeDescription")]
        string StripeDescription { get; }

        // @property (readonly, nonatomic) NSString * _Nullable skipButtonText;
        [NullAllowed, Export("skipButtonText")]
        string SkipButtonText { get; }

        // @property (readonly, nonatomic) NSString * _Nullable submitButtonText;
        [NullAllowed, Export("submitButtonText")]
        string SubmitButtonText { get; }

        // @property (readonly, copy, nonatomic) NSArray<SCPToggle *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        SCPToggle[] Toggles { get; }
    }

    // @interface SCPTextInputBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPTextInput>))]
    [DisableDefaultCtor]
    interface SCPTextInputBuilder
    {
        // -(instancetype _Nonnull)initWithTitle:(NSString * _Nonnull)title __attribute__((objc_designated_initializer));
        [Export("initWithTitle:")]
        [DesignatedInitializer]
        NativeHandle Constructor(string title);

        // -(SCPTextInputBuilder * _Nonnull)setTitle:(NSString * _Nonnull)title;
        [Export("setTitle:")]
        SCPTextInputBuilder SetTitle(string title);

        // -(SCPTextInputBuilder * _Nonnull)setRequired:(BOOL)required;
        [Export("setRequired:")]
        SCPTextInputBuilder SetRequired(bool required);

        // -(SCPTextInputBuilder * _Nonnull)setStripeDescription:(NSString * _Nonnull)stripeDescription;
        [Export("setStripeDescription:")]
        SCPTextInputBuilder SetStripeDescription(string stripeDescription);

        // -(SCPTextInputBuilder * _Nonnull)setSkipButtonText:(NSString * _Nonnull)skipButtonText;
        [Export("setSkipButtonText:")]
        SCPTextInputBuilder SetSkipButtonText(string skipButtonText);

        // -(SCPTextInputBuilder * _Nonnull)setSubmitButtonText:(NSString * _Nonnull)submitButtonText;
        [Export("setSubmitButtonText:")]
        SCPTextInputBuilder SetSubmitButtonText(string submitButtonText);

        // -(SCPTextInputBuilder * _Nonnull)setToggles:(NSArray<SCPToggle *> * _Nonnull)toggles;
        [Export("setToggles:")]
        SCPTextInputBuilder SetToggles(SCPToggle[] toggles);
    }

    // @interface SCPTextResult : SCPCollectInputsResult
    [BaseType(typeof(SCPCollectInputsResult))]
    [DisableDefaultCtor]
    interface SCPTextResult
    {
        // @property (readonly, nonatomic) NSString * _Nullable text;
        [NullAllowed, Export("text")]
        string Text { get; }

        // @property (readonly, copy, nonatomic) NSArray<NSNumber *> * _Nonnull toggles;
        [Export("toggles", ArgumentSemantic.Copy)]
        NSNumber[] Toggles { get; }
    }

    // @interface SCPTip : NSObject <SCPJSONDecodable, NSCopying>
    [BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPTip : ISCPJSONDecodable, INSCopying
	{
		// @property (readonly, nonatomic) NSNumber * _Nullable amount;
		[NullAllowed, Export ("amount")]
		NSNumber Amount { get; }
	}

	// @interface SCPTippingConfiguration : NSObject <NSCopying>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPTippingConfiguration : INSCopying
	{
		// @property (assign, nonatomic) NSInteger eligibleAmount;
		[Export ("eligibleAmount")]
		nint EligibleAmount { get; set; }
    }

    // @interface SCPTippingConfigurationBuilder : SCPBuilder
    [BaseType(typeof(SCPBuilder<SCPTippingConfiguration>))]
    interface SCPTippingConfigurationBuilder
    {
        // -(instancetype _Nonnull)setEligibleAmount:(NSInteger)eligibleAmount;
        [Export("setEligibleAmount:")]
        SCPTippingConfigurationBuilder SetEligibleAmount(nint eligibleAmount);
    }

    // @interface SCPWallet : NSObject <SCPJSONDecodable>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface SCPWallet : ISCPJSONDecodable
    {
        // @property (readonly, nonatomic) NSString * _Nullable type;
        [NullAllowed, Export("type")]
        string Type { get; }
    }

    // @protocol SCPAppleBuiltInReader <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface SCPAppleBuiltInReader
	{
		// @required +(BOOL)isSupportedWithSimulated:(BOOL)simulated error:(NSError * _Nullable * _Nullable)error;
		[Static, Abstract]
		[Export ("isSupportedWithSimulated:error:")]
		bool IsSupportedWithSimulated (bool simulated, [NullAllowed] out NSError error);

		// @required +(void)discoverAvailableReaderIdentifiersWithCompletion:(void (^ _Nonnull)(NSSet<NSString *> * _Nullable, NSError * _Nullable))completion;
		[Static, Abstract]
        [Async]
        [Export ("discoverAvailableReaderIdentifiersWithCompletion:")]
		void DiscoverAvailableReaderIdentifiersWithCompletion (Action<NSSet<NSString>, NSError> completion);

		// @required @property (readonly, copy, nonatomic) NSString * _Nonnull readerIdentifier;
		[Abstract]
		[Export ("readerIdentifier")]
		string ReaderIdentifier { get; }

		// @required @property (readonly, nonatomic) BOOL isSimulated;
		[Abstract]
		[Export ("isSimulated")]
		bool IsSimulated { get; }

		// @required @property (readonly, nonatomic, strong) SCPLocalMobileConnectionConfiguration * _Nonnull connectionConfiguration;
		[Abstract]
		[Export ("connectionConfiguration", ArgumentSemantic.Strong)]
		SCPLocalMobileConnectionConfiguration ConnectionConfiguration { get; }

		// @required @property (copy, nonatomic) NSString * _Nullable merchantReference;
		[Abstract]
		[NullAllowed, Export ("merchantReference")]
		string MerchantReference { get; set; }

		[Wrap ("WeakAccountLinkingDelegate"), Abstract]
		[NullAllowed]
		SCPAppleBuiltInReaderAccountLinkingDelegate AccountLinkingDelegate { get; set; }

		// @required @property (nonatomic, weak) id<SCPAppleBuiltInReaderAccountLinkingDelegate> _Nullable accountLinkingDelegate;
		[Abstract]
		[NullAllowed, Export ("accountLinkingDelegate", ArgumentSemantic.Weak)]
		NSObject WeakAccountLinkingDelegate { get; set; }

		[Wrap ("WeakPrepareDelegate"), Abstract]
		[NullAllowed]
		SCPAppleBuiltInReaderPrepareDelegate PrepareDelegate { get; set; }

		// @required @property (nonatomic, weak) id<SCPAppleBuiltInReaderPrepareDelegate> _Nullable prepareDelegate;
		[Abstract]
		[NullAllowed, Export ("prepareDelegate", ArgumentSemantic.Weak)]
		NSObject WeakPrepareDelegate { get; set; }

		[Wrap ("WeakTransactionDelegate"), Abstract]
		[NullAllowed]
		SCPAppleBuiltInReaderTransactionDelegate TransactionDelegate { get; set; }

		// @required @property (nonatomic, weak) id<SCPAppleBuiltInReaderTransactionDelegate> _Nullable transactionDelegate;
		[Abstract]
		[NullAllowed, Export ("transactionDelegate", ArgumentSemantic.Weak)]
		NSObject WeakTransactionDelegate { get; set; }

		// @required -(instancetype _Nonnull)initWithReaderIdentifier:(NSString * _Nonnull)crid connectionConfiguration:(SCPLocalMobileConnectionConfiguration * _Nonnull)connectionConfiguration isSimulated:(BOOL)isSimulated;
		//[Abstract]
		[Export ("initWithReaderIdentifier:connectionConfiguration:isSimulated:")]
		NativeHandle Constructor (string crid, SCPLocalMobileConnectionConfiguration connectionConfiguration, bool isSimulated);

		// @required -(BOOL)linkAccountUsingToken:(NSString * _Nonnull)token merchantReference:(NSString * _Nonnull)merchantReference error:(NSError * _Nullable * _Nullable)error;
		[Abstract]
		[Export ("linkAccountUsingToken:merchantReference:error:")]
		bool LinkAccountUsingToken (string token, string merchantReference, [NullAllowed] out NSError error);

		// @required -(BOOL)prepareUsingToken:(NSString * _Nonnull)token merchantReference:(NSString * _Nonnull)merchantReference error:(NSError * _Nullable * _Nullable)error;
		[Abstract]
		[Export ("prepareUsingToken:merchantReference:error:")]
		bool PrepareUsingToken (string token, string merchantReference, [NullAllowed] out NSError error);

		// @required -(BOOL)cancelTransactionAndReturnError:(NSError * _Nullable * _Nullable)error;
		[Abstract]
		[Export ("cancelTransactionAndReturnError:")]
		bool CancelTransactionAndReturnError ([NullAllowed] out NSError error);

		// @required -(BOOL)performTransactionWithType:(enum SCPAppleBuiltInReaderTransactionType)transactionType amount:(NSDecimalNumber * _Nullable)amount currencyCode:(NSString * _Nonnull)currencyCode error:(NSError * _Nullable * _Nullable)error;
		[Abstract]
		[Export ("performTransactionWithType:amount:currencyCode:error:")]
		bool PerformTransactionWithType (SCPAppleBuiltInReaderTransactionType transactionType, [NullAllowed] NSDecimalNumber amount, string currencyCode, [NullAllowed] out NSError error);

        // @required -(BOOL)capturePINUsingToken:(NSString * _Nonnull)token cardReaderTransactionID:(NSString * _Nonnull)cardReaderTransactionID error:(NSError * _Nullable * _Nullable)error;
        [Abstract]
        [Export("capturePINUsingToken:cardReaderTransactionID:error:")]
        bool CapturePINUsingToken(string token, string cardReaderTransactionID, [NullAllowed] out NSError error);

        // @required -(BOOL)performMockTransactionWithType:(enum SCPAppleBuiltInReaderTransactionType)transactionType amount:(NSDecimalNumber * _Nullable)amount currencyCode:(NSString * _Nonnull)currencyCode error:(NSError * _Nullable * _Nullable)error;
        [Abstract]
		[Export ("performMockTransactionWithType:amount:currencyCode:error:")]
		bool PerformMockTransactionWithType (SCPAppleBuiltInReaderTransactionType transactionType, [NullAllowed] NSDecimalNumber amount, string currencyCode, [NullAllowed] out NSError error);
	}
    /*
	// @interface SCPAppleBuiltInReader : NSObject <SCPAppleBuiltInReader>
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SCPAppleBuiltInReader : ISCPAppleBuiltInReader
	{
		// +(BOOL)isSupportedWithSimulated:(BOOL)simulated error:(NSError * _Nullable * _Nullable)error;
		[Static]
		[Export ("isSupportedWithSimulated:error:")]
		bool IsSupportedWithSimulated (bool simulated, [NullAllowed] out NSError error);

		// +(void)discoverAvailableReaderIdentifiersWithCompletion:(void (^ _Nonnull)(NSSet<NSString *> * _Nullable, NSError * _Nullable))completion;
		[Static]
		[Export ("discoverAvailableReaderIdentifiersWithCompletion:")]
		void DiscoverAvailableReaderIdentifiersWithCompletion (Action<NSSet<NSString>, NSError> completion);

		// -(instancetype _Nonnull)initWithReaderIdentifier:(NSString * _Nonnull)crid connectionConfiguration:(SCPLocalMobileConnectionConfiguration * _Nonnull)connectionConfiguration isSimulated:(BOOL)simulated __attribute__((objc_designated_initializer));
		[Export ("initWithReaderIdentifier:connectionConfiguration:isSimulated:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string crid, SCPLocalMobileConnectionConfiguration connectionConfiguration, bool simulated);

		// @property (readonly, copy, nonatomic) NSString * _Nonnull readerIdentifier;
		[Export ("readerIdentifier")]
		string ReaderIdentifier { get; }

		// @property (readonly, nonatomic) BOOL isSimulated;
		[Export ("isSimulated")]
		bool IsSimulated { get; }

		// @property (readonly, nonatomic, strong) SCPLocalMobileConnectionConfiguration * _Nonnull connectionConfiguration;
		[Export ("connectionConfiguration", ArgumentSemantic.Strong)]
		SCPLocalMobileConnectionConfiguration ConnectionConfiguration { get; }

		// @property (copy, nonatomic) NSString * _Nullable merchantReference;
		[NullAllowed, Export ("merchantReference")]
		string MerchantReference { get; set; }

		[Wrap ("WeakAccountLinkingDelegate")]
		[NullAllowed]
		SCPAppleBuiltInReaderAccountLinkingDelegate AccountLinkingDelegate { get; set; }

		// @property (nonatomic, weak) id<SCPAppleBuiltInReaderAccountLinkingDelegate> _Nullable accountLinkingDelegate;
		[NullAllowed, Export ("accountLinkingDelegate", ArgumentSemantic.Weak)]
		NSObject WeakAccountLinkingDelegate { get; set; }

		[Wrap ("WeakPrepareDelegate")]
		[NullAllowed]
		SCPAppleBuiltInReaderPrepareDelegate PrepareDelegate { get; set; }

		// @property (nonatomic, weak) id<SCPAppleBuiltInReaderPrepareDelegate> _Nullable prepareDelegate;
		[NullAllowed, Export ("prepareDelegate", ArgumentSemantic.Weak)]
		NSObject WeakPrepareDelegate { get; set; }

		[Wrap ("WeakTransactionDelegate")]
		[NullAllowed]
		SCPAppleBuiltInReaderTransactionDelegate TransactionDelegate { get; set; }

		// @property (nonatomic, weak) id<SCPAppleBuiltInReaderTransactionDelegate> _Nullable transactionDelegate;
		[NullAllowed, Export ("transactionDelegate", ArgumentSemantic.Weak)]
		NSObject WeakTransactionDelegate { get; set; }

		// -(BOOL)linkAccountUsingToken:(NSString * _Nonnull)token merchantReference:(NSString * _Nonnull)merchantReference error:(NSError * _Nullable * _Nullable)error;
		[Export ("linkAccountUsingToken:merchantReference:error:")]
		bool LinkAccountUsingToken (string token, string merchantReference, [NullAllowed] out NSError error);

		// -(BOOL)prepareUsingToken:(NSString * _Nonnull)token merchantReference:(NSString * _Nonnull)merchantReference error:(NSError * _Nullable * _Nullable)error;
		[Export ("prepareUsingToken:merchantReference:error:")]
		bool PrepareUsingToken (string token, string merchantReference, [NullAllowed] out NSError error);

		// -(BOOL)cancelTransactionAndReturnError:(NSError * _Nullable * _Nullable)error;
		[Export ("cancelTransactionAndReturnError:")]
		bool CancelTransactionAndReturnError ([NullAllowed] out NSError error);

		// -(BOOL)performTransactionWithType:(enum SCPAppleBuiltInReaderTransactionType)transactionType amount:(NSDecimalNumber * _Nullable)amount currencyCode:(NSString * _Nonnull)currencyCode error:(NSError * _Nullable * _Nullable)error;
		[Export ("performTransactionWithType:amount:currencyCode:error:")]
		bool PerformTransactionWithType (SCPAppleBuiltInReaderTransactionType transactionType, [NullAllowed] NSDecimalNumber amount, string currencyCode, [NullAllowed] out NSError error);
	
		// -(BOOL)capturePINUsingToken:(NSString * _Nonnull)token cardReaderTransactionID:(NSString * _Nonnull)id error:(NSError * _Nullable * _Nullable)error;
		[Export ("capturePINUsingToken:cardReaderTransactionID:error:")]
		bool CapturePINUsingToken (string token, string id, [NullAllowed] out NSError error);

		// -(BOOL)performMockTransactionWithType:(enum SCPAppleBuiltInReaderTransactionType)transactionType amount:(NSDecimalNumber * _Nullable)amount currencyCode:(NSString * _Nonnull)currencyCode error:(NSError * _Nullable * _Nullable)error;
		[Export ("performMockTransactionWithType:amount:currencyCode:error:")]
		bool PerformMockTransactionWithType (SCPAppleBuiltInReaderTransactionType transactionType, [NullAllowed] NSDecimalNumber amount, string currencyCode, [NullAllowed] out NSError error);
	}
	*/
    // @protocol SCPAppleBuiltInReaderAccountLinkingDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPAppleBuiltInReaderAccountLinkingDelegate
	{
		// @required -(void)appleBuiltInReaderDidLinkAccount:(id<SCPAppleBuiltInReader> _Nonnull)reader;
		[Abstract]
		[Export ("appleBuiltInReaderDidLinkAccount:")]
		void AppleBuiltInReaderDidLinkAccount (SCPAppleBuiltInReader reader);

		// @required -(void)appleBuiltInReaderDidPreviouslyLinkAccount:(id<SCPAppleBuiltInReader> _Nonnull)reader;
		[Abstract]
		[Export ("appleBuiltInReaderDidPreviouslyLinkAccount:")]
		void AppleBuiltInReaderDidPreviouslyLinkAccount (SCPAppleBuiltInReader reader);

		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didFailToLinkAccountWithError:(NSError * _Nonnull)error;
		[Abstract]
		[Export ("appleBuiltInReader:didFailToLinkAccountWithError:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, NSError error);
	}

    // @protocol SCPAppleBuiltInReaderPrepareDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPAppleBuiltInReaderPrepareDelegate
	{
		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didReportPrepareProgress:(float)progress;
		[Abstract]
		[Export ("appleBuiltInReader:didReportPrepareProgress:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, float progress);

		// @required -(void)appleBuiltInReaderDidPrepare:(id<SCPAppleBuiltInReader> _Nonnull)reader;
		[Abstract]
		[Export ("appleBuiltInReaderDidPrepare:")]
		void AppleBuiltInReaderDidPrepare (SCPAppleBuiltInReader reader);

		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didFailToPrepareWithError:(NSError * _Nonnull)error;
		[Abstract]
		[Export ("appleBuiltInReader:didFailToPrepareWithError:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, NSError error);
	}

    // @protocol SCPAppleBuiltInReaderTransactionDelegate <NSObject>
    [Protocol, Model]
    [BaseType (typeof(NSObject))]
	interface SCPAppleBuiltInReaderTransactionDelegate
	{
		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didCollectPaymentCard:(NSString * _Nullable)data paymentCardId:(NSString * _Nonnull)paymentCardId merchantReference:(NSString * _Nonnull)merchantReference forTransactionOfType:(enum SCPAppleBuiltInReaderTransactionType)type amount:(NSDecimalNumber * _Nullable)amount currencyCode:(NSString * _Nonnull)currencyCode;
		[Abstract]
		[Export ("appleBuiltInReader:didCollectPaymentCard:paymentCardId:merchantReference:forTransactionOfType:amount:currencyCode:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, [NullAllowed] string data, string paymentCardId, string merchantReference, SCPAppleBuiltInReaderTransactionType type, [NullAllowed] NSDecimalNumber amount, string currencyCode);

		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didFailToPerformTransactionOfType:(enum SCPAppleBuiltInReaderTransactionType)type error:(NSError * _Nonnull)error;
		[Abstract]
		[Export ("appleBuiltInReader:didFailToPerformTransactionOfType:error:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, SCPAppleBuiltInReaderTransactionType type, NSError error);

		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didReportTransactionEvent:(SCPAppleBuiltInReaderTransactionEvent * _Nonnull)event;
		[Abstract]
		[Export ("appleBuiltInReader:didReportTransactionEvent:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, SCPAppleBuiltInReaderTransactionEvent @event);

		// @required -(void)appleBuiltInReader:(id<SCPAppleBuiltInReader> _Nonnull)reader didFailToCancelTransaction:(NSError * _Nonnull)error;
		[Abstract]
		[Export ("appleBuiltInReader:didFailToCancelTransaction:")]
		void AppleBuiltInReader (SCPAppleBuiltInReader reader, NSError error);

		// @required -(void)appleBuiltInReaderDidCompleteMockTransaction:(id<SCPAppleBuiltInReader> _Nonnull)reader;
		[Abstract]
		[Export ("appleBuiltInReaderDidCompleteMockTransaction:")]
		void AppleBuiltInReaderDidCompleteMockTransaction (SCPAppleBuiltInReader reader);
	}

	// @interface SCPAppleBuiltInReaderTransactionEvent : NSObject
	[BaseType (typeof(NSObject))]
	interface SCPAppleBuiltInReaderTransactionEvent
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; }

		// @property (readonly, nonatomic) enum SCPAppleBuiltInReaderTransactionEventCode code;
		[Export ("code")]
		SCPAppleBuiltInReaderTransactionEventCode Code { get; }

		// -(BOOL)isEqualToTransactionEvent:(SCPAppleBuiltInReaderTransactionEvent * _Nonnull)other __attribute__((warn_unused_result("")));
		[Export ("isEqualToTransactionEvent:")]
		bool IsEqualToTransactionEvent (SCPAppleBuiltInReaderTransactionEvent other);

		// @property (readonly, nonatomic) NSUInteger hash;
		[Export ("hash")]
		nuint Hash { get; }

		// -(BOOL)isEqual:(id _Nullable)object __attribute__((warn_unused_result("")));
		[Export ("isEqual:")]
		bool IsEqual ([NullAllowed] NSObject @object);

		// -(id _Nonnull)copy __attribute__((warn_unused_result("")));
		[Export("copy")]
		NSObject Copy();
	}

	// @interface StripeTerminal_Swift_684 (NSError)
	[Category]
	[BaseType (typeof(NSError))]
	interface NSError_StripeTerminal
	{
		// @property (readonly, copy, nonatomic, class) NSString * _Nonnull scp_appleBuiltInReaderErrorDomain;
		[Static]
		[Export ("scp_appleBuiltInReaderErrorDomain")]
		string Scp_appleBuiltInReaderErrorDomain { get; }

		// @property (readonly, copy, nonatomic, class) NSString * _Nonnull scp_appleBuiltInReaderErrorUserInfoNameKey;
		[Static]
		[Export ("scp_appleBuiltInReaderErrorUserInfoNameKey")]
		string Scp_appleBuiltInReaderErrorUserInfoNameKey { get; }

		// @property (readonly, copy, nonatomic, class) NSString * _Nonnull scp_appleBuiltInReaderErrorUserInfoReaderMessageKey;
		[Static]
		[Export ("scp_appleBuiltInReaderErrorUserInfoReaderMessageKey")]
		string Scp_appleBuiltInReaderErrorUserInfoReaderMessageKey { get; }

		// @property (readonly, copy, nonatomic, class) NSString * _Nonnull scp_appleBuiltInReaderErrorUserInfoDeviceBannedUntilDateKey;
		[Static]
		[Export ("scp_appleBuiltInReaderErrorUserInfoDeviceBannedUntilDateKey")]
		string Scp_appleBuiltInReaderErrorUserInfoDeviceBannedUntilDateKey { get; }

        // @property (readonly, copy, nonatomic, class) NSString * _Nonnull scp_appleBuiltInReaderErrorUserInfoPrepareFailedReasonKey;
        [Static]
        [Export("scp_appleBuiltInReaderErrorUserInfoPrepareFailedReasonKey")]
        string Scp_appleBuiltInReaderErrorUserInfoPrepareFailedReasonKey { get; }

        // +(NSError * _Nonnull)scp_unknownAppleBuiltInReaderError __attribute__((warn_unused_result("")));
        [Static]
		[Export ("scp_unknownAppleBuiltInReaderError")]
		NSError Scp_unknownAppleBuiltInReaderError { get; }

		// +(NSError * _Nonnull)scp_invalidAmountError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_invalidAmountError")]
		NSError Scp_invalidAmountError { get; }

		// +(NSError * _Nonnull)scp_invalidCurrencyError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_invalidCurrencyError")]
		NSError Scp_invalidCurrencyError { get; }

		// +(NSError * _Nonnull)scp_invalidTransactionTypeError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_invalidTransactionTypeError")]
		NSError Scp_invalidTransactionTypeError { get; }

		// +(NSError * _Nonnull)scp_osVersionNotSupportedError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_osVersionNotSupportedError")]
		NSError Scp_osVersionNotSupportedError { get; }

		// +(NSError * _Nonnull)scp_modelNotSupportedError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_modelNotSupportedError")]
		NSError Scp_modelNotSupportedError { get; }

		// +(NSError * _Nonnull)scp_readerNotReadyError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_readerNotReadyError")]
		NSError Scp_readerNotReadyError { get; }

		// +(NSError * _Nonnull)scp_unexpectedNilError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_unexpectedNilError")]
		NSError Scp_unexpectedNilError { get; }

		// +(NSError * _Nonnull)scp_readCanceledError __attribute__((warn_unused_result("")));
		[Static]
		[Export ("scp_readCanceledError")]
		NSError Scp_readCanceledError { get; }

		// @property (readonly, nonatomic) BOOL scp_isAppleBuiltInReaderError;
		[Static]
		[Export ("scp_isAppleBuiltInReaderError")]
		bool Scp_isAppleBuiltInReaderError { get; }
	}
}
