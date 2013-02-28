/*
 * Copyright 2012 Applifier
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

#include "iPhone_GlesSupport.h"

#if !defined(everyplay_msaaDepthbuffer) && !defined(everyplay_systemFrameBuffer)
#error "Everyplay integration error"
#error "Try rebuilding and replacing the Xcode project from scratch in Unity"
#error "Make sure all Assets/Editor/PostprocessBuildPlayer* files are run correctly"
#endif

// Original GLES methods
void CreateSurfaceGLES_Unity(EAGLSurfaceDesc* surface);
void DestroySurfaceGLES_Unity(EAGLSurfaceDesc* surface);
void PreparePresentSurfaceGLES_Unity(EAGLSurfaceDesc* surface);
void AfterPresentSurfaceGLES_Unity(EAGLSurfaceDesc* surface);
extern "C" bool UnityResolveMSAA_Unity(GLuint destFBO, GLuint colorTex, GLuint colorBuf, GLuint depthTex, GLuint depthBuf);

#ifdef EVERYPLAY_GLES_WRAPPER

#include "iPhone_Profiler.h"
#import "AppController.h"

// iPhone_View.h
extern UIViewController* UnityGetGLViewController();
extern UIView* UnityGetGLView();

// AppController.m
extern EAGLSurfaceDesc _surface;
extern id _displayLink;

extern "C" void InitEAGLLayer(void* eaglLayer, bool use32bitColor);
extern "C" bool AllocateRenderBufferStorageFromEAGLLayer(void* eaglLayer);
extern "C" void DeallocateRenderBufferStorageFromEAGLLayer();

// iPhone_GlesSupport.cpp
extern bool	UnityIsCaptureScreenshotRequested();
extern void	UnityCaptureScreenshot();

// libiPhone
extern GLint gDefaultFBO;

extern bool UnityUse32bitDisplayBuffer();
extern int UnityGetDesiredMSAASampleCount(int defaultSampleCount);
extern void UnityGetRenderingResolution(unsigned* w, unsigned* h);

extern void UnityBlitToSystemFB(unsigned tex, unsigned w, unsigned h, unsigned sysw, unsigned sysh);

extern void UnityPause(bool pause);

#if defined(everyplay_msaaDepthbuffer)

enum EnabledOrientation
{
    kAutorotateToPortrait = 1,
    kAutorotateToPortraitUpsideDown = 2,
    kAutorotateToLandscapeLeft = 4,
    kAutorotateToLandscapeRight = 8
};

enum ScreenOrientation
{
    kScreenOrientationUnknown,
    portrait,
    portraitUpsideDown,
    landscapeLeft,
    landscapeRight,
    autorotation,
    kScreenOrientationCount
};
#else
#import "iPhone_OrientationSupport.h"
#endif

extern ScreenOrientation ConvertToUnityScreenOrientation(int hwOrient, EnabledOrientation* outAutorotOrient);
extern void UnitySetScreenOrientation(ScreenOrientation orientation);

#else

#define CreateSurfaceGLES CreateSurfaceGLES_Unity
#define DestroySurfaceGLES DestroySurfaceGLES_Unity
#define PreparePresentSurfaceGLES PreparePresentSurfaceGLES_Unity
#define AfterPresentSurfaceGLES AfterPresentSurfaceGLES_Unity
#define UnityResolveMSAA UnityResolveMSAA_Unity

#endif
