# iYi
This system is for animating the face of an avatar. It's convenient for VTuber or creating animation with your avatar.
This system cooperates with [iYi FaceTracker](https://github.com/azwjp/iyi-facetracker) on iOS.

## Flow of the system
First of all, you need to build and install iYi FaceTracker into your iPhone.
It sends the tracked face data to your computer through a network (UDP).
Then the Unity application made with this repository receives the data.
Finally, you can animate an avatar using the tracked data.

## First step with the sample class
1. Prepare an avatar with BlendShape named values in `ARKitBlendShapeLocation`
  - `AvatarAnimator` class changes values having the same name of `ARKitBlendShapeLocation` and the BlendShape
  - `ARKitBlendShapeLocation` is a list of keys of the data from ARKit
1. Add `IyiServer` on a GameObject
1. Attach `AvatarAnimator` on a GamaObject (note: this class is a sample. I recommend to write your new class or modify the class because it is created from my personal dirty class)
1. Set meshes with BlendShape to `Blend Shapes`, head bones of your avatar to `Head Bones`, the eye bones to `Left Eye` and `Right Eye`
1. Run [iYi FaceTracker](https://github.com/azwjp/iyi-facetracker) on your iPhone
1. Run the Unity project on your computer

## IyiServer class
It receives data from iYi FaceTracker.
And the parsed data is in `IyiServer.faceData`.

For example, the `Update()` method in your class might access like below.

```
var eyeBlinkLeft = GetComponent<IyiServer>().faceData.data[EyeBlinkLeft];
```
