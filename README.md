# Stripe SDK Binding
Maui Binding for Stripe's SDK
https://github.com/stripe

## Stripe Terminal SDK Binding
### iOS
https://stripe.com/docs/terminal
https://github.com/stripe/stripe-terminal-ios

The Stripe Terminal iOS SDK is compatible with apps supporting iOS 11 and above.

#### Info.plist
Add the correct permissions:

    <key>NSLocationWhenInUseUsageDescription</key>
    <string>Location access is required in order to accept payments.</string>

    <key>NSBluetoothPeripheralUsageDescription</key>
    <string>Bluetooth access is required in order to connect to supported bluetooth card readers.</string>

    <key>NSBluetoothAlwaysUsageDescription</key>
    <string>This app uses Bluetooth to connect to supported card readers.</string>

Add background modes (optional):

    <key>UIBackgroundModes</key>
    <array>
      <string>bluetooth-central</string>
    </array>