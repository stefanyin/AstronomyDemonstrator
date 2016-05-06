#pragma once


#define HOOK_API 


extern "C" HOOK_API void InitVeeFileHook(BOOL p_bVeeFile=FALSE);//p_bVeeFile表示读的都是加密文件，若为false则根据名称中是否由文件后缀名vee或者文件自动进行判断
extern "C" HOOK_API void StopVeeFileHook();


