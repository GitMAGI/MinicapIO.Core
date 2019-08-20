@echo off

::Samsung S5
SET device = 07ea9707 

::OnePlus3T
::SET device = 743580a0

::C:\android-sdk\tools\android-ndk-r14b\
::C:\android-sdk\platform-tools\adb.exe -s "%device%"

::To Restart The Device
::C:\android-sdk\platform-tools\adb.exe shell am broadcast -a android.intent.action.BOOT_COMPLETED

::ADD Touch forward
C:\android-sdk\platform-tools\adb.exe -s "%device%" forward tcp:9889 tcp:9889

::PUSH Touch
C:\android-sdk\platform-tools\adb.exe -s "%device%" push "touch/libs/armeabi-v7a/touch" "/data/local/tmp/"

::Give Exe Permission to Touch
C:\android-sdk\platform-tools\adb.exe -s "%device%" shell chmod 777 /data/local/tmp/touch

::Touch execution in background (use of &)
C:\android-sdk\platform-tools\adb.exe -s "%device%" shell /data/local/tmp/touch

::To Log
::C:\android-sdk\platform-tools\adb.exe -s "%device%"  logcat | find "touch"

C:\android-sdk\platform-tools\adb.exe -s "%device%" forward --remove tcp:9889