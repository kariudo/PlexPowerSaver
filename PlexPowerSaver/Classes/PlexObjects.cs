﻿using RestSharp.Deserializers;
using System.Collections.Generic;

namespace PlexPowerSaver
{
    public partial class MediaContainer
    {
        public byte size { get; set; }
        public Video video { get; set; }
    }

    public partial class Video
    {
        public uint addedAt { get; set; }
        public string art { get; set; }
        public string chapterSource { get; set; }
        public string contentRating { get; set; }
        public Director director { get; set; }
        public uint duration { get; set; }
        public string grandparentArt { get; set; }
        public string grandparentKey { get; set; }
        public ushort grandparentRatingKey { get; set; }
        public string grandparentThumb { get; set; }
        public string grandparentTitle { get; set; }
        public string guid { get; set; }
        public byte index { get; set; }
        public string key { get; set; }
        public uint lastViewedAt { get; set; }
        public byte librarySectionID { get; set; }
        public Media media { get; set; }

        public System.DateTime originallyAvailableAt { get; set; }
        public byte parentIndex { get; set; }
        public string parentKey { get; set; }
        public ushort parentRatingKey { get; set; }
        public string parentThumb { get; set; }
        public Player player { get; set; }
        public ushort ratingKey { get; set; }
        public byte sessionKey { get; set; }
        public string summary { get; set; }
        public string thumb { get; set; }
        public string title { get; set; }
        public string titleSort { get; set; }
        public TranscodeSession transcodeSession { get; set; }
        public string type { get; set; }
        public uint updatedAt { get; set; }
        public User user { get; set; }
        public uint viewOffset { get; set; }
        public Writer writer { get; set; }
        public ushort year { get; set; }
    }

    public partial class Director
    {
        public ushort id { get; set; }

        public string tag { get; set; }
    }

    public partial class Media
    {
        public decimal aspectRatio { get; set; }
        public byte audioChannels { get; set; }
        public string audioCodec { get; set; }
        public string audioProfile { get; set; }
        public ushort bitrate { get; set; }
        public string container { get; set; }
        public uint duration { get; set; }
        public ushort height { get; set; }
        public ushort id { get; set; }
        public Part part { get; set; }
        public string videoCodec { get; set; }

        public string videoFrameRate { get; set; }

        public string videoProfile { get; set; }

        public ushort videoResolution { get; set; }

        public ushort width { get; set; }
    }

    public partial class Part
    {
        public string audioProfile { get; set; }
        public string container { get; set; }
        public uint duration { get; set; }
        public string file { get; set; }
        public ushort id { get; set; }
        public string key { get; set; }
        public uint size { get; set; }
        public List<Stream> stream { get; set; }
        public string videoProfile { get; set; }
    }

    public partial class Stream
    {
        public string audioChannelLayout { get; set; }
        public byte bitDepth { get; set; }

        public bool bitDepthSpecified { get; set; }

        public ushort bitrate { get; set; }

        public bool bitrateSpecified { get; set; }

        public byte cabac { get; set; }

        public bool cabacSpecified { get; set; }

        public byte channels { get; set; }
        public bool channelsSpecified { get; set; }
        public string chromaSubsampling { get; set; }

        public string codec { get; set; }

        public string codecID { get; set; }

        [DeserializeAs(Name = "default")]
        public byte defaultProp { get; set; }

        public uint duration { get; set; }

        public bool durationSpecified { get; set; }

        public string format { get; set; }
        public decimal frameRate { get; set; }

        public string frameRateMode { get; set; }
        public bool frameRateSpecified { get; set; }
        public byte hasScalingMatrix { get; set; }

        public bool hasScalingMatrixSpecified { get; set; }

        public ushort height { get; set; }

        public bool heightSpecified { get; set; }

        public ushort id { get; set; }

        public byte index { get; set; }

        public string language { get; set; }
        public string languageCode { get; set; }
        public byte level { get; set; }

        public bool levelSpecified { get; set; }

        public string pixelFormat { get; set; }

        public string profile { get; set; }

        public byte refFrames { get; set; }

        public bool refFramesSpecified { get; set; }

        public ushort samplingRate { get; set; }
        public bool samplingRateSpecified { get; set; }
        public string scanType { get; set; }

        public byte selected { get; set; }
        public bool selectedSpecified { get; set; }
        public byte streamType { get; set; }

        public ushort width { get; set; }

        public bool widthSpecified { get; set; }
    }

    public partial class Player
    {
        public string address { get; set; }

        public string device { get; set; }

        public string machineIdentifier { get; set; }

        public string model { get; set; }

        public string platform { get; set; }

        public decimal platformVersion { get; set; }

        public string product { get; set; }

        public string state { get; set; }

        public string title { get; set; }

        public string vendor { get; set; }
        public string version { get; set; }
    }

    public partial class TranscodeSession
    {
        public byte audioChannels { get; set; }
        public string audioCodec { get; set; }
        public string audioDecision { get; set; }
        public string container { get; set; }
        public string context { get; set; }
        public uint duration { get; set; }
        public ushort height { get; set; }
        public string key { get; set; }

        public decimal progress { get; set; }
        public string protocol { get; set; }
        public byte remaining { get; set; }
        public decimal speed { get; set; }
        public string subtitleDecision { get; set; }
        public byte throttled { get; set; }
        public string videoCodec { get; set; }
        public string videoDecision { get; set; }
        public ushort width { get; set; }
    }

    public partial class Writer
    {
        public ushort id { get; set; }

        public string tag { get; set; }
    }

    public partial class User
    {
        public string authenticationToken { get; set; }
        public string certificateVersion { get; set; }
        public string cloudSyncDevice { get; set; }
        public string email { get; set; }
        public userEntitlements entitlements { get; set; }
        public string guest { get; set; }
        public string home { get; set; }
        public string homeSize { get; set; }
        public uint id { get; set; }
        public userJoinedat joinedat { get; set; }
        public string locale { get; set; }
        public string mailing_list_status { get; set; }
        public userProfile_settings profile_settings { get; set; }
        public string restricted { get; set; }
        public string scrobbleTypes { get; set; }
        public string secure { get; set; }
        public string thumb { get; set; }
        public string title { get; set; }
        public string username { get; set; }
        public string uuid { get; set; }
    }

    public partial class userEntitlements
    {
        public string all { get; set; }
        public List<userEntitlementsEntitlement> entitlement { get; set; }
    }

    public partial class userEntitlementsEntitlement
    {
        public string id { get; set; }
    }

    public partial class userJoinedat
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public partial class userProfile_settings
    {
        public byte auto_select_audio { get; set; }
        public byte auto_select_subtitle { get; set; }
        public string default_audio_language { get; set; }
        public string default_subtitle_language { get; set; }
    }
}