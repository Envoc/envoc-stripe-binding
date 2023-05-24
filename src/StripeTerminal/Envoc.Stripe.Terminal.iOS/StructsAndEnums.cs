using System;
using ObjCRuntime;

namespace StripeTerminal
{
	[Native]
	public enum SCPBatteryStatus : ulong
	{
		Unknown,
		Critical,
		Low,
		Nominal
	}

	[Native]
	public enum SCPReaderDisplayMessage : ulong
	{
		RetryCard,
		InsertCard,
		InsertOrSwipeCard,
		SwipeCard,
		RemoveCard,
		MultipleContactlessCardsDetected,
		TryAnotherReadMethod,
		TryAnotherCard
	}

	[Native]
	public enum SCPReaderEvent : ulong
	{
		Inserted,
		Removed
	}

	[Flags]
	[Native]
	public enum SCPReaderInputOptions : ulong
	{
		None = 0x0,
		SwipeCard = 1uL << 0,
		InsertCard = 1uL << 1,
		TapCard = 1uL << 2
	}

	[Native]
	public enum SCPCardBrand : long
	{
		Visa,
		Amex,
		MasterCard,
		Discover,
		Jcb,
		DinersClub,
		Interac,
		UnionPay,
		EftposAu,
		Unknown
	}

	[Native]
	public enum SCPConnectionStatus : ulong
	{
		NotConnected,
		Connected,
		Connecting
	}

	[Native]
	public enum SCPDeviceType : ulong
	{
		Chipper2X,
		VerifoneP400,
		WisePad3,
		StripeM2,
		WisePosE,
		WisePosEDevKit,
		Etna,
		Chipper1X,
		WiseCube,
		StripeS700,
		StripeS700DevKit,
		AppleBuiltIn
	}

	[Native]
	public enum SCPDiscoveryMethod : ulong
	{
		BluetoothScan,
		BluetoothProximity,
		Internet,
		LocalMobile
	}

	[Native]
	public enum SCPLogLevel : ulong
	{
		None,
		Verbose
	}

	[Native]
	public enum SCPCardPresentCaptureMethod : ulong
	{
		SCPCardPresentCaptureMethodManualPreferred
	}

	[Native]
	public enum SCPCardPresentRouting : ulong
	{
		Domestic,
		International
	}

	[Native]
	public enum SCPCaptureMethod : ulong
	{
		Manual,
		Automatic
	}

	[Native]
	public enum SCPPaymentStatus : ulong
	{
		NotReady,
		Ready,
		WaitingForInput,
		Processing
	}

	[Native]
	public enum SCPPaymentIntentStatus : ulong
	{
		RequiresPaymentMethod,
		RequiresConfirmation,
		RequiresCapture,
		Processing,
		Canceled,
		Succeeded
	}

	[Native]
	public enum SCPSimulateReaderUpdate : ulong
	{
		Available = 0,
		None,
		Required,
		LowBattery,
		Random
	}

	[Native]
	public enum SCPSimulatedCardType : ulong
	{
		Visa = 0,
		VisaDebit,
		Mastercard,
		MasterDebit,
		MastercardPrepaid,
		Amex,
		Amex2,
		Discover,
		Discover2,
		Diners,
		Diners14Digit,
		Jcb,
		UnionPay,
		Interac,
		EftposAuDebit,
		EftposAuVisaDebit,
		EftposAuDebitMastercard,
		ChargeDeclined,
		ChargeDeclinedInsufficientFunds,
		ChargeDeclinedLostCard,
		ChargeDeclinedStolenCard,
		ChargeDeclinedExpiredCard,
		ChargeDeclinedProcessingError,
		RefundFailed,
		OnlinePinCvm,
		OnlinePinScaRetry,
		OfflinePinCvm,
		OfflinePinScaRetry
	}

	[Native]
	public enum SCPCardFundingType : long
	{
		Debit,
		Credit,
		Prepaid,
		Other
	}

	[Native]
	public enum SCPIncrementalAuthorizationStatus : ulong
	{
		Unknown,
		NotSupported,
		Supported
	}

	[Native]
	public enum SCPChargeStatus : ulong
	{
		Succeeded,
		Pending,
		Failed
	}

	[Native]
	public enum SCPError : long
	{
		Busy = 1000,
		CancelFailedAlreadyCompleted = 1010,
		NotConnectedToReader = 1100,
		AlreadyConnectedToReader = 1110,
		ConnectionTokenProviderCompletedWithNothing = 1510,
		ProcessInvalidPaymentIntent = 1530,
		NilPaymentIntent = 1540,
		NilSetupIntent = 1542,
		NilRefundPaymentMethod = 1550,
		InvalidRefundParameters = 1555,
		InvalidClientSecret = 1560,
		MustBeDiscoveringToConnect = 1570,
		CannotConnectToUndiscoveredReader = 1580,
		InvalidDiscoveryConfiguration = 1590,
		InvalidReaderForUpdate = 1861,
		UnsupportedSDK = 1870,
		FeatureNotAvailableWithConnectedReader = 1880,
		FeatureNotAvailable = 1890,
		InvalidListLocationsLimitParameter = 1900,
		BluetoothConnectionInvalidLocationIdParameter = 1910,
		InvalidRequiredParameter = 1920,
		ReaderConnectionConfigurationInvalid = 1940,
		ReaderTippingParameterInvalid = 1950,
		InvalidLocationIdParameter = 1960,
		Canceled = 2020,
		LocationServicesDisabled = 2200,
		BluetoothDisabled = 2320,
		BluetoothAccessDenied = 2321,
		BluetoothScanTimedOut = 2330,
		BluetoothLowEnergyUnsupported = 2340,
		ReaderSoftwareUpdateFailedBatteryLow = 2650,
		ReaderSoftwareUpdateFailedInterrupted = 2660,
		ReaderSoftwareUpdateFailedExpiredUpdate = 2670,
		BluetoothConnectionFailedBatteryCriticallyLow = 2680,
		CardInsertNotRead = 2810,
		CardSwipeNotRead = 2820,
		CardReadTimedOut = 2830,
		CardRemoved = 2840,
		CardLeftInReader = 2850,
		MissingEMVData = 2892,
		CommandNotAllowed = 2900,
		UnsupportedMobileDeviceConfiguration = 2910,
		PasscodeNotEnabled = 2920,
		CommandNotAllowedDuringCall = 2930,
		InvalidAmount = 2940,
		InvalidCurrency = 2950,
		AppleBuiltInReaderTOSAcceptanceRequiresiCloudSignIn = 2960,
		AppleBuiltInReaderTOSAcceptanceCanceled = 2970,
		ReaderBusy = 3010,
		IncompatibleReader = 3030,
		ReaderCommunicationError = 3060,
		NFCDisabled = 3100,
		BluetoothError = 3200,
		BluetoothConnectTimedOut = 3210,
		BluetoothDisconnected = 3230,
		BluetoothPeerRemovedPairingInformation = 3240,
		BluetoothAlreadyPairedWithAnotherDevice = 3241,
		ReaderSoftwareUpdateFailed = 3800,
		ReaderSoftwareUpdateFailedReaderError = 3830,
		ReaderSoftwareUpdateFailedServerError = 3840,
		UnsupportedReaderVersion = 3850,
		UnknownReaderIpAddress = 3860,
		InternetConnectTimeOut = 3870,
		ConnectFailedReaderIsInUse = 3880,
		BluetoothReconnectStarted = 3890,
		ReaderNotAccessibleInBackground = 3900,
		AppleBuiltInReaderFailedToPrepare = 3910,
		AppleBuiltInReaderDeviceBanned = 3920,
		AppleBuiltInReaderTOSNotYetAccepted = 3930,
		AppleBuiltInReaderTOSAcceptanceFailed = 3940,
		AppleBuiltInReaderMerchantBlocked = 3950,
		AppleBuiltInReaderInvalidMerchant = 3960,
		UnexpectedSdkError = 5000,
		UnexpectedReaderError = 5001,
		DeclinedByStripeAPI = 6000,
		DeclinedByReader = 6500,
		CommandRequiresCardholderConsent = 6700,
		RefundFailed = 6800,
		NotConnectedToInternet = 9000,
		RequestTimedOut = 9010,
		StripeAPIError = 9020,
		StripeAPIResponseDecodingError = 9030,
		InternalNetworkError = 9040,
		ConnectionTokenProviderCompletedWithError = 9050,
		SessionExpired = 9060
	}

	[Native]
	public enum SCPLocationStatus : ulong
	{
		Unknown,
		Set,
		NotSet
	}

	[Native]
	public enum SCPPaymentMethodType : ulong
	{
		Card,
		CardPresent,
		InteracPresent,
		Unknown
	}

	[Native]
	public enum SCPReaderNetworkStatus : ulong
	{
		Offline,
		Online
	}

	[Native]
	public enum SCPUpdateTimeEstimate : ulong
	{
		LessThan1Minute,
		SCPUpdateTimeEstimate1To2Minutes,
		SCPUpdateTimeEstimate2To5Minutes,
		SCPUpdateTimeEstimate5To15Minutes
	}

	[Flags]
	[Native]
	public enum SCPUpdateComponent : ulong
	{
		Incremental = 1uL << 0,
		Firmware = 1uL << 1,
		Config = 1uL << 2,
		Keys = 1uL << 3
	}

	[Native]
	public enum SCPRefundStatus : ulong
	{
		Succeeded,
		Pending,
		Failed,
		Unknown
	}

	[Native]
	public enum SCPSetupIntentStatus : ulong
	{
		RequiresPaymentMethod,
		RequiresConfirmation,
		RequiresAction,
		Processing,
		Canceled,
		Succeeded
	}

	[Native]
	public enum SCPSetupIntentUsage : ulong
	{
		ffSession,
		nSession
	}

	[Native]
	public enum SCPAppleBuiltInReaderErrorCode : long
	{
		Unknown = 0,
		UnexpectedNil = 1,
		InvalidTransactionType = 2,
		PasscodeDisabled = 3,
		NotAllowed = 4,
		BackgroundRequestNotAllowed = 5,
		Unsupported = 6,
		OsVersionNotSupported = 7,
		ModelNotSupported = 8,
		NetworkError = 9,
		NetworkAuthenticationError = 10,
		ServiceConnectionError = 11,
		NotReady = 12,
		EmptyReaderToken = 13,
		InvalidReaderToken = 14,
		PrepareFailed = 15,
		PrepareExpired = 16,
		TokenExpired = 17,
		DeviceBanned = 18,
		ReaderMemoryFull = 19,
		ReaderBusy = 20,
		AccountNotLinked = 21,
		AccountLinkingFailed = 22,
		AccountLinkingRequiresiCloudSignIn = 23,
		AccountLinkingCancelled = 24,
		MerchantBlocked = 25,
		InvalidMerchant = 26,
		ReadNotAllowed = 27,
		ReadFromBackgroundError = 28,
		ReaderServiceConnectionError = 29,
		ReaderServiceError = 30,
		NoReaderSession = 31,
		ReaderSessionExpired = 32,
		ReaderTokenExpired = 33,
		ReaderSessionNetworkError = 34,
		ReaderSessionAuthenticationError = 35,
		ReaderSessionBusy = 36,
		ReadCancelled = 37,
		InvalidAmount = 38,
		InvalidCurrency = 39,
		NfcDisabled = 40,
		ReadNotAllowedDuringCall = 41,
		CardReadFailed = 42,
		PaymentReadFailed = 43,
		PaymentCardDeclined = 44
	}

	[Native]
	public enum SCPAppleBuiltInReaderTransactionEventCode : long
	{
		Unknown = 0,
		ReadyForTap = 1,
		CardDetected = 2,
		RemoveCard = 3,
		Completed = 4,
		Retry = 5,
		ReadCanceled = 6,
		ReadNotCompleted = 7
	}

	[Native]
	public enum SCPAppleBuiltInReaderTransactionType : long
	{
		Unknown = 0,
		Purchase = 1,
		Refund = 2,
		Verification = 3
	}
}
