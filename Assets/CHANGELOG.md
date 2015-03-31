# Change Log
Change log for deltaDNA's Unity SDK.  The SDK should work with any recent 4.* version of Unity.

## 3.5 - 2015-03-05
### Added
* Integration for iOS push notifications.  The `NotificationsPlugin` can register a game for push notifications, and has events that trigger when the push token is acquired and notifications are received.  Integrates with the existing SDK to send a `notificationOpened` event automatically.
* An example scene to show off the platform's features.  This can be found under DeltaDNA/Example.

### Changed
* `DeltaDNA.SDK` has been deprecated in favour of `DeltaDNA.DDNA`.  This makes the code more readable and brings the naming inline with our other SDK's.  

### Fixed
* Help event store to recover if PlayerPrefs keys accidentally deleted.
* Event store dispose is more robust.
* Workaround for timestamps with > 999 milliseconds on Android devices.

## 3.4.3 - 2015-02-27
### Added
* You can return null from the timestamp function, which prevents using the device clock and relies on Collect injecting the timestamp when it receives the event.  This behaviour can be controlled with `UseCollectTimestamp`.  

## 3.4.2 - 2015-02-26
### Added
* The source of the event timestamp can be overridden with your own timestamp.  Use `SetTimestampFunc` if you want to use something other than `DateTime.UtcNow` for your event timestamps.

## 3.4.1 - 2015-02-25
### Fixed
* Webplayer build no longer calls System.IO.Directory which was causing a runtime exception.

## 3.4 - 2015-02-13
### Added
* Support for Windows Phone 8.1.

### Changed
* Added a separate logger with DEBUG, INFO, WARNING and ERROR levels.  Default level is WARNING.  Call `SetLoggingLevel` to change it.  The `DebugMode` flag now switches between WARNING and DEBUG.

## 3.3.3 - 2015-01-12
### Fixed
* Fixed regression introduced in v3.2 which could prevent some events being uploaded.

## 3.3.2 - 2015-01-09
### Fixed
* Minor change to address potential crash from an engage request.

## 3.3.1 - 2015-01-06
### Fixed
* EventStore is now threadsafe.
* Catch exception from `getOperatingSystemVersion`.

## 3.3 - 2014-12-15
### Added
* Added support for rich messaging.  Call `RequestImageMessage` to display a popup image from an engagement.  See Docs for details on how it works.
* Added support for Kindle Fire devices.

### Changed
* Moved our copy of MiniJSON inside DeltaDNA namespace to avoid namespace clashes.
* If Collect and/or Engage URL's don't start with 'http://' I silently prepend it.

### Fixed
* Minor change so works with Windows Phone 8 build.
* Tweaked ClientInfo to return better names for deviceName and deviceType.
* Improved how ClientInfo determines operating system version.


## 3.2.1 - 2014-10-14
### Changed
* No longer need to wait for an Engagement to finish before making another.

## 3.2 - 2014-10-07
### Added
* `StartSDK` can be called during a game with a different User ID.
* `StopSDK` method added, which sends a 'gameEnded' event before stopping background uploads.
* `NewSession` will generate a new session ID for subsequent events.

### Changed
* The `Init` method has be deprecated in favour of `StartSDK`, this will be removed in a future release.
* The `TriggerEvent` method has been deprecated for `RecordEvent`, this will be removed in a future release.