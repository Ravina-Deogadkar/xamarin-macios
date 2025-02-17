//
// PushToTalk C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2022 Microsoft Corporation All rights reserved.
//

using System;

using AVFoundation;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using UIKit;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace PushToTalk {

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PTChannelJoinReason : long {
		DeveloperRequest = 0,
		ChannelRestoration = 1,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PTChannelLeaveReason : long {
		Unknown = 0,
		UserRequest = 1,
		DeveloperRequest = 2,
		SystemPolicy = 3,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PTChannelTransmitRequestSource : long {
		Unknown = 0,
		UserRequest = 1,
		DeveloperRequest = 2,
		HandsfreeButton = 3,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PTServiceStatus : long {
		Ready,
		Connecting,
		Unavailable,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum PTTransmissionMode : long {
		FullDuplex,
		HalfDuplex,
		ListenOnly,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	[ErrorDomain ("PTInstantiationErrorDomain")]
	public enum PTInstantiationError : long {
		Unknown = 0,
		InvalidPlatform = 1,
		MissingBackgroundMode = 2,
		MissingPushServerEnvironment = 3,
		MissingEntitlement = 4,
		InstantiationAlreadyInProgress = 5,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	[ErrorDomain ("PTChannelErrorDomain")]
	public enum PTChannelError : long {
		Unknown = 0,
		ChannelNotFound = 1,
		ChannelLimitReached = 2,
		CallActive = 3,
		TransmissionInProgress = 4,
		TransmissionNotFound = 5,
		AppNotForeground = 6,
		DeviceManagementRestriction = 7,
		ScreenTimeRestriction = 8,
		TransmissionNotAllowed = 9,
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PTParticipant {
		[Export ("name")]
		string Name { get; }

		[NullAllowed, Export ("image", ArgumentSemantic.Copy)]
		UIImage Image { get; }

		[Export ("initWithName:image:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, [NullAllowed] UIImage image);
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PTPushResult {
		[Static]
		[Export ("leaveChannelPushResult")]
		PTPushResult LeaveChannelPushResult { get; }

		[Static]
		[Export ("pushResultForActiveRemoteParticipant:")]
		PTPushResult Create (PTParticipant participant);
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PTChannelDescriptor {
		[Export ("initWithName:image:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string name, [NullAllowed] UIImage image);

		[Export ("name")]
		string Name { get; }

		[NullAllowed, Export ("image", ArgumentSemantic.Copy)]
		UIImage Image { get; }
	}

	interface IPTChannelManagerDelegate { }

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	[BaseType (typeof (NSObject))]
	interface PTChannelManagerDelegate {
		[Abstract]
		[Export ("channelManager:didJoinChannelWithUUID:reason:")]
		void DidJoinChannel (PTChannelManager channelManager, NSUuid channelUuid, PTChannelJoinReason reason);

		[Abstract]
		[Export ("channelManager:didLeaveChannelWithUUID:reason:")]
		void DidLeaveChannel (PTChannelManager channelManager, NSUuid channelUuid, PTChannelLeaveReason reason);

		[Abstract]
		[Export ("channelManager:channelUUID:didBeginTransmittingFromSource:")]
		void DidBeginTransmitting (PTChannelManager channelManager, NSUuid channelUuid, PTChannelTransmitRequestSource source);

		[Abstract]
		[Export ("channelManager:channelUUID:didEndTransmittingFromSource:")]
		void DidEndTransmitting (PTChannelManager channelManager, NSUuid channelUuid, PTChannelTransmitRequestSource source);

		[Abstract]
		[Export ("channelManager:receivedEphemeralPushToken:")]
		void ReceivedEphemeralPushToken (PTChannelManager channelManager, NSData pushToken);

		[Abstract]
		[Export ("incomingPushResultForChannelManager:channelUUID:pushPayload:")]
		PTPushResult IncomingPushResult (PTChannelManager channelManager, NSUuid channelUuid, NSDictionary<NSString, NSObject> pushPayload);

		[NoMac]
		[Abstract]
		[Export ("channelManager:didActivateAudioSession:")]
		void DidActivateAudioSession (PTChannelManager channelManager, AVAudioSession audioSession);

		[NoMac]
		[Abstract]
		[Export ("channelManager:didDeactivateAudioSession:")]
		void DidDeactivateAudioSession (PTChannelManager channelManager, AVAudioSession audioSession);

		[Export ("channelManager:failedToJoinChannelWithUUID:error:")]
		void FailedToJoinChannel (PTChannelManager channelManager, NSUuid channelUuid, NSError error);

		[Export ("channelManager:failedToLeaveChannelWithUUID:error:")]
		void FailedToLeaveChannel (PTChannelManager channelManager, NSUuid channelUuid, NSError error);

		[Export ("channelManager:failedToBeginTransmittingInChannelWithUUID:error:")]
		void FailedToBeginTransmittingInChannel (PTChannelManager channelManager, NSUuid channelUuid, NSError error);

		[Export ("channelManager:failedToStopTransmittingInChannelWithUUID:error:")]
		void FailedToStopTransmittingInChannel (PTChannelManager channelManager, NSUuid channelUuid, NSError error);
	}

	interface IPTChannelRestorationDelegate { }

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	[BaseType (typeof (NSObject))]
	interface PTChannelRestorationDelegate {
		[Abstract]
		[Export ("channelDescriptorForRestoredChannelUUID:")]
		PTChannelDescriptor Create (NSUuid channelUuid);
	}

	[NoWatch, NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PTChannelManager {
		[Async]
		[Static]
		[Export ("channelManagerWithDelegate:restorationDelegate:completionHandler:")]
		void Create (IPTChannelManagerDelegate @delegate, IPTChannelRestorationDelegate restorationDelegate, Action<PTChannelManager, NSError> completionHandler);

		[NullAllowed, Export ("activeChannelUUID", ArgumentSemantic.Strong)]
		NSUuid ActiveChannelUuid { get; }

		[Export ("requestJoinChannelWithUUID:descriptor:")]
		void RequestJoinChannel (NSUuid channelUuid, PTChannelDescriptor descriptor);

		[Export ("requestBeginTransmittingWithChannelUUID:")]
		void RequestBeginTransmitting (NSUuid channelUuid);

		[Export ("stopTransmittingWithChannelUUID:")]
		void StopTransmitting (NSUuid channelUuid);

		[Export ("leaveChannelWithUUID:")]
		void LeaveChannel (NSUuid channelUuid);

		[Async]
		[Export ("setChannelDescriptor:forChannelUUID:completionHandler:")]
		void SetChannelDescriptor (PTChannelDescriptor channelDescriptor, NSUuid channelUuid, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("setActiveRemoteParticipant:forChannelUUID:completionHandler:")]
		void SetActiveRemoteParticipant ([NullAllowed] PTParticipant participant, NSUuid channelUuid, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("setServiceStatus:forChannelUUID:completionHandler:")]
		void SetServiceStatus (PTServiceStatus status, NSUuid channelUuid, [NullAllowed] Action<NSError> completionHandler);

		[Async]
		[Export ("setTransmissionMode:forChannelUUID:completionHandler:")]
		void SetTransmissionMode (PTTransmissionMode transmissionMode, NSUuid channelUuid, [NullAllowed] Action<NSError> completionHandler);
	}

}
