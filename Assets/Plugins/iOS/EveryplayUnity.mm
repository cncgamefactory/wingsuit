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

#import "EveryplayUnity.h"

#define EVERYPLAY_GLES_WRAPPER
#import "EveryplayGlesSupport.h"

void UnitySendMessage(const char* obj, const char* method, const char* msg);

extern "C" {
  char* EveryplayCopyString (const char* string) {
    if (string != NULL) {
      char* res = strdup(string);
      return res;
    }

    return NULL;
  }

  NSString* EveryplayCreateNSString (const char* string) {
    return string ? [NSString stringWithUTF8String: string] : [NSString stringWithUTF8String: ""];
  }
}

@implementation EveryplayUnity

- (id)initWithClientId:(NSString *)clientId andClientSecret:(NSString *)clientSecret andRedirectURI:(NSString *)redirectURI {
  self = [super init];
  if (self != nil) {

    [Everyplay initWithDelegate: self andParentViewController: UnityGetGLViewController()];
    [Everyplay setClientId: clientId clientSecret: clientSecret redirectURI: redirectURI];

    displayLinkPaused = NO;

    EveryplayLog(@"Everyplay init from Unity with client ID: %@ and client secret: %@ and redirect URI: %@", clientId, clientSecret, redirectURI);
  }
  return self;
}

- (void)everyplayShown {
  ELOG;
  UnityPause(true);
  CADisplayLink *displayLink = (CADisplayLink *) _displayLink;
  if(displayLink != nil) {
    if([displayLink isPaused] == NO) {
      displayLinkPaused = YES;
      [displayLink setPaused: YES];
      EveryplayLog(@"Everyplay paused _displayLink");
    }
  }
}

- (void)everyplayHidden {
  ELOG;
  CADisplayLink *displayLink = (CADisplayLink *) _displayLink;
  if(displayLink != nil && displayLinkPaused) {
    displayLinkPaused = NO;
    [displayLink setPaused: NO];
    EveryplayLog(@"Everyplay unpaused _displaylink");
  }
  UnityPause(false);

  /* Force orientation check, orientation could have changed while Unity was paused */
  UIInterfaceOrientation orientIOS = UnityGetGLViewController().interfaceOrientation;
  ScreenOrientation orientation = ConvertToUnityScreenOrientation(orientIOS, 0);
  UnitySetScreenOrientation(orientation);

  UnitySendMessage("Everyplay", "EveryplayHidden", "");
}

- (void)everyplayRecordingStarted {
  ELOG;
  UnitySendMessage("Everyplay", "EveryplayRecordingStarted", "");
}

- (void)everyplayRecordingStopped {
  ELOG;
  UnitySendMessage("Everyplay", "EveryplayRecordingStopped", "");
}

- (void)everyplayThumbnailReadyAtFilePath:(NSString *)thumbnailFilePath {
  ELOG;
  UnitySendMessage("Everyplay", "EveryplayThumbnailReadyAtFilePath", [thumbnailFilePath UTF8String]);
}

@end

static EveryplayUnity *everyplay = NULL;

extern "C" {

  void InitEveryplay(const char *clientId, const char *clientSecret, const char *redirectURI) {
    if (everyplay == NULL) {
      everyplay = [[EveryplayUnity alloc] initWithClientId: EveryplayCreateNSString(clientId) andClientSecret: EveryplayCreateNSString(clientSecret) andRedirectURI: EveryplayCreateNSString(redirectURI)];
    }
  }

  void EveryplayShow() {
    [[Everyplay sharedInstance] showEveryplay];
  }

  void EveryplayStartRecording() {
    [[[Everyplay sharedInstance] capture] startRecording];
  }

  void EveryplayStopRecording() {
    [[[Everyplay sharedInstance] capture] stopRecording];
  }

  void EveryplayPauseRecording() {
    [[[Everyplay sharedInstance] capture] pauseRecording];
  }

  void EveryplayResumeRecording() {
    [[[Everyplay sharedInstance] capture] resumeRecording];
  }

  bool EveryplayIsRecording() {
    return [[[Everyplay sharedInstance] capture] isRecording];
  }

  bool EveryplayIsPaused() {
    return [[[Everyplay sharedInstance] capture] isPaused];
  }

  bool EveryplaySnapshotRenderbuffer() {
    return [[[Everyplay sharedInstance] capture] snapshotRenderbuffer];
  }

  void EveryplayPlayLastRecording() {
    [[Everyplay sharedInstance] playLastRecording];
  }

  void EveryplaySetMetadata(const char *val) {
    Class jsonSerializationClass = NSClassFromString(@"NSJSONSerialization");

    if (!jsonSerializationClass) {
        return;
    }

    NSString *strValue = EveryplayCreateNSString(val);

    EveryplayLog(@"Set metadata %@", strValue);

    NSError *jsonError = nil;
    NSData *jsonData = [strValue dataUsingEncoding:NSUTF8StringEncoding];

    id jsonParsedObj = [jsonSerializationClass JSONObjectWithData:jsonData options:0 error:&jsonError];

    if (jsonError == nil) {
      if ([jsonParsedObj isKindOfClass:[NSDictionary class]]) {
        [[Everyplay sharedInstance] mergeSessionDeveloperData: (NSDictionary *) jsonParsedObj];
      }
    } else {
        EveryplayLog(@"Failed parsing JSON: %@", jsonError);
    }
  }

  void EveryplaySetMaxRecordingMinutesLength(int minutes) {
    [[Everyplay sharedInstance] capture].maxRecordingMinutesLength = minutes;
  }

  bool EveryplayIsSupported() {
    return [Everyplay isSupported];
  }
}