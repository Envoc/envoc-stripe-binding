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
        TryAnotherCard,
        CardRemovedTooEarly
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
    public enum SCPNetworkStatus : ulong
    {
        Unknown,
        Offline,
        Online
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
    public enum SCPReadMethod : ulong
    {
        Unknown = 0,
        ContactEMV = 5,
        ContactlessEMV = 7,
        MagneticStripeFallback = 80,
        MagneticStripeTrack2 = 90,
        ContactlessMagstripeMode = 91
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
        CancelFailedAlreadyCompleted = 1010,
        NotConnectedToReader = 1100,
        AlreadyConnectedToReader = 1110,
        ConnectionTokenProviderCompletedWithNothing = 1510,
        ConnectionTokenProviderCompletedWithNothingWhileForwarding = 1511,
        ConfirmInvalidPaymentIntent = 1530,
        NilPaymentIntent = 1540,
        NilSetupIntent = 1542,
        NilRefundPaymentMethod = 1550,
        InvalidRefundParameters = 1555,
        InvalidClientSecret = 1560,
        InvalidDiscoveryConfiguration = 1590,
        InvalidReaderForUpdate = 1861,
        UnsupportedSDK = 1870,
        FeatureNotAvailableWithConnectedReader = 1880,
        FeatureNotAvailable = 1890,
        InvalidListLocationsLimitParameter = 1900,
        BluetoothConnectionInvalidLocationIdParameter = 1910,
        InvalidRequiredParameter = 1920,
        InvalidRequiredParameterOnBehalfOf = 1921,
        AccountIdMismatchWhileForwarding = 1930,
        UpdatePaymentIntentUnavailableWhileOffline = 1935,
        UpdatePaymentIntentUnavailableWhileOfflineModeEnabled = 1936,
        ForwardingTestModePaymentInLiveMode = 1937,
        ForwardingLiveModePaymentInTestMode = 1938,
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
        OfflinePaymentsDatabaseTooLarge = 2860,
        ReaderConnectionNotAvailableOffline = 2870,
        ReaderConnectionOfflineLocationMismatch = 2871,
        ReaderConnectionOfflineNeedsUpdate = 2872,
        NoLastSeenAccount = 2880,
        AmountExceedsMaxOfflineAmount = 2890,
        InvalidOfflineCurrency = 2891,
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
        EncryptionKeyFailure = 5002,
        EncryptionKeyStillInitializing = 5003,
        DeclinedByStripeAPI = 6000,
        DeclinedByReader = 6500,
        CommandRequiresCardholderConsent = 6700,
        RefundFailed = 6800,
        CardSwipeNotAvailable = 6900,
        InteracNotSupportedOffline = 6901,
        OfflineAndCardExpired = 6902,
        OfflineTransactionDeclined = 6903,
        OfflineCollectAndConfirmMismatch = 6904,
        OnlinePinNotSupportedOffline = 6905,
        OfflineTestCardInLivemode = 6906,
        NotConnectedToInternet = 9000,
        RequestTimedOut = 9010,
        StripeAPIError = 9020,
        StripeAPIResponseDecodingError = 9030,
        InternalNetworkError = 9040,
        ConnectionTokenProviderCompletedWithError = 9050,
        ConnectionTokenProviderCompletedWithErrorWhileForwarding = 9051,
        ConnectionTokenProviderTimedOut = 9052,
        SessionExpired = 9060,
        NotConnectedToInternetAndOfflineBehaviorRequireOnline = 10106,
        OfflineBehaviorForceOfflineWithFeatureDisabled = 10107
    }

    [Native]
    public enum SCPOfflineBehavior : long
    {
        PreferOnline,
        RequireOnline,
        ForceOffline
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
        Online,
        Unknown
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
		OffSession,
		OnSession
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
        AccountLinkingCheckFailed = 25,
        MerchantBlocked = 26,
        InvalidMerchant = 27,
        ReadNotAllowed = 28,
        ReadFromBackgroundError = 29,
        ReaderServiceConnectionError = 30,
        ReaderServiceError = 31,
        NoReaderSession = 32,
        ReaderSessionExpired = 33,
        ReaderTokenExpired = 34,
        ReaderSessionNetworkError = 35,
        ReaderSessionAuthenticationError = 36,
        ReaderSessionBusy = 37,
        ReadCancelled = 38,
        InvalidAmount = 39,
        InvalidCurrency = 40,
        NfcDisabled = 41,
        ReadNotAllowedDuringCall = 42,
        CardReadFailed = 43,
        PaymentReadFailed = 44,
        PaymentCardDeclined = 45,
        InvalidPreferredAID = 46,
        PinEntryFailed = 47,
        PinTokenInvalid = 48,
        PinEntryTimeout = 49,
        PinCancelled = 50,
        PinNotAllowed = 51
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
        ReadNotCompleted = 7,
        PinEntryRequested = 8,
        PinEntryCompleted = 9
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
