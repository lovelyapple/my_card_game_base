using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDefinition
{
    public enum FieldObjectType
    {
        None,
        SpaceShip,
        Drill,
        Scanner,
        Resource,
    }
    public enum OutLineType
    {
        OutSide,
        Neighbor,
        Center,
    }
    public enum TileActionProcessState
    {
        Idle,
        Processing,
        WaitSendFinished,
        SendingFinished,
        SendingCancel,
        Finished,
    }
    public enum TileStatus
    {
        Idle,
        Scaning,
        Drilling,
    }
    public enum CommonResult
    {
        NonResult,
        Confirm,
        Cnacel,

        CommonError = 100,
        Error_NotEnoughItem = 101,

        CommonException = 1000,
        Exception_InvalidInventoryCount,
        Exception_InvalidTileRequest,
        Exception_MaxAutoDrillLevel,
    }
    public enum GamePhase
    {
        None,
        Ready,
        ReadyFade,
        Playing,
        Result,
    }
    public enum MinePlayMode
    {
        Traditional,
        Resource,
    }

    public static class ConstantDefine
    {
        public static bool IsDebugMode = false;
        public const int MinWindowSortingOrder = 1000;
        public const float SystemInputMinRange = 1f;
        public const float MinCameraDistance = -150f;
        public const float MaxCameraDistance = -250f;
#if UNITY_STANDALONE
        public const float CameraMoveSpeed = 2500f;
#else
        public const float CameraMoveSpeed = 2500f * 0.005f;
#endif
        public const float TileExtrudeRange = 0.01f;
        public const float DebugTileBombExtrudeRange = 0.02f;
        public static int TileBombProbability = 30;
        public const int TileOpenCheckDepth = 1;

        public const long ToolScannerCode = 20000001;
        public const long DefaultScannerCount = 4;
        public const long ToolDrillCode = 20000010;
        public const long DefaultDrillCount = 3;

        public const long ResourceYellowCode = 10000001;
        public const long DefaultYellowCount = 40;
        public const long ResourcePurpleCode = 10000002;
        public const long DefaultPurpleCount = 36;
        public const float FieldDrillObjectOffsetY = 0.04f;
        public const float FieldBombExploScale = 0.04f;
        public const float FieldResourceScale = 1f;


        public const long FieldScannTime = 3;
        public const long FieldDrillTime = 3;
        public const long DebugPlanetCode = 9999;
        public const int DebugPlanetLevel = 3;
        public const long DebugSpaceshipCode = 1;
        public const long SpaceShipAutoDrillResultElapsedTime = 60;//s
    }
}