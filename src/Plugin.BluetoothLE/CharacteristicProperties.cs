using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.BluetoothLE
{
  [Flags]
  public enum CharacteristicProperties
  {
    /// <summary>Characteristic value can be broadcasted.</summary>
    Broadcast = 1,                    // 0x1
    /// <summary>Characteristic value can be read.</summary>
    Read = 2,                         // 0x2
    /// <summary>Characteristic value can be written without response.</summary>
    WriteWithoutResponse = 4,         // 0x4
    /// <summary>Characteristic can be written with response.</summary>
    Write = 8,                        // 0x8
    /// <summary>Characteristic can notify value changes without acknowledgment.</summary>
    Notify = 16,                      // 0x10
    /// <summary>Characteristic can indicate value changes with acknowledgment.</summary>
    Indicate = 32,                    // 0x20
    /// <summary>Characteristic value can be written signed.</summary>
    AuthenticatedSignedWrites = 64,   // 0x40
    /// <summary>Indicates that more properties are set in the extended properties descriptor.</summary>
    ExtendedProperties = 128,         // 0x80
    /// <summary>IOS SPECIFIC.</summary>
    NotifyEncryptionRequired = 256,   // 0x100
    /// <summary>IOS SPECIFIC.</summary>
    IndicateEncryptionRequired = 512, // 0x200

    // Android
    //		Broadcast = 1,            // 0x1
    //		Read = 2,                 // 0x2
    //		Write = 8,                // 0x8
    //		Notify = 16,              // 0x10
    //		Indicate = 32,            // 0x20
    //		SignedWrite = 64,         // 0x40
    //		ExtendedProperties = 128, // 0x80
    //
    //iOS
    //		Broadcast = 0x01,
    //		Read = 0x02,
    //		WriteWithoutResponse = 0x04,
    //		PropertyWrite = 0x08,
    //		Notify = 0x10,
    //		Indicate = 0x20,
    //		AuthenticatedSignedWrites = 0x40,
    //		ExtendedProperties = 0x80,
    //		NotifyEncryptionRequired = 0x100,
    //		IndicateEncryptionRequired = 0x200,

  }
}