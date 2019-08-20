using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL_Core
{
    public class TouchEvent
    {
        [JsonProperty("type")]
        public string Type { get; set; }        
    }

    public class DownTouchEvent: TouchEvent
    {       
        public DownTouchEvent() { base.Type = TouchCommandType.Down; }
        [JsonProperty("contact")]
        public uint Contact { get; set; }
        [JsonProperty("x")]
        public uint X { get; set; }
        [JsonProperty("y")]
        public uint Y { get; set; }
        [JsonProperty("pressure")]
        public uint Pressure { get; set; }
    }

    public class MoveTouchEvent : TouchEvent
    {
        public MoveTouchEvent() { base.Type = TouchCommandType.Move; }
        [JsonProperty("contact")]
        public uint Contact { get; set; }
        [JsonProperty("x")]
        public uint X { get; set; }
        [JsonProperty("y")]
        public uint Y { get; set; }
        [JsonProperty("pressure")]
        public uint Pressure { get; set; }
    }

    public class UpTouchEvent : TouchEvent
    {
        public UpTouchEvent() { base.Type = TouchCommandType.Up; }
        [JsonProperty("contact")]
        public uint Contact { get; set; }
    }

    public class CommitTouchEvent : TouchEvent
    {
        public CommitTouchEvent() { base.Type = TouchCommandType.Commit; }
    }

    public class DelayTouchEvent : TouchEvent
    {
        public DelayTouchEvent() { base.Type = TouchCommandType.Delay; }
        [JsonProperty("value")]
        public uint Value { get; set; }
    }

    public static class TouchCommandType
    {
        public readonly static string Down = "down";
        public readonly static string Move = "move";
        public readonly static string Up = "up";
        public readonly static string Commit = "commit";
        public readonly static string Delay = "delay";
    } 
}
