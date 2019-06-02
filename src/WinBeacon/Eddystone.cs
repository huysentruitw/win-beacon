/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace WinBeacon
{
    /// <summary>
    /// Abstract base class that represents an Eddystone Beacon.
    /// </summary>
    public abstract class Eddystone
    {
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-UID frames.
    /// </summary>
    public sealed class EddystoneUid : Eddystone
    {
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-URL frames.
    /// </summary>
    public sealed class EddystoneUrl : Eddystone
    {
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-TLM frames.
    /// </summary>
    public sealed class EddystoneTlm : Eddystone
    {
    }

    /// <summary>
    /// Class that represents an Eddystone Beacon sending Eddystone-EID frames.
    /// </summary>
    public sealed class EddystoneEid : Eddystone
    {
    }
}
