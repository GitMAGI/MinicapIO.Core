@echo off

::Samsung S5
SET device = 07ea9707 

 ::OnePlus3T
::SET device = 743580a0

::C:\android-sdk\tools\android-ndk-r14b\
::C:\android-sdk\platform-tools\adb.exe -s "%device%"

::To Restart The Device
::\android-sdk\platform-tools\adb.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED

