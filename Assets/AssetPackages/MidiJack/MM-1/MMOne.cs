using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MidiJack {
	public class MMOne {

		// MIDI event delegates
        public static MidiDriver.NoteOnDelegate noteOnDelegate {
            get { return MidiDriver.Instance.noteOnDelegate; }
            set { MidiDriver.Instance.noteOnDelegate = value; }
        }

        public static MidiDriver.NoteOffDelegate noteOffDelegate {
            get { return MidiDriver.Instance.noteOffDelegate; }
            set { MidiDriver.Instance.noteOffDelegate = value; }
        }

        public static MidiDriver.KnobDelegate knobDelegate {
            get { return MidiDriver.Instance.knobDelegate; }
            set { MidiDriver.Instance.knobDelegate = value; }
        }

        // Returns the key state (on: velocity, off: zero).
        public static float GetKey(MidiChannel channel, int noteNumber)
        {
            return MidiDriver.Instance.GetKey(channel, noteNumber);
        }

        public static float GetKey(int noteNumber)
        {
            return MidiDriver.Instance.GetKey(MidiChannel.All, noteNumber);
        }

        // Returns true if the key was pressed down in the current frame.
        public static bool GetKeyDown(MidiChannel channel, int noteNumber)
        {
            return MidiDriver.Instance.GetKeyDown(channel, noteNumber);
        }

        public static bool GetKeyDown(int noteNumber)
        {
            return MidiDriver.Instance.GetKeyDown(MidiChannel.All, noteNumber);
        }

        // Returns true if the key was released in the current frame.
        public static bool GetKeyUp(MidiChannel channel, int noteNumber)
        {
            return MidiDriver.Instance.GetKeyUp(channel, noteNumber);
        }

        public static bool GetKeyUp(int noteNumber)
        {
            return MidiDriver.Instance.GetKeyUp(MidiChannel.All, noteNumber);
        }

        // Provides the CC (knob) list.
        public static int[] GetKnobNumbers(MidiChannel channel)
        {
            return MidiDriver.Instance.GetKnobNumbers(channel);
        }

        public static int[] GetKnobNumbers()
        {
            return MidiDriver.Instance.GetKnobNumbers(MidiChannel.All);
        }

        // Returns the CC (knob) value.
        public static float GetKnob(MidiChannel channel, int knobNumber, float defaultValue = 0)
        {
            return MidiDriver.Instance.GetKnob(channel, knobNumber, defaultValue);
        }

        public static float GetKnob(int knobNumber, float defaultValue = 0)
        {
            return MidiDriver.Instance.GetKnob(MidiChannel.All, knobNumber, defaultValue);
        }

		//strings
		 // Provides the CC (knob) list.

        // Returns the CC (knob) value.
        public static float GetKnob(string knobName, float defaultValue = 0)
        {
			int knobNumber = 0;
			try {
				knobNumber = GetKnobNumber(knobName);
			} catch(System.Exception e) {
				Debug.LogError(e.Message);
				return 0;
			}
            return MidiDriver.Instance.GetKnob(MidiChannel.All, knobNumber, defaultValue);
        }
		public static float GetSlider(string sliderName, float defaultValue = 0)
        {
			int sliderNumber = 0;
			try {
				sliderNumber = GetSliderNumber(sliderName);
			} catch(System.Exception e) {
				Debug.LogError(e.Message);
				return 0;
			}
            return MidiDriver.Instance.GetKnob(MidiChannel.All, sliderNumber, defaultValue);
        }
		public static bool GetButton(string buttonName) {
			int buttonNumber = 0;
			try {
				buttonNumber = GetButtonNumber(buttonName);
			} catch(System.Exception e) {
				Debug.LogError(e.Message);
				return false;
			}
			return MidiDriver.Instance.GetKey(MidiChannel.All, buttonNumber) > 0;
		}
		public static bool GetButtonDown(string buttonName) {
			int buttonNumber = 0;
			try {
				buttonNumber = GetButtonNumber(buttonName);
			} catch(System.Exception e) {
				Debug.LogError(e.Message);
				return false;
			}
			return MidiDriver.Instance.GetKeyDown(MidiChannel.All, buttonNumber);
		}
		public static bool GetButtonUp(string buttonName) {
			int buttonNumber = 0;
			try {
				buttonNumber = GetButtonNumber(buttonName);
			} catch(System.Exception e) {
				Debug.LogError(e.Message);
				return false;
			}
			return MidiDriver.Instance.GetKeyUp(MidiChannel.All, buttonNumber);
		}

		

		public static int GetKnobNumber(string knobName) {
			string[] Names = new string[] {
				"Out1","Out2","BrowseKnob","CueVol","CueMix",
				"Matrix00", "Matrix10", "Matrix20", "Matrix30",
				"Matrix01", "Matrix11", "Matrix21", "Matrix31",
				"Matrix02", "Matrix12", "Matrix22", "Matrix32",
				"Matrix03", "Matrix13", "Matrix23", "Matrix33"
			};
			int[] Outputs = new int[] {
				1,2,3,4,5,
				6,7,8,9,
				10,11,12,13,
				14,15,16,17,
				18,19,20,21
			};
			for(int i = 0; i < Names.Length; i++) {
				if(knobName.ToLower().Trim() == Names[i].ToLower().Trim()) {
					return Outputs[i];
				}
			}
			throw new UnityException("Knob name '" + knobName + "' not found");
		}

		public static int GetSliderNumber(string sliderName) {
			string[] Names = new string[] {
				"Ch1_Vol", "Ch2_Vol", "Ch3_Vol", "Ch4_Vol",
				"Crossfade"
			};
			int[] Outputs = new int[] {
				48,49,50,51,
				64
			};
			for(int i = 0; i < Names.Length; i++) {
				if(sliderName.ToLower().Trim() == Names[i].ToLower().Trim()) {
					return Outputs[i];
				}
			}
			throw new UnityException("Slider name '" + sliderName + "' not found");
		}

		public static int GetButtonNumber(string buttonName) {
			string[] Names = new string[] {
				"BrowseLeft", "BrowseRight",
				"Mute",
				"Ch1_1", "Ch1_2", "Ch1_Cue",
				"Ch2_1", "Ch2_2", "Ch2_Cue",
				"Ch3_1", "Ch3_2", "Ch3_Cue",
				"Ch4_1", "Ch4_2", "Ch4_Cue"
			};
			int[] Outputs = new int[] {
				16, 17,
				18,
				19, 20, 48,
				23, 24, 49,
				27, 28, 50,
				31, 32, 51
			};
			for(int i = 0; i < Names.Length; i++) {
				if(buttonName.ToLower().Trim() == Names[i].ToLower().Trim()) {
					return Outputs[i];
				}
			}
			throw new UnityException("Slider name '" + buttonName + "' not found");
		}
	}
}