# VRミラーセラピー UnityProject

基本的には main/Assets/Scenes/main で開発しています。  
新しい機能を試す場合は、別プロジェクトで動作確認後にマージしてください。  
その際には main/MirrorTherapylib_version.unitypackage から必要なライブラリ等をインポートしてください。

## スクリプト構成
主なスクリプトは main/Assets/Scripts にあります。  
main/Assets/Scripts/MirrorTherapy 配下にミラーセラピー機能を実装するクラスをまとめています。

- **DAConverter**  
  fNIRS への同期信号を扱います。Raspberry Pi 4 を介して RS-232C により同期信号を送信します。  
- **Editor**  
  各種エディタ用ファイルを含みます。  
- **HandTracking**  
  ViveHandTracking を用いた手指の動作推定を行います。  
- **PoseTracking**  
  MediaPipeUnity を使って全身の動作推定を行います。  
- **Settings**  
  カメラやキャリブレーションデータなどの設定ファイルを管理します。

## 使用アセット等
- UniVRM  
- MediaPipeUnity  
- SteamVR  
- ViveHandTracking
- FinalIK
