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

::ADD Minicap forward
C:\android-sdk\platform-tools\adb.exe -s "%device%" forward tcp:1717 localabstract:minicap

::PUSH Minicap
C:\android-sdk\platform-tools\adb.exe -s "%device%" push "minicap/libs/armeabi-v7a/minicap" "/data/local/tmp/"
::Samsung S5 - PUSH lib
C:\android-sdk\platform-tools\adb.exe -s "%device%" push "minicap/jni/minicap-shared/aosp/libs/android-23/armeabi-v7a/minicap.so" "/data/local/tmp/"
::OnePlus3T - PUSH lib
::C:\android-sdk\platform-tools\adb.exe -s "%device%" push "minicap/jni/minicap-shared/aosp/libs/android-28/armeabi-v7a/minicap.so" "/data/local/tmp/"

::Give Exe Permission to Minicap
C:\android-sdk\platform-tools\adb.exe -s "%device%" shell chmod 777 /data/local/tmp/minicap

::Touch execution in background (use of &)
C:\android-sdk\platform-tools\adb.exe -s "%device%" shell /data/local/tmp/touch

::Minicap execution in backgroung (use of &)
C:\android-sdk\platform-tools\adb.exe -s "%device%" shell LD_LIBRARY_PATH=/data/local/tmp /data/local/tmp/minicap -P 1920x1080@480x270/0 -Q 60 -S

::REMOVE Touch forwarding
C:\android-sdk\platform-tools\adb.exe -s "%device%" forward --remove tcp:9889

::REMOVE Minicap forwarding
C:\android-sdk\platform-tools\adb.exe -s "%device%" forward --remove tcp:1717