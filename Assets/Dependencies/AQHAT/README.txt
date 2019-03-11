---AQHAT README---

Hello and thankyou for purchasing AQHAT!

AQHAT is a Unity plugin that analyses device information and scores it's potential performance, this score can then be used to set quality upon launch or configure anything that requires you know the rough performance of a device.

AQHAT caches data after it's first run, to ensure the script is not unnecescarily ran.
This cache is ignored if we detect a hardware deviation that would effect the scoring system.

If you have any issues or improvements for us, please let us know via our websites contact page - http://eageramoeba.co.uk/Contact/

Contents:
-A data folder containing 5 files. The are device/hardware/ios configurations and free sample passmark data.
-A documentation folder containing instrctions and explanations of the methods and systems involved.
-A prefabs folder containing a sample implementation of the methods.
-A scenes folder containing a debug/test scene.
-A scripts folder containing CSVReader.cs, DeviceRating.cs and AutoQuality.cs.
-This README file

Quick start/example method:
1. Open the sample scene “testScene” found in the prefabs folder.
2. Check that all settings in the module DeviceRating.cs attached to AutoQuality.cs are correct. This can be left as-is if you are unsure.
3. Run this scene on all relevant devices and note down their scores, both media and total. These are lines one and two. Alternatively, limit each hardware configuration reading using the cap and mock settings.
4. Using the noted down scores, set the “Quality Bands” array in accordance with your quality settings defined within unity. Each array entry should be the lowest score needed to hit this quality setting.
5. Save the “AutoQuality.cs” module as a SEPARATE prefab to the one found in the AQHAT folder.
6. Use the prefab you’ve saved in the first scene your project accesses.
7. Make sure the asset is loaded and the script runs BEFORE any other assets are loaded.
8. Enjoy!

Notes:
There is an experimental feature contained within that uses hardware names to add score to ceratin aspects (GFX/Processor).
While this system does work, it is inteded to be used when benchmark data is not available/unusable for various reasons. Please bear this in mind when enabling this feature.

The AutoQuality script is intended as an example, while it does work for us you may want to create a custom script if your game really delves into configuration of it's quality settings.

We have ensured that this modules caching functionality is compaible with UWP programs, for this we have avoided using "playerprefs" to store cached data (bar tsOS, Unity does not support storing data on this platform upon time of creation).

We have tested AQHAT on; UWP, Windows, Linux, Android, Amazon Android, OSX, IOS and tvOS.