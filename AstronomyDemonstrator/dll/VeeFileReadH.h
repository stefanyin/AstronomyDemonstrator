#pragma once


#define HOOK_API 


extern "C" HOOK_API void InitVeeFileHook(BOOL p_bVeeFile=FALSE);//p_bVeeFile��ʾ���Ķ��Ǽ����ļ�����Ϊfalse������������Ƿ����ļ���׺��vee�����ļ��Զ������ж�
extern "C" HOOK_API void StopVeeFileHook();


